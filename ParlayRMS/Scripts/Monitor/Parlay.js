var refreshTime = 60000;
var maxTransId = 0;

function Alert() {
    var settings = settingsController.get();
    if ((settings.reportType === 1 && settings.stakeAfterPT != 0) || (settings.reportType === 2 && settings.netWinAfterPT != 0)) {
        soundManager.play('aSound');
    }
}

function UserSettings() {
    this.combos = '';
    this.parlayTypes = '';
    this.sportsTypes = '';
    this.levels = '';
    this.selectTop = 20;
    this.markets = '';
    this.stakeAfterPT = 0;
    this.netWinAfterPT = 0;
    this.reportType = 1;
    this.isDiff = function (settings) {
        if (this.combos == settings.combos
            && this.parlayTypes == settings.parlayTypes
            && this.sportsTypes == settings.sportsTypes
            && this.levels == settings.levels
            && this.selectTop == settings.selectTop
            && this.markets == settings.markets
            && this.stakeAfterPT == settings.stakeAfterPT
            && this.netWinAfterPT == settings.netWinAfterPT
            && this.reportType == settings.reportType) {
            return false;
        }

        return true;
    }
}

var uiController = function () {
    return {
        isPause: function () {
            return $('#pause').is(':checked');
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
            var combos = $("#combo").multiselect("getChecked").map(function () {
                return this.value;
            }).get().join(',');
            if (combos == '') {
                Validators.show($("#combo").multiselect("getButton").get(0), 'Please select your options');
                return null;
            }

            var parlayTypes = $("#parlayType").multiselect("getChecked").map(function () {
                return this.value;
            }).get().join(',');
            if (parlayTypes == '') {
                Validators.show($("#parlayType").multiselect("getButton").get(0), 'Please select your options');
                return null;
            }

            var sportsTypes = $("#sportsType").multiselect("getChecked").map(function () {
                return this.value;
            }).get().join(',');
            if (sportsTypes == '') {
                Validators.show($('#sportsType').multiselect("getButton").get(0), 'Please select your options');
                return null;
            }

            var levels = $("#level").multiselect("getChecked").map(function () {
                return this.value;
            }).get().join(',');
            if (levels == '') {
                Validators.show($('#level').multiselect("getButton").get(0), 'Please select your options');
                return null;
            }

            var markets = $("#market").multiselect("getChecked").map(function () {
                return this.value;
            }).get().join(',');
            if (markets == '') {
                Validators.show($("#market").multiselect("getButton").get(0), 'Please select your options');
                return null;
            }

            var filterAfterPT = $.trim($('#filterAfterPT').val().replace(/,/g, ''));
            if (filterAfterPT == '') {
                filterAfterPT = '0';
            }

            if (!isNumeric(filterAfterPT) || parseInt(filterAfterPT) < 0) {
                Validators.show($('#filterAfterPT').get(0), 'This value must be greater or than 0');
                return null;
            }

            var selectTop = parseInt($("#selectTop").val());

            var reportType = $('input[name=radFilterAfterPT]:checked').val() * 1;

            var userSettings = new UserSettings();
            userSettings.combos = combos;
            userSettings.parlayTypes = parlayTypes;
            userSettings.sportsTypes = sportsTypes;
            userSettings.levels = levels;
            userSettings.reportType = reportType;
            if (reportType === 1) {
                userSettings.stakeAfterPT = filterAfterPT;
                userSettings.netWinAfterPT = 0;
            }
            else {
                userSettings.stakeAfterPT = 0;
                userSettings.netWinAfterPT = filterAfterPT;
            }
            userSettings.selectTop = selectTop;
            userSettings.markets = markets;
            return userSettings;
        }
    }
}();

