if (typeof ParlayRMS == 'undefined') {
    ParlayRMS = {};
};

ParlayRMS.Winloss = {
    Init: function () {
        var nowDate = moment(dateRanges.NowDate).format('MMMM DD, YYYY');

        $("#betType").multiselect({
            minWidth: 120,
            height: 82,
            header: false,
            multiple: false,
            selectedList: 1
        });

        $("#combo").multiselect({
            minWidth: 132,
            height: 136,
            create: function () {
                $("#combo").multiselect("checkAll");
            },
            selectedText: function (numChecked, numTotal, checkedItems) {
                if (numChecked == 0) {
                    return "Select options";
                }
                else if (numChecked == 1) {
                    return $(checkedItems).attr('title');
                }
                else if (numChecked == numTotal) {
                    return "All";
                }
                else {
                    return numChecked + ' options';
                }
            }
        });
        
        $("#fromDate").datepicker({
            dateFormat: "MM dd, yy",
            showButtonPanel: true,
            changeMonth: true,
            changeYear: true,
            minDate: (diffDate(dateRanges.LimitDataFromDate, dateRanges.NowDate) > 60 ? 60 * -1 : diffDate(dateRanges.LimitDataFromDate, dateRanges.NowDate) * -1),
            maxDate: nowDate,
            onSelect: function (dateText, inst) {
                if (dateText != inst.lastVal) {
                    $("#dateRange option[value='7']").attr('selected', 1);
                    $("#dateRange").multiselect("refresh");
                }
            },
            onClose: function (selectedDate) {
                $("#toDate").datepicker("option", "minDate", selectedDate);
            }
        });

        $("#toDate").datepicker({
            dateFormat: "MM dd, yy",
            showButtonPanel: true,
            changeMonth: true,
            changeYear: true,
            minDate: (diffDate(dateRanges.LimitDataFromDate, dateRanges.NowDate) > 60 ? 60 * -1 : diffDate(dateRanges.LimitDataFromDate, dateRanges.NowDate) * -1),
            maxDate: nowDate,
            onSelect: function (dateText, inst) {
                if (dateText != inst.lastVal) {
                    $("#dateRange option[value='7']").attr('selected', 1);
                    $("#dateRange").multiselect("refresh");
                }
            },
            onClose: function (selectedDate) {
                $("#fromDate").datepicker("option", "maxDate", selectedDate);
            }
        });

        var nowDate = moment(dateRanges.NowDate);
        $("#fromDate").val(nowDate.format('MMMM DD, YYYY'));
        $("#toDate").val(nowDate.format('MMMM DD, YYYY'));

        $("#btnSearch").addClass("button");
        $('#btnSearch').click(function () {
            $("#divResult").html('');
            var betType = $("#betType").val();
            var fromDate = $("#fromDate").val();
            var toDate = $("#toDate").val();
            var sportsType = $('input[name=SportsTypes]:checked', '#divSportsTypes').val();

            if (validator.validateDate("#fromDate")) {
                return;
            }

            if (validator.validateDate("#toDate")) {
                return;
            }

            var combos = $("#combo").multiselect("getChecked").map(function () {
                return this.value;
            }).get().join(',');
            if (combos == '') {
                Validators.show($("#combo").multiselect("getButton").get(0), 'Please select your options');
                return;
            }

            ParlayRMS.Winloss.Data(betType, fromDate, toDate, sportsType, combos);
        });
    },
    Data: function (betType, fromDate, toDate, sportsType, combos) {
        var data = {
            FromDate: fromDate,
            ToDate: toDate,
            BetType: betType,
            SportsType: sportsType,
            Combos: combos
        }

        $.ajax({
            url: resolveClientUrl('Winloss/Data'),
            type: 'POST',
            data: data,
            dataType: 'html',
            cache: false,
            beforeSend: function (xhr) {
                loading.show();
            },
            success: function (result) {
                $("#divResult").html(result);
            },
            error: function (xhr, status, err) {
                alertBox.showError(xhr);
            },
            complete: function () {
                loading.hide();
            }
        });
    },
    ExportExcel: function (betType, fromDate, toDate, sportsType, combos) {
        var data = {
            FromDate: fromDate,
            ToDate: toDate,
            BetType: betType,
            SportsType: sportsType,
            Combos: combos
        }

        var url = 'Winloss/Excel?' + $.param(data);
        window.location.href = resolveClientUrl(url);
    }
}

$(document).ready(function () {
    ParlayRMS.Winloss.Init();
});