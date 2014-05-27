var refreshTime = 60000;
var maxTransId = 0;

function Alert() {
    var settings = settingsController.get();
    if (settings.stakeAfterPT != 0) {
        soundManager.play('aSound');
    }
}

function UserSettings() {
    this.combos = '2,3,4,5,6';
    this.sportsTypes = '1,2,3';
    this.levels = '0,1,2,3,4,5,6,7,8,9,10';
    this.selectTop = 20;
    this.stakeAfterPT = 0;
    this.isDiff = function (settings) {
        if (this.combos == settings.combos
            && this.sportsTypes == settings.sportsTypes
            && this.levels == settings.levels
            && this.selectTop == settings.selectTop
            && this.stakeAfterPT == settings.stakeAfterPT) {
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

            var stakeAfterPT = $.trim($('#stakeAfterPT').val().replace(/,/g, ''));
            if (stakeAfterPT == '') {
                stakeAfterPT = '0';
            }

            if (!isNumeric(stakeAfterPT) || parseInt(stakeAfterPT) < 0) {
                Validators.show($('#stakeAfterPT').get(0), 'Stake After PT must be greater or than 0');
                return null;
            }

            var selectTop = parseInt($("#selectTop").val());

            var userSettings = new UserSettings();
            userSettings.combos = combos;
            userSettings.sportsTypes = sportsTypes;
            userSettings.levels = levels;
            userSettings.stakeAfterPT = stakeAfterPT;
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
            stakeAfterPT: settings.stakeAfterPT,
            selectTop: settings.selectTop,
            maxTransId: maxTransId
        }

        $.ajax({
            url: resolveClientUrl('Monitor/MixParlayData'),
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
} ();

$(document).ready(function () {
    $("#stakeAfterPT").bind('keyup', function (event) {
        FormatNumber(event, true);
        var stakeAfterPT = parseInt($.trim($(this).val().replace(/,/g, '')));
        if (stakeAfterPT > 1000000000000) {
            $(this).val("1,000,000,000,000");
        }

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
                var noinfo = CreateNoInfoRow(10);
                tbody.append(noinfo);
            }
        }
    }

    $('#divResult').show();
}

function CreateRow(order, item) {
    var r = new Array();
    var j = -1;
    r[++j] = '<tr style="height: 22px;">';
    r[++j] = '<td class="center">' + order + '</td>';
    r[++j] = '<td>' + moment(item.TransDate).format('MM/D/YYYY h:mm:ss A') + '</td>';
    r[++j] = '<td class="numeric"><a href="#" onclick="window.open(resolveClientUrl(\'SimilarBets?betid=' + item.TransId + '\'), \'_blank\');">' + +item.TransId + '</a></td>';
    r[++j] = CreateLinkLevelCell(item.Username, item.Level, "ViewTotalOutstanding('" + item.Username + "')");
    r[++j] = '<td class="numeric">' + numeral(item.Odds).format('0,0.00') + '</td>';
    r[++j] = '<td class="numeric">' + numeral(item.Stake).format('0,0.00') + '</td>';
    r[++j] = '<td class="numeric">' + numeral(item.NetWinAfterPT).format('0,0.00') + '</td>';
    r[++j] = '<td class="center">' + numeral(item.Combo).format('0,0') + '</td>';

    if (!item.IP || item.IP === 'Null') {
        r[++j] = '<td class="right">----</td>';
    }
    else {
        r[++j] = '<td class="numeric"><a href="#" onclick="OpenViewIpInfo(resolveClientUrl(\'ViewIPInfo?ip=' + item.IP + '&date=' + item.TransDate + '\'));">' + item.IP + '</a></td>';
    }
    r[++j] = '<td class="center"><button class="button-disable-2" style="width:40px;" onclick="ViewBetDetails(\'' + item.Username + '\', ' + item.TransId + ', ' + item.RefNo + ');">View</td>';
    r[++j] = '</tr>';
    return r.join('');
}
function OpenViewIpInfo(url) {
    if (hasIpPermission) {
        window.open(url, "_blank");
    }
    else {
        alertBox.show("Error", "You need Admin permission to view this report");
    }
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

function ViewBetDetails(userName, transId, refNo) {
    var data = {
        BetId: transId,
        RefNo: refNo,
        Username: userName
    }

    $.ajax({
        url: resolveClientUrl('Monitor/MixParlayBetDetails'),
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