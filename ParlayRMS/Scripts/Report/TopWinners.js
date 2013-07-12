var userOptions = {
    FromDate: new Date(),
    ToDate: new Date(),
    SportsType: 0,
    SelectTop: 100,
    Save: function (fromDate, toDate, sportsType, selectTop) {
        this.FromDate = fromDate;
        this.ToDate = toDate;
        this.SportsType = sportsType;
        this.SelectTop = selectTop;
    }
};

$(function () {
    $("#selectTop").multiselect({
        minWidth: 60,
        height: 105,
        header: false,
        multiple: false,
        selectedList: 1
    });
    $("#dateRange").multiselect({
        minWidth: 140,
        height: 184,
        header: false,
        multiple: false,
        selectedList: 1
    });
    $("#btnSearch").addClass("button");
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

    $("#dateRange").change(function () {
        var fromDate = null;
        var toDate = null;
        var limitDate = new Date(dateRanges.LimitDataFromDate);
        var selValue = parseInt($("#dateRange option:selected").val());
        switch (selValue) {
            case dateRanges.Yesterday:
                var nowDate = moment(dateRanges.NowDate);
                var yesDate = moment(dateRanges.NowDate).subtract('days', 1);

                fromDate = (yesDate < moment(limitDate) ? nowDate : yesDate);
                toDate = fromDate;
                break;

            case dateRanges.CurrentWeek:
                var currWeek = moment(dateRanges.NowDate).day(0);

                fromDate = (currWeek < limitDate ? moment(dateRanges.NowDate) : currWeek);
                toDate = moment(dateRanges.NowDate);
                break;

            case dateRanges.CurrentMonth:
                var nowDate = moment(dateRanges.NowDate);
                var currMonth = moment(dateRanges.NowDate).startOf('month');

                fromDate = (currMonth < moment(limitDate) ? nowDate : currMonth);
                toDate = nowDate;
                break;

            case dateRanges.LastWeek:
                var nowDate = moment(dateRanges.NowDate);
                var lstWeek = moment(dateRanges.NowDate).subtract('weeks', 1).day(1);

                fromDate = (lstWeek < limitDate ? moment(limitDate) : lstWeek);

                var lstWeekToDate = moment(fromDate).add('days', 6);
                toDate = (lstWeekToDate < limitDate ? nowDate : lstWeekToDate);
                break;

            case dateRanges.LastMonth:
                var nowDate = moment(dateRanges.NowDate);
                var lstMonth = moment(dateRanges.NowDate).subtract('months', 1).startOf('month');

                fromDate = (lstMonth < limitDate ? moment(limitDate) : lstMonth);

                var lstMonthToDate = moment(dateRanges.NowDate).startOf('month').subtract('days', 1);
                toDate = (lstMonthToDate < limitDate ? moment(limitDate) : lstMonthToDate);
                break;

            case dateRanges.SinceLastMonth:
                var nowDate = moment(dateRanges.NowDate);
                var sinLastMonth = moment(dateRanges.NowDate).subtract('months', 1).startOf('month');

                fromDate = (sinLastMonth < limitDate ? moment(limitDate) : sinLastMonth);
                toDate = nowDate;
                break;
            case dateRanges.SpecificDate:
                fromDate = moment($("#fromDate").val());
                toDate = moment($("#toDate").val());
                break;
        }

        $("#fromDate").val(fromDate.format('MMMM DD, YYYY'));
        $("#toDate").val(toDate.format('MMMM DD, YYYY'));
    })

    $("#dateRange").change();

    $('#btnSearch').click(function () {
        var selectTop = $("#selectTop").val();
        var fromDate = $("#fromDate").val();
        var toDate = $("#toDate").val();
        var sportsType = $('input[name=SportsTypes]:checked', '#divSportsTypes').val();

        if (!moment(fromDate, "MMMM DD, YYYY").isValid() || !moment(fromDate).isValid()) {
            Validators.show($("#fromDate").get(0), 'Invalid Date');
            return;
        }

        if (!moment(toDate, "MMMM DD, YYYY").isValid() || !moment(toDate).isValid()) {
            Validators.show($("#toDate").get(0), 'Invalid Date');
            return;
        }

        ViewTopWinners(fromDate, toDate, sportsType, selectTop);
    });
});

function ViewTopWinners(fromDate, toDate, sportsType, selectTop) {
    userOptions.Save(fromDate, toDate, sportsType, selectTop);

    var data = {
        FromDate: fromDate,
        ToDate: toDate,
        SportsType: sportsType,
        SelectTop: selectTop
    }
    $.ajax({
        url: resolveClientUrl('TopWinners/Data'),
        type: 'POST',
        data: data,
        dataType: 'html',
        cache: false,
        beforeSend: function (xhr) {
            loading.show();
        },
        success: function (result) {
            var data = $.parseJSON(result);
            FillData($('#result'), data);
            ParlayRMS.Core.ResolvePermission();
        },
        error: function (xhr, status, err) {
            alertBox.showError(xhr);
        },
        complete: function () {
            loading.hide();
        }
    });
}