var timerController = function () {
    var timeout = null;
    var ticker = 0;
    var remaining = refreshTime;

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
            sportsTypes: settings.sportsTypes,
            levels: settings.levels,
            stakeAfterPT: settings.stakeAfterPT,
            netWinAfterPT: settings.netWinAfterPT,
            reportType: settings.reportType,
            selectTop: settings.selectTop,
            maxTransId: maxTransId,
            markets: settings.markets
        }

        $.ajax({
            url: resolveClientUrl('ParlayMonitor/Data'),
            type: 'POST',
            data: data,
            dataType: 'html',
            cache: false,
            beforeSend: function (xhr) {
                loading.show();
            },
            success: function (result) {
                var json = $.parseJSON(result);
                if (json.code != 0) {
                    alertBox.showErrorMessage(json.message);
                    return;
                }
                maxTransId = json.MaxTransId;
                var table = $('#result');
                FillData(table, json);
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
    $("#filterAfterPT").autoNumeric({
        aSep: ',',
        aDec: '.',
        vMin: '0.00',
        vMax: '999999999999',
        aPad: false,
        aForm: false
    });

    $("#filterAfterPT").bind('keyup', function (event) {
        if (event.keyCode == 13) {
            $("#submit").click();
        }
    });

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

    $("#parlayType").multiselect({
        minWidth: 200,
        height: 120,
        create: function () {
            $("#parlayType").multiselect();
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

    $("#level").multiselect({
        minWidth: 132,
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

    $("#selectTop").multiselect({
        minWidth: 80,
        height: 105,
        header: false,
        multiple: false,
        selectedList: 1
    });

    $("#market").multiselect({
        minWidth: 132,
        height: 100,
        noneSelectedText: "Select options",
        create: function () {
            $("#market").multiselect();
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

    $("#refreshTime").multiselect({
        minWidth: 60,
        height: 60,
        header: false,
        multiple: false,
        selectedList: 1,

        close: function () {
            var newRefreshTime = Math.max($("#refreshTime").val(), 30000);
            if (refreshTime != newRefreshTime) {
                refreshTime = newRefreshTime;
                $("#refreshNow").click();
            }
        }
    });

    $("#refreshNow").button();
    $("#refreshNow").button('disable');
    $("#pause").prop('disabled', true);

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
            $('#result').children('tbody').html('');
            $("#refreshNow").button('enable');
            $("#pause").prop('disabled', false);
            $("#header").find(".sortable").each(function () {
                $(this).removeClass("headerSortDown").removeClass("headerSortUp");
                this.sortDirection = "";
            });
            $("#transTime").addClass("headerSortDown");
            $("#transTime").get(0).sortDirection = "asc";
            tableSorter.currentOrderCol = null;
            maxTransId = 0;
            timerController.start();
        }
    });

    $("#pause").click(function () {
        if ($(this).is(':checked')) {
            timerController.pause();
        }
        else {
            timerController.resume();
        }
    });

    var table = $('#result');
    var header = $('#header');
    tableSorter.bind2(header, table, {
        type: 'text',
        onFinish: function () {
            var trs = table.children('tbody').children("tr");
            trs.each(function (index, tr) {
                $(tr).children('td:first').text(index + 1);
            });
        }
    });

    //    closeBox.render($('#noflash'),
    //    {
    //        title: 'Alert Sound is disabled'
    //    });

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

function FillData(table, data) {
    var records = data.Records;
    var length = records.length;
    var tbody = $(table).children('tbody');

    //Remove highlight rows
    tbody.children('tr.highlight').each(function () {
        $(this).removeAttr('class');
    });

    if (length > 0) {
        Alert();
        var rows = tbody.children('tr');
        if (rows.length == 0) {
            var r = new Array();
            var j = -1;
            for (var i = 0; i < length; i++) {
                var item = records[i];
                r[++j] = CreateRow(i + 1, item);
            }

            tbody.html(r.join(''));
        }
        else {
            // find to remove noinfo row
            rows.children('td.noinfo').remove();

            //Re-order by Trans. Time
            tableSorter.sortColumn(table, 1, "asc", 'date');

            for (var i = length - 1; i >= 0; i--) {
                var item = records[i];
                var row = CreateRow(i + 1, item);

                rows = tbody.children('tr');
                var settings = settingsController.get();
                if (settings.selectTop == rows.length) {
                    var last = rows.last();
                    last.remove();
                }

                row = $(row);
                tbody.prepend(row);
                //row.effect("highlight", { color: '#FFDBA8' }, 120000);
                row.addClass('highlight');
            }

            //Order by current ordered column
            var orderCol = tableSorter.currentOrderCol;
            if (orderCol) {
                var type = $(orderCol).attr("data-col-type");
                tableSorter.sortColumn(table, $(orderCol).index(), orderCol.sortDirection, type);
            }

            //Update row no.
            var trs = tbody.children('tr');
            trs.each(function (index, tr) {
                $(tr).children('td:first').text(index + 1);
            });
        }
    } else {
        if (maxTransId == 0) {
            if (tbody.children('tr:first-child').children('td.noinfo').length == 0) {
                var noinfo = CreateNoInfoRow(12);
                tbody.append(noinfo);
            }
        }
    }

    $('#divResult').show();
}

function CreateRow(order, item) {
    var betSuffix = '';
    if (item.BetType === 38) {
        betSuffix = ' (S)';
    }

    if (item.BetType === 376) {
        betSuffix = ' (P)';
    }

    var r = new Array();
    var j = -1;
    r[++j] = '<tr style="height: 22px;">';
    r[++j] = '<td class="center">' + order + '</td>';
    r[++j] = '<td>' + item.TransDateString + '</td>';
    if (item.IsBetRadar) {
        r[++j] = '<td class="numeric betRadar" title="Betradar"><a href="#" onclick="window.open(resolveClientUrl(\'SimilarBets?betid=' + item.TransId + '\'), \'_blank\');">' + +item.TransId + '</a></td>';
    } else {
        r[++j] = '<td class="numeric"><a href="#" onclick="window.open(resolveClientUrl(\'SimilarBets?betid=' + item.TransId + '\'), \'_blank\');">' + +item.TransId + '</a></td>';
    }
    r[++j] = CreateLinkLevelCell(item.Username, item.Level, "ViewTotalOutstanding('" + item.Username + "')");
    r[++j] = '<td class="numeric">' + formatOdds(item.Odds) + '</td>';
    r[++j] = '<td class="numeric">' + formatOdds(item.Stake) + '</td>';
    r[++j] = '<td class="numeric">' + numeral(Math.floor(item.NetWinAfterPT)).format('0,0') + '</td>';
    r[++j] = '<td class="center">' + numeral(item.TotalBet).format('0,0') + ' ' + (item.TotalBet === 1 ? 'bet' : 'bets') + betSuffix + '</td>';
    r[++j] = '<td class="center">' + (item.NumberWon === 0 || item.BetType === 38 ? item.TotalSelection : item.NumberWon + '/' + item.TotalSelection) + '</td>';
    r[++j] = '<td class="center">' + item.SportTypeName + '</td>';

    if (!item.IP || item.IP === 'Null' || item.IP === 'NULL') {
        r[++j] = '<td class="right">----</td>';
    }
    else {
        r[++j] = '<td class="numeric"><a href="#" onclick="OpenViewIpInfo(resolveClientUrl(\'ViewIPInfo?ip=' + item.IP + '&date=' + item.TransDateString + '\'));">' + item.IP + '</a></td>';
    }
    r[++j] = '<td class="center"><button class="button-disable-2" style="width:40px;" onclick="ViewBetDetails(\'' + item.Username + '\', ' + item.TransId + ', ' + +item.RefNo + ', ' + item.BetType + ', ' + item.CustId + ');">View</td>';
    r[++j] = '</tr>';
    return r.join('');
}
function OpenViewIpInfo(url) {
    if (hasIpPermission) {
        window.open(url, "_blank");
    }
    else {
        alertBox.show("Report Access Denied", "You need Admin permission to view this report");
    }
}

function ViewTotalOutstanding(username) {
    var combos = $("#combo").multiselect("getChecked").map(function () {
        return this.value;
    }).get().join(',');
    var parlaysTypes = $("#parlayType").multiselect("getChecked").map(function () {
        return this.value;
    }).get().join(',');
    var sportsTypes = $("#sportsType").multiselect("getChecked").map(function () {
        return this.value;
    }).get().join(',');
    var url = resolveClientUrl('TotalOutstanding?username=') + username + '&combo=' + combos + '&sportsType=' + sportsTypes + '&parlayTypes=' + parlaysTypes;
    window.open(url, '_blank');
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
}


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