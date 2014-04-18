$(function () {
    "use strict";
    //config
    var ipViewSiteURL = 'http://www.iplocationfinder.com/';

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

    //set datepicker default value. TODO: check if input is null - ok, checked
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
    load();

    //view ip link
    $('#ipViewLink').click(function () {
        var ip = $.trim($('#ip').val().replace(/,/g, ''));
        window.open(ipViewSiteURL + ip);
    });

    //event for enter-key press
    $('#btnSearch').click(function () {
        load();
    });

    function load() {
        //validate input
        var ip = $.trim($('#ip').val().replace(/,/g, ''));
        if (ip === '') {
            Validators.show($('#ip').get(0), 'This field is required');
            return false;
        }

        var date = $("#date").val();

        var data = {
            ip: ip,
            date: date
        };

        $.ajax({
            url: resolveClientUrl('ViewIPInfo/Data'),
            type: 'POST',
            data: data,
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
    function FillData(table, data) {
        var records = data.Records;
        var length = records.length;
        var tbody = $(table).children('tbody');

        //Remove highlight rows
        tbody.children('tr.highlight').each(function () {
            $(this).removeAttr('class');
        });

        if (length > 0) {
            //Alert();
            var rows = tbody.children('tr');
            if (rows.length === 0) {
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
                    //                    var settings = settingsController.get();
                    //                    if (settings.selectTop == rows.length) {
                    //                        var last = rows.last();
                    //                        last.remove();
                    //                    }

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
            if (maxTransId === 0) {
                if (tbody.children('tr:first-child').children('td.noinfo').length === 0) {
                    var noinfo = CreateNoInfoRow(9);
                    tbody.append(noinfo);
                }
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
        r[++j] = '<td class="numeric"><a href="#" onclick="window.open(resolveClientUrl(\'ViewParlay?betid=' + item.Soccer + '\'), \'_blank\');">' + item.Soccer + '</a></td>';
        r[++j] = '<td class="numeric"><a href="#" onclick="window.open(resolveClientUrl(\'ViewParlay?betid=' + item.Others + '\'), \'_blank\');">' + item.Others + '</a></td>';
        r[++j] = '<td class="numeric">' + Total + '</td>';
        r[++j] = '</tr>';
        return r.join('');
    }
});