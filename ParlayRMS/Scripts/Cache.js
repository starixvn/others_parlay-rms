function ProcessCache() {
    this.cancel = function () {
        setCacheTopSelection.cancel();
        if (dataSelected.IsSelectionDetail) {
            setCacheEvent.cancel();
        }
    };
    this.load = function () {
        setCacheTopSelection.set();
    };
}

var isCacheEnabled = false;

function enableCache(status) {
    isCacheEnabled = status;
}

function SetCacheTopSelection() {
    var timeout = 0;
    var jqxhr = null;
    var data = {
        bettype: filterData.BetType
    };
    this.set = function () {
        if (timeout != 0) {
            clearTimeout(timeout);
            timeout = 0;
        }
        load();
    };
    this.cancel = function () {
        if (jqxhr != null) {
            jqxhr.abort();
        }
    };
    var load = function () {
        var x = {
            bettype: filterData.BetType
        };
        if (dataSelected.IsSelection) {
            x = {
                bettype: filterData.BetType,
                isticket: dataSelected.IsTicket,
                selection: dataSelected.Selection,
                sportType: dataSelected.SportType,
                currency: dataSelected.CurrencyId,
                topcurrency: dataSelected.TopCurrency,
                leagueId: dataSelected.LeagueId,
                isSelectDetail: dataSelected.IsSelectionDetail,
                isEvent: dataSelected.IsEvent,
                isBettype: dataSelected.IsBettype,
                isChoice: dataSelected.IsChoice,
                bettype2: dataSelected.BetType2,
                matchid: dataSelected.MatchId
            };
        }

        jqxhr = $.ajax({
            url: resolveClientUrl('Cache/SetTopSelection'),
            data: x,
            type: 'GET',
            dataType: 'html',
            cache: false,
            success: function (data) {
                if (data == "True") {
                    timeout = setTimeout(processCache.load, filterData.CacheTime);
                }
            },
            error: function (xhr, status, err) {
            }
        });
    };
}

function SetCacheSelectionByTicket() {
    var jqxhr = null;
    this.set = function (dataHandler) {
        load(dataHandler);
    };
    this.cancel = function () {
        if (jqxhr != null) {
            jqxhr.abort();
        }
    };
    var load = function (dataHandler) {
        jqxhr = $.ajax({
            url: resolveClientUrl('Cache/SetSelectionByTicket'),
            data: dataHandler,
            type: 'GET',
            dataType: 'html',
            cache: false,
            success: function (data) {
            },
            error: function (xhr, status, err) {
            }
        });
    };
}

function SetCacheSelectionByTurnover() {
    var jqxhr = null;
    this.set = function (dataHandler) {
        load(dataHandler);
    };
    this.cancel = function () {
        if (jqxhr != null) {
            jqxhr.abort();
        }
    };
    var load = function (dataHandler) {
        jqxhr = $.ajax({
            url: resolveClientUrl('Cache/SetSelectionByTurnover'),
            data: dataHandler,
            type: 'GET',
            dataType: 'html',
            cache: false,
            success: function (data) {
            },
            error: function (xhr, status, err) {
            }
        });
    };
}