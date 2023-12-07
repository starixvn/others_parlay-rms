ParlayWon = {
    cachedObject: {},
    Init: function () {
        var nowDate = moment(dateRanges.NowDate).format('MMMM DD, YYYY');

        $("#houseLoseAmountFrom").autoNumeric({
            aSep: ',',
            aDec: '.',
            vMin: '0.00',
            vMax: '999999999999',
            aPad: false,
            aForm: false
        });

        $("#houseLoseAmountTo").autoNumeric({
            aSep: ',',
            aDec: '.',
            vMin: '0.00',
            vMax: '999999999999',
            aPad: false,
            aForm: false
        });

        $("#parlayType").multiselect({
            minWidth: 210,
            height: 165,
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

        $('#parlayType').on('change', function () {
            var selectedParlays = $(this).multiselect("getChecked").map(function () {
                return this.value;
            });
            if ($.inArray('1', selectedParlays) > -1) {
                if ($.inArray('7', selectedParlays) < 0) {
                    selectedParlays.push('7');
                }
                if ($.inArray('6', selectedParlays) < 0) {
                    selectedParlays.push('6');
                }
                $(this).val(selectedParlays).multiselect('refresh');
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

        $("#ddlSite").multiselect({
            minWidth: 80,
            height: 80,
            header: false,
            multiple: false,
            selectedList: 1
        });

        $("#ddlLiveType").multiselect({
            minWidth: 100,
            height: 110,
            header: false,
            multiple: false,
            selectedList: 1
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

        $("#currencies").multiselect({
            minWidth: 132,
            height: 136,
            create: function () {
                $("#currencies").multiselect("checkAll");
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
        }).multiselectfilter({
            autoReset: true
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
            ParlayWon.ResetCachedObject();
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

            var combos = $("#combo").multiselect("getChecked").map(function () {
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

            var currencies = $("#currencies").multiselect("getChecked").map(function () {
                return this.value;
            }).get().join(',');
            if (currencies == '') {
                Validators.show($("#currencies").multiselect("getButton").get(0), 'Please select at least one currency!');
                return;
            }

            var site = parseInt($("#ddlSite").val());

            var liveType = parseInt($("#ddlLiveType").val());

            var customerClass = parseInt($("#txtCustomerClass").val());

            var ip = $("#txtIp").val().trim();

            var houseLoseAmountFrom = parseFloat(($('#houseLoseAmountFrom').val()).replace(/,/g, ''));
            var houseLoseAmountTo = parseFloat(($('#houseLoseAmountTo').val()).replace(/,/g, ''));

            if (houseLoseAmountTo > 0 && houseLoseAmountFrom > houseLoseAmountTo) {
                Validators.show($("#houseLoseAmountTo"), 'House Lose Amount From must be less than To');
                return;
            }

            houseLoseAmountFrom = isNaN(houseLoseAmountFrom) ? 0 : houseLoseAmountFrom;
            if (houseLoseAmountFrom < 1000 && validator.validateDateInOneWeek("#fromDate", "#toDate")) {
                return;
            }

            ParlayWon.Data(parlayTypes,
                fromDate,
                toDate,
                combos,
                levels,
                sportsTypes,
                houseLoseAmountFrom,
                houseLoseAmountTo,
                currencies,
                site,
                liveType,
                customerClass,
                ip,
                ParlayWon.cachedObject.sortById,
                ParlayWon.cachedObject.isSortAsc,
                ParlayWon.cachedObject.pageIndex,
                ParlayWon.cachedObject.pageSize);
        });

        $(document).on('change', '#ddlPageSize', function () {
            ParlayWon.cachedObject.pageSize = parseInt($(this).val());
            ParlayWon.cachedObject.pageIndex = 1;
            ParlayWon.Data(ParlayWon.cachedObject.parlayTypes,
                ParlayWon.cachedObject.fromDate,
                ParlayWon.cachedObject.toDate,
                ParlayWon.cachedObject.combos,
                ParlayWon.cachedObject.levels,
                ParlayWon.cachedObject.sportsType,
                ParlayWon.cachedObject.houseAmountFrom,
                ParlayWon.cachedObject.houseAmountTo,
                ParlayWon.cachedObject.currencies,
                ParlayWon.cachedObject.site,
                ParlayWon.cachedObject.liveType,
                ParlayWon.cachedObject.customerClass,
                ParlayWon.cachedObject.ip,
                ParlayWon.cachedObject.sortById,
                ParlayWon.cachedObject.isSortAsc,
                ParlayWon.cachedObject.pageIndex,
                ParlayWon.cachedObject.pageSize);
        });

        $(document).on('click', '#pagination .page-item', function (e) {
            $("#pagination .page-item").removeClass("current");
            $(this).addClass("current");

            if ($(this).text() == "...") {
                return;
            }

            if ($(this).text() == "Prev" && ParlayWon.cachedObject.pageIndex == 1) {
                return;
            }

            if ($(this).text() == "Next" && ParlayWon.cachedObject.pageIndex == ParlayWon.cachedObject.maxPage) {
                return;
            }

            if ($(this).text() == "Prev") {
                ParlayWon.cachedObject.pageIndex -= 1;
            } else if ($(this).text() == "Next") {
                ParlayWon.cachedObject.pageIndex += 1;
            } else {
                ParlayWon.cachedObject.pageIndex = parseInt($(this).text());
            }

            ParlayWon.Data(ParlayWon.cachedObject.parlayTypes,
                ParlayWon.cachedObject.fromDate,
                ParlayWon.cachedObject.toDate,
                ParlayWon.cachedObject.combos,
                ParlayWon.cachedObject.levels,
                ParlayWon.cachedObject.sportsType,
                ParlayWon.cachedObject.houseAmountFrom,
                ParlayWon.cachedObject.houseAmountTo,
                ParlayWon.cachedObject.currencies,
                ParlayWon.cachedObject.site,
                ParlayWon.cachedObject.liveType,
                ParlayWon.cachedObject.customerClass,
                ParlayWon.cachedObject.ip,
                ParlayWon.cachedObject.sortById,
                ParlayWon.cachedObject.isSortAsc,
                ParlayWon.cachedObject.pageIndex,
                ParlayWon.cachedObject.pageSize);
        });

        $(document).on("click", '#header .sortable', function () {
            var currentObject = this;
            var currentColIndex = parseInt($(currentObject).attr("data-col-index"));

            if (currentColIndex == ParlayWon.cachedObject.sortById) {
                if (ParlayWon.cachedObject.isSortAsc) {
                    ParlayWon.cachedObject.isSortAsc = false;
                    $(this).removeClass("headerSortUp").addClass("headerSortDown");
                } else {
                    ParlayWon.cachedObject.isSortAsc = true;
                    $(this).removeClass("headerSortDown").addClass("headerSortUp");
                }
            } else {
                ParlayWon.cachedObject.isSortAsc = false;
                $(this).removeClass("headerSortUp").addClass("headerSortDown");

                $('#header').find(".sortable").each(function () {
                    if ($(currentObject).index() != $(this).index()) {
                        $(this).removeClass("headerSortDown").removeClass("headerSortUp");
                    }
                });
            }

            if (!currentColIndex) {
                currentColIndex = $(currentObject).index();
            }

            ParlayWon.cachedObject.sortById = parseInt(currentColIndex);
            ParlayWon.Data(ParlayWon.cachedObject.parlayTypes,
                ParlayWon.cachedObject.fromDate,
                ParlayWon.cachedObject.toDate,
                ParlayWon.cachedObject.combos,
                ParlayWon.cachedObject.levels,
                ParlayWon.cachedObject.sportsType,
                ParlayWon.cachedObject.houseAmountFrom,
                ParlayWon.cachedObject.houseAmountTo,
                ParlayWon.cachedObject.currencies,
                ParlayWon.cachedObject.site,
                ParlayWon.cachedObject.liveType,
                ParlayWon.cachedObject.customerClass,
                ParlayWon.cachedObject.ip,
                ParlayWon.cachedObject.sortById,
                ParlayWon.cachedObject.isSortAsc,
                ParlayWon.cachedObject.pageIndex,
                ParlayWon.cachedObject.pageSize);
        });
    },
    Data: function (parlayTypes, fromDate, toDate, combos, levels, sportsType, houseLoseAmountFrom, houseLoseAmountTo, currencies, site, liveType, customerClass, ip, sortById, isSortAsc, pageIndex, pageSize) {
        var data = {
            fromDate: fromDate,
            toDate: toDate,
            parlayTypes: parlayTypes,
            sportsType: sportsType,
            combos: combos,
            levels: levels,
            houseAmountFrom: houseLoseAmountFrom,
            houseAmountTo: houseLoseAmountTo,
            currencies: currencies,
            site: site,
            liveType: liveType,
            customerClass: customerClass,
            ip: ip,
            sortById: sortById,
            isSortAsc: isSortAsc,
            pageIndex: pageIndex,
            pageSize: pageSize
        }

        ParlayWon.cachedObject = data;

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
                var totalRows = parseInt($("#totalRows").val());

                $('#header').find(".sortable").each(function () {
                    if ($(this).attr("data-col-index") == ParlayWon.cachedObject.sortById) {
                        if (ParlayWon.cachedObject.isSortAsc) {
                            $(this).removeClass("headerSortDown").addClass("headerSortUp");
                        } else {
                            $(this).removeClass("headerSortUp").addClass("headerSortDown");
                        }
                    }
                });

                $("#ddlPageSize").val(ParlayWon.cachedObject.pageSize.toString());

                ParlayWon.Paging(totalRows, data.pageIndex);
            },
            error: function (xhr, status, err) {
                alertBox.showError(xhr);
            },
            complete: function () {
                loading.hide();
            }
        });
    },
    ExportExcel: function (parlayTypes, fromDate, toDate, combos, levels, sportsType, houseLoseAmountFrom, houseLoseAmountTo, currencies, site, liveType, customerClass, ip) {
        var data = {
            fromDate: fromDate,
            toDate: toDate,
            parlayTypes: parlayTypes,
            sportsType: sportsType,
            combos: combos,
            levels: levels,
            houseAmountFrom: houseLoseAmountFrom,
            houseAmountTo: houseLoseAmountTo,
            currencies: currencies,
            site: site,
            liveType: liveType,
            customerClass: customerClass,
            ip: ip,
            sortById: null,
            isSortAsc: null,
            pageIndex: 0,
            pageSize: 0
        };

        var url = 'ParlayWon/Excel?' + $.param(data);
        window.location.href = resolveClientUrl(url);
    },
    ViewBetDetail: function (refNo, transId, username, custId, bettype) {
        if (bettype == 38) {
            ParlayRMS.Core.OpenWindow(resolveClientUrl('ParlayWon/SingleMatchParlayBetDetails?custid=' + custId + '&TransId=' + transId + '&username=' + username), 700, 600);
        } else if (bettype == 376) {
            ParlayRMS.Core.OpenWindow(resolveClientUrl('ParlayWon/PenaltyShootoutParlayBetDetails?&TransId=' + transId + '&username=' + username), 700, 600);
        } else {
            ParlayRMS.Core.OpenWindow(resolveClientUrl('ParlayWon/BetDetails?refno=' + refNo + '&TransId=' + transId + '&username=' + username), 700, 600);
        }
    },
    Paging: function (totalRows, pageIndex) {
        // Number of items and limits the number of items per page
        var numberOfItems = totalRows;
        var limitPerPage = ParlayWon.cachedObject.pageSize;
        // Total pages rounded upwards
        var totalPages = Math.ceil(numberOfItems / limitPerPage);
        ParlayWon.cachedObject.maxPage = totalPages;
        // Number of buttons at the top, not counting prev/next,
        // but including the dotted buttons.
        // Must be at least 5:
        var paginationSize = 7;
        var currentPage = pageIndex;

        function showPage(whichPage) {
            if (whichPage < 1 || whichPage > totalPages) return false;
            currentPage = whichPage;
            // Replace the navigation items (not prev/next):
            $(".pagination li").slice(1, -1).remove();
            ParlayWon.GetPageList(totalPages, currentPage, paginationSize).forEach(item => {
                $("<li>").addClass("page-item")
                    .addClass(item ? "current-page" : "disabled")
                    .append(
                        $("<a>").addClass(item === currentPage ? "page-link active" : "page-link").attr({
                            href: "javascript:void(0)"
                        }).text(item || "...")
                    ).insertBefore("#next-page");
            });
            // Disable prev/next when at first/last page:
            $("#previous-page").toggleClass("disabled", currentPage === 1);
            $("#next-page").toggleClass("disabled", currentPage === totalPages);
            return true;
        }

        // Include the prev/next buttons:
        $(".pagination ul").append(
            $("<li>").addClass("page-item").attr({ id: "previous-page" }).append(
                $("<a>").addClass("page-link").attr({
                    href: "javascript:void(0)"
                }).text("Prev")
            ),
            $("<li>").addClass("page-item").attr({ id: "next-page" }).append(
                $("<a>").addClass("page-link").attr({
                    href: "javascript:void(0)"
                }).text("Next")
            )
        );
        // Show the page links
        showPage(currentPage);
    },

    // Returns an array of maxLength (or less) page numbers
    // where a 0 in the returned array denotes a gap in the series.
    // Parameters:
    //   totalPages:     total number of pages
    //   page:           current page
    //   maxLength:      maximum size of returned array
    GetPageList: function (totalPages, page, maxLength) {
        if (maxLength < 5) throw "maxLength must be at least 5";

        function range(start, end) {
            return Array.from(Array(end - start + 1), (_, i) => i + start);
        }

        var sideWidth = maxLength < 9 ? 1 : 2;
        var leftWidth = (maxLength - sideWidth * 2 - 3) >> 1;
        var rightWidth = (maxLength - sideWidth * 2 - 2) >> 1;
        if (totalPages <= maxLength) {
            // no breaks in list
            return range(1, totalPages);
        }
        if (page <= maxLength - sideWidth - 1 - rightWidth) {
            // no break on left of page
            return range(1, maxLength - sideWidth - 1)
                .concat(0, range(totalPages - sideWidth + 1, totalPages));
        }
        if (page >= totalPages - sideWidth - 1 - rightWidth) {
            // no break on right of page
            return range(1, sideWidth)
                .concat(0, range(totalPages - sideWidth - 1 - rightWidth - leftWidth, totalPages));
        }
        // Breaks on both sides
        return range(1, sideWidth)
            .concat(0, range(page - leftWidth, page + rightWidth),
                0, range(totalPages - sideWidth + 1, totalPages));
    },

    ResetCachedObject: function () {
        ParlayWon.cachedObject.fromDate = null;
        ParlayWon.cachedObject.toDate = null;
        ParlayWon.cachedObject.combos = null;
        ParlayWon.cachedObject.levels = null;
        ParlayWon.cachedObject.sportsType = null;
        ParlayWon.cachedObject.houseAmountFrom = 0;
        ParlayWon.cachedObject.houseAmountTo = 0;
        ParlayWon.cachedObject.currencies = null;
        ParlayWon.cachedObject.site = '2';
        ParlayWon.cachedObject.liveType = '3';
        ParlayWon.cachedObject.customerClass = null;
        ParlayWon.cachedObject.ip = null;
        ParlayWon.cachedObject.sortById = null;
        ParlayWon.cachedObject.isSortAsc = null;
        ParlayWon.cachedObject.pageIndex = 1;
        ParlayWon.cachedObject.pageSize = 100;
    },

    IsNumber: function (event) {
        var code = event.keyCode;
        if (code >= 48 && code <= 57) {
            return true;
        }

        return false;
    }
}

$(document).ready(function () {
    ParlayWon.Init();
});