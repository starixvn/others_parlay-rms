ParlayWon = {
    Init: function () {
        var nowDate = moment(dateRanges.NowDate).format('MMMM DD, YYYY');

        $("#parlayType").multiselect({
            minWidth: 162,
            height: 82,
            create: function () {
                $("#parlayType").multiselect("checkAll");
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

        $("#level").multiselect({
            minWidth: 132,
            height: 136,
            create: function () {
                $("#level").multiselect("checkAll");
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

        $("#sportsType").multiselect({
            minWidth: 220,
            height: 300,
            create: function () {
                $("#sportsType").multiselect("checkAll");
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
            var fromDate = $("#fromDate").val();
            var toDate = $("#toDate").val();

            if (validator.validateDate("#fromDate")) {
                return;
            }

            if (validator.validateDate("#toDate")) {
                return;
            }

            var parlayTypes = $("#parlayType").multiselect("getChecked").map(function () {
                return this.value;
            }).get().join(',');
            if (parlayTypes == '') {
                Validators.show($("#parlayType").multiselect("getButton").get(0), 'Please select your options');
                return;
            }

            var combos = $("#combo").multiselect("getChecked").map(function() {
                return this.value;
             }).get().join(',');
            if (combos == '') {
                Validators.show($("#combo").multiselect("getButton").get(0), 'Please select your options');
                return;
        }

            var sportsTypes = $("#sportsType").multiselect("getChecked").map(function () {
                return this.value;
            }).get().join(',');
            if (sportsTypes == '') {
                Validators.show($("#sportsType").multiselect("getButton").get(0), 'Please select your options');
                return;
            }

            var levels = $("#level").multiselect("getChecked").map(function () {
                return this.value;
            }).get().join(',');
            if (levels == '') {
                Validators.show($("#level").multiselect("getButton").get(0), 'Please select your options');
                return;
            }

            var houseLoseAmountFrom = $('#houseLoseAmountFrom').val();
            var houseLoseAmountTo = $('#houseLoseAmountTo').val();

            ParlayWon.Data(parlayTypes, fromDate, toDate, combos, levels, sportsTypes, houseLoseAmountFrom, houseLoseAmountTo);
        });
    },
    Data: function (parlayTypes, fromDate, toDate, combos, levels, sportsType, houseLoseAmountFrom, houseLoseAmountTo) {
        var data = {
            FromDate: fromDate,
            ToDate: toDate,
            ParlayTypes: parlayTypes,
            SportsType: sportsType,
            Combos: combos,
            Levels: levels,
            HouseAmountFrom: houseLoseAmountFrom,
            HouseAmountTo: houseLoseAmountTo
        }

        $.ajax({
            url: resolveClientUrl('ParlayWon/Data'),
            type: 'POST',
            data: data,
            dataType: 'html',
            cache: false,
            beforeSend: function (xhr) {
                loading.show();
            },
            success: function (result) {
                $("#divResult").html(result);
                var table = $('#result');
                var header = $('#header');
                tableSorter.bind2(header, table, {
                    type: 'text',
                    onFinish: function () {
                        var trs = table.children('tbody').children("tr");
                        trs.each(function (index, tr) {
                            $(tr).attr('style', function (i, style) {
                                return style && style.replace(/display[^;]+;?/g, '');
                            });

                            $(tr).children('td:first').text(index + 1);
                        });

                        ParlayWon.Paging();
                    }
                });
                
                ParlayWon.Paging();
            },
            error: function (xhr, status, err) {
                alertBox.showError(xhr);
            },
            complete: function () {
                loading.hide();
            }
        });
    },
    ExportExcel: function (parlayTypes, fromDate, toDate, combos, levels, sportsType, houseLoseAmountFrom, houseLoseAmountTo) {
        var data = {
            FromDate: fromDate,
            ToDate: toDate,
            ParlayTypes: parlayTypes,
            SportsType: sportsType,
            Combos: combos,
            Levels: levels,
            HouseAmountFrom: houseLoseAmountFrom,
            HouseAmountTo: houseLoseAmountTo
        };

        var url = 'ParlayWon/Excel?' + $.param(data);
        window.location.href = resolveClientUrl(url);
    },
    ViewBetDetail: function(refNo, transId, username, custId, bettype) {
        if (bettype == 38) {
            ParlayRMS.Core.OpenWindow(resolveClientUrl('ParlayWon/SingleMatchParlayBetDetails?custid=' + custId + '&TransId=' + transId + '&username=' + username), 700, 600);
        }
        else {
            ParlayRMS.Core.OpenWindow(resolveClientUrl('ParlayWon/BetDetails?refno=' + refNo + '&TransId=' + transId + '&username=' + username), 700, 600);
        }
    },
    Paging: function () {
        var items = $("#result tbody tr");
        var numItems = items.length;
        var perPage = 100;

        // Only show the first 2 (or first `per_page`) items initially.
        items.slice(perPage).hide();

        // Now setup the pagination using the `#pagination` div.
        $("#pagination").pagination({
                items: numItems,
                itemsOnPage: perPage,
                cssStyle: "light-theme",
                // This is the actual page changing functionality.
                onPageClick: function(pageNumber) {
                    // We need to show and hide `tr`s appropriately.
                    var showFrom = perPage *(pageNumber -1);
                    var showTo = showFrom + perPage;

                        // We'll first hide everything...
                    items.hide()
                        // ... and then only show the appropriate rows.
                    .slice(showFrom, showTo).show();
                }
        });
    }
}

$(document).ready(function () {
    ParlayWon.Init();
});



