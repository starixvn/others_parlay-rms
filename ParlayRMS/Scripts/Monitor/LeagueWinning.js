var refreshTime = 60000;
var isPressSubmit = false;
var cachedFilter = {};
var fixedMaxPayoutGroup = '99'
function Alert() {
    soundManager.play('aSound');
}

function UserSettings() {
    this.combos = '2,3,4,5,6,7,8,9,10';
    this.parlayTypes = '1,2';
    this.sportTypes = '1,-3,2';
    this.levels = '0,1,2,3,4,5,6,7,8,9,10';
    this.selectTop = 200;
    this.winningAmount = 30000;
    this.reportBy = '1';
    this.group = 12;
    this.isDiff = function (settings) {
        if (this.combos == settings.combos
            && this.parlayTypes == settings.parlayTypes
            && this.sportTypes == settings.sportTypes
            && this.levels == settings.levels
            && this.selectTop == settings.selectTop
            && this.winningAmount == settings.winningAmount
            && this.reportBy == settings.reportBy
            && this.group == settings.group) {
            return false;
        }

        return true;
    }
}

var uiController = function () {
    return {
        isPause: function () {
            return $('#chkPause').is(':checked');
        }
    }
}();

var settingsController = function () {
    var current = new UserSettings();

    return {
        set: function (settings) {
            current = settings;
        },
        get: function () {
            return current;
        },
        validate: function () {
            var combos = $("#ddlCombo").multiselect("getChecked").map(function () {
                return this.value;
            }).get().join(',');
            if (combos == '') {
                Validators.show($("#ddlCombo").multiselect("getButton").get(0), 'Please select your options');
                return null;
            }

            var parlayTypes = $("#ddlParlayType").multiselect("getChecked").map(function () {
                return this.value;
            }).get().join(',');
            if (parlayTypes == '') {
                Validators.show($('#ddlParlayType').multiselect("getButton").get(0), 'Please select your options');
                return null;
            }

            var levels = $("#ddlLevel").multiselect("getChecked").map(function () {
                return this.value;
            }).get().join(',');
            if (levels == '') {
                Validators.show($('#ddlLevel').multiselect("getButton").get(0), 'Please select your options');
                return null;
            }

            var winningAmount = $.trim($('#txtWinningAmount').val().replace(/,/g, ''));
            if (winningAmount == '') {
                winningAmount = '0';
            }

            if (!isNumeric(winningAmount) || parseInt(winningAmount) < 0) {
                Validators.show($('#txtWinningAmount').get(0), 'This value must be greater or than 0');
                return null;
            }

            var selectTop = parseInt($("#ddlSelectTop").val());

            var group = parseInt($("#ddlGroup").val());

            var sportTypesElement = group == fixedMaxPayoutGroup ? $("#someSportDiv #sportsType") : $("#fullSportDiv #sportsType")
            var sportTypes = sportTypesElement.multiselect("getChecked").map(function () {
                return this.value;
            }).get().join(',');
            if (sportTypes == '') {
                Validators.show(sportTypesElement.multiselect("getButton").get(0), 'Please select your options');
                return null;
            }

            var reportBy = $('input[name=radReportBy]:checked').val();

            var userSettings = new UserSettings();
            userSettings.combos = combos;
            userSettings.parlayTypes = parlayTypes;
            userSettings.sportTypes = sportTypes;
            userSettings.levels = levels;
            userSettings.winningAmount = winningAmount;
            userSettings.selectTop = selectTop;
            userSettings.group = group;
            userSettings.reportBy = reportBy;

            return userSettings;
        }
    }
}();

