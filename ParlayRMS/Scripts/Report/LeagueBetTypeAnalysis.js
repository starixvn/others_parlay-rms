LeagueBetTypeAnalysis = {
    cachedObject: {},
    cachedObjectLeagueDetail: {},
    cachedObjectBetType: {},
    LeagueList: [],
    totalLeagueGroup: 0,
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
            sportType: null,
            sortById: null,
            isSortAsc: null,
            page: 1,
            pageSize: 100
        };
        LeagueBetTypeAnalysis.cachedObjectLeagueDetail = {
            parlayType: null,
            fromDate: null,
            toDate: null,
            leagueGroups: null,
            leagues: null,
            site: null,
            houseWinloss: null,
            sportType: null,
            sortById: null,
            isSortAsc: null,
            page: 1,
            pageSize: 100
        };
        LeagueBetTypeAnalysis.cachedObjectBetType = {
            parlayType: null,
            fromDate: null,
            toDate: null,
            leagueGroups: null,
            betType: null,
            site: null,
            houseWinloss: null,
            sportType: null,
            sortById: null,
            isSortAsc: null,
            page: 1,
            pageSize: 100
        };
        var nowDate = moment(dateRanges.NowDate).format('MMMM DD, YYYY');
        var minDate = moment(dateRanges.LimitDataFromDate).format('MMMM DD, YYYY');

        LeagueBetTypeAnalysis.HandleElementForLeagueBetTypeTab();

        $("#tabs").tabs({
            activate: function (event, ui) {
                var element = ui.newPanel.selector;
                switch (element) {
                    case '#tabs-league-bettype':
                        LeagueBetTypeAnalysis.HandleElementForLeagueBetTypeTab();
                        break;
                    case '#tabs-league':
                        LeagueBetTypeAnalysis.HandleElementForLeagueTab(-1);
                        break;
                    default:
                        LeagueBetTypeAnalysis.HandleElementForBetTypeTab();
                    // code block
                }
            },
            beforeActivate: function (event, ui) {
                Validators.hide();
            }
        });
        $("#tabs").removeClass("ui-widget");

        $("#ddlParlayType").multiselect({
            minWidth: 165,
            height: 55,
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
            minWidth: 176,
            height: 136,
            create: function () {
                $("#ddlGroup").multiselect("checkAll");
            },
            checkAll: function () { LeagueBetTypeAnalysis.HandleLeagueControl(1) },
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
            checkAll: function () { LeagueBetTypeAnalysis.HandleLeagueControl(2) },
            selectedText: function (numChecked, numTotal, checkedItems) {
                if (numChecked == 0) {
                    return "Select options";
                }
                else if (numChecked == 1) {
                    var selectedText = $(checkedItems).attr('title');
                    var characters = parseInt($("#ddlSport").val()) === 1 ? 20 : 24;
                    return selectedText.length > characters ? selectedText.substring(0, characters) + ' ...' : selectedText;
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

        $("#ddlSite").multiselect({
            minWidth: 80,
            height: 80,
            header: false,
            multiple: false,
            selectedList: 1
        });

        $("#ddlSport").multiselect({
            minWidth: 80,
            height: 55,
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

        $("#txtHouseWL").autoNumeric({
            aSep: ',',
            aDec: '.',
            vMin: '-99999999999',
            vMax: '99999999999',
            aPad: false,
            aForm: false
        });

        $("#btnSearch").addClass("button");
        $('#btnSearch').click(function () {
            Validators.hide();
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

            var leagues = '';
            if (leagueGroups == '-1') {
                leagues = '-1';
            } else {
                leagues = $("#ddlLeague").multiselect("getChecked").map(function () {
                    return this.value;
                }).get().join(',');
                if ($("#ddlLeague").multiselect("getButton").get(0).innerText == 'All' && leagueGroups.search('-1') > -1) {
                    leagues = '-1,' + leagues;
                }
            }

            if (leagues == '') {
                Validators.show($("#ddlLeague").multiselect("getButton").get(0), 'Please select your options');
                return;
            }

            var reportBy = $('input[name=rdlReportBy]:checked').val();

            var sportType = parseInt($("#ddlSport").val());

            if (leagueGroups == '-1' && reportBy == '2') {
                Validators.show($("#ddlGroup").multiselect("getButton").get(0), 'Please select another option to get correct report!');
            }

            if (leagues.split(',').length > 1 && reportBy == '2' && $("#ddlGroup").multiselect("getButton").get(0).innerText != 'All') {
                Validators.show($("#ddlLeague").multiselect("getButton").get(0), 'Please select single League to get correct report!');
            }

            var site = parseInt($("#ddlSite").val());

            var ratio = isNaN(parseInt($("#txtRatio").val())) ? 0 : parseFloat($("#txtRatio").val()) / 100;

            if (ratio > 1) {
                Validators.show($("#txtRatio"), 'Invalid Ratio');
                return;
            }

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
                sportType,
                LeagueBetTypeAnalysis.cachedObject.sortById,
                LeagueBetTypeAnalysis.cachedObject.isSortAsc,
                LeagueBetTypeAnalysis.cachedObject.page,
                LeagueBetTypeAnalysis.cachedObject.pageSize);
        });

        $('#ddlGroup').on('change', function () {
            var leagueGroups = $("#ddlGroup").multiselect("getChecked").map(function () {
                return this.value;
            }).get().join(',');
            var leagues = $("#ddlLeague").multiselect("getChecked").map(function () {
                return this.value;
            }).get().join(',');
            if (LeagueBetTypeAnalysis.IsDisabled($('input[name=rdlReportBy]:checked').val(), leagueGroups, leagues)) {
                $("#ddlLeague").multiselect("disable");
                if (leagueGroups == '-1') {
                    var html = leagueGroups == '' ? '' : '<option value="0" aria-selected="true">All</option>';
                    $('#ddlLeague').html(html);
                    $('#ddlLeague').multiselect('refresh');
                }
            } else {
                $("#ddlLeague").multiselect("enable");
                LeagueBetTypeAnalysis.ReloadLeagues(leagueGroups);
            }
        });

        $('input[name=rdlReportBy]').on('change', function () {
            var leagueGroups = $("#ddlGroup").multiselect("getChecked").map(function () {
                return this.value;
            }).get().join(',');
            var leagues = $("#ddlLeague").multiselect("getChecked").map(function () {
                return this.value;
            }).get().join(',');
            if (LeagueBetTypeAnalysis.IsDisabled(this.value, leagueGroups, leagues)) {
                $("#ddlLeague").multiselect("disable");
                if (leagueGroups.split(',').length == LeagueBetTypeAnalysis.totalLeagueGroup) {
                    LeagueBetTypeAnalysis.ReloadLeagues(leagueGroups);
                }
            } else {
                $("#ddlLeague").multiselect("enable");
            }
        });

        LeagueBetTypeAnalysis.GroupsAndLeagues(1, $("#fromDate").val(), $("#toDate").val());

        $('#ddlSport').on('change', function () {
            LeagueBetTypeAnalysis.GroupsAndLeagues(parseInt($(this).val()), $("#fromDate").val(), $("#toDate").val());
        });

        $(document).on("click", '#header .sortable', function () {
            var currentObject = this;
            var currentColIndex = parseInt($(currentObject).attr("data-col-index"));

            if (currentColIndex == LeagueBetTypeAnalysis.cachedObject.sortById) {
                if (LeagueBetTypeAnalysis.cachedObject.isSortAsc) {
                    LeagueBetTypeAnalysis.cachedObject.isSortAsc = false;
                    $(this).removeClass("headerSortUp").addClass("headerSortDown");
                } else {
                    LeagueBetTypeAnalysis.cachedObject.isSortAsc = true;
                    $(this).removeClass("headerSortDown").addClass("headerSortUp");
                }
            } else {
                if (currentObject.sortDirection == 'asc') {
                    LeagueBetTypeAnalysis.cachedObject.isSortAsc = true;
                    $(this).removeClass("headerSortDown").addClass("headerSortUp");
                } else {
                    LeagueBetTypeAnalysis.cachedObject.isSortAsc = false;
                    $(this).removeClass("headerSortUp").addClass("headerSortDown");
                }

                $('#header').find(".sortable").each(function () {
                    if ($(currentObject).index() != $(this).index()) {
                        $(this).removeClass("headerSortDown").removeClass("headerSortUp");
                        this.sortDirection = "";
                    }
                });
            }

            if (!currentColIndex) {
                currentColIndex = $(currentObject).index();
            }

            LeagueBetTypeAnalysis.cachedObject.sortById = parseInt(currentColIndex);
            LeagueBetTypeAnalysis.Data(LeagueBetTypeAnalysis.cachedObject.parlayType,
                LeagueBetTypeAnalysis.cachedObject.fromDate,
                LeagueBetTypeAnalysis.cachedObject.toDate,
                LeagueBetTypeAnalysis.cachedObject.leagueGroups,
                LeagueBetTypeAnalysis.cachedObject.leagues,
                LeagueBetTypeAnalysis.cachedObject.site,
                LeagueBetTypeAnalysis.cachedObject.ratio,
                LeagueBetTypeAnalysis.cachedObject.reportBy,
                LeagueBetTypeAnalysis.cachedObject.sportType,
                LeagueBetTypeAnalysis.cachedObject.sortById,
                LeagueBetTypeAnalysis.cachedObject.isSortAsc,
                LeagueBetTypeAnalysis.cachedObject.page,
                LeagueBetTypeAnalysis.cachedObject.pageSize);
        });

        $(document).on('click', '.leagueTab', function () {
            $("#tabs").tabs("option", "active", 1);
            var leagueId = this.getAttribute('data-leagueId');
            LeagueBetTypeAnalysis.HandleElementForLeagueTab(leagueId);
            LeagueBetTypeAnalysis.LeagueDetail();
        });

        $(document).on('click', '.betTypeTab', function () {
            $("#tabs").tabs("option", "active", 2);
            LeagueBetTypeAnalysis.BetTypeDetail();
        });
    },

    GroupsAndLeagues: function (sportType, fromDate, toDate) {
        var data = {
            sportType: sportType,
            fromDate: fromDate,
            toDate: toDate
        }

        $.ajax({
            url: resolveClientUrl('LeagueBetTypeAnalysis/GroupsAndLeagues'),
            type: 'POST',
            data: data,
            dataType: 'html',
            cache: false,
            beforeSend: function (xhr) {
                loading.show();
            },
            success: function (result) {
                var json = $.parseJSON(result);
                LeagueBetTypeAnalysis.ReloadLeagueGroups(json.Groups);
                LeagueBetTypeAnalysis.totalLeagueGroup = json.Groups.length;
                LeagueBetTypeAnalysis.LeagueList = json.Leagues;
                var leagueGroups = $("#ddlGroup").multiselect("getChecked").map(function () {
                    return this.value;
                }).get().join(',');
                LeagueBetTypeAnalysis.ReloadLeagues(leagueGroups);
            },
            error: function (xhr, status, err) {
                alertBox.showError(xhr);
            },
            complete: function () {
                loading.hide();
            }
        });
    },

    Data: function (parlayType, fromDate, toDate, leagueGroups, leagues, site, ratio, reportBy, sportType, sortById, isSortAsc, page, pageSize) {
        var data = {
            fromDate: fromDate,
            toDate: toDate,
            parlayType: parlayType,
            leagueGroups: leagueGroups,
            leagues: leagues,
            site: site,
            ratio: ratio,
            reportBy: reportBy,
            sportType: sportType,
            page: page,
            sortById: sortById,
            isSortAsc: isSortAsc,
            pageSize: pageSize
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
                $('#header').find(".sortable").each(function () {
                    if ($(this).attr("data-col-index") == LeagueBetTypeAnalysis.cachedObject.sortById) {
                        if (LeagueBetTypeAnalysis.cachedObject.isSortAsc) {
                            $(this).removeClass("headerSortDown").addClass("headerSortUp");
                        } else {
                            $(this).removeClass("headerSortUp").addClass("headerSortDown");
                        }
                    }
                });
                $('div#leagueBettype > #pageSize').on('change', function () {
                    LeagueBetTypeAnalysis.cachedObject.pageSize = parseInt($(this).val());
                    LeagueBetTypeAnalysis.cachedObject.page = 1;
                    LeagueBetTypeAnalysis.Data(LeagueBetTypeAnalysis.cachedObject.parlayType,
                        LeagueBetTypeAnalysis.cachedObject.fromDate,
                        LeagueBetTypeAnalysis.cachedObject.toDate,
                        LeagueBetTypeAnalysis.cachedObject.leagueGroups,
                        LeagueBetTypeAnalysis.cachedObject.leagues,
                        LeagueBetTypeAnalysis.cachedObject.site,
                        LeagueBetTypeAnalysis.cachedObject.ratio,
                        LeagueBetTypeAnalysis.cachedObject.reportBy,
                        LeagueBetTypeAnalysis.cachedObject.sportType,
                        LeagueBetTypeAnalysis.cachedObject.sortById,
                        LeagueBetTypeAnalysis.cachedObject.isSortAsc,
                        LeagueBetTypeAnalysis.cachedObject.page,
                        LeagueBetTypeAnalysis.cachedObject.pageSize);
                });
            },
            error: function (xhr, status, err) {
                alertBox.showError(xhr);
            },
            complete: function () {
                loading.hide();
            }
        });
    },

    LeagueDetail: function (parlayType, fromDate, toDate, leagueGroups, leagues, site, houseWinloss, sportType, sortById, isSortAsc, page, pageSize) {
        var data = {
            fromDate: fromDate,
            toDate: toDate,
            parlayType: parlayType,
            leagueGroups: leagueGroups,
            leagues: leagues,
            site: site,
            houseWinloss: houseWinloss,
            sportType: sportType,
            page: page,
            sortById: sortById,
            isSortAsc: isSortAsc,
            pageSize: pageSize
        }

        LeagueBetTypeAnalysis.cachedObjectLeagueDetail = data;

        $.ajax({
            url: resolveClientUrl('LeagueBetTypeAnalysis/LeagueDetail'),
            type: 'POST',
            data: data,
            dataType: 'html',
            cache: false,
            beforeSend: function (xhr) {
                loading.show();
            },
            success: function (result) {
                $("#divLeagueDetailResult").html(result);
                $('#leagueDetailHeader').find(".sortable").each(function () {
                    if ($(this).attr("data-col-index") == LeagueBetTypeAnalysis.cachedObjectLeagueDetail.sortById) {
                        if (LeagueBetTypeAnalysis.cachedObjectLeagueDetail.isSortAsc) {
                            $(this).removeClass("headerSortDown").addClass("headerSortUp");
                        } else {
                            $(this).removeClass("headerSortUp").addClass("headerSortDown");
                        }
                    }
                });
                $('div#leagueDetail > #pageSize').on('change', function () {
                    LeagueBetTypeAnalysis.cachedObjectLeagueDetail.pageSize = parseInt($(this).val());
                    LeagueBetTypeAnalysis.cachedObjectLeagueDetail.page = 1;
                    LeagueBetTypeAnalysis.LeagueDetail(LeagueBetTypeAnalysis.cachedObjectLeagueDetail.parlayType,
                        LeagueBetTypeAnalysis.cachedObjectLeagueDetail.fromDate,
                        LeagueBetTypeAnalysis.cachedObjectLeagueDetail.toDate,
                        LeagueBetTypeAnalysis.cachedObjectLeagueDetail.leagueGroups,
                        LeagueBetTypeAnalysis.cachedObjectLeagueDetail.leagues,
                        LeagueBetTypeAnalysis.cachedObjectLeagueDetail.site,
                        LeagueBetTypeAnalysis.cachedObjectLeagueDetail.houseWinloss,
                        LeagueBetTypeAnalysis.cachedObjectLeagueDetail.sportType,
                        LeagueBetTypeAnalysis.cachedObjectLeagueDetail.sortById,
                        LeagueBetTypeAnalysis.cachedObjectLeagueDetail.isSortAsc,
                        LeagueBetTypeAnalysis.cachedObjectLeagueDetail.page,
                        LeagueBetTypeAnalysis.cachedObjectLeagueDetail.pageSize);
                });
            },
            error: function (xhr, status, err) {
                alertBox.showError(xhr);
            },
            complete: function () {
                loading.hide();
            }
        });
    },

    BetTypeDetail: function (parlayType, fromDate, toDate, leagueGroups, betType, site, houseWinloss, sportType, sortById, isSortAsc, page, pageSize) {
        var data = {
            fromDate: fromDate,
            toDate: toDate,
            parlayType: parlayType,
            leagueGroups: leagueGroups,
            betType: betType,
            site: site,
            houseWinloss: houseWinloss,
            sportType: sportType,
            page: page,
            sortById: sortById,
            isSortAsc: isSortAsc,
            pageSize: pageSize
        }

        LeagueBetTypeAnalysis.cachedObjectBetType = data;

        $.ajax({
            url: resolveClientUrl('LeagueBetTypeAnalysis/BetTypeDetail'),
            type: 'POST',
            data: data,
            dataType: 'html',
            cache: false,
            beforeSend: function (xhr) {
                loading.show();
            },
            success: function (result) {
                $("#divBetTypeDetailResult").html(result);
                $('#betTypeDetailHeader').find(".sortable").each(function () {
                    if ($(this).attr("data-col-index") == LeagueBetTypeAnalysis.cachedObjectBetType.sortById) {
                        if (LeagueBetTypeAnalysis.cachedObjectBetType.isSortAsc) {
                            $(this).removeClass("headerSortDown").addClass("headerSortUp");
                        } else {
                            $(this).removeClass("headerSortUp").addClass("headerSortDown");
                        }
                    }
                });
                $('div#betTypeDetail > #pageSize').on('change', function () {
                    LeagueBetTypeAnalysis.cachedObjectBetType.pageSize = parseInt($(this).val());
                    LeagueBetTypeAnalysis.cachedObjectBetType.page = 1;
                    LeagueBetTypeAnalysis.LeagueDetail(LeagueBetTypeAnalysis.cachedObjectBetType.parlayType,
                        LeagueBetTypeAnalysis.cachedObjectBetType.fromDate,
                        LeagueBetTypeAnalysis.cachedObjectBetType.toDate,
                        LeagueBetTypeAnalysis.cachedObjectBetType.leagueGroups,
                        LeagueBetTypeAnalysis.cachedObjectBetType.betType,
                        LeagueBetTypeAnalysis.cachedObjectBetType.site,
                        LeagueBetTypeAnalysis.cachedObjectBetType.houseWinloss,
                        LeagueBetTypeAnalysis.cachedObjectBetType.sportType,
                        LeagueBetTypeAnalysis.cachedObjectBetType.sortById,
                        LeagueBetTypeAnalysis.cachedObjectBetType.isSortAsc,
                        LeagueBetTypeAnalysis.cachedObjectBetType.page,
                        LeagueBetTypeAnalysis.cachedObjectBetType.pageSize);
                });
            },
            error: function (xhr, status, err) {
                alertBox.showError(xhr);
            },
            complete: function () {
                loading.hide();
            }
        });
    },

    ExportExcel: function (parlayType, fromDate, toDate, leagues, leagueGroups, site, ratio, reportBy, sportType) {
        var data = {
            fromDate: fromDate,
            toDate: toDate,
            parlayType: parlayType,
            leagueGroups: leagueGroups,
            leagues: leagues,
            site: site,
            ratio: ratio,
            reportBy: reportBy,
            sportType: sportType
        };

        var url = reportBy == 1 ? 'LeagueBetTypeAnalysis/ExportExcelForLeague' : 'LeagueBetTypeAnalysis/ExportExcelForBetType';

        var xhttp = new XMLHttpRequest();
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

    ExportExcelForLeagueDetail: function (parlayType, fromDate, toDate, leagues, leagueGroups, site, houseWinloss, sportType) {
        var data = {
            fromDate: fromDate,
            toDate: toDate,
            parlayType: parlayType,
            leagueGroups: leagueGroups,
            leagues: leagues,
            site: site,
            houseWinloss: houseWinloss,
            sportType: sportType
        };

        var url = 'LeagueBetTypeAnalysis/ExportExcelForLeagueDetail';

        var xhttp = new XMLHttpRequest();
        xhttp.onreadystatechange = function () {
            var element;
            if (xhttp.readyState === 4 && xhttp.status === 200) {
                element = document.createElement('a');
                element.href = window.URL.createObjectURL(xhttp.response);
                element.download = "LeagueDetail.xls";
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

    ExportExcelForBetTypeDetail: function (parlayType, fromDate, toDate, betType, leagueGroups, site, houseWinloss, sportType) {
        var data = {
            fromDate: fromDate,
            toDate: toDate,
            parlayType: parlayType,
            leagueGroups: leagueGroups,
            betType: betType,
            site: site,
            houseWinloss: houseWinloss,
            sportType: sportType
        };

        var url = 'LeagueBetTypeAnalysis/ExportExcelForBetTypeDetail';

        var xhttp = new XMLHttpRequest();
        xhttp.onreadystatechange = function () {
            var element;
            if (xhttp.readyState === 4 && xhttp.status === 200) {
                element = document.createElement('a');
                element.href = window.URL.createObjectURL(xhttp.response);
                element.download = "BetTypeDetail.xls";
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
        LeagueBetTypeAnalysis.cachedObject.sportType = 1;
        LeagueBetTypeAnalysis.cachedObject.sortById = null;
        LeagueBetTypeAnalysis.cachedObject.isSortAsc = null;
        LeagueBetTypeAnalysis.cachedObject.page = 1;
        LeagueBetTypeAnalysis.cachedObject.pageSize = 100;
    },

    IsNumber: function (event) {
        var code = event.keyCode;
        if (code >= 48 && code <= 57) {
            return true;
        }

        return false;
    },

    ReloadLeagues: function (leagueGroups) {
        var leagueGroupsList = leagueGroups.split(',');
        var data = LeagueBetTypeAnalysis.LeagueList.filter(function (obj) {
            return leagueGroupsList.indexOf(obj.GroupId.toString()) > -1
        });
        var html = '';
        $.each(data, function (key, obj) {
            html += '<option value="' + obj.LeagueId + '" aria-selected="true">' + obj.LeagueName + '</option>';
        });
        $('#ddlLeague').html(html);
        $('#ddlLeague').multiselect('refresh');
        $("#ddlLeague").multiselect("checkAll");
    },

    ReloadLeagueGroups: function (data) {
        var html = '';
        $.each(data, function (key, obj) {
            html += '<option value="' + obj.Id + '" aria-selected="true">' + obj.Name + '</option>';
        });
        $('#ddlGroup').html(html);
        $('#ddlGroup').multiselect('refresh');
        $("#ddlGroup").multiselect("checkAll");
    },

    IsDisabled: function (reportBy, leagueGroups, leagues) {
        return leagueGroups == '-1' || leagueGroups == '' ||
                (leagueGroups.split(',').length == LeagueBetTypeAnalysis.totalLeagueGroup
                && reportBy == '2');
    },

    HandleLeagueControl: function (type) {
        var reportBy = $('input[name=rdlReportBy]:checked').val();
        var leagueGroups = $("#ddlGroup").multiselect("getChecked").map(function () {
            return this.value;
        }).get().join(',');
        if (reportBy == '2') {
            if (type == 2 && leagueGroups.split(',').length == LeagueBetTypeAnalysis.totalLeagueGroup) {
                $("#ddlLeague").multiselect("disable");
            }
            if (type == 1 && leagueGroups.split(',').length == LeagueBetTypeAnalysis.totalLeagueGroup) {
                $("#ddlLeague").multiselect("disable");
                LeagueBetTypeAnalysis.ReloadLeagues(leagueGroups);
            }
        }
    },

    HandleElementForLeagueBetTypeTab: function () {
        $("#houseWinLossDiv").hide();
        $("#reportByDiv").show();
        $("#ratioDiv").show();
    },

    HandleElementForLeagueTab: function (leagueId) {
        $("#houseWinLossDiv").show();
        $("#reportByDiv").hide();
        $("#ratioDiv").hide();
        leagueId > 0 ? $("#ddlLeague").val([leagueId]).multiselect("refresh") : $("#ddlLeague").multiselect("checkAll");
    },

    HandleElementForBetTypeTab: function () {
        $("#houseWinLossDiv").show();
        $("#reportByDiv").hide();
        $("#ratioDiv").hide();
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
                LeagueBetTypeAnalysis.cachedObject.sportType,
                LeagueBetTypeAnalysis.cachedObject.sortById,
                LeagueBetTypeAnalysis.cachedObject.isSortAsc,
                pageNumber,
                LeagueBetTypeAnalysis.cachedObject.pageSize);
}

