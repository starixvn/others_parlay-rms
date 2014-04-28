$(function () {
    "use strict";
    //config
    // URL of IP checking website, open in a new tab when user click to IP
    var ipViewSiteURL = 'http://www.iplocationfinder.com/';
    var queryData;
    //Now Date
    var nowDate = moment(dateOption.NowDate).format('MMMM DD, YYYY');
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
        if (!ValidateIPaddress(ip)) {
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
        if (!ValidateIPaddress(ip)) {
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

    function ValidateIPaddress(inputText) {
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
        var TotalSoccer = 0.0;
        var TotalOthers = 0.0;
        var Total = 0.0;

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
                TotalSoccer += 0.0 + item.Soccer;
                TotalOthers += 0.0 + item.Others;
                Total += 0.0 + item.Soccer + item.Others;
            }
            tbody.html(r.join(''));

            //Add total row
            var totalRow = CreateTotalRow(TotalSoccer, TotalOthers, Total);
            tfooter.html(totalRow);
        }
        else {
            if (tbody.children('tr:first-child').children('td.noinfo').length === 0) {
                var noinfo = CreateNoInfoRow(5);
                tbody.html(noinfo);
                tfooter.html('');
            }
        }

        $('#divResult').show();
    }

    function CreateRow(order, item) {
        var r = new Array();
        var Total = 0.0 + item.Soccer + item.Others;
        var j = -1;
        r[++j] = '<tr style="height: 22px;">';
        r[++j] = '<td class="center">' + order + '</td>';
        r[++j] = '<td>' + item.Username + '</td>';
        if (item.Soccer == 0) {
            r[++j] = '<td class="numeric">-</td>';
        }
        else {
            r[++j] = '<td class="numeric"><a href="#" class= "rowSoccer" data-id ="' + item.UserId + '" data-name="' + item.Username + '" >' + item.Soccer + '</a></td>';
        }

        if (item.Others == 0) {
            r[++j] = '<td class="numeric">-</td>';
        }
        else {
            r[++j] = '<td class="numeric"><a href="#" class= "rowOthers" data-id ="' + item.UserId + '" data-name= "' + item.Username + '" >' + item.Others + '</a></td>';
        }
        r[++j] = '<td class="numeric">' + Total + '</td>';
        r[++j] = '</tr>';
        return r.join('');
    }
    function CreateTotalRow(TotalSoccer, TotalOthers, Total) {
        var r = new Array();
        var j = -1;
        r[++j] = '<tr style="height: 22px;">';
        r[++j] = '<td class="center" colspan="2">Total</td>';
        r[++j] = '<td class="numeric">' + numeral(TotalSoccer).format('0,0') + '</td>';
        r[++j] = '<td class="numeric">' + numeral(TotalOthers).format('0,0') + '</td>';
        r[++j] = '<td class="numeric">' + numeral(Total).format('0,0') + '</td>';
        r[++j] = '</tr>';
        return r.join('');
    }
    $(document).on('click', '.rowSoccer', function () {
        var id = this.getAttribute('data-id');
        var name = this.getAttribute('data-name');
        var type = 1;
        openDrillDown(id, name, type);
    });
    $(document).on('click', '.rowOthers', function () {
        var id = this.getAttribute('data-id');
        var name = this.getAttribute('data-name');
        var type = 2;
        openDrillDown(id, name, type);
    });
    function openDrillDown(id, name, type) {
        var url = generateUrl(id, name, type);
        window.open(url, '_blank');
    }
    function generateUrl(id, name, type) {
        var url = '\ViewIpInfo/Drilldown' + resolveClientUrl('?date=' + queryData.date + '&ip=' + queryData.ip + '&userid=' + id + '&username=' + name + '&sporttype=' + type);
        return url;
    }
});