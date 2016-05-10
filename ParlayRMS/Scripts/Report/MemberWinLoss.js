MemberWinLoss = {
    CachedDOM: {
        username: null,
        parlayType: null,
        combo: null,
        fromDate: null,
        toDate: null,
        btnSearch: null,
        divResult: null
    },
    GetCachedDOM: function () {
        MemberWinLoss.CachedDOM.username = $("#username");
        MemberWinLoss.CachedDOM.parlayType = $("#parlayType");
        MemberWinLoss.CachedDOM.combo = $("#combo");
        MemberWinLoss.CachedDOM.fromDate = $("#fromDate");
        MemberWinLoss.CachedDOM.toDate = $("#toDate");
        MemberWinLoss.CachedDOM.btnSearch = $("#btnSearch");
        MemberWinLoss.CachedDOM.divResult = $("#divResult");
    },
    Init: function () {
        var nowDate = moment(dateRanges.NowDate).format('MMMM DD, YYYY');
        var minDate = moment(dateRanges.LimitDataFromDate).format('MMMM DD, YYYY');

        MemberWinLoss.GetCachedDOM();

        MemberWinLoss.CachedDOM.parlayType.multiselect({
            minWidth: 132,
            height: 82,
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
            MemberWinLoss.GetData(1, false);
        });
    },
    Data: function (parlayTypes, fromDate, toDate, sportsType, combos, username, pageIndex, isCached) {
        var data = {
            FromDate: fromDate,
            ToDate: toDate,
            ParlayTypes: parlayTypes,
            SportsType: sportsType,
            Combos: combos,
            Username: username,
            PageIndex: pageIndex,
            IsCached: isCached
        }

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
        var sportsType = $('input[name=SportsTypes]:checked', '#divSportsTypes').val();

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

        var combos = MemberWinLoss.CachedDOM.combo.multiselect("getChecked").map(function () {
            return this.value;
        }).get().join(',');
        if (combos == '') {
            Validators.show(MemberWinLoss.CachedDOM.combo.multiselect("getButton").get(0), 'Please select your options');
            return;
        }

        var parlayTypes = MemberWinLoss.CachedDOM.parlayType.multiselect("getChecked").map(function () {
            return this.value;
        }).get().join(',');
        if (parlayTypes == '') {
            Validators.show(MemberWinLoss.CachedDOM.parlayType.multiselect("getButton").get(0), 'Please select your options');
            return;
        }

        MemberWinLoss.Data(parlayTypes, fromDate, toDate, sportsType, combos, username, pageIndex, isCached);
    }
}
function MovePage(pageIndex) {
    MemberWinLoss.GetData(pageIndex, true);
};

String.prototype.trim = function () {
    return this.replace(/^\s+|\s+$/g, ''); 
};

$(document).ready(function () {
    MemberWinLoss.Init();
});