function FillData(table, data) {
    var records = data.Records;
    var length = records.length;
    var tbody = $(table).children('tbody');
    tbody.html('');

    $('#title').html(data.Title);
    if (length == 0) {
        tbody.html('<tr style="height:0px"></tr>');
        $('#totalRow').hide();
        $('#noinfoRow').show();
    }
    else {
        var r = new Array();
        var j = -1;
        for (var i = 0; i < length; i++) {
            var item = records[i];
            var className = '';
            if (item.Level == 0) {
                className = 'levelNoneColor';
            }
            else {
                className = 'level' + item.Level;
            }

            r[++j] = '<tr id="tr_' + item.CustId + '">';
            r[++j] = '<td class="center">' + (i + 1) + '</td>';
            var disabled = 'disabled';
            if (hasWritePermission) {
                disabled = '';
            }
            r[++j] = '<td class="' + className + '"><span>' + item.Username + '</span> <span class="arrowDown_table ' + disabled + '" title="Click to change level" username="' + item.Username + '" custid="' + item.CustId + '"></span></td>';
            r[++j] = '<td>' + item.Currency + '</td>';
            r[++j] = '<td class="numeric">' + numeral(item.Stake).format('0,0') + '</td>';
            r[++j] = '<td class="numeric">' + numeral(item.Winlost).format('0,0.00') + '</td>';
            r[++j] = '<td class="numeric">' + numeral(item.Company_Winlost).format('0,0.00') + '</td>';
            r[++j] = '<td class="numeric">' + numeral(item.Stake_BaseRM).format('0,0.00') + '</td>';
            r[++j] = '<td class="numeric">' + numeral(item.Winlost_BaseRM).format('0,0.00') + '</td>';
            r[++j] = '<td class="numeric">' + numeral(item.Company_Winlost_BaseRM).format('0,0.00') + '</td>';
            r[++j] = '<td class="center"><button title="Click to view detail" class="button-disable" style="width:40px;" onclick="ViewDetails(' + item.CustId + ')">' + item.BetCount + '</button></td>';
            r[++j] = '<td class="numeric">' + numeral(item.Avg_Odds).format('0,0.00') + '</td>';
            r[++j] = '</tr>';
        }

        tbody.html(r.join(''));
        tbody.children("tr:even").addClass("even");
        var tfoot = $('#footer').children('tfoot');
        tfoot.find('#totalStake').html(numeral(data.TotalStake).format('0,0.00'));
        tfoot.find('#totalWA').html(numeral(data.TotalWinlost).format('0,0.00'));
        tfoot.find('#totalCLA').html(numeral(data.TotalCompanyWinlost).format('0,0.00'));
        tfoot.find('#totalTB').html(data.TotalBet);

        $('#totalRow').show();
        $('#noinfoRow').hide();
    }

    $('#divResult').show();
    RenderColorLevel();
}

function RenderColorLevel() {
    $(".arrowDown_table").contextMenu({
        menu: 'LevelList'
    }, function (elOption, objMenu, pos) {
        changeLevel(elOption, objMenu);
    });
}

function ViewDetails(custId) {
    var table = $('#result');
    var found = table.find("tr[custid='" + custId + "']");
    if (found.length != 0) {
        table.find("tr[custid='" + custId + "']").remove();
    }
    else {
        table.find('tr[custid]').remove();

        var row = $("#tr_" + custId);
        row.after('<tr class="details" custid="' + custId + '"><td colspan="11" class="center"></td></tr>');
        var newRow = row.next();
        var td = newRow.find('td');
        var waitingImg = $('<img src="../Images/Content/icon_wait.gif")" />');
        td.html(waitingImg);

        var username = row.children('td').eq(1).text();
        var data = {
            CustId: custId,
            Username: username,
            FromDate: userOptions.FromDate,
            ToDate: userOptions.ToDate,
            SportsType: userOptions.SportsType,
            SelectTop: userOptions.SelectTop
        }

        $.ajax({
            url: resolveClientUrl('TopWinners/DataDetails'),
            type: 'POST',
            data: data,
            dataType: 'html',
            cache: false,
            beforeSend: function (xhr) {
            },
            success: function (result) {
                td.html(result);
                // animating
                $('div[custid="' + custId + '"]').css('display', 'none').slideToggle("slow");
                $('div[custid="' + custId + '"]').find('table').children('tbody').children("tr:even").addClass("even");
            },
            error: function (xhr, status, err) {
                alertBox.showError(xhr);
            },
            complete: function () {
                loading.hide();
            }
        });
    }

    return false;
}

function ExportExcel() {
    var fromdate = moment(userOptions.FromDate);
    var todate = moment(userOptions.ToDate);

    var url = 'TopWinners/Excel?fromdate=' + fromdate.format("L") + '&todate=' + todate.format("L") + '&sportstype=' + userOptions.SportsType + '&selecttop=' + userOptions.SelectTop;
    window.location.href = resolveClientUrl(url);
}