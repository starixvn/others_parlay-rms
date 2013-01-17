var chartTopSelectionByTicket;
var chartTopLeagueByTicket;
var chartTopCurrencyByTicket;

var chartTopSelectionByTurnover;
var chartTopLeagueByTurnover;
var chartTopCurrencyByTurnover;

var chartTopEventLeagueByTicket;
var chartTopEventCurrencyByTicket;
var chartTopEventLeagueByTurnover;
var chartTopEventCurrencyByTurnover;

var chartTopFavouriteByTicket;
var chartTopFavouriteByTurnover;

var chartTopSelectionByTicketEvent;
var chartTopSelectionByTicketEventBetType;
var chartTopSelectionByTicketEventBetTypeChoice;

var chartTopSelectionByTurnoverEvent;
var chartTopSelectionByTurnoverEventBetType;
var chartTopSelectionByTurnoverEventBetTypeChoice;

var allSports;
var soccerOnly;
var combinedSports;
var otherSports;

$(document).ready(function () {
    if ($('#back')) {
        $('#back').hide();

        $('#back').click(function () {
            $('#back').hide();
            $('#detailTitle').css('display', 'none');
            $('#detailTitle2').css('display', 'none');

            chartTopFavouriteByTicket.show();
            chartTopFavouriteByTurnover.show();

            chartTopSelectionByTicketEvent.hide();
            chartTopSelectionByTicketEventBetType.hide();
            chartTopSelectionByTicketEventBetTypeChoice.hide();

            chartTopSelectionByTurnoverEvent.hide();
            chartTopSelectionByTurnoverEventBetType.hide();
            chartTopSelectionByTurnoverEventBetTypeChoice.hide();
        });
    }
    else {
        dataSelected.setBetType2(-1);
    }

    allSports = _resource.allSports;
    soccerOnly = _resource.soccerOnly;
    combinedSports = _resource.combinedSports;
    otherSports = _resource.otherSports;

    chartTopSelectionByTicket = new SummaryChart("#topSelectionByTicket", "Monitoring/TopSelectionByTicket");
    chartTopLeagueByTicket = new Chart("#topSelectionByLeague", "Monitoring/TopLeagueByTicket", DataDetailGetting);
    chartTopCurrencyByTicket = new Chart("#topSelectionByCurrency", "Monitoring/TopCurrencyByTicket", DataDetailGetting);

    chartTopSelectionByTurnover = new SummaryChart("#topSelectionByTurnover", "Monitoring/TopSelectionByTurnover");
    chartTopLeagueByTurnover = new Chart("#topSelectionByLeague", "Monitoring/TopLeagueByTurnover", DataDetailGetting);
    chartTopCurrencyByTurnover = new Chart("#topSelectionByCurrency", "Monitoring/TopCurrencyByTurnover", DataDetailGetting);

    chartTopEventLeagueByTurnover = new Chart("#popularEvent", "Monitoring/TopEventLeagueByTurnover", DataDetailGetting);
    chartTopEventCurrencyByTurnover = new Chart("#popularEvent", "Monitoring/TopEventCurrencyByTurnover", DataDetailGetting);
    chartTopEventLeagueByTicket = new Chart("#popularEvent", "Monitoring/TopEventLeagueByTicket", DataDetailGetting);
    chartTopEventCurrencyByTicket = new Chart("#popularEvent", "Monitoring/TopEventCurrencyByTicket", DataDetailGetting);

    chartTopFavouriteByTicket = new SummaryChart("#topSelectionByTicket", "Monitoring/TopFavouriteByTicket");
    chartTopSelectionByTicketEvent = new Chart("#favoriteEvent", "Monitoring/TopSelectionByTicket_Event", DataDetailGetting);
    chartTopSelectionByTicketEventBetType = new Chart("#favoriteBetType", "Monitoring/TopSelectionByTicket_Event_BetType", DataDetailGetting);
    chartTopSelectionByTicketEventBetTypeChoice = new Chart("#favoriteChoice", "Monitoring/TopSelectionByTicket_Event_BetType_Choice", DataDetailGetting);

    chartTopFavouriteByTurnover = new SummaryChart("#topSelectionByTurnover", "Monitoring/TopFavouriteByTurnover");
    chartTopSelectionByTurnoverEvent = new Chart("#favoriteEvent", "Monitoring/TopSelectionByTurnover_Event", DataDetailGetting);
    chartTopSelectionByTurnoverEventBetType = new Chart("#favoriteBetType", "Monitoring/TopSelectionByTurnover_Event_BetType", DataDetailGetting);
    chartTopSelectionByTurnoverEventBetTypeChoice = new Chart("#favoriteChoice", "Monitoring/TopSelectionByTurnover_Event_BetType_Choice", DataDetailGetting);

    HideCharts();
});

