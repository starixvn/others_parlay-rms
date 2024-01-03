var refreshTime = 60000;
var isPressSubmit = false;
var agentResultSelector = '#divAgentResult';
var memberResultSelector = '#divMemberResult';
var breadcrumbSelector = '#divBreadcrumb';
var cachedAgentId = null;
var cachedAgent = null;
var cachedFilter = {};
function Alert() {
    soundManager.play('aSound');
}

function UserSettings() {
    this.combos = '2,3,4,5,6,7,8,9,10';
    this.parlaysTypes = '1,2';
    this.sportsTypes = '1,2,3';
    this.levels = '0,1,2,3,4,5,6,7,8,9,10';
    this.selectTop = 200000;
    this.winningAmount = 0;
    this.isCornerParlay = false;
    this.filterBy = 0;
    this.customerClass = null;
    this.company = '';
    this.username = '';
    this.isDiff = function (settings) {
        if (this.combos == settings.combos
            && this.parlaysTypes == settings.parlaysTypes
            && this.sportsTypes == settings.sportsTypes
            && this.levels == settings.levels
            && this.selectTop == settings.selectTop
            && this.winningAmount == settings.winningAmount
            && this.reportType == settings.reportType
            && this.isCornerParlay == setting.isCornerParlay
            && this.filterBy == settings.filterBy
            && this.customerClass == settings.customerClass
            && this.company == settings.company
            && this.username == settings.username) {
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

            var parlaysTypes = $("#parlaysType").multiselect("getChecked").map(function () {
                return this.value;
            }).get().join(',');
            if (parlaysTypes == '') {
                Validators.show($('#parlaysType').multiselect("getButton").get(0), 'Please select your options');
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

            var company = $("#ddlCompany").multiselect("getChecked").map(function () {
                return this.value;
            }).get().join(',');
            if (company == '') {
                Validators.show($('#ddlCompany').multiselect("getButton").get(0), 'Please select your options');
                return null;
            }

            var selectTop = parseInt($("#selectTop").val());

            var isCornerParlay = $("#cornerParlay").is(":checked");

            var filterBy = $('input[name=radFilterBy]:checked').val();

            var customerClass = filterBy == '0' ? parseInt($("#customerClass").val()) : null;

            var username = $("#smaMember").val().trim();

            var userSettings = new UserSettings();
            userSettings.combos = combos;
            userSettings.parlaysTypes = parlaysTypes;
            userSettings.sportsTypes = sportsTypes;
            userSettings.levels = levels;
            userSettings.winningAmount = winningAmount;
            userSettings.selectTop = selectTop;
            userSettings.isCornerParlay = isCornerParlay;
            userSettings.filterBy = filterBy;
            userSettings.customerClass = customerClass;
            userSettings.company = company;
            userSettings.username = username;

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
            parlayTypes: settings.parlaysTypes,
            sportsTypes: settings.sportsTypes,
            levels: settings.levels,
            winningAmount: settings.winningAmount,
            selectTop: settings.selectTop,
            isCornerParlay: settings.isCornerParlay,
            filterBy: settings.filterBy,
            customerClass: settings.customerClass,
            company: settings.company,
            agentId: cachedAgentId,
            username: settings.username
        };

        $.ajax({
            url: resolveClientUrl('PotentialWinning/GetData'),
            type: 'POST',
            data: data,
            dataType: 'html',
            cache: false,
            beforeSend: function (xhr) {
                $(memberResultSelector).hide();
                $(agentResultSelector).hide();
                $(breadcrumbSelector).hide();
                loading.show();
            },
            success: function (result) {
                var json = $.parseJSON(result);
                if (json.Code != 0) {
                    alertBox.showErrorMessage(json.message);
                    return;
                }

                FillData(settings.filterBy, json);
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
    $("#winningAmount").autoNumeric({
        aSep: ',',
        aDec: '.',
        vMin: '0.00',
        vMax: '999999999999',
        aPad: false,
        aForm: false
    });

    $("#winningAmount").bind('keyup', function (event) {
        if (event.keyCode == 13) {
            $("#submit").click();
        }
    });

    $("#combo").multiselect({
        buttonWidth: 132,
        menuHeight: 176,
        create: function () {
            $("#combo").multiselect();
        },
        selectedText: function (numChecked, numTotal, checkedItems) {
            if (numChecked == 0) {
                return "Select options";
            }
            else if (numChecked == 1) {
                return checkedItems[0].labels[0].innerText;
            }
            else if (numChecked == numTotal) {
                return "All";
            }
            else {
                return numChecked + ' options';
            }
        }
    });

    $("#parlaysType").multiselect({
        buttonWidth: 210,
        menuHeight: 300,
        create: function () {
            $("#parlaysType").multiselect("checkAll");
        },
        selectedText: function (numChecked, numTotal, checkedItems) {
            if (numChecked == 0) {
                return "Select options";
            }
            else if (numChecked == 1) {
                return checkedItems[0].labels[0].innerText;
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
        buttonWidth: 205,
        menuHeight: 300,
        selectedList: 1,
        groupsCollapsable: true,
        create: function () {
            $("#sportsType").multiselect("checkAll");
        },
        selectedText: function (numChecked, numTotal, checkedItems) {
            if (numChecked == 0) {
                return "Select options";
            }
            else if (numChecked == 1) {
                return checkedItems[0].labels[0].innerText;
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
        buttonWidth: 132,
        create: function () {
            $("#level").multiselect("checkAll");
        },
        selectedText: function (numChecked, numTotal, checkedItems) {
            if (numChecked == 0) {
                return "Select options";
            }
            else if (numChecked == 1) {
                return checkedItems[0].labels[0].innerText;
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
        buttonWidth: 80,
        menuHeight: 145,
        header: false,
        multiple: false,
        selectedList: 1
    });

    $("#ddlCompany").multiselect({
        menuHeight: 300,
        buttonWidth: 180,
        groupsCollapsable: true,
        selectedList: 1,
        create: function () {
            $("#ddlCompany").multiselect("checkAll");
        },
        selectedText: function (numChecked, numTotal, checkedItems) {
            if (numChecked == 0) {
                return "Select options";
            }
            else if (numChecked == 1) {
                return checkedItems[0].labels[0].innerText;
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

    getCompanies();

    $("#refreshTime").multiselect({
        buttonWidth: 60,
        menuHeight: 120,
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
            GetData();
            cachedFilter = userSettings;
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

    $('input[name=radFilterBy]').change(function () {
        $("#customerClass").prop('disabled', $(this).val() == '1');
        var username = $(this).val() == '1' ? 'SMA' : 'SMA/Member';
        $("#username").text(username);
    });

    InitTableSorter($(memberResultSelector + ' #result'), $(memberResultSelector + ' #header'));
    InitTableSorter($(agentResultSelector + ' #result'), $(agentResultSelector + ' #header'));

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

function FillData(filterBy, data) {
    var table = "";
    if (filterBy == 0) {
        table = memberResultSelector + " #result";
        $(memberResultSelector).show();
        if (cachedAgent) {
            $(breadcrumbSelector).html('<b>(<span style="color: #489ab2; cursor: pointer;" onclick="ViewData(null, null, 1);">Agent</span> >> ' + cachedAgent + ')</b>');
            $(breadcrumbSelector).show();
        }
    } else {
        table = agentResultSelector + " #result";
        $(agentResultSelector).show();
        $(breadcrumbSelector).show();
        $(breadcrumbSelector).html('<b>(Agent)</b>');
    }

    var records = data.Records;
    var isSound = false;
    var length = records.length;
    var oldUserName = $(table).children('tbody').children('tr');
    var viewTotalBetId = $(table).find("tr.details").attr('custid');
    var tbody = $(table).children('tbody');

    $(table).children('tbody').html('');

    if (length > 0) {
        var r = new Array();
        var j = -1;
        for (var i = 0; i < length; i++) {
            var item = records[i];
            var isHighLight = true;

            oldUserName.each(function () {
                if ($(this).children('td').eq(3).text() == item.UserName) {
                    isHighLight = false;
                    $(this).remove();
                }
            });

            if (isHighLight) {
                isSound = true;
            }
            r[++j] = filterBy == 0 ? CreateRowForMember(i + 1, item, (isHighLight && !isPressSubmit)) : CreateRowForAgent(i + 1, item);
        }

        if (isSound) {
            Alert();
        }
        tbody.html(r.join(''));

        //Order by current ordered column
        var orderCol = tableSorter.currentOrderCol;
        if (orderCol) {
            var type = $(orderCol).attr("data-col-type");
            tableSorter.sortColumn(table, $(orderCol).index(), orderCol.sortDirection, type);
        }
    } else {
        if (tbody.children('tr:first-child').children('td.noinfo').length == 0) {
            var noinfo = CreateNoInfoRow(10);
            tbody.append(noinfo);
        }
    }

    isPressSubmit = false;
    cachedAgent = null;
    cachedAgentId = null;
    $('#divResult').show();
}

function CreateRowForMember(order, item, isHighLight) {
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
    r[++j] = '<td class="center">' + item.SiteGroup + '</td>';
    r[++j] = CreateLinkLevelCell(item.UserName, item.Level, "ViewTotalOutstanding('" + item.UserName + "')");
    r[++j] = '<td>' + item.CustomerClass + '</td>';
    r[++j] = '<td>' + item.Currency + '</td>';
    r[++j] = '<td class="numeric">' + formatOdds(item.Odds) + '</td>';
    r[++j] = '<td class="numeric">' + formatOdds(item.Stake) + '</td>';
    r[++j] = '<td class="numeric">' + numeral(Math.floor(item.WinningAmount)).format('0,0') + '</td>';
    r[++j] = '<td class="numeric">' + numeral(Math.floor(item.CompanyLoseAmount)).format('0,0') + '</td>';
    if (cachedAgentId == null) {
        r[++j] = '<td class="center"><button class="button-disable-2" style="min-width:40px;" onclick="ViewTotalBetDetails(\'' + item.CustId + '\',' + cachedAgentId + ');">' + item.TotalTrackingBets + '/' + item.TotalTicket + '</td>';
    } else {
        r[++j] = '<td class="center"><button class="button-disable-2" style="min-width:40px;" onclick="ViewTotalBetDetails(\'' + item.CustId + '\',' + cachedAgentId + ');">' + item.TotalTicket + '</td>';
    }
    r[++j] = '</tr>';
    return r.join('');
}

function CreateRowForAgent(order, item) {
    var r = new Array();
    var j = -1;
    r[++j] = '<tr id="tr_' + item.Recommend + '">';
    r[++j] = '<td class="center">' + order + '</td>';
    r[++j] = '<td>' + item.AgentName + '</td>';
    r[++j] = '<td class="numeric">' + numeral(Math.floor(item.Stake)).format('0,0') + '</td>';
    r[++j] = '<td class="numeric">' + numeral(Math.floor(item.WinningAmount)).format('0,0') + '</td>';
    r[++j] = '<td class="right"><a href="#" onclick="ViewData(' + item.Recommend + ',\'' + item.AgentName + '\'' + ', 0);" >' + item.TotalMember + '</a></td>';
    r[++j] = '</tr>';
    return r.join('');
}

function ViewTotalOutstanding(username) {
    var combos = $("#combo").multiselect("getChecked").map(function () {
        return this.value;
    }).get().join(',');
    var parlaysTypes = $("#parlaysType").multiselect("getChecked").map(function () {
        return this.value;
    }).get().join(',');
    var sportsTypes = $("#sportsType").multiselect("getChecked").map(function () {
        return this.value;
    }).get().join(',');
    var url = resolveClientUrl('TotalOutstanding?username=') + username + '&combo=' + combos + '&sportsType=' + sportsTypes + '&parlaysType=' + parlaysTypes;
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

function ViewTotalBetDetails(custId, agentid) {
    var table = $('#result');
    var found = table.find("tr[custid='" + custId + "']");
    if (found.length != 0) {
        table.find("tr[custid='" + custId + "']").remove();
    }
    else {
        table.find('tr[custid]').remove();

        var row = $("#tr_" + custId);
        row.after('<tr class="details" custid="' + custId + '"><td colspan="11" class="center"></td></tr>');
        var newRow = row.next();
        var td = newRow.find('td');
        var imgURL = resolveClientUrl('Content/Images/Content/icon_wait.gif');
        var waitingImg = $('<img src="' + imgURL + '" />');
        td.html(waitingImg);

        var userSettings = settingsController.get();
        var username = row.children('td').eq(3).text();
        var data = {
            Combos: userSettings.combos,
            parlayTypes: userSettings.parlaysTypes,
            SportsTypes: userSettings.sportsTypes,
            CustId: custId,
            UserName: username,
            IsCornerParlay: userSettings.isCornerParlay,
            AgentId: agentid
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
                checkAll();
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
    };

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
    };
}

function ViewData(agentId, agent, filterBy) {
    cachedAgentId = agentId;
    cachedAgent = agent;
    cachedFilter.filterBy = filterBy;
    settingsController.set(cachedFilter);
    GetData();
}

function GetData() {
    $('#result').children('tbody').html('');
    $("#refreshNow").button('enable');
    $("#pause").prop('disabled', false);
    $("#header").find(".sortable").each(function () {
        $(this).removeClass("headerSortDown").removeClass("headerSortUp");
        this.sortDirection = "";
    });
    tableSorter.currentOrderCol = null;
    isPressSubmit = true;
    timerController.start();
}

function InitTableSorter(table, header) {
    tableSorter.bind2(header, table, {
        type: 'text',
        onStart: function () {
            table.find('tr[custid]').remove();
        },
        onFinish: function () {
            var trs = table.children('tbody').children("tr");
            trs.each(function (index, tr) {
                $(tr).children('td:first').text(index + 1);
            });
        }
    });
}

function isNumber(event) {
    var code = event.keyCode;
    if (code >= 48 && code <= 57) {
        return true;
    }

    return false;
}

function updateCheckedUser(element, userid, username, transid, isAll) {
    if (isAll) {
        confirmWithFullEvent('Confirm', 'Are you sure you reviewed all tickets?', function () {
            var data = {
                TransId: getTransIds(),
                Username: username,
                UserId: userid
            }

            $.ajax({
                url: resolveClientUrl('PotentialWinning/CheckBy'),
                type: 'POST',
                data: data,
                dataType: 'html',
                cache: false,
                beforeSend: function () {
                    loading.show();
                },
                success: function (result) {
                    var json = $.parseJSON(result);
                    switch (json.StatusId) {
                        case 0:
                            disabledandCheckedCheckBox("#cbTrackingUser");
                            reUpdateCheckedUser(username, false);
                            break;
                        case 1:
                            disabledandCheckedCheckBox("#cbTrackingUser");
                            reUpdateCheckedUser(json.Message, true);
                            break;
                        case 2:
                            $("#cbTrackingUser").prop("checked", false);
                            alertBox.showErrorMessage(json.Message);
                            break;
                    }
                },
                error: function (xhr, status, err) {
                    alertBox.showError(xhr.responseText);
                },
                complete: function () {
                    loading.hide();
                }
            });
        }, function () {
            $("#cbTrackingUser").prop("checked", false);
        });
    } else {
        var data = {
            TransId: transid,
            Username: username,
            UserId: userid
        }

        $.ajax({
            url: resolveClientUrl('PotentialWinning/CheckBy'),
            type: 'POST',
            data: data,
            dataType: 'html',
            cache: false,
            beforeSend: function () {
                loading.show();
            },
            success: function (result) {
                var json = $.parseJSON(result);
                switch (json.StatusId) {
                    case 0:
                        disabledandCheckedCheckBox(element);
                        $(element + "-text").text(username);
                        checkAll();
                        break;
                    case 1:
                        alertBox.show("Information", "This ticket has been already checked by another user");
                        disabledandCheckedCheckBox(element);
                        var object = json.Message.split(',')[0];
                        $(element + "-text").text(object.split(':')[1]);
                        checkAll();
                        break;
                    case 2:
                        $(element).prop("checked", false);
                        alertBox.showErrorMessage(json.Message);
                        break;
                }
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

function checkAll() {
    var isCheckedAll = true;
    $(".cbTrackingUser-item").each(function (inx, element) {
        if (!element.checked) {
            isCheckedAll = false;
            return false;
        }
    });

    if (isCheckedAll) {
        $("#cbTrackingUser").prop("checked", true);
        $("#cbTrackingUser").prop("disabled", true);
    }
}

function getTransIds() {
    var transids = '';
    $(".cbTrackingUser-item").each(function (inx, element) {
        if (!element.disabled) {
            transids += element.getAttribute("data-transid") + ',';
        }
    });
    return transids;
}

function reUpdateCheckedUser(object, isUserList) {
    if (isUserList) {
        var list = object.split(',');
        $.each(list, function (inx, val) {
            var userInfo = val.split(':');
            $(".cbTrackingUser-item").each(function (inx, element) {
                if (!element.disabled) {
                    var transid = element.getAttribute("data-transid");
                    if (transid === userInfo[0]) {
                        element.disabled = true;
                        element.checked = true;
                        var labelId = "#" + element.getAttribute("id") + "-text";
                        $(labelId).text(userInfo[1]);
                    }
                }
            });
        });
    } else {
        $(".cbTrackingUser-item").each(function (inx, element) {
            if (!element.disabled) {
                element.disabled = true;
                element.checked = true;
                var labelId = "#" + element.getAttribute("id") + "-text";
                $(labelId).text(object);
            }
        });
    }
}

function disabledandCheckedCheckBox(element) {
    $(element).prop("checked", true);
    $(element).prop("disabled", true);
}

function getCompanies() {

    $.ajax({
        url: resolveClientUrl('PotentialWinning/Company'),
        type: 'POST',
        dataType: 'html',
        cache: false,
        beforeSend: function () {
            loading.show();
        },
        success: function (result) {
            var json = $.parseJSON(result);
            loadCompanyControl(json);
        },
        error: function (xhr, status, err) {
            alertBox.showError(xhr.responseText);
        },
        complete: function () {
            loading.hide();
        }
    });
}

function loadCompanyControl(data) {
    var html = '';
    $.each(data, function (key, obj) {
        if (obj.GroupId == 0) {
            html += '<optgroup label="MBC Group" selected="selected">';
        }
        else {
            html += '<optgroup label="Licensee Group" selected="selected">';
        }
        $.each(obj.Items, function (inx, site) {
            html += '<option value="' + site + '" aria-selected="true">' + site + '</option>';
        })
        html += '</optgroup>';
    });

    $('#ddlCompany').html(html);
    $('#ddlCompany').multiselect('refresh');
    $("#ddlCompany").multiselect("checkAll");
}