$(function () {
    "use strict";
    $("#tabs").tabs({
        activate: function (event, ui) {
            //alert('active');
        },
        beforeActivate: function (event, ui) {
            Validators.hide();
        }
    });
    $("#tabs").removeClass("ui-widget");
    //config
    // URL of IP checking website, open in a new tab when user click to IP
    var ipViewSiteURL = 'http://www.iplocationfinder.com/';
    var queryData;
    //Now Date
    var nowDate = moment(dateOption.NowDate).format('MMMM DD, YYYY');
    var minDate = moment(dateOption.MinDate).format('MMMM DD, YYYY');
    var inputDate;
    var today = moment(dateOption.NowDate).format('MMMM DD, YYYY');
    if (dateOption.InputDate) {
        inputDate = moment(dateOption.InputDate).format('MMMM DD, YYYY');
    }
    else {
        inputDate = today;
    }

    //add css class
    $("#btnSearchIpInfo").addClass("button");
    $("#btnSearchIPStats").addClass("button");

    //setup datepicker
    $("#date-ipstats").datepicker({
        dateFormat: "MM dd, yy",
        showButtonPanel: true,
        changeMonth: true,
        changeYear: true,
        maxDate: nowDate,
        minDate: minDate,
        onSelect: function (dateText, inst) {
            if (dateText !== inst.lastVal) {
                $("#date-ipstats option[value='7']").attr('selected', 1);
            }
        },
        onClose: function (selectedDate) {
            $("#date-ipstats").datepicker("option", null, selectedDate);
        }
    });

    $("#date-ipinfo").datepicker({
        dateFormat: "MM dd, yy",
        showButtonPanel: true,
        changeMonth: true,
        changeYear: true,
        maxDate: nowDate,
        minDate: minDate,
        onSelect: function (dateText, inst) {
            if (dateText !== inst.lastVal) {
                $("#date-ipinfo option[value='7']").attr('selected', 1);
            }
        },
        onClose: function (selectedDate) {
            $("#date-ipinfo").datepicker("option", null, selectedDate);
        }
    });

    $("#ticketStatus-ipstats").multiselect({
        buttonWidth: 132,
        menuHeight: 250,
        selectedList: 1,
        create: function () {
            $("#ticketStatus-ipstats").multiselect();
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

    $("#ticketStatus-ipinfo").multiselect({
        buttonWidth: 132,
        menuHeight: 250,
        selectedList: 1,
        create: function () {
            $("#ticketStatus-ipinfo").multiselect();
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

    //set datepicker default value.
    $("#date-ipstats").val(today);
    $("#date-ipinfo").val(inputDate);

    //set event for enter-key press
    $("#date-ipinfo").keypress(function (e) {
        switch (e.keyCode) {
            case 13: // enter
                $("#btnSearchIpInfo").click();
                break;
        }
    });
    $("#ip").keypress(function (e) {
        switch (e.keyCode) {
            case 13: // enter
                $("#btnSearchIpInfo").click();
                break;
        }
    });
    if (dateOption.InputDate) {
        OpenViewIpInfoTab();
    }
    else {
        OpenIpStatsTab();
    }
    //view ip link
    $('#ipViewLink').click(function () {
        var ip = $.trim($('#ip').val().replace(/,/g, ''));
        if (!ValidateIPAddress(ip)) {
            Validators.show($('#ip').get(0), 'This IP is not valid');
            return false;
        }
        window.open(ipViewSiteURL + ip);
    });

    //event for enter-key press
    $('#btnSearchIpInfo').click(function () {
        LoadMainReportIpInfo();
    });
    $('#btnSearchIPStats').click(function () {
        LoadReportIpStats(1, true);
    });

    $(document).on('click', '.rowSoccer', function () {
        var id = this.getAttribute('data-id');
        var name = this.getAttribute('data-name');
        var type = 1;
        OpenDrillDown(id, name, type);
    });
    $(document).on('click', '.rowOthers', function () {
        var id = this.getAttribute('data-id');
        var name = this.getAttribute('data-name');
        var type = 2;
        OpenDrillDown(id, name, type);
    });
    $(document).on('click', '.link-ip-stats', function () {
        var ip = this.getAttribute('data-ip');
        var date = $("#date-ipstats").val();
        OpenViewIpInfoTab(ip, date);
    });

    InitTableSorter($('#result-ip-info'), $('#header'), 0);
});

function OpenViewIpInfoTab(ip, date) {
    $("#tabs").tabs("option", "active", 1);
    if (date) {
        $("#date-ipinfo").val(date);
    }
    if (ip) {
        $("#ip").val(ip);
    }
    var statuses = $('#ticketStatus-ipstats').val();
    if (statuses.length > 0) {
        $('#ticketStatus-ipinfo').val(statuses).multiselect('refresh');
    }
    LoadMainReportIpInfo();
}

function OpenIpStatsTab() {
    $("#tabs").tabs("option", "active", 0);
    LoadReportIpStats(1, true);
}

function LoadMainReportIpInfo() {
    //validate input
    var ip = $.trim($('#ip').val().replace(/,/g, ''));
    if (ip === '') {
        Validators.show($('#ip').get(0), 'This field is required');
        return false;
    }
    if (!ValidateIPAddress(ip)) {
        Validators.show($('#ip').get(0), 'This IP is not valid');
        return false;
    }

    var date = $("#date-ipinfo").val();
    if (date === '') {
        Validators.show($('#date-ipinfo').get(0), 'This field is required');
        return false;
    }

    var ticketStatus = $("#ticketStatus-ipinfo").multiselect("getChecked").map(function () {
        return this.value;
    }).get().join(',');
    if (ticketStatus == '') {
        Validators.show($("#ticketStatus-ipinfo").multiselect("getButton").get(0), 'Please select your options');
        return false;
    }

    queryData = {
        ip: ip,
        date: date,
        ticketStatus: ticketStatus.split(',').length == 6 ? '-1' : ticketStatus
    };

    $.ajax({
        url: resolveClientUrl('ViewIPInfo/MainReportData'),
        type: 'POST',
        data: queryData,
        dataType: 'html',
        cache: false,
        beforeSend: function (xhr) {
            loading.show();
        },
        success: function (result) {
            var json = $.parseJSON(result);
            if (json.code !== 0) {
                alertBox.showErrorMessage(json.message);
                return;
            }
            //maxTransId = json.MaxTransId;
            var table = $('#result-ip-info');
            FillData(table, json);
            
        },
        error: function (xhr, status, err) {
            alertBox.showError(xhr);
        },
        complete: function () {
            loading.hide();
        }
    });
};

function LoadReportIpStats(pageIndex, isNewSubmit) {
    //validate input
    var fromDate = $("#date-ipstats").val();
    if (fromDate === '') {
        Validators.show($('#date-ipstats').get(0), 'This field is required');
        return false;
    }
    var ticketStatus = $("#ticketStatus-ipstats").multiselect("getChecked").map(function () {
        return this.value;
    }).get().join(',');
    if (ticketStatus == '') {
        Validators.show($("#ticketStatus-ipstats").multiselect("getButton").get(0), 'Please select your options');
        return false;
    }
    queryData = {
        FromDate: fromDate,
        PageIndex: pageIndex,
        PageSize: 100,
        TotalItems: 0,
        MaxTransId: 0,
        TicketStatus: ticketStatus.split(',').length == 6 ? '-1' : ticketStatus
    };

    if (typeof pageData !== "undefined" && !isNewSubmit) {
        if (pageData.TotalItems > 0) {
            queryData.TotalItems = pageData.TotalItems;
        }
        if (pageData.MaxTransId > 0) {
            queryData.MaxTransId = pageData.MaxTransId;
        }
    }

    $.ajax({
        url: resolveClientUrl('ViewIPInfo/IpStatsData'),
        type: 'POST',
        data: queryData,
        dataType: 'html',
        cache: false,
        beforeSend: function (xhr) {
            loading.show();
        },
        success: function (result) {
            $('#divResultIpStats').show();
            $('#divResultIpStats').html(result);
            InitTableSorter($('#result-ip-stats'), $('#header'), startCounter);
        },
        error: function (xhr, status, err) {
            alertBox.showError(xhr);
        },
        complete: function () {
            loading.hide();
        }
    });
};

function ValidateIPAddress(inputText) {
    var ipformat = /^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$/;
    var ip6format = /(([0-9a-fA-F]{1,4}:){7,7}[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,7}:|([0-9a-fA-F]{1,4}:){1,6}:[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,5}(:[0-9a-fA-F]{1,4}){1,2}|([0-9a-fA-F]{1,4}:){1,4}(:[0-9a-fA-F]{1,4}){1,3}|([0-9a-fA-F]{1,4}:){1,3}(:[0-9a-fA-F]{1,4}){1,4}|([0-9a-fA-F]{1,4}:){1,2}(:[0-9a-fA-F]{1,4}){1,5}|[0-9a-fA-F]{1,4}:((:[0-9a-fA-F]{1,4}){1,6})|:((:[0-9a-fA-F]{1,4}){1,7}|:)|fe80:(:[0-9a-fA-F]{0,4}){0,4}%[0-9a-zA-Z]{1,}|::(ffff(:0{1,4}){0,1}:){0,1}((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])|([0-9a-fA-F]{1,4}:){1,4}:((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9]))/;
    if (inputText.length <= 15) {
        return inputText.match(ipformat);
    }
    else {
        return inputText.match(ip6format);
    }
};

function FillData(table, data) {
    var records = data.Records;
    var length = records.length;
    var tbody = $(table).children('tbody');
    var tfooter = $("#total-footer-ip-info").children('tfoot');
    var totalSoccer = 0.0;
    var totalOthers = 0.0;
    var total = 0.0;
    var stake = 0.0;
    var winningAmount = 0.0;
    var houseLoseAmount = 0.0;

    if (length > 0) {
        var rows = tbody.children('tr');
        if (rows.has('td.noinfo')) {
            rows.children('td.noinfo').remove();
        }
        var r = new Array();
        var j = -1;
        for (var i = 0; i < length; i++) {
            var item = records[i];
            r[++j] = CreateRow(i + 1, item);
            totalSoccer += 0.0 + item.Soccer;
            totalOthers += 0.0 + item.Others;
            total += 0.0 + item.Soccer + item.Others;
            stake += 0.0 + item.Stake;
            winningAmount += 0.0 + item.WinningAmount;
            houseLoseAmount += 0.0 + item.HouseLoseAmount;
        }
        tbody.html(r.join(''));

        //Add total row
        var totalRow = CreateTotalRow(totalSoccer, totalOthers, total, stake, winningAmount, houseLoseAmount);
        tfooter.html(totalRow);
    }
    else {
        if (tbody.children('tr:first-child').children('td.noinfo').length === 0) {
            var noInfo = CreateNoInfoRow(9);
            tbody.html(noInfo);
            tfooter.html('');
        }
    }

    $('#divResultIpInfo').show();
}

function FillDataIpStats(table, data) {
    var records = data.IPStats;
    var totalItems = data.TotalItems;
    var maxTransId = data.MaxTransId;

    var length = records.length;
    var tbody = $(table).children('tbody');

    if (length > 0) {
        var rows = tbody.children('tr');
        if (rows.has('td.noinfo')) {
            rows.children('td.noinfo').remove();
        }
        var r = new Array();
        var j = -1;
        for (var i = 0; i < length; i++) {
            var item = records[i];
            r[++j] = CreateRowIpStats(i + 1, item);
        }
        tbody.html(r.join(''));
    }
    else {
        if (tbody.children('tr:first-child').children('td.noinfo').length === 0) {
            var noInfo = CreateNoInfoRow(3);
            tbody.html(noInfo);
        }
    }

    $('#divResultIpStats').show();
}

function CreateRow(order, item) {
    var r = new Array();
    var total = 0.0 + item.Soccer + item.Others;
    var j = -1;
    r[++j] = '<tr style="height: 22px;">';
    r[++j] = '<td class="center">' + order + '</td>';
    r[++j] = '<td class="center">' + item.SiteGroup + '</td>';
    r[++j] = '<td class="center">' + item.Username + '</td>';
    if (item.Soccer == 0) {
        r[++j] = '<td class="center">-</td>';
    }
    else {
        r[++j] = '<td class="center"><a href="#" class= "rowSoccer" data-id ="' + item.UserId + '" data-name="' + item.Username + '" >' + item.Soccer + '</a></td>';
    }

    if (item.Others == 0) {
        r[++j] = '<td class="center">-</td>';
    }
    else {
        r[++j] = '<td class="center"><a href="#" class= "rowOthers" data-id ="' + item.UserId + '" data-name= "' + item.Username + '" >' + item.Others + '</a></td>';
    }
    r[++j] = '<td class="center">' + total + '</td>';
    r[++j] = '<td class="center">' + numeral(item.Stake).format('0,0') + '</td>';
    r[++j] = '<td class="center">' + numeral(item.WinningAmount).format('0,0') + '</td>';
    r[++j] = '<td class="center">' + numeral(item.HouseLoseAmount).format('0,0') + '</td>';
    r[++j] = '</tr>';
    return r.join('');
}

function CreateRowIpStats(order, item) {
    var r = new Array();
    var j = -1;
    r[++j] = '<tr style="height: 22px;">';
    r[++j] = '<td class="center">' + order + '</td>';
    r[++j] = '<td class="center">' + item.IP + '</td>';
    r[++j] = '<td class="center">' + item.cntUsers + '</td>';
    r[++j] = '</tr>';
    return r.join('');
}

function CreateTotalRow(totalSoccer, totalOthers, total, stake, winningAmount, houseLoseAmount) {
    var r = new Array();
    var j = -1;
    r[++j] = '<tr style="height: 29px;">';
    r[++j] = '<td class="center" colspan="3">Total</td>';
    r[++j] = '<td class="center">' + numeral(totalSoccer).format('0,0') + '</td>';
    r[++j] = '<td class="center">' + numeral(totalOthers).format('0,0') + '</td>';
    r[++j] = '<td class="center">' + numeral(total).format('0,0') + '</td>';
    r[++j] = '<td class="center">' + numeral(stake).format('0,0') + '</td>';
    r[++j] = '<td class="center">' + numeral(winningAmount).format('0,0') + '</td>';
    r[++j] = '<td class="center">' + numeral(houseLoseAmount).format('0,0') + '</td>';
    r[++j] = '</tr>';
    return r.join('');
}

function OpenDrillDown(id, name, type) {
    var url = GenerateUrl(id, name, type);
    window.open(url, '_blank');
}
function GenerateUrl(id, name, type) {
    var url = '\ViewIpInfo/Drilldown' + resolveClientUrl('?date=' + queryData.date + '&ip=' + queryData.ip + '&userid=' + id + '&username=' + name + '&sporttype=' + type);
    return url;
}

function MovePage(pageNumber) {
    this.LoadReportIpStats(pageNumber, false);
}

function ExportExcelForIpStats() {
    var fromDate = $("#date-ipstats").val();
    var ticketStatus = $("#ticketStatus-ipstats").multiselect("getChecked").map(function () {
        return this.value;
    }).get().join(',');
    queryData = {
        FromDate: fromDate,
        PageIndex: 1,
        PageSize: 1000000,
        TotalItems: 0,
        MaxTransId: 0,
        TicketStatus: ticketStatus.split(',').length == 6 ? '-1' : ticketStatus
    };
    var url = 'ViewIPInfo/ExcelForIpStats?' + $.param(queryData);
    window.location.href = resolveClientUrl(url);
}

function ExportExcelForIpInfo() {
    var ip = $.trim($('#ip').val().replace(/,/g, ''));
    var date = $("#date-ipinfo").val();
    var ticketStatus = $("#ticketStatus-ipinfo").multiselect("getChecked").map(function () {
        return this.value;
    }).get().join(',');

    queryData = {
        ip: ip,
        date: date,
        ticketStatus: ticketStatus.split(',').length == 6 ? '-1' : ticketStatus
    };
    var url = 'ViewIPInfo/ExcelForIpInfo?' + $.param(queryData);
    window.location.href = resolveClientUrl(url);
}

function InitTableSorter(table, header, counter) {
    tableSorter.bind2(header, table, {
        type: 'text',
        onStart: function () {
            table.find('tr[custid]').remove();
        },
        onFinish: function () {
            var trs = table.children('tbody').children("tr");
            trs.each(function (index, tr) {
                $(tr).children('td:first').text(counter + index + 1);
            });
        }
    });
}