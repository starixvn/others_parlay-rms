if (typeof ParlayRMS == 'undefined') {
    ParlayRMS = {};
};

ParlayRMS.Winloss = {
    Init: function () {
        $("#betType").multiselect({
            minWidth: 120,
            height: 82,
            header: false,
            multiple: false,
            selectedList: 1
        });

        $("#fromDate").datepicker({
            dateFormat: "MM dd, yy",
            showButtonPanel: true,
            minDate: (diffDate(dateRanges.LimitDataFromDate, dateRanges.NowDate) > 60 ? 60 * -1 : diffDate(dateRanges.LimitDataFromDate, dateRanges.NowDate) * -1),

            onSelect: function (dateText, inst) {
                if (dateText != inst.lastVal) {
                    $("#dateRange option[value='7']").attr('selected', 1);
                    $("#dateRange").multiselect("refresh");
                }
            },
            onClose: function (selectedDate) {
            }
        });

        $("#toDate").datepicker({
            dateFormat: "MM dd, yy",
            showButtonPanel: true,
            minDate: (diffDate(dateRanges.LimitDataFromDate, dateRanges.NowDate) > 60 ? 60 * -1 : diffDate(dateRanges.LimitDataFromDate, dateRanges.NowDate) * -1),

            onSelect: function (dateText, inst) {
                if (dateText != inst.lastVal) {
                    $("#dateRange option[value='7']").attr('selected', 1);
                    $("#dateRange").multiselect("refresh");
                }
            },
            onClose: function (selectedDate) {
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

            if (validator.validateDate("#fromDate")) {
                return;
            }

            if (validator.validateDate("#toDate")) {
                return;
            }

            ParlayRMS.Winloss.Data(betType, fromDate, toDate);
        });
    },
    Data: function (betType, fromDate, toDate) {
        var data = {
            FromDate: fromDate,
            ToDate: toDate,
            BetType: betType
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
    ExportExcel: function (betType, fromDate, toDate) {
        var url = 'Winloss/Excel?fromDate=' + fromDate + '&toDate=' + toDate + '&betType=' + betType;
        window.location.href = resolveClientUrl(url);
    }
}

$(document).ready(function () {
    ParlayRMS.Winloss.Init();
});