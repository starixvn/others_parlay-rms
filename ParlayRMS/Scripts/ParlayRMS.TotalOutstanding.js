if (typeof ParlayRMS == 'undefined') {
    ParlayRMS = {};
};

ParlayRMS.TotalOutstanding = {
    Init: function () {
        $("#combo").multiselect({
            minWidth: 132,
            height: 136,
            create: function () {
                $("#combo").multiselect();
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
            minWidth: 150,
            height: 82,
            create: function () {
                $("#sportsType").multiselect();
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

        var now = moment();
        var date = $.trim($("#date").val());
        if (date == "") {
            $("#date").val(now.format('MMMM DD, YYYY'));
        }
        $("#date").datepicker({
            dateFormat: "MM dd, yy",
            showButtonPanel: true,
            minDate: '-2M',
            maxDate: '0'
        });

        $('#submit').click(this.LoadData);

        var username = $.trim($('#username').val());
        if (username != '') {
            this.LoadData();
        }
    },
    ClearDate: function () {
        $('#result').html("");
    },
    LoadData: function () {
        var date = $.trim($('#date').val());
        if (username == '') {
            Validators.show($('#date').get(0), 'Please input a date');
            return null;
        }

        var combos = $("#combo").multiselect("getChecked").map(function () {
            return this.value;
        }).get().join(',');
        if (combos == '') {
            Validators.show($("#combo").multiselect("getButton").get(0), 'Please select your options');
            return null;
        }

        var sportsTypes = $("#sportsType").multiselect("getChecked").map(function () {
            return this.value;
        }).get().join(',');
        if (sportsTypes == '') {
            Validators.show($('#sportsType').multiselect("getButton").get(0), 'Please select your options');
            return null;
        }

        var username = $.trim($('#username').val());
        if (username == '') {
            Validators.show($('#username').get(0), 'Please input an username');
            return null;
        }

        var data = {
            Date: date,
            Combo: combos,
            SportsType: sportsTypes,
            Username: username,
            SearchBy: $("input[type='radio'][name='searchby']:checked").val()
        }
        $.ajax({
            url: resolveClientUrl('TotalOutstanding/Data'),
            type: 'POST',
            data: data,
            dataType: 'html',
            cache: false,
            beforeSend: function (xhr) {
                loading.show();
            },
            success: function (result) {
                $('#result').show();
                $('#result').html(result);
                loading.hide();
            },
            error: function (xhr, status, err) {
                alertBox.showError(xhr);
                loading.hide();
            }
        });
    },
    ViewBetDetails: function (username, transId, refNo, originalRefNo) {
        var data = {
            BetId: transId,
            Username: username,
            RefNo: refNo,
            OriginalRefNo: originalRefNo
        }

        $.ajax({
            url: resolveClientUrl('SimilarBets/BetDetails'),
            type: 'POST',
            data: data,
            dataType: 'html',
            cache: false,
            beforeSend: function () {
                loading.show();
            },
            success: function (data) {
                popup.show(data);
            },
            error: function (xhr, status, err) {
                loading.hide();
                alertBox.showError(xhr);
            },
            complete: function () {
                loading.hide();
            }
        });
    },
    ViewSimilarBets: function (transId) {
        var url = resolveClientUrl('SimilarBets?betid=') + transId;
        window.open(url, '_blank');
    },
    ViewMember: function (username) {
        var date = $.trim($('#date').val());
        var combos = $("#combo").multiselect("getChecked").map(function () {
            return this.value;
        }).get().join(',');
        var sportsTypes = $("#sportsType").multiselect("getChecked").map(function () {
            return this.value;
        }).get().join(',');
        var url = resolveClientUrl('TotalOutstanding?username=') + username + '&date=' + date + '&combo=' + combos + '&sportsType=' + sportsTypes;
        window.open(url, '_blank');
    }
};

$(document).ready(function () {
    ParlayRMS.TotalOutstanding.Init();
});