var timerController = function () {
    var timeout = null;
    var ticker = 0;

    var clearTimeout = function () {
        if (timeout) {
            window.clearTimeout(timeout);
            timeout = null;
        }
    }

    var load = function () {
        var settings = settingsController.get();
        var data = {
            combos: settings.combos,
            parlayTypes: settings.parlayTypes,
            sportsTypes: settings.sportTypes,
            levels: settings.levels,
            winningAmount: $("#txtWinningAmount").prop("disabled") ? "" : settings.winningAmount,
            selectTop: settings.selectTop,
            group: settings.group,
            reportBy: settings.reportBy,
        };

        $.ajax({
            url: resolveClientUrl('LeagueWinningMonitor/Data'),
            type: 'POST',
            data: data,
            dataType: 'html',
            cache: false,
            beforeSend: function (xhr) {
                loading.show();
            },
            success: function (result) {
                $("#divResult").html(result);
                if (result.search('No information is available') < 0) {
                    Alert();
                }
                tableSorter.bind2($('#header'), $('#result'), {
                    onStart: function () {
                        $('#result').find('tr[dataid]').remove();
                    },
                    onFinish: function () {
                        var trs = $('#result').children('tbody').children("tr");
                        trs.each(function (index, tr) {
                            $(tr).children('td:first').text(index + 1);
                        });
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
    }

    var runTicker = function () {
        if (ticker > 0) {
            timeout = setTimeout(function () {
                ticker--;
                $("#timer").html("[" + ticker + "]");
                if (ticker == 0) {
                    load();
                    ticker = refreshTime / 1000;
                }

                runTicker();
            }, 1000);
        }
    }

    return {
        start: function () {
            clearTimeout();
            load();
            if (!uiController.isPause()) {
                ticker = refreshTime / 1000;
                runTicker();
            }
        },
        pause: function () {
            clearTimeout();
        },
        resume: function () {
            clearTimeout();
            if (!uiController.isPause()) {
                runTicker();
            }
        }
    }
}();

$(document).ready(function () {
    $("#fullSportDiv").hide();
    $("#someSportDiv").show();

    $("#txtWinningAmount").autoNumeric({
        aSep: ',',
        aDec: '.',
        vMin: '0.00',
        vMax: '999999999999',
        aPad: false,
        aForm: false
    });

    $("#ddlCombo").multiselect({
        minWidth: 132,
        height: 136,
        create: function () {
            $("#ddlCombo").multiselect();
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

    $("#ddlParlayType").multiselect({
        minWidth: 150,
        height: 60,
        create: function () {
            $("#ddlParlayType").multiselect("checkAll");
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

    $("#fullSportDiv #sportsType").multiselect({
        minWidth: 190,
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

    $("#someSportDiv #sportsType").multiselect({
        minWidth: 190,
        height: 88,
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

    $("#ddlGroup").multiselect({
        minWidth: 132,
        height: 136,
        header: false,
        multiple: false,
        selectedList: 1,
    });

    $("#ddlLevel").multiselect({
        minWidth: 132,
        create: function () {
            $("#ddlLevel").multiselect("checkAll");
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

    $("#ddlSelectTop").multiselect({
        minWidth: 80,
        height: 105,
        header: false,
        multiple: false,
        selectedList: 1
    });

    $("#ddlRefreshTime").multiselect({
        minWidth: 60,
        height: 80,
        header: false,
        multiple: false,
        selectedList: 1,

        close: function () {
            var newRefreshTime = Math.max($("#ddlRefreshTime").val(), 30000);
            if (refreshTime != newRefreshTime) {
                refreshTime = newRefreshTime;
                $("#refreshNow").click();
            }
        }
    });

    $("#refreshNow").button();
    $("#refreshNow").button('disable');
    $("#chkPause").prop('disabled', true);

    $("#ddlGroup").change(function () {
        $("#fullSportDiv").hide();
        $("#someSportDiv").hide();
        var isFixedMaxPayoutGroup = $(this).val() == fixedMaxPayoutGroup;
        $("#txtWinningAmount").prop('disabled', !isFixedMaxPayoutGroup);
        if (isFixedMaxPayoutGroup) {
            $("#someSportDiv").show();
        } else {
            $("#fullSportDiv").show();
        }
    });

    $("#refreshNow").click(function () {
        var userSettings = settingsController.validate();
        if (userSettings != null) {
            settingsController.set(userSettings);
            timerController.start();
        }
    });

    $("#submit").click(function () {
        var userSettings = settingsController.validate();
        if (userSettings != null) {
            settingsController.set(userSettings);
            GetData();
            cachedFilter = userSettings;
        }
    });

    $("#chkPause").click(function () {
        if ($(this).is(':checked')) {
            timerController.pause();
        }
        else {
            timerController.resume();
        }
    });

    soundManager.setup({
        url: resolveClientUrl('Scripts/Libs/soundManager/swf/'),
        flashVersion: 9, // optional: shiny features (default = 8)// optional: ignore Flash where possible, use 100% HTML5 mode
        // preferFlash: false,
        debugMode: false,
        onready: function () {
            var mySound = soundManager.createSound({
                id: 'aSound',
                url: resolveClientUrl("Sounds/alert.mp3")
            });
        },
        ontimeout: function () {
            closeBox.render($('#noflash'),
                {
                    title: 'Alert Sound is disabled'
                });
        }
    });
});

function GetData() {
    $('#result').children('tbody').html('');
    $("#refreshNow").button('enable');
    $("#chkPause").prop('disabled', false);
    $("#header").find(".sortable").each(function () {
        $(this).removeClass("headerSortDown").removeClass("headerSortUp");
        this.sortDirection = "";
    });
    tableSorter.currentOrderCol = null;
    isPressSubmit = true;
    timerController.start();
}

function ViewTotalBetDetails(dataId) {
    var table = $('#result');
    var found = table.find("tr[dataId='" + dataId + "']");
    if (found.length != 0) {
        table.find("tr[dataId='" + dataId + "']").remove();
    }
    else {
        table.find('tr[dataId]').remove();

        var row = $("#tr_" + dataId);
        row.after('<tr class="details" dataId="' + dataId + '"><td colspan="9" class="center"></td></tr>');
        var newRow = row.next();
        var td = newRow.find('td');
        var imgURL = resolveClientUrl('Content/Images/Content/icon_wait.gif');
        var waitingImg = $('<img src="' + imgURL + '" />');
        td.html(waitingImg);

        var settings = settingsController.get();
        var data = {
            combos: settings.combos,
            parlayTypes: settings.parlayTypes,
            sportsTypes: settings.sportTypes,
            levels: settings.levels,
            winningAmount: $("#txtWinningAmount").prop("disabled") ? "" : settings.winningAmount,
            selectTop: settings.selectTop,
            group: settings.group,
            reportBy: settings.reportBy,
            custId: settings.reportBy == '2' ? dataId : '0',
            leagueId: settings.reportBy == '1' ? dataId : '0',
        };

        $.ajax({
            url: resolveClientUrl('LeagueWinningMonitor/ViewDetailList'),
            type: 'POST',
            data: data,
            dataType: 'html',
            cache: false,
            beforeSend: function (xhr) {
                loading.show();
            },
            success: function (result) {
                result = result.replace(/\s+/g, ' ');
                td.html(result);
                tableSorter.bind2($('#detailHeader'), $('#detailResult'), {});
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

function ViewBetDetails(username, transId, refNo, betType, custId) {
    if (betType === 38) {
        ViewSingleMatchParlayDetails(transId, custId, username);
    } else if (betType === 376) {
        ViewPenaltyShootoutParlayDetails(transId, username);
    }
    else {
        ViewSystemParlayDetails(username, transId, refNo);
    }

    function ViewSystemParlayDetails(username, transId, refNo) {
        var data = {
            TransId: transId,
            RefNo: refNo,
            Username: username
        }

        $.ajax({
            url: resolveClientUrl('BetList/BetDetails'),
            type: 'POST',
            data: data,
            dataType: 'html',
            cache: false,
            beforeSend: function () {
                loading.show();
            },
            success: function (result) {
                modalPopup(result);
                $(".betdetails").css('overflow-y', 'auto');
                $("#simplemodal-container").width($("#simplemodal-container").width() + 20);
            },
            error: function (xhr, status, err) {
                alertBox.showError(xhr.responseText);
            },
            complete: function () {
                loading.hide();
            }
        });
    };

    function ViewSingleMatchParlayDetails(transId, custId, username) {
        var data = {
            TransId: transId,
            CustId: custId,
            Username: username
        }

        $.ajax({
            url: resolveClientUrl('BetList/SingleMatchParlayDetailPopup'),
            type: 'POST',
            data: data,
            dataType: 'html',
            cache: false,
            beforeSend: function () {
                loading.show();
            },
            success: function (result) {
                modalPopup(result);
                $(".betdetails").css('overflow-y', 'auto');
                $("#simplemodal-container").width($("#simplemodal-container").width() + 20);
            },
            error: function (xhr, status, err) {
                alertBox.showError(xhr.responseText);
            },
            complete: function () {
                loading.hide();
            }
        });
    }

    function ViewPenaltyShootoutParlayDetails(transId, username) {
        var data = {
            TransId: transId,
            Username: username
        }

        $.ajax({
            url: resolveClientUrl('BetList/PenaltyShootoutParlayDetailPopup'),
            type: 'POST',
            data: data,
            dataType: 'html',
            cache: false,
            beforeSend: function () {
                loading.show();
            },
            success: function (result) {
                modalPopup(result);
                $(".betdetails").css('overflow-y', 'auto');
                $("#simplemodal-container").width($("#simplemodal-container").width() + 20);
            },
            error: function (xhr, status, err) {
                alertBox.showError(xhr.responseText);
            },
            complete: function () {
                loading.hide();
            }
        });
    }
}