MemberWinLoss = {
    CachedObject: {
        FromDate: null,
        ToDate: null,
        ParlayTypes: null,
        SportsType: null,
        LiveType: null,
        Combos: null,
        Username: null,
        PageIndex: null,
        IsCached: null,
        SortById: null,
        IsSortAsc: null
    },

    CachedDOM: {
        username: null,
        parlayType: null,
        combo: null,
        fromDate: null,
        toDate: null,
        btnSearch: null,
        divResult: null,
        lbUserExtra: null,
        filterControl: null,
        liveType: null
    },
    GetCachedDOM: function () {
        MemberWinLoss.CachedDOM.username = $("#username");
        MemberWinLoss.CachedDOM.parlayType = $("#parlayType");
        MemberWinLoss.CachedDOM.combo = $("#combo");
        MemberWinLoss.CachedDOM.sportsType = $("#sportsType");
        MemberWinLoss.CachedDOM.fromDate = $("#fromDate");
        MemberWinLoss.CachedDOM.toDate = $("#toDate");
        MemberWinLoss.CachedDOM.btnSearch = $("#btnSearch");
        MemberWinLoss.CachedDOM.divResult = $("#divResult");
        MemberWinLoss.CachedDOM.lbUserExtra = $("#lbUserExtra");
        MemberWinLoss.CachedDOM.filterControl = $(".filter-control");
        MemberWinLoss.CachedDOM.liveType = $("#ddlLiveType");
    },

    filterWidth: 0,

    Init: function () {
        var nowDate = moment(dateRanges.NowDate).format('MMMM DD, YYYY');
        var minDate = moment(dateRanges.LimitDataFromDate).format('MMMM DD, YYYY');

        MemberWinLoss.GetCachedDOM();

        MemberWinLoss.CachedDOM.parlayType.multiselect({
            minWidth: 210,
            height: 138,
            create: function () {
                MemberWinLoss.CachedDOM.parlayType.multiselect("checkAll");
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

        MemberWinLoss.CachedDOM.combo.multiselect({
            minWidth: 135,
            height: 136,
            create: function () {
                MemberWinLoss.CachedDOM.combo.multiselect("checkAll");
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

        MemberWinLoss.CachedDOM.sportsType.multiselect({
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

        MemberWinLoss.CachedDOM.liveType.multiselect({
            minWidth: 100,
            height: 110,
            header: false,
            multiple: false,
            selectedList: 1
        });

        MemberWinLoss.CachedDOM.fromDate.datepicker({
            dateFormat: "MM dd, yy",
            showButtonPanel: true,
            minDate: minDate,
            maxDate: nowDate,
            changeMonth: true,
            changeYear: true,
            onSelect: function (dateText, inst) {
                if (dateText != inst.lastVal) {
                    $("#dateRange option[value='7']").attr('selected', 1);
                    $("#dateRange").multiselect("refresh");
                }
                MemberWinLoss.CachedDOM.fromDate.change();
            },
            onClose: function (selectedDate) {
                MemberWinLoss.CachedDOM.toDate.datepicker("option", "minDate", selectedDate);
            }
        });

        MemberWinLoss.CachedDOM.toDate.datepicker({
            dateFormat: "MM dd, yy",
            showButtonPanel: true,
            minDate: minDate,
            maxDate: nowDate,
            changeMonth: true,
            changeYear: true,
            onSelect: function (dateText, inst) {
                if (dateText != inst.lastVal) {
                    $("#dateRange option[value='7']").attr('selected', 1);
                    $("#dateRange").multiselect("refresh");
                }
                MemberWinLoss.CachedDOM.toDate.change();
            },
            onClose: function (selectedDate) {
                MemberWinLoss.CachedDOM.fromDate.datepicker("option", "maxDate", selectedDate);
            }
        });

        MemberWinLoss.CachedDOM.username.bind("blur", function () {
            var username = MemberWinLoss.CachedDOM.username.val().trim();
            MemberWinLoss.CachedDOM.username.val(username);
        });

        MemberWinLoss.CachedDOM.username.bind("keyup", function (event) {
            if (event.keyCode == 13) {
                MemberWinLoss.CachedDOM.username.blur();
                MemberWinLoss.CachedDOM.btnSearch.click();
            }
        });

        //var nowDate = moment(dateRanges.NowDate);
        MemberWinLoss.CachedDOM.fromDate.val(nowDate);
        MemberWinLoss.CachedDOM.toDate.val(nowDate);

        MemberWinLoss.CachedDOM.btnSearch.click(function () {
            MemberWinLoss.CachedDOM.divResult.html('');
            MemberWinLoss.CachedObject = {};
            MemberWinLoss.GetData(1, false);
        });

        $(document).on("click", '#header .sortable', function () {
            var currentObject = this;
            var currentColIndex = parseInt($(currentObject).attr("data-col-index"));

            if (currentColIndex == MemberWinLoss.CachedObject.SortById) {
                if (MemberWinLoss.CachedObject.IsSortAsc) {
                    MemberWinLoss.CachedObject.IsSortAsc = false;
                    $(this).removeClass("headerSortUp").addClass("headerSortDown");
                } else {
                    MemberWinLoss.CachedObject.IsSortAsc = true;
                    $(this).removeClass("headerSortDown").addClass("headerSortUp");
                }
            } else {
                if (currentObject.sortDirection == 'asc') {
                    MemberWinLoss.CachedObject.IsSortAsc = true;
                    $(this).removeClass("headerSortDown").addClass("headerSortUp");
                } else {
                    MemberWinLoss.CachedObject.IsSortAsc = false;
                    $(this).removeClass("headerSortUp").addClass("headerSortDown");
                }

                $('#header').find(".sortable").each(function () {
                    if ($(currentObject).index() != $(this).index()) {
                        $(this).removeClass("headerSortDown").removeClass("headerSortUp");
                        this.sortDirection = "";
                    }
                });
            }

            if (!currentColIndex) {
                currentColIndex = $(currentObject).index();
            }

            MemberWinLoss.CachedObject.SortById = parseInt(currentColIndex);
            MemberWinLoss.Data(MemberWinLoss.CachedObject.ParlayTypes,
                MemberWinLoss.CachedObject.FromDate,
                MemberWinLoss.CachedObject.ToDate,
                MemberWinLoss.CachedObject.SportsType,
                MemberWinLoss.CachedObject.LiveType,
                MemberWinLoss.CachedObject.Combos,
                MemberWinLoss.CachedObject.Username,
                1,
                true,
                MemberWinLoss.CachedObject.SortById,
                MemberWinLoss.CachedObject.IsSortAsc);
        });

        MemberWinLoss.filterWidth = MemberWinLoss.CachedDOM.filterControl.width();
    },
    Data: function (parlayTypes, fromDate, toDate, sportsType, liveType, combos, username, pageIndex, isCached, sortById, isSortAsc) {
        var data = {
            FromDate: fromDate,
            ToDate: toDate,
            ParlayTypes: parlayTypes,
            SportsType: sportsType,
            LiveType: liveType,
            Combos: combos,
            Username: username,
            PageIndex: pageIndex,
            IsCached: isCached,
            SortById: sortById,
            IsSortAsc: isSortAsc
        }

        MemberWinLoss.CachedObject = data;

        $.ajax({
            url: resolveClientUrl('MemberWinLoss/Data'),
            type: 'POST',
            data: data,
            dataType: 'html',
            cache: false,
            beforeSend: function (xhr) {
                loading.show();
            },
            success: function (result) {
                MemberWinLoss.CachedDOM.divResult.html(result);
                $('#header').find(".sortable").each(function () {
                    if ($(this).attr("data-col-index") == MemberWinLoss.CachedObject.SortById) {
                        if (MemberWinLoss.CachedObject.IsSortAsc) {
                            $(this).removeClass("headerSortDown").addClass("headerSortUp");
                        } else {
                            $(this).removeClass("headerSortUp").addClass("headerSortDown");
                        }
                    }
                });
                MemberWinLoss.DisplayInfoExtra()
            },
            error: function (xhr, status, err) {
                alertBox.showError(xhr);
            },
            complete: function () {
                loading.hide();
            }
        });
    },
    ExportExcel: function (data) {
        var url = 'MemberWinLoss/Excel?' + $.param(data);
        window.location.href = resolveClientUrl(url);
    },
    GetData: function (pageIndex, isCached) {
        var username = MemberWinLoss.CachedDOM.username.val();
        var fromDate = MemberWinLoss.CachedDOM.fromDate.val();
        var toDate = MemberWinLoss.CachedDOM.toDate.val();

        if (validator.validateRequired("#username")) {
            return;
        }

        if (validator.validateDate("#fromDate")) {
            return;
        }

        if (validator.validateDate("#toDate")) {
            return;
        }

        if (validator.validateDateRange("#fromDate", "#toDate")) {
            return;
        }

        var parlayTypes = MemberWinLoss.CachedDOM.parlayType.multiselect("getChecked").map(function () {
            return this.value;
        }).get().join(',');
        if (parlayTypes == '') {
            Validators.show(MemberWinLoss.CachedDOM.parlayType.multiselect("getButton").get(0), 'Please select your options');
            return;
        }

        var combos = MemberWinLoss.CachedDOM.combo.multiselect("getChecked").map(function () {
            return this.value;
        }).get().join(',');
        if (combos == '') {
            Validators.show(MemberWinLoss.CachedDOM.combo.multiselect("getButton").get(0), 'Please select your options');
            return;
        }

        var sportsTypes = MemberWinLoss.CachedDOM.sportsType.multiselect("getChecked").map(function () {
            return this.value;
        }).get().join(',');
        if (sportsTypes == '') {
            Validators.show(MemberWinLoss.CachedDOM.sportsType.multiselect("getButton").get(0), 'Please select your options');
            return;
        }

        var liveType = parseInt(MemberWinLoss.CachedDOM.liveType.val());

        MemberWinLoss.Data(parlayTypes, fromDate, toDate, sportsTypes, liveType, combos, username, pageIndex, isCached, MemberWinLoss.CachedObject.SortById, MemberWinLoss.CachedObject.IsSortAsc);
    },
    DisplayInfoExtra: function () {
        var html = '(Site: <b>' + $("#inpSiteGroup").val() + '</b>, CC: <b>' + $("#inpCustomerClass").val() + '</b>)';
        MemberWinLoss.CachedDOM.lbUserExtra.html(html);
        var filterCustomerClass = MemberWinLoss.CachedDOM.lbUserExtra.width();
        MemberWinLoss.CachedDOM.filterControl.width(MemberWinLoss.filterWidth + filterCustomerClass);
    },
    ViewDetails: function (month) {
        var table = $('#result');
        var found = table.find("tr[monthid='" + month + "']");
        if (found.length != 0) {
            table.find("tr[monthid='" + month + "']").remove();
        }
        else {
            table.find('tr[monthid]').remove();

            var row = $("#tr_" + month);
            row.after('<tr class="details" monthid="' + month + '"><td colspan="11" class="center"></td></tr>');
            var newRow = row.next();
            var td = newRow.find('td');
            var imgURL = resolveClientUrl('Content/Images/Content/icon_wait.gif');
            var waitingImg = $('<img src="' + imgURL + '" />');
            td.html(waitingImg);

            var data = {
                Month: month
            }

            $.ajax({
                url: resolveClientUrl('MemberWinLoss/DataDetails'),
                type: 'POST',
                data: data,
                dataType: 'html',
                cache: false,
                beforeSend: function (xhr) {
                },
                success: function (result) {
                    result = result.replace(/\s+/g, ' ');
                    td.html(result);
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
}
function MovePage(pageIndex) {
    MemberWinLoss.Data(MemberWinLoss.CachedObject.ParlayTypes,
        MemberWinLoss.CachedObject.FromDate,
        MemberWinLoss.CachedObject.ToDate,
        MemberWinLoss.CachedObject.SportsType,
        MemberWinLoss.CachedObject.LiveType,
        MemberWinLoss.CachedObject.Combos,
        MemberWinLoss.CachedObject.Username,
        pageIndex, true,
        MemberWinLoss.CachedObject.SortById,
        MemberWinLoss.CachedObject.IsSortAsc);
};

String.prototype.trim = function () {
    return this.replace(/^\s+|\s+$/g, '');
};

$(document).ready(function () {
    MemberWinLoss.Init();
});