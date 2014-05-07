$(function () {
    "use strict";
    //config
    // URL of IP checking website, open in a new tab when user click to IP
    var ipViewSiteURL = 'http://www.iplocationfinder.com/';
    var queryData;
    //Now Date
    var nowDate = moment(dateOption.NowDate).format('MMMM DD, YYYY');
    var minDate = moment(dateOption.MinDate).format('MMMM DD, YYYY');
    var inputDate;
    if (dateOption.InputDate) {
        inputDate = moment(dateOption.InputDate).format('MMMM DD, YYYY');
    }
    else {
        inputDate = moment(dateOption.NowDate).format('MMMM DD, YYYY');
    }

    //add css class
    $("#btnSearch").addClass("button");

    //setup datepicker
    $("#date").datepicker({
        dateFormat: "MM dd, yy",
        showButtonPanel: true,
        changeMonth: true,
        changeYear: true,
        maxDate: nowDate,
        minDate: minDate,
        onSelect: function (dateText, inst) {
            if (dateText !== inst.lastVal) {
                $("#date option[value='7']").attr('selected', 1);
            }
        },
        onClose: function (selectedDate) {
            $("#date").datepicker("option", null, selectedDate);
        }
    });

    //set datepicker default value.
    $("#date").val(inputDate);

    //set event for enter-key press
    $("#date").keypress(function (e) {
        switch (e.keyCode) {
            case 13: // enter
                $("#btnSearch").click();
                break;
        }
    });
    $("#ip").keypress(function (e) {
        switch (e.keyCode) {
            case 13: // enter
                $("#btnSearch").click();
                break;
        }
    });
    if (dateOption.InputDate) {
        LoadMainReport();
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
    $('#btnSearch').click(function () {
        LoadMainReport();
    });

    function LoadMainReport() {
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
        var date = $("#date").val();

        queryData = {
            ip: ip,
            date: date
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

    function ValidateIPAddress(inputText) {
        var ipformat = /^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$/;
        if (inputText.match(ipformat)) {
            return true;
        }
        else {
            return false;
        }
    }

    function FillData(table, data) {
        var records = data.Records;
        var length = records.length;
        var tbody = $(table).children('tbody');
        var tfooter = $("#footer").children('tfoot');
        var totalSoccer = 0.0;
        var totalOthers = 0.0;
        var total = 0.0;

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
            }
            tbody.html(r.join(''));

            //Add total row
            var totalRow = CreateTotalRow(totalSoccer, totalOthers, total);
            tfooter.html(totalRow);
        }
        else {
            if (tbody.children('tr:first-child').children('td.noinfo').length === 0) {
                var noInfo = CreateNoInfoRow(5);
                tbody.html(noInfo);
                tfooter.html('');
            }
        }

        $('#divResult').show();
    }

    function CreateRow(order, item) {
        var r = new Array();
        var total = 0.0 + item.Soccer + item.Others;
        var j = -1;
        r[++j] = '<tr style="height: 22px;">';
        r[++j] = '<td class="center">' + order + '</td>';
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
        r[++j] = '</tr>';
        return r.join('');
    }
    function CreateTotalRow(totalSoccer, totalOthers, total) {
        var r = new Array();
        var j = -1;
        r[++j] = '<tr style="height: 22px;">';
        r[++j] = '<td class="center" colspan="2">Total</td>';
        r[++j] = '<td class="center">' + numeral(totalSoccer).format('0,0') + '</td>';
        r[++j] = '<td class="center">' + numeral(totalOthers).format('0,0') + '</td>';
        r[++j] = '<td class="center">' + numeral(total).format('0,0') + '</td>';
        r[++j] = '</tr>';
        return r.join('');
    }
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
    function OpenDrillDown(id, name, type) {
        var url = GenerateUrl(id, name, type);
        window.open(url, '_blank');
    }
    function GenerateUrl(id, name, type) {
        var url = '\ViewIpInfo/Drilldown' + resolveClientUrl('?date=' + queryData.date + '&ip=' + queryData.ip + '&userid=' + id + '&username=' + name + '&sporttype=' + type);
        return url;
    }
});