function Chart(containerId, url, dataHandler) {
    var container = $(containerId);
    var url = url;
    var dataHandler = dataHandler;
    var timeout = null;
    var isLoad = false;

    this.showtext = function (text) {
        container.css('display', 'block');
        var div = "<div id='applicabletext' class='chart chartText'>" + text + "</div>"
        container.html(div);
        this.stop();
    };
    this.show = function () {
        container.css('display', 'block');
        this.stop();
        this.load();
    };
    this.isexisting = function () {
        if (container.css('display') == 'block') {
            return true;
        }
        else {
            return false;
        }
    };
    this.reload = function () {
        if (container.css('display') == 'block') {
            this.stop();
            this.load();
        }
    };

    this.hide = function () {
        container.css('display', 'none');
        this.stop();
    };

    this.clear = function () {
        container.html('');
        container.css('display', 'none');
        this.stop();
    };

    this.stop = function () {
        if (timeout != null) {
            clearTimeout(timeout);
            timeout = null;
        }
    };

    this.load = function () {
        if (isLoad == true) {
            return;
        }

        isLoad = true;
        var current = this;
        seperateDiv.Hide();

        $.ajax({
            url: resolveClientUrl(url),
            data: dataHandler(),
            type: 'GET',
            dataType: 'html',
            cache: false,
            beforesend: function (xhr) {
            },
            success: function (result) {
                container.html(result);
                isLoad = false;

                loadingHelper.Finish();
            },
            error: function () {
                container.showError();
                isLoad = false;

                loadingHelper.Finish();
            }
        });
    };
}

function SummaryChart(containerId, url, dataHandler) {
    this.All_Sports = true;
    this.Soccer_Only = true;
    this.Combined_Sports = true;
    this.Other_Sports = true;
    var current = this;
    var dataHandler = function () {
        var x = current;
        return {
            All_Sports: x.All_Sports,
            Soccer_Only: x.Soccer_Only,
            Combined_Sports: x.Combined_Sports,
            Other_Sports: x.Other_Sports,
            maxTransId: maxTransId
        };
    }
    Chart.call(this, containerId, url, dataHandler);
}

function LegendItemClick() {
    var name = this.name;
    var visible = this.visible;
    var container = $(this.chart.container).parent().parent();
    var id = container.attr("id");
    var target = null;
    if (id === "topSelectionByTicket") {
        if ($("input[id='rdbfavouritechoice']:checked").val() === "on") {
            target = chartTopFavouriteByTicket;
        }
        else {
            target = chartTopSelectionByTicket;
        }
    }
    else if (id === "topSelectionByTurnover") {
        if ($("input[id='rdbfavouritechoice']:checked").val() === "on") {
            target = chartTopFavouriteByTurnover;
        }
        else {
            target = chartTopSelectionByTurnover;
        }
    }
    if (target !== null && target instanceof SummaryChart) {
        switch (name) {
            case allSports:
                target.All_Sports = !visible;
                break;
            case soccerOnly:
                target.Soccer_Only = !visible;
                break;
            case combinedSports:
                target.Combined_Sports = !visible;
                break;
            case otherSports:
                target.Other_Sports = !visible;
                break;
        }
    }
}

function HideCharts() {
    chartTopSelectionByTicket.hide();
    chartTopCurrencyByTicket.hide();
    chartTopLeagueByTicket.hide();
    chartTopSelectionByTurnover.hide();
    chartTopCurrencyByTurnover.hide();
    chartTopLeagueByTurnover.hide();
    chartTopEventCurrencyByTicket.hide();
    chartTopEventCurrencyByTurnover.hide();
    chartTopEventLeagueByTicket.hide();
    chartTopEventLeagueByTurnover.hide();
}

function ClearCharts() {
    chartTopSelectionByTicket.clear();
    chartTopCurrencyByTicket.clear();
    chartTopLeagueByTicket.clear();
    chartTopSelectionByTurnover.clear();
    chartTopCurrencyByTurnover.clear();
    chartTopLeagueByTurnover.clear();
    chartTopEventCurrencyByTicket.clear();
    chartTopEventCurrencyByTurnover.clear();
    chartTopEventLeagueByTicket.clear();
    chartTopEventLeagueByTurnover.clear();
}

function ReloadCharts(IsTicket) {
    if ($("input[id='rdbfavouritechoice']:checked").val() === "on") {
        chartTopFavouriteByTicket.reload();
        chartTopFavouriteByTurnover.reload();
    }
    else {
        chartTopSelectionByTicket.reload();
        chartTopSelectionByTurnover.reload();
    }

    if (IsTicket) {
        if ($("input[id='rdbfavouritechoice']:checked").val() != "on") {
            chartTopCurrencyByTicket.reload();
            chartTopLeagueByTicket.reload();
        }
        else {
            if (dataSelected.IsFavourite) {
                chartTopSelectionByTicketEvent.reload();
            }
        }

        if (dataSelected.LeagueId != 0) {
            if (dataSelected.IsFavourite) {
                chartTopSelectionByTicketEvent.reload();
            }
            else {
                if (dataSelected.BetType2 != -1) {
                    if (dataSelected.IsApplicable == true) {
                        chartTopSelectionByTicketEventBetTypeChoice.stop();
                    }
                    else {
                        chartTopSelectionByTicketEventBetTypeChoice.reload();
                    }
                }
                else {
                    if (dataSelected.CurrencyId != -1) {
                        chartTopEventCurrencyByTicket.reload();
                    }
                    else {
                        chartTopEventLeagueByTicket.reload();
                    }
                }
            }
        }
        else if (dataSelected.MatchId != 0) {
            chartTopSelectionByTicketEventBetType.reload();
            if (dataSelected.BetType2 != -1) {
                if (dataSelected.IsApplicable == true) {
                    chartTopSelectionByTicketEventBetTypeChoice.stop();
                }
                else {
                    chartTopSelectionByTicketEventBetTypeChoice.reload();
                }
            }
        }
        else {
            if (dataSelected.IsFavourite) {
                chartTopSelectionByTicketEvent.reload();
            }
            else {
                if (dataSelected.CurrencyId != -1) {
                    chartTopEventCurrencyByTicket.reload();
                }
                else {
                    chartTopEventLeagueByTicket.reload();
                }
            }
        }

        chartTopCurrencyByTurnover.stop();
        chartTopLeagueByTurnover.stop();

        chartTopEventCurrencyByTurnover.stop();
        chartTopEventLeagueByTurnover.stop();
    }
    else {
        if ($("input[id='rdbfavouritechoice']:checked").val() != "on") {
            chartTopCurrencyByTurnover.reload();
            chartTopLeagueByTurnover.reload();
        }
        else {
            chartTopSelectionByTurnoverEvent.reload();
        }

        if (dataSelected.LeagueId != 0) {
            if (dataSelected.IsFavourite) {
                chartTopSelectionByTurnoverEvent.reload();
            }
            else {
                if (dataSelected.BetType2 != -1) {
                    if (dataSelected.IsApplicable == true) {
                        chartTopSelectionByTurnoverEventBetTypeChoice.stop();
                    }
                    else {
                        chartTopSelectionByTurnoverEventBetTypeChoice.reload();
                    }
                }
                else {
                    if (dataSelected.CurrencyId != -1) {
                        chartTopEventCurrencyByTurnover.reload();
                    }
                    else {
                        chartTopEventLeagueByTurnover.reload();
                    }
                }
            }
        }
        else if (dataSelected.MatchId != 0) {
            chartTopSelectionByTurnoverEventBetType.reload();
            if (dataSelected.BetType2 != -1) {
                if (dataSelected.IsApplicable == true) {
                    chartTopSelectionByTurnoverEventBetTypeChoice.stop();
                }
                else {
                    chartTopSelectionByTurnoverEventBetTypeChoice.reload();
                }
            }
        }
        else {
            if (dataSelected.IsFavourite) {
                chartTopSelectionByTurnoverEvent.reload();
            }
            else {
                if (dataSelected.CurrencyId != -1) {
                    chartTopEventCurrencyByTurnover.reload();
                }
                else {
                    chartTopEventLeagueByTurnover.reload();
                }
            }
        }

        chartTopCurrencyByTicket.stop();
        chartTopLeagueByTicket.stop();

        chartTopEventCurrencyByTicket.stop();
        chartTopEventLeagueByTicket.stop();
    }
}

function ProcessChartByTicket(selection, sportType) {
    $('#detailTitle').css('display', 'block');
    $('#detailTitle2').css('display', 'none');

    $('#detailFrom').html(_resource.ticket);
    $('#detailFrom2').html('');

    //selectData.set(true, selection, sportType, true);
    dataSelected.setIsTicket(true);
    dataSelected.setSelection(selection);
    dataSelected.setSportType(sportType);
    dataSelected.setIsSelection(true);
    dataSelected.setCurrencyId(-1);

    loadingHelper.Run(5);

    chartTopCurrencyByTurnover.hide();
    chartTopLeagueByTurnover.hide();

    chartTopSelectionByTicket.show();
    chartTopSelectionByTurnover.show();

    chartTopCurrencyByTicket.show();
    chartTopLeagueByTicket.show();

    chartTopEventCurrencyByTicket.hide();
    chartTopEventCurrencyByTurnover.hide();
    chartTopEventLeagueByTicket.hide();
    chartTopEventLeagueByTurnover.hide();

    ReloadSummary();
}

function ProcessChartByTurnover(selection, sportType) {
    $('#detailTitle').css('display', 'block');
    $('#detailTitle2').css('display', 'none');

    $('#detailFrom').html(_resource.turnover);
    $('#detailFrom2').html('');

    //selectData.set(false, selection, sportType, true);
    dataSelected.setIsTicket(false);
    dataSelected.setSelection(selection);
    dataSelected.setSportType(sportType);
    dataSelected.setIsSelection(true);
    dataSelected.setCurrencyId(-1);

    loadingHelper.Run(5);

    chartTopCurrencyByTicket.hide();
    chartTopLeagueByTicket.hide();

    chartTopEventCurrencyByTicket.hide();
    chartTopEventCurrencyByTurnover.hide();
    chartTopEventLeagueByTicket.hide();
    chartTopEventLeagueByTurnover.hide();

    chartTopSelectionByTicket.show();
    chartTopSelectionByTurnover.show();

    chartTopCurrencyByTurnover.show();
    chartTopLeagueByTurnover.show();

    ReloadSummary();
}

function ProcessChartEventCurrencyByTurnover(currencyId, currencyName, topCurrency) {
    $('#detailTitle2').css('display', 'block');
    $('#detailFrom2').html(_resource.detailCurrencies + ' (' + _resource.byturnover + ')');

    //selectData.set(false, null, null, true, currencyId, currencyName, null, null, topCurrency, true);
    dataSelected.setIsTicket(false);
    dataSelected.setIsSelection(true);
    dataSelected.setCurrencyId(currencyId);
    dataSelected.setLeagueId(0);
    dataSelected.setCurrencyName(currencyName);
    dataSelected.setTopCurrency(topCurrency);
    dataSelected.setIsSelectionDetail(true);

    if (chartTopLeagueByTurnover.isexisting()) {
        loadingHelper.Run(6);
    }
    else {
        loadingHelper.Run(5);
    }

    chartTopEventCurrencyByTicket.stop();
    chartTopEventLeagueByTicket.stop();
    chartTopEventLeagueByTurnover.stop();

    chartTopCurrencyByTicket.stop();
    chartTopLeagueByTicket.stop();

    chartTopSelectionByTicket.show();
    chartTopSelectionByTurnover.show();

    if (chartTopLeagueByTurnover.isexisting()) {
        chartTopLeagueByTurnover.show();
    }
    chartTopCurrencyByTurnover.show();

    chartTopEventCurrencyByTurnover.show();

    ReloadSummary();
}

function ProcessChartEventLeagueByTurnover(leagueId, leagueName) {
    $('#detailTitle2').css('display', 'block');
    $('#detailFrom2').html(_resource.detailLeagues + ' (' + _resource.byturnover + ')');

    //selectData.set(false, null, null, true, null, null, leagueId, leagueName, null, true);
    dataSelected.setIsTicket(false);
    dataSelected.setIsSelection(true);
    dataSelected.setLeagueId(leagueId);
    dataSelected.setCurrencyId(-1);
    dataSelected.setLeagueName(leagueName);
    dataSelected.setIsSelectionDetail(true);

    if (chartTopCurrencyByTurnover.isexisting()) {
        loadingHelper.Run(6);
    }
    else {
        loadingHelper.Run(5);
    }

    chartTopEventCurrencyByTicket.stop();
    chartTopEventLeagueByTicket.stop();
    chartTopEventCurrencyByTurnover.stop();

    chartTopLeagueByTicket.stop();
    chartTopCurrencyByTicket.stop();

    chartTopSelectionByTicket.show();
    chartTopSelectionByTurnover.show();
    if (chartTopCurrencyByTurnover.isexisting()) {
        chartTopCurrencyByTurnover.show();
    }

    chartTopLeagueByTurnover.show();

    chartTopEventLeagueByTurnover.show();

    ReloadSummary();
}

function ProcessChartEventCurrencyByTicket(currencyId, currencyName, topCurrency) {
    $('#detailTitle2').css('display', 'block');
    $('#detailFrom2').html(_resource.detailCurrencies + ' (' + _resource.byticket + ')');

    //selectData.set(true, null, null, true, currencyId, currencyName, null, null, topCurrency, true);
    dataSelected.setIsTicket(true);
    dataSelected.setIsSelection(true);
    dataSelected.setCurrencyId(currencyId);
    dataSelected.setLeagueId(0);
    dataSelected.setCurrencyName(currencyName);
    dataSelected.setTopCurrency(topCurrency);
    dataSelected.setIsSelectionDetail(true);

    if (chartTopLeagueByTicket.isexisting()) {
        loadingHelper.Run(6);
    }
    else {
        loadingHelper.Run(5);
    }
    chartTopEventCurrencyByTurnover.stop();
    chartTopEventLeagueByTicket.stop();
    chartTopEventLeagueByTurnover.stop();

    chartTopCurrencyByTurnover.stop();
    chartTopLeagueByTurnover.stop();

    chartTopSelectionByTicket.show();
    chartTopSelectionByTurnover.show();

    chartTopCurrencyByTicket.show();

    if (chartTopLeagueByTicket.isexisting()) {
        chartTopLeagueByTicket.show();
    }

    chartTopEventCurrencyByTicket.show();

    ReloadSummary();
}

function ProcessChartEventLeagueByTicket(leagueId, leagueName) {
    $('#detailTitle2').css('display', 'block');
    $('#detailFrom2').html(_resource.detailLeagues + ' (' + _resource.byticket + ')');

    //selectData.set(true, null, null, true, null, null, leagueId, leagueName, null, true);
    dataSelected.setIsTicket(true);
    dataSelected.setIsSelection(true);
    dataSelected.setLeagueId(leagueId);
    dataSelected.setCurrencyId(-1);
    dataSelected.setLeagueName(leagueName);
    dataSelected.setIsSelectionDetail(true);

    if (chartTopCurrencyByTicket.isexisting()) {
        loadingHelper.Run(6);
    }
    else {
        loadingHelper.Run(5);
    }
    chartTopEventLeagueByTurnover.stop();
    chartTopEventLeagueByTicket.stop();
    chartTopEventCurrencyByTurnover.stop();

    chartTopCurrencyByTurnover.stop();
    chartTopLeagueByTurnover.stop();

    chartTopSelectionByTicket.show();
    chartTopSelectionByTurnover.show();
    if (chartTopCurrencyByTicket.isexisting()) {
        chartTopCurrencyByTicket.show();
    }

    chartTopLeagueByTicket.show();

    chartTopEventLeagueByTicket.show();

    ReloadSummary();
}

function ProcessChartFavouriteByTicket(selection, sportType) {
    $('#detailTitle').css('display', 'block');
    $('#detailFrom').html(_resource.ticket);

    //selectData.set(true, selection, sportType, true, null, null, null, null, null, null, null, true, null, null, null, null, null, true, null, null);

    dataSelected.setIsTicket(true);
    dataSelected.setSelection(selection);
    dataSelected.setSportType(sportType);
    dataSelected.setIsSelection(true);
    dataSelected.setIsFavourite(true);
    dataSelected.setIsEvent(true);

    chartTopSelectionByTicket.hide();
    chartTopSelectionByTicketEventBetType.hide();
    chartTopSelectionByTicketEventBetTypeChoice.hide();

    chartTopSelectionByTurnover.hide();
    chartTopSelectionByTurnoverEvent.hide();
    chartTopSelectionByTurnoverEventBetType.hide();
    chartTopSelectionByTicketEventBetTypeChoice.hide();

    chartTopEventCurrencyByTicket.hide();
    chartTopEventCurrencyByTurnover.hide();
    chartTopEventLeagueByTicket.hide();
    chartTopEventLeagueByTurnover.hide();

    loadingHelper.Run(2);

    chartTopSelectionByTicketEvent.show();
    getDataSum.reload();

    $('#back').show();
}

function ProcessChartFavouriteByTurnover(selection, sportType) {
    $('#detailTitle').css('display', 'block');
    $('#detailFrom').html(_resource.turnover);

    //selectData.set(false, selection, sportType, true, null, null, null, null, null, null, null, true, null, null, null, null, null, true, null, null);
    dataSelected.setIsTicket(false);
    dataSelected.setSelection(selection);
    dataSelected.setSportType(sportType);
    dataSelected.setIsSelection(true);
    dataSelected.setIsFavourite(true);
    dataSelected.setIsEvent(true);

    chartTopSelectionByTicket.hide();
    chartTopSelectionByTicketEvent.hide();
    chartTopSelectionByTicketEventBetType.hide();
    chartTopSelectionByTicketEventBetTypeChoice.hide();

    chartTopSelectionByTurnover.hide();
    chartTopSelectionByTurnoverEventBetType.hide();
    chartTopSelectionByTicketEventBetTypeChoice.hide();

    chartTopEventCurrencyByTicket.hide();
    chartTopEventCurrencyByTurnover.hide();
    chartTopEventLeagueByTicket.hide();
    chartTopEventLeagueByTurnover.hide();

    loadingHelper.Run(2);

    chartTopSelectionByTurnoverEvent.show();
    getDataSum.reload();

    $('#back').show();
}

function ProcessChartByTicketEventBetType(matchId, matchName, homeId, awayId) {
    $('#detailTitle2').css('display', 'block');
    $('#detailFrom2').html(_resource.detailPopularEvent + ' (' + _resource.byticket + ')');

    //selectData.set(true, null, null, true, null, null, null, null, null, null, null, true, matchId, matchName, null, homeId, awayId, true, true, null);

    dataSelected.setIsTicket(true);
    dataSelected.setIsSelection(true);
    dataSelected.setIsFavourite(true);
    dataSelected.setMatchId(matchId);    
    dataSelected.setHomeId(homeId);
    dataSelected.setAwayId(awayId);
    dataSelected.setIsEvent(true);
    dataSelected.setIsBettype(true);
   
    //dataSelected.setIsBettype(true);

    chartTopSelectionByTicketEventBetTypeChoice.hide();

    chartTopSelectionByTurnoverEventBetType.hide();
    chartTopSelectionByTurnoverEventBetTypeChoice.hide();

    loadingHelper.Run(3);

    chartTopSelectionByTicketEventBetType.show();
    chartTopSelectionByTicketEvent.reload();
    getDataSum.reload();
}

function ProcessChartByTicketEventBetTypeChoice(matchId, betType2, betTypeName) {
    var isOk = IsIncludeBettype(betType2);
    if (isOk) {
        //selectData.set(true, null, null, true, null, null, null, null, null, null, betType2, true, matchId, null, betTypeName, null, null, true, true, true);

        dataSelected.setIsTicket(true);
        dataSelected.setIsSelection(true);
        dataSelected.setBettype2(betType2);
        dataSelected.setIsFavourite(true);
        dataSelected.setMatchId(matchId);
        dataSelected.setBettypeName(betTypeName);
        dataSelected.setIsEvent(true);
        dataSelected.setIsBettype(true);
        dataSelected.setIsChoice(true);
        dataSelected.setIsApplicable(false);

        chartTopSelectionByTurnoverEventBetTypeChoice.stop();
        chartTopSelectionByTurnoverEventBetType.stop();
        chartTopSelectionByTurnoverEvent.stop();

        loadingHelper.Run(4);

        chartTopSelectionByTicketEventBetTypeChoice.show();
        chartTopSelectionByTicketEventBetType.show();
        chartTopSelectionByTicketEvent.reload();

        getDataSum.reload();
    }
    else {
        chartTopSelectionByTicketEventBetTypeChoice.showtext(_resource.notAvailable);
        dataSelected.setIsApplicable(true);
    }
}

function ProcessChartByTurnoverEventBetType(matchId, matchName, homeId, awayId) {
    $('#detailTitle2').css('display', 'block');
    $('#detailFrom2').html(_resource.detailPopularEvent + ' (' + _resource.byturnover + ')');

    // selectData.set(false, null, null, true, null, null, null, null, null, null, null, true, matchId, matchName, null, homeId, awayId, true, true, null);

    dataSelected.setIsTicket(false);
    dataSelected.setIsSelection(true);
    dataSelected.setIsFavourite(true);
    dataSelected.setMatchId(matchId);
    dataSelected.setHomeId(homeId);
    dataSelected.setAwayId(awayId);
    dataSelected.setIsEvent(true);
    dataSelected.setIsBettype(true);

    chartTopSelectionByTicketEventBetTypeChoice.hide();
    chartTopSelectionByTicketEvent.hide();
    chartTopSelectionByTicketEventBetType.hide();
    chartTopSelectionByTurnoverEventBetTypeChoice.hide();

    loadingHelper.Run(3);

    chartTopSelectionByTurnoverEventBetType.show();
    chartTopSelectionByTurnoverEvent.show();
    getDataSum.reload();
}

function ProcessChartByTurnoverEventBetTypeChoice(matchId, betType2, betTypeName) {
    var isOk = IsIncludeBettype(betType2);
    if (isOk) {
        //selectData.set(false, null, null, true, null, null, null, null, null, null, betType2, true, matchId, null, betTypeName, null, null, true, true, true);
        dataSelected.setIsTicket(false);
        dataSelected.setIsSelection(true);
        dataSelected.setBettype2(betType2);
        dataSelected.setIsFavourite(true);
        dataSelected.setMatchId(matchId);
        dataSelected.setBettypeName(betTypeName);
        dataSelected.setIsEvent(true);
        dataSelected.setIsBettype(true);
        dataSelected.setIsChoice(true);
        dataSelected.setIsApplicable(false);

        chartTopSelectionByTicketEventBetTypeChoice.stop();
        chartTopSelectionByTicketEventBetType.stop();
        chartTopSelectionByTicketEvent.stop();

        loadingHelper.Run(4);

        chartTopSelectionByTurnoverEventBetTypeChoice.show();
        chartTopSelectionByTurnoverEventBetType.show();
        chartTopSelectionByTurnoverEvent.reload();

        getDataSum.reload();
    }
    else {
        chartTopSelectionByTurnoverEventBetTypeChoice.showtext(_resource.notAvailable);
        dataSelected.setIsApplicable(true);
    }
}

SummaryChart.prototype = new Chart();

function onLoad() {
    var width = 16;
    var height = 16;
    var x = this.chartWidth - width;
    var y = 0;
    var closeButton = this.renderer.image('../Images/Content/Close.png', x, y, width, height);
    var chart = this;
    closeButton.on('click', function () {
        var removeDiv = chart.container.parentNode;
        var parentDiv = removeDiv.parentNode;
        $(parentDiv).css('display', 'none');
        parentDiv.removeChild(removeDiv);
        clearTimeout(this.timeout);

        if ($('#favoriteBetType') && $('#favoriteBetType').css('display') == 'none' &&
        $('#favoriteChoice') && $('#favoriteChoice').css('display') == 'none') {
            $('#detailTitle2').css('display', 'none');
        }
        else if ($('#topSelectionByCurrency') && $('#topSelectionByCurrency').css('display') == 'none' &&
        $('#topSelectionByLeague') && $('#topSelectionByLeague').css('display') == 'none') {
            $('#detailTitle').css('display', 'none');
        }

        if ($('#popularEvent') && $('#popularEvent').css('display') == 'none') {
            $('#detailTitle2').css('display', 'none');
        }
    })
    .css({
        position: 'relative',
        opacity: 0.75,
        cursor: 'pointer'
    })
    .attr({
        zIndex: 100,
        title: "Close"
    })
    .add();
    this.closeButton = closeButton;
}

function onRedraw() {
    var x = this.chartWidth - $(this.closeButton.element).width();
    $(this.closeButton.element).position().left = x + 100;
}

function IsIncludeBettype(bettype) {
    if (bettype == 0)
        return false;

    return true;
} 