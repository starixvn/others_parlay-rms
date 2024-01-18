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
        IsSortAsc: null,
        ReportBy: null
    },

    CachedFilteredObject: {
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
        IsSortAsc: null,
        ReportBy: null
    },

    CachedDOM: {
        username: null,
        parlayType: null,
        combo: null,
        fromDate: null,
        toDate: null,
        btnSearch: null,
        divResult: null,
        divReportByDate: null,
        lbUserExtra: null,
        filterControl: null,
        liveType: null
    },
    GetCachedDOM: function () {
        MemberWinLoss.CachedDOM.username = $("#usernameOfMember");
        MemberWinLoss.CachedDOM.parlayType = $("#parlayType");
        MemberWinLoss.CachedDOM.combo = $("#combo");
        MemberWinLoss.CachedDOM.sportsType = $("#sportsType");
        MemberWinLoss.CachedDOM.fromDate = $("#fromDate");
        MemberWinLoss.CachedDOM.toDate = $("#toDate");
        MemberWinLoss.CachedDOM.btnSearch = $("#btnSearch");
        MemberWinLoss.CachedDOM.divResult = $("#divResult");
        MemberWinLoss.CachedDOM.divReportByDate = $("#divReportByDate");
        MemberWinLoss.CachedDOM.lbUserExtra = $("#lbUserExtra");
        MemberWinLoss.CachedDOM.filterControl = $(".filter-control");
        MemberWinLoss.CachedDOM.liveType = $("#ddlLiveType");
    },

    filterWidth: 0,
    clickedMonth: '',

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

        $(document).on("click", '#divResult #header .sortable', function () {
            var currentObject = this;
            var currentColIndex = parseInt($(currentObject).attr("data-col-index"));

            if (currentColIndex == MemberWinLoss.CachedFilteredObject.SortById) {
                if (MemberWinLoss.CachedFilteredObject.IsSortAsc) {
                    MemberWinLoss.CachedFilteredObject.IsSortAsc = false;
                    $(this).removeClass("headerSortUp").addClass("headerSortDown");
                } else {
                    MemberWinLoss.CachedFilteredObject.IsSortAsc = true;
                    $(this).removeClass("headerSortDown").addClass("headerSortUp");
                }
            } else {
                if (currentObject.sortDirection == 'asc') {
                    MemberWinLoss.CachedFilteredObject.IsSortAsc = true;
                    $(this).removeClass("headerSortDown").addClass("headerSortUp");
                } else {
                    MemberWinLoss.CachedFilteredObject.IsSortAsc = false;
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

            MemberWinLoss.CachedFilteredObject.SortById = parseInt(currentColIndex);
            MemberWinLoss.Data(MemberWinLoss.CachedFilteredObject.ParlayTypes,
                MemberWinLoss.CachedFilteredObject.FromDate,
                MemberWinLoss.CachedFilteredObject.ToDate,
                MemberWinLoss.CachedFilteredObject.SportsType,
                MemberWinLoss.CachedFilteredObject.LiveType,
                MemberWinLoss.CachedFilteredObject.Combos,
                MemberWinLoss.CachedFilteredObject.Username,
                1,
                true,
                MemberWinLoss.CachedFilteredObject.SortById,
                MemberWinLoss.CachedFilteredObject.IsSortAsc);
        });

        $(document).on("click", '#divReportByDate #header .sortable', function () {
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
            MemberWinLoss.ShowReportByDate(1, MemberWinLoss.clickedMonth, true);
        });

        $(document).on("click", '#exportMemberWinLoss-ByDate', function () {
            var data = {
                FromDate: MemberWinLoss.CachedObject.FromDate,
                ToDate: MemberWinLoss.CachedObject.ToDate,
                ParlayTypes: MemberWinLoss.CachedObject.ParlayTypes,
                SportsType: MemberWinLoss.CachedObject.SportsType,
                Combos: MemberWinLoss.CachedObject.Combos,
                LiveType: MemberWinLoss.CachedObject.LiveType,
                Username: MemberWinLoss.CachedObject.Username,
                ReportBy: MemberWinLoss.CachedObject.ReportBy,
                IsCached: false
            }
            MemberWinLoss.ExportExcel(data);
        });

        $(document).on("click", '#exportMemberWinLoss', function () {
            var data = {
                FromDate: MemberWinLoss.CachedFilteredObject.FromDate,
                ToDate: MemberWinLoss.CachedFilteredObject.ToDate,
                ParlayTypes: MemberWinLoss.CachedFilteredObject.ParlayTypes,
                SportsType: MemberWinLoss.CachedFilteredObject.SportsType,
                Combos: MemberWinLoss.CachedFilteredObject.Combos,
                LiveType: MemberWinLoss.CachedFilteredObject.LiveType,
                Username: MemberWinLoss.CachedFilteredObject.Username,
                ReportBy: MemberWinLoss.CachedFilteredObject.ReportBy,
                IsCached: false
            }
            MemberWinLoss.ExportExcel(data);
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
            IsSortAsc: isSortAsc,
            ReportBy: 1
        }

        MemberWinLoss.CachedFilteredObject = data;

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
                MemberWinLoss.CachedDOM.divReportByDate.hide();
                MemberWinLoss.CachedDOM.divResult.html(result);
                MemberWinLoss.CachedDOM.divResult.show();
                $('#divResult #header').find(".sortable").each(function () {
                    if ($(this).attr("data-col-index") == MemberWinLoss.CachedFilteredObject.SortById) {
                        if (MemberWinLoss.CachedFilteredObject.IsSortAsc) {
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

        if (validator.validateRequired("#usernameOfMember")) {
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

        MemberWinLoss.Data(parlayTypes, fromDate, toDate, sportsTypes, liveType, combos, username, pageIndex, isCached, null, null);
    },
    DisplayInfoExtra: function () {
        var html = $("#inpIsAgent").val() ? '(CC: <b>' + $("#inpCustomerClass").val() + '</b>)' :
            '(Site: <b>' + $("#inpSiteGroup").val() + '</b>, CC: <b>' + $("#inpCustomerClass").val() + '</b>)';
        MemberWinLoss.CachedDOM.lbUserExtra.html(html);
        var filterCustomerClass = MemberWinLoss.CachedDOM.lbUserExtra.width();
        MemberWinLoss.CachedDOM.filterControl.width(MemberWinLoss.filterWidth + filterCustomerClass);
    },
    ViewDetails: function (index, month, param, isMember, username, date) {
        var table = isReportByDate ? $("#divReportByDate #result") : $('#divResult #result');
        var found = table.find("tr[info='" + index + "']");
        if (found.length != 0) {
            table.find("tr[info='" + index + "']").remove();
        }
        else {
            table.find('tr[info]').remove();
            var colspanNumber = isMember ? 11 : 14;
            var row = table.find("#tr_" + index);
            row.after('<tr class="details" info="' + index + '"><td colspan="' + colspanNumber +' " class="center"></td></tr>');
            var newRow = row.next();
            var td = newRow.find('td');
            var imgURL = resolveClientUrl('Content/Images/Content/icon_wait.gif');
            var waitingImg = $('<img src="' + imgURL + '" />');
            td.html(waitingImg);
            var fromDate = '';
            var toDate = '';
            var userNameInfo = isMember ? MemberWinLoss.CachedDOM.username.val() : username;
            if (isReportByDate) {
                fromDate = date;
                toDate = date;
            } else {
                var dateInfo = month.trim().split('-');
                var lastDateOfMonth = new Date(dateInfo[1], dateInfo[0], 0).getDate();
                var fromDateString = dateInfo[0] + '/1/' + dateInfo[1];
                var toDateString = dateInfo[0] + '/' + lastDateOfMonth + '/' + dateInfo[1];
                fromDate = new Date(fromDateString) < new Date(MemberWinLoss.CachedFilteredObject.FromDate) ? MemberWinLoss.CachedFilteredObject.FromDate : fromDateString;
                toDate = new Date(toDateString) > new Date(MemberWinLoss.CachedFilteredObject.ToDate) ? MemberWinLoss.CachedFilteredObject.ToDate : toDateString;
            }
            var data = {
                FromDate: fromDate,
                ToDate: toDate,
                ParlayTypes: param.ParlayTypes,
                SportsType: param.SportsType,
                Combos: param.Combos,
                LiveType: param.LiveType,
                Username: userNameInfo
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
    },

    ShowReportByDate: function (pageIndex, month, isCached) {
        MemberWinLoss.clickedMonth = month;
        MemberWinLoss.CachedDOM.divResult.hide();
        if (!isCached) {
            MemberWinLoss.CachedObject.SortById = null;
            MemberWinLoss.CachedObject.IsSortAsc = null;
        }
        var dateInfo = month.trim().split('-');
        var lastDateOfMonth = new Date(dateInfo[1], dateInfo[0], 0).getDate();
        var fromDate = dateInfo[0] + '/1/' + dateInfo[1];
        var toDate = dateInfo[0] + '/' + lastDateOfMonth + '/' + dateInfo[1];
        var data = {
            FromDate: fromDate,
            ToDate: toDate,
            ParlayTypes: MemberWinLoss.CachedFilteredObject.ParlayTypes,
            SportsType: MemberWinLoss.CachedFilteredObject.SportsType,
            LiveType: MemberWinLoss.CachedFilteredObject.LiveType,
            Combos: MemberWinLoss.CachedFilteredObject.Combos,
            Username: MemberWinLoss.CachedFilteredObject.Username,
            PageIndex: pageIndex,
            IsCached: isCached,
            SortById: MemberWinLoss.CachedObject.SortById,
            IsSortAsc: MemberWinLoss.CachedObject.IsSortAsc,
            ReportBy: 2
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
                MemberWinLoss.CachedDOM.divReportByDate.html(result);
                MemberWinLoss.CachedDOM.divReportByDate.show();
                $('#divReportByDate #header').find(".sortable").each(function () {
                    if ($(this).attr("data-col-index") == MemberWinLoss.CachedObject.SortById) {
                        if (MemberWinLoss.CachedObject.IsSortAsc) {
                            $(this).removeClass("headerSortDown").addClass("headerSortUp");
                        } else {
                            $(this).removeClass("headerSortUp").addClass("headerSortDown");
                        }
                    }
                });
            },
            error: function (xhr, status, err) {
                alertBox.showError(xhr);
            },
            complete: function () {
                loading.hide();
            }
        });
    },

    BackReportByMonth: function () {
        MemberWinLoss.CachedDOM.divReportByDate.hide();
        MemberWinLoss.CachedDOM.divResult.show();
        isReportByDate = false;
    }
}
function MovePage(pageIndex) {
    if (isReportByDate) {
        MemberWinLoss.ShowReportByDate(pageIndex, MemberWinLoss.clickedMonth, true)
    } else {
        MemberWinLoss.Data(MemberWinLoss.CachedFilteredObject.ParlayTypes,
            MemberWinLoss.CachedFilteredObject.FromDate,
            MemberWinLoss.CachedFilteredObject.ToDate,
            MemberWinLoss.CachedFilteredObject.SportsType,
            MemberWinLoss.CachedFilteredObject.LiveType,
            MemberWinLoss.CachedFilteredObject.Combos,
            MemberWinLoss.CachedFilteredObject.Username,
            pageIndex, true,
            MemberWinLoss.CachedFilteredObject.SortById,
            MemberWinLoss.CachedFilteredObject.IsSortAsc);
    }
    
};

String.prototype.trim = function () {
    return this.replace(/^\s+|\s+$/g, '');
};

function ViewBetDetail(refNo, transId, username, custId, bettype) {
    if (bettype == 38) {
        ParlayRMS.Core.OpenWindow(resolveClientUrl('MemberWinLoss/SingleMatchParlayBetDetails?custid=' + custId + '&TransId=' + transId + '&username=' + username), 700, 600);
    }
    else if (bettype == 376) {
        ParlayRMS.Core.OpenWindow(resolveClientUrl('MemberWinLoss/PenaltyShootoutParlayBetDetails?&TransId=' + transId + '&username=' + username), 700, 600);
    }
    else {
        ParlayRMS.Core.OpenWindow(resolveClientUrl('MemberWinLoss/BetDetails?refno=' + refNo + '&TransId=' + transId + '&username=' + username), 700, 600);
    }
}

$(document).ready(function () {
    MemberWinLoss.Init();
});