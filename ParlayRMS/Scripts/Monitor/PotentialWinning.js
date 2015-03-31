var refreshTime = 60000;
var isPressSubmit = false;
function Alert() {    
    soundManager.play('aSound');
}

function UserSettings() {
    this.combos = '2,3,4,5,6';
    this.sportsTypes = '1,2,3';
    this.levels = '0,1,2,3,4,5,6,7,8,9,10';
    this.selectTop = 200;
    this.winningAmount = 0;
    this.isDiff = function (settings) {
        if (this.combos == settings.combos
            && this.sportsTypes == settings.sportsTypes
            && this.levels == settings.levels
            && this.selectTop == settings.selectTop
            && this.winningAmount == settings.winningAmount
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
} ();

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

            var winningAmount = $.trim($('#winningAmount').val().replace(/,/g, ''));
            if (winningAmount == '') {
                winningAmount = '0';
            }

            if (!isNumeric(winningAmount) || parseInt(winningAmount) < 0) {
                Validators.show($('#winningAmount').get(0), 'This value must be greater or than 0');
                return null;
            }

            var selectTop = parseInt($("#selectTop").val());

            var userSettings = new UserSettings();
            userSettings.combos = combos;
            userSettings.sportsTypes = sportsTypes;
            userSettings.levels = levels;
            userSettings.winningAmount = winningAmount;
            userSettings.selectTop = selectTop;

            return userSettings;
        }
    }
} ();

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
            sportsTypes: settings.sportsTypes,
            levels: settings.levels,
            winningAmount: settings.winningAmount,
            selectTop: settings.selectTop
        };

        $.ajax({
            url: resolveClientUrl('PotentialWinning/GetData'),
            type: 'POST',
            data: data,
            dataType: 'html',
            cache: false,
            beforeSend: function (xhr) {
                loading.show();
            },
            success: function (result) {
                var json = $.parseJSON(result);
                if (json.Code != 0) {
                    alertBox.showErrorMessage(json.message);
                    return;
                }

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
} ();

$(document).ready(function () {
    $("#winningAmount").bind('keyup', function (event) {
        FormatNumber(event, true);
        var winningAmount = parseInt($.trim($(this).val().replace(/,/g, '')));

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

    $("#sportsType").multiselect({
        minWidth: 150,
        height: 82,
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

    $("#refreshTime").multiselect({
        minWidth: 60,
        height: 80,
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

            isPressSubmit = true;
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
    var oldUserName = $(table).children('tbody').children('tr');
    var tbody = $(table).children('tbody');    
    
    $(table).children('tbody').html('');

    if (length > 0) {
        Alert();
        var r = new Array();
        var j = -1;
        for (var i = 0; i < length; i++) {
            var item = records[i];
            var isHighLight = true;

            oldUserName.each(function () {
                if ($(this).children('td').eq(2).text() == item.UserName) {
                    isHighLight = false;
                }
            });

            r[++j] = CreateRow(i + 1, item, (isHighLight && !isPressSubmit));        
        }
        
        tbody.html(r.join(''));        

    } else {

        if (tbody.children('tr:first-child').children('td.noinfo').length == 0) {
            var noinfo = CreateNoInfoRow(10);
            tbody.append(noinfo);
        }
    }

    isPressSubmit = false;
    $('#divResult').show();
}

function CreateRow(order, item, isHighLight) {
    var r = new Array();
    var j = -1;
    if (isHighLight) {
        r[++j] = '<tr class="highlight" id="tr_' + item.CustId + '">';
    }
    else {
        r[++j] = '<tr id="tr_' + item.CustId + '">';
    }
        
    r[++j] = '<td class="center">' + order + '</td>';
    r[++j] = '<td>' + item.TransDateString + '</td>';
    r[++j] = CreateLinkLevelCell(item.UserName, item.Level, "ViewTotalOutstanding('" + item.UserName + "')");
    r[++j] = '<td class="numeric">' + numeral(item.Odds).format('0,0.00') + '</td>';
    r[++j] = '<td class="numeric">' + numeral(item.Stake).format('0,0.00') + '</td>';
    r[++j] = '<td class="numeric">' + numeral(item.WinningAmount).format('0,0.00') + '</td>';
    r[++j] = '<td class="numeric">' + numeral(item.CompanyLoseAmount).format('0,0.00') + '</td>';
    r[++j] = '<td class="center"><button class="button-disable-2" style="width:40px;" onclick="ViewTotalBetDetails(\'' + item.CustId + '\'' + ');">' + item.TotalTicket + '</td>';
    r[++j] = '</tr>';
    return r.join('');
}

function ViewTotalOutstanding(username) {
    var combos = $("#combo").multiselect("getChecked").map(function () {
        return this.value;
    }).get().join(',');
    var sportsTypes = $("#sportsType").multiselect("getChecked").map(function () {
        return this.value;
    }).get().join(',');
    var url = resolveClientUrl('TotalOutstanding?username=') + username + '&combo=' + combos + '&sportsType=' + sportsTypes;
    window.open(url, '_blank');
}

function OpenViewIpInfo(url) {
    if (hasIpPermission) {
        window.open(url, "_blank");
    }
    else {
        alertBox.show("Report Access Denied", "You need Admin permission to view this report");
    }
}

function ViewTotalBetDetails(custId) {
    var table = $('#result');
    var found = table.find("tr[custid='" + custId + "']");
    if (found.length != 0) {
        table.find("tr[custid='" + custId + "']").remove();
    }
    else {
        table.find('tr[custid]').remove();

        var row = $("#tr_" + custId);
        row.after('<tr class="details" custid="' + custId + '"><td colspan="8" class="center"></td></tr>');
        var newRow = row.next();
        var td = newRow.find('td');
        var waitingImg = $('<img src="../Images/Content/icon_wait.gif")" />');
        td.html(waitingImg);

        var userSettings = settingsController.validate();
        var userName = row.children('td').eq(2).text();
        var data = {
            Combos: userSettings.combos,
            SportsTypes: userSettings.sportsTypes,
            CustId: custId,
            UserName: userName
        }

        $.ajax({
            url: resolveClientUrl('PotentialWinning/ViewDetailList'),
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

function ViewBetDetails(userName, transId, refNo) {
    var data = {
        BetId: transId,
        RefNo: refNo,
        Username: userName
    }

    $.ajax({
        url: resolveClientUrl('SystemParlayMonitor/SystemParlayBetDetails'),
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