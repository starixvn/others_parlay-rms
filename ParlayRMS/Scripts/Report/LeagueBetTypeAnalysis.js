LeagueBetTypeAnalysis = {
    cachedObject: {},
    Init: function () {
        LeagueBetTypeAnalysis.cachedObject = {
            parlayType: null,
            fromDate: null,
            toDate: null,
            leagueGroups: null,
            leagues: null,
            site: null,
            ratio: null,
            reportBy: null,
            page: 1
        };
        var nowDate = moment(dateRanges.NowDate).format('MMMM DD, YYYY');
        var minDate = moment(dateRanges.LimitDataFromDate).format('MMMM DD, YYYY');

        $("#ddlParlayType").multiselect({
            minWidth: 165,
            height: 82,
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

        $("#ddlGroup").multiselect({
            minWidth: 132,
            height: 136,
            create: function () {
                $("#ddlGroup").multiselect("checkAll");
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

        $("#ddlLeague").multiselect({
            beforeopen: function () { $(this).multiselect('widget').width(200); },
            minWidth: 132,
            height: 136,
            create: function () {
                $("#ddlLeague").multiselect("checkAll");
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

        $("#ddlSite").multiselect({
            minWidth: 80,
            height: 80,
            header: false,
            multiple: false,
            selectedList: 1
        });

        $("#ddlReportBy").multiselect({
            minWidth: 80,
            height: 56,
            header: false,
            multiple: false,
            selectedList: 1
        });

        $("#fromDate").datepicker({
            dateFormat: "MM dd, yy",
            showButtonPanel: true,
            changeMonth: true,
            changeYear: true,
            minDate: minDate,
            maxDate: nowDate,
            onSelect: function (dateText, inst) {
                if (dateText != inst.lastVal) {
                    $("#dateRange option[value='7']").attr('selected', 1);
                    $("#dateRange").multiselect("refresh");
                }
            },
            onClose: function (selectedDate) {
                $("#toDate").datepicker("option", "minDate", selectedDate);
            }
        });

        $("#toDate").datepicker({
            dateFormat: "MM dd, yy",
            showButtonPanel: true,
            changeMonth: true,
            changeYear: true,
            minDate: minDate,
            maxDate: nowDate,
            onSelect: function (dateText, inst) {
                if (dateText != inst.lastVal) {
                    $("#dateRange option[value='7']").attr('selected', 1);
                    $("#dateRange").multiselect("refresh");
                }
            },
            onClose: function (selectedDate) {
                $("#fromDate").datepicker("option", "maxDate", selectedDate);
            }
        });

        var nowDate = moment(dateRanges.NowDate);
        $("#fromDate").val(nowDate.format('MMMM DD, YYYY'));
        $("#toDate").val(nowDate.format('MMMM DD, YYYY'));

        $("#btnSearch").addClass("button");
        $('#btnSearch').click(function () {
            $("#divResult").html('');
            LeagueBetTypeAnalysis.ResetCachedObject();
            var fromDate = $("#fromDate").val();
            var toDate = $("#toDate").val();

            if (validator.validateDate("#fromDate")) {
                return;
            }

            if (validator.validateDate("#toDate")) {
                return;
            }

            var parlayType = $("#ddlParlayType").multiselect("getChecked").map(function () {
                return this.value;
            }).get().join(',');
            if (parlayType == '') {
                Validators.show($("#ddlParlayType").multiselect("getButton").get(0), 'Please select your options');
                return;
            }

            var leagueGroups = $("#ddlGroup").multiselect("getChecked").map(function () {
                return this.value;
            }).get().join(',');
            if (leagueGroups == '') {
                Validators.show($("#ddlGroup").multiselect("getButton").get(0), 'Please select your options');
                return;
            }

            var leagues = $("#ddlLeague").multiselect("getChecked").map(function () {
                return this.value;
            }).get().join(',');
            if ($("#ddlLeague").multiselect("getButton").get(0).innerText == 'All') {
                leagues = '-1,' + leagues;
            }

            if (leagues == '') {
                Validators.show($("#ddlLeague").multiselect("getButton").get(0), 'Please select your options');
                return;
            }

            var site = parseInt($("#ddlSite").val());

            var reportBy = parseInt($("#ddlReportBy").val());

            var ratio = isNaN(parseInt($("#txtRatio").val())) ? 0 : parseFloat($("#txtRatio").val()) / 100;

            $('#header').find(".sortable").each(function () {
                $(this).removeClass("headerSortDown").removeClass("headerSortUp");
                this.sortDirection = "";
            });

            LeagueBetTypeAnalysis.Data(parlayType,
                fromDate,
                toDate,
                leagueGroups,
                leagues,
                site,
                ratio,
                reportBy,
                LeagueBetTypeAnalysis.cachedObject.page);
        });
    },

    Data: function (parlayType, fromDate, toDate, leagueGroups, leagues, site, ratio, reportBy, page) {
        var data = {
            fromDate: fromDate,
            toDate: toDate,
            parlayType: parlayType,
            leagueGroups: leagueGroups,
            leagues: leagues,
            site: site,
            ratio: ratio,
            reportBy: reportBy,
            page: page
        }

        LeagueBetTypeAnalysis.cachedObject = data;

        $.ajax({
            url: resolveClientUrl('LeagueBetTypeAnalysis/Data'),
            type: 'POST',
            data: data,
            dataType: 'html',
            cache: false,
            beforeSend: function (xhr) {
                loading.show();
            },
            success: function (result) {
                $("#divResult").html(result);
                tableSorter.bind2($('#header'), $('#result'), {});
            },
            error: function (xhr, status, err) {
                alertBox.showError(xhr);
            },
            complete: function () {
                loading.hide();
            }
        });
    },
    ExportExcel: function (parlayType, fromDate, toDate, leagues, leagueGroups, site, ratio, reportBy) {
        var data = {
            fromDate: fromDate,
            toDate: toDate,
            parlayType: parlayType,
            leagueGroups: leagueGroups,
            leagues: leagues,
            site: site,
            ratio: ratio,
            reportBy: reportBy
        };

        var url = reportBy == 1 ? 'LeagueBetTypeAnalysis/ExportExcelForLeague' : 'LeagueBetTypeAnalysis/ExportExcelForBetType';

        xhttp = new XMLHttpRequest();
        xhttp.onreadystatechange = function () {
            var element;
            if (xhttp.readyState === 4 && xhttp.status === 200) {
                element = document.createElement('a');
                element.href = window.URL.createObjectURL(xhttp.response);
                element.download = "LeagueBetTypeDataAnalysis.xls";
                element.style.display = 'none';
                document.body.appendChild(element);
                element.click();
            }
        };
        xhttp.open("POST", resolveClientUrl(url));
        xhttp.setRequestHeader("Content-Type", "application/json");
        xhttp.responseType = 'blob';
        xhttp.send(JSON.stringify(data));

    },

    ResetCachedObject: function () {
        LeagueBetTypeAnalysis.cachedObject.fromDate = null;
        LeagueBetTypeAnalysis.cachedObject.toDate = null;
        LeagueBetTypeAnalysis.cachedObject.parlayType = null;
        LeagueBetTypeAnalysis.cachedObject.leagueGroups = null;
        LeagueBetTypeAnalysis.cachedObject.leagues = null;
        LeagueBetTypeAnalysis.cachedObject.site = null;
        LeagueBetTypeAnalysis.cachedObject.ratio = null;
        LeagueBetTypeAnalysis.cachedObject.reportBy = 0;
        LeagueBetTypeAnalysis.cachedObject.page = 1
    },

    IsNumberLessThan100: function (event) {
        var number = isNaN(parseInt($("#txtRatio").val())) ? 0 : parseInt($("#txtRatio").val());
        var code = event.keyCode;
        if (code >= 48 && code <= 57 && (number * 10 + parseInt(event.key)) <= 100) {
            return true;
        }

        return false;
    }
}

$(document).ready(function () {
    LeagueBetTypeAnalysis.Init();
});

function MovePage(pageNumber) {
    LeagueBetTypeAnalysis.Data(LeagueBetTypeAnalysis.cachedObject.parlayType,
                LeagueBetTypeAnalysis.cachedObject.fromDate,
                LeagueBetTypeAnalysis.cachedObject.toDate,
                LeagueBetTypeAnalysis.cachedObject.leagueGroups,
                LeagueBetTypeAnalysis.cachedObject.leagues,
                LeagueBetTypeAnalysis.cachedObject.site,
                LeagueBetTypeAnalysis.cachedObject.ratio,
                LeagueBetTypeAnalysis.cachedObject.reportBy,
                pageNumber);
}