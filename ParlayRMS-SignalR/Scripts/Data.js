var filterData = {
    IsChart: true,
    BetType: 9,
    RefreshTime: 30000,
    Dimension: "selection",
    CacheTime: 10000,
    PreSelectedChart: 1,
    CurSelectedChart: 1,
    flash: 0,

    update: function () {
        this.IsChart = ($("input[id='rdbchart']:checked").val() === "on");
        this.BetType = $("#selBetType").val();
        this.RefreshTime = parseInt($("#selRefreshTime").val());
        if (this.RefreshTime > 0 && this.RefreshTime < 30000) {
            this.RefreshTime = 30000;
        }
    },

    clearAreas: function () {
        chartTopCurrencyByTicket.clear();
        chartTopCurrencyByTurnover.clear();
        chartTopLeagueByTicket.clear();
        chartTopLeagueByTurnover.clear();
        chartTopEventCurrencyByTicket.clear();
        chartTopEventLeagueByTicket.clear();

        tabularTopCurrencyByTicket.clear();
        tabularTopCurrencyByTurnover.clear();
        tabularTopLeagueByTicket.clear();
        tabularTopLeagueByTurnover.clear();

        $('#topSelectionByCurrency').html('');
        $('#topSelectionByLeague').html('');
        $('#popularEvent').html('');
    },

    clearAll: function () {
        chartTopCurrencyByTicket.clear();
        chartTopCurrencyByTurnover.clear();
        chartTopLeagueByTicket.clear();
        chartTopLeagueByTurnover.clear();
        chartTopSelectionByTicket.clear();
        chartTopSelectionByTurnover.clear();
        chartTopEventCurrencyByTicket.clear();
        chartTopEventLeagueByTicket.clear();

        tabularTopCurrencyByTicket.clear();
        tabularTopCurrencyByTurnover.clear();
        tabularTopLeagueByTicket.clear();
        tabularTopLeagueByTurnover.clear();
        tabularTopSelectionByTicket.clear();
        tabularTopSelectionByTurnover.clear();

        $('#topSelectionByTicket').html('');
        $('#topSelectionByTurnover').html('');
        $('#topSelectionByCurrency').html('');
        $('#topSelectionByLeague').html('');
        $('#popularEvent').html('');
    }
};

var dataSelected = {
    IsTicket: true,
    Selection: 2,
    SportType: "All Sports",
    IsSelection: false,
    LeagueId: 0,
    LeagueName: null,
    CurrencyId: -1,
    CurrencyName: null,
    TopCurrency: null,
    IsSelectionDetail: false,
    BetType2: -1,
    IsFavourite: false,
    MatchId: 0,
    MatchName: null,
    BetTypeName: null,
    HomeId: 0,
    AwayId: 0,
    IsEvent: false,
    IsBettype: false,
    IsChoice: false,
    IsApplicable: false,

    reNew: function () {
        this.IsTicket = true;
        this.Selection = 2;
        this.SportType = "All Sports";
        this.IsSelection = false;
        this.LeagueId = 0;
        this.LeagueName = null;
        this.CurrencyId = -1;
        this.CurrencyName = null;
        this.TopCurrency = null;
        this.IsSelectionDetail = false;
        this.BetType2 = -1;
        this.IsFavourite = false;
        this.MatchId = 0;
        this.MatchName = null;
        this.BetTypeName = null;
        this.HomeId = 0;
        this.AwayId = 0;
        this.IsEvent = false;
        this.IsBettype = false;
        this.IsChoice = false;
        this.IsApplicable = false;
    },
    setIsTicket: function (value) {
        this.IsTicket = value;
    },
    setSelection: function (value) {
        this.Selection = value;
    },
    setSportType: function (value) {
        this.SportType = value;
    },
    setIsSelection: function (value) {
        this.IsSelection = value;
    },
    setLeagueId: function (value) {
        this.LeagueId = value;
    },
    setLeagueName: function (value) {
        this.LeagueName = value;
    },
    setCurrencyId: function (value) {
        this.CurrencyId = value;
    },
    setCurrencyName: function (value) {
        this.CurrencyName = value;
    },
    setTopCurrency: function (value) {
        this.TopCurrency = value;
    },
    setIsSelectionDetail: function (value) {
        this.IsSelectionDetail = value;
    },
    setBettype2: function (value) {
        this.BetType2 = value;
    },
    setIsFavourite: function (value) {
        this.IsFavourite = value;
    },
    setMatchId: function (value) {
        this.MatchId = value;
    },
    setMatchName: function (value) {
        this.MatchName = value;
    },
    setBettypeName: function (value) {
        this.BetTypeName = value;
    },
    setHomeId: function (value) {
        this.HomeId = value;
    },
    setAwayId: function (value) {
        this.AwayId = value;
    },
    setIsEvent: function (value) {
        this.IsEvent = value;
    },
    setIsBettype: function (value) {
        this.IsBettype = value;
    },
    setIsChoice: function (value) {
        this.IsChoice = value;
    },
    setIsApplicable: function (value) {
        this.IsApplicable = value;
    }
};

function DataDetailGetting() {
    return {
        selection: dataSelected.Selection,
        sporttype: dataSelected.SportType,
        currencyId: dataSelected.CurrencyId,
        currencyName: dataSelected.CurrencyName,
        leagueId: dataSelected.LeagueId,
        leagueName: dataSelected.LeagueName,
        topCurrency: dataSelected.TopCurrency,
        betType2: dataSelected.BetType2,
        matchId: dataSelected.MatchId,
        matchName: dataSelected.MatchName,
        betTypeName: dataSelected.BetTypeName,
        homeId: dataSelected.HomeId,
        awayId: dataSelected.AwayId,
        maxTransId: maxTransId
    };
}

function GetSummaryData() {
    return {
        maxTransId: maxTransId
    };
}