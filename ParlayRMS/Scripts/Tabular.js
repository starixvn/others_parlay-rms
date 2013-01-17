var valueChanged = null;

var tableSum = null;
var tableTicket = null;
var tableTurnover = null;

var tabularTopSelectionByTicket;
var tabularTopLeagueByTicket;
var tabularTopCurrencyByTicket;
var tabularTopSelectionByTurnover;
var tabularTopLeagueByTurnover;
var tabularTopCurrencyByTurnover;

$(document).ready(function () {
    valueChanged = new ValueChanged();

    tabularTopSelectionByTicket = new SummaryTabular("#topSelectionByTicket", "Monitoring/Tabular_TopSelectionByTicket", GetSummaryData, 'Loading_Tabular_TopSelectionByTicket');
    tabularTopLeagueByTicket = new Tabular("#topSelectionByLeague", "Monitoring/Tabular_TopLeagueByTicket", DataDetailGetting, 'Loading_Tabular_League', 'leagueTitle', _resource.topLeagues);
    tabularTopCurrencyByTicket = new Tabular("#topSelectionByCurrency", "Monitoring/Tabular_TopCurrencyByTicket", DataDetailGetting, 'Loading_Tabular_Currency', 'currencyTitle', _resource.topCurrencies);
    tabularTopSelectionByTurnover = new SummaryTabular("#topSelectionByTurnover", "Monitoring/Tabular_TopSelectionByTurnover", GetSummaryData, 'Loading_Tabular_TopSelectionByTurnover');
    tabularTopLeagueByTurnover = new Tabular("#topSelectionByLeague", "Monitoring/Tabular_TopLeagueByTurnover", DataDetailGetting, 'Loading_Tabular_League', 'leagueTitle', _resource.topLeagues);
    tabularTopCurrencyByTurnover = new Tabular("#topSelectionByCurrency", "Monitoring/Tabular_TopCurrencyByTurnover", DataDetailGetting, 'Loading_Tabular_Currency', 'currencyTitle', _resource.topCurrencies);

    HideTabulars();
});

function Tabular(containerId, url, dataHandler, spanLoading, titleId, topType) {
    var container = $(containerId);
    var url = url;
    var dataHandler = dataHandler;
    var timeout = null;
    var isLoad = false;

    this.show = function () {
        container.css('display', 'block');
        this.stop();
        this.load();
    };

    this.reload = function () {
        if (container.css('display') == 'block') {
            this.stop();
            this.load();
        }
    };

    this.hide = function () {
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

    this.highlighting = function (table) {
        var position = null;
        if (this instanceof SummaryTabular) {
            position = this.selectPosition(table);
        };
        table.tableHover(position);
    };

    var current = this;
    this.load = function () {
        if (isLoad == true) {
            return;
        }

        isLoad = true;
        seperateDiv.Show();

        var contentDiv = container.find(".tabularcontainer");

        if ($('#' + spanLoading).html() || $('#' + spanLoading).html() == '') {
            $('#' + spanLoading).showLoading();
        }

        var current = this;

        $.ajax({
            url: resolveClientUrl(url),
            data: dataHandler(),
            type: 'GET',
            dataType: 'html',
            cache: false,
            beforeSend: function () {
                loadingHelper.increaseTask();
            },
            success: function (result) {
                //                var newTable = null;

                if (contentDiv.length == 1) {
                    contentDiv.html($(result).find(".tabularcontainer").find("table"));
                }
                else {
                    container.html(result);
                }

                //                newTable = $(result).find(".tabularcontainer").find("table");

                //                if (newTable[0].id == 'tblTicket') {
                //                    valueChanged.showValueChanged(1, document.getElementById(newTable[0].id));
                //                }
                //                else if (newTable[0].id == 'tblTurnover') {
                //                    valueChanged.showValueChanged(2, document.getElementById(newTable[0].id));
                //                }

                if (titleId != null && topType != null) {
                    $('#' + titleId).html(dataSelected.Selection + ' ' + _resource.selections + ' - ' + dataSelected.SportType + ' - ' + topType);
                }

                var table = container.find("table");
                current.highlighting(table);
                $('#' + spanLoading).html('');

            },
            error: function () {
                container.showError();
                $('#' + spanLoading).html('');
            },
            complete: function () {
                isLoad = false;
                loadingHelper.decreaseTask();
            }
        });
    };
}

function SummaryTabular(containerId, url, dataHandler, spanLoading, titleId, topType) {
    this.Selection = -1;
    this.SportType = null;

    this.select = function (selection, sportType) {
        var container = $(containerId);
        var table = container.find("table");

        var position = this.selectPosition(table);
        if (position != null) {
            table.cellDeselect(position.Row, position.Col);
        };

        this.Selection = selection;
        this.SportType = sportType;
        position = this.selectPosition(table);
        table.cellSelect(position.Row, position.Col);
    };

    this.deselect = function (selection, sportType) {
        var container = $(containerId);
        var table = container.find("table");
        var position = this.selectPosition(table);
        if (position != null) {
            table.cellDeselect(position.Row, position.Col);
        };

        this.Selection = -1;
        this.SportType = null;
    };

    this.dataSelected = function () {
        return {
            Selection: this.Selection,
            SportType: this.SportType,

            isValid: function () {
                if (this.Selection != -1 && this.SportType != null) {
                    return true;
                }

                return false;
            }
        };
    };

    this.selectPosition = function (table) {
        var row = -1;
        var col = -1;
        var data = this.dataSelected();
        if (data.isValid()) {
            var firstRow = $(table).find("tr:first");
            firstRow.children("th").each(function (index, element) {
                var selection = parseInt($.trim($(element).text()));
                if (selection === data.Selection) {
                    col = index;
                    return;
                };
            });

            var firstTds = $(table).find("tr th:nth-child(1)");
            firstTds.each(function (index, element) {
                var sportType = $.trim($(element).text());
                if (sportType.toLowerCase() === data.SportType.toLowerCase()) {
                    row = index;
                    return;
                };
            });
        };

        if (row == -1 || col == -1)
            return null;

        return {
            Row: row,
            Col: col
        };
    };

    Tabular.call(this, containerId, url, dataHandler, spanLoading, titleId, topType);
};

function GetDataSum() {
    var isFirst = true;
    var container = $('#dataSum');
    var timeout = null;
    var data = {
        bettype: filterData.BetType
    };
    var url = 'TopSelection/DataSum';
    var isLoad = false;

    filterData.update();

    // 0: NoThing, 1: All, 2: TopSelection.
    this.RequestType = 1;

    this.show = function () {
        load();
    }

    this.reload = function () {
        if (timeout !== null) {
            clearTimeout(timeout);
            timeout = null;
        }
        load();
    };

    var load = function () {
        if (timeout !== null) {
            clearTimeout(timeout);
            timeout = null;
        }

        if (isLoad === true) {
            return;
        }
        isLoad = true;

        if ($('#waiting').html() || $('#waiting').html() == '') {
            $('#waiting').showLoading();
        }

        $.ajax({
            url: resolveClientUrl(url),
            type: 'GET',
            data: data,
            dataType: 'html',
            cache: false,
            beforeSend: function () {
                loadingHelper.increaseTask();
            },
            success: function (result) {
                //                var newTable = document.getElementById('tblSum');

                //                valueChanged.showValueChanged(0, newTable);

                $('#waiting').html('');
                container.html(result);

                if (getDataSum.RequestType === 0) {
                    getDataSum.RequestType = 1;
                }
                else if (getDataSum.RequestType === 1) {
                    if (filterData.IsChart) {
                        ReloadCharts(dataSelected.IsTicket);
                    }
                    else {
                        ReloadTabulars(dataSelected.IsTicket);
                    }
                }
                else {
                    if (filterData.IsChart) {
                        if ($("input[id='rdbfavouritechoice']:checked").val() === "on") {
                            chartTopFavouriteByTicket.show();
                            chartTopFavouriteByTurnover.show();
                        }
                        else {
                            chartTopSelectionByTicket.show();
                            chartTopSelectionByTurnover.show();
                        }
                    }
                    else {
                        tabularTopSelectionByTicket.show();
                        tabularTopSelectionByTurnover.show();
                    }

                    getDataSum.RequestType = 1;
                }

                if (filterData.RefreshTime != 0) {
                    timeout = setTimeout(function () {
                        load();
                    }, filterData.RefreshTime);
                }
            },
            error: function () {
                container.showError();
            },
            complete: function () {
                isLoad = false;
                loadingHelper.decreaseTask();
            }
        });
    }
}

function ProcessTabularByTicket(selection, sportType, obj) {
    $('#detailTitle').css('display', 'block');
    $('#detailFrom').html(_resource.ticket);

    //dataSelected.set(true, selection, sportType, true);
    dataSelected.setIsTicket(true);
    dataSelected.setSelection(selection);
    dataSelected.setSportType(sportType);
    dataSelected.setIsSelection(true);

    tabularTopSelectionByTicket.select(selection, sportType);
    tabularTopSelectionByTurnover.deselect(selection, sportType);

    tabularTopCurrencyByTurnover.hide();
    tabularTopLeagueByTurnover.hide();

    tabularTopSelectionByTicket.show();
    tabularTopSelectionByTurnover.show();

    tabularTopCurrencyByTicket.show();
    tabularTopLeagueByTicket.show();

    ReloadSummary();
}

function ProcessTabularByTurnover(selection, sportType, obj) {
    $('#detailTitle').css('display', 'block');
    $('#detailFrom').html(_resource.turnover);

    //dataSelected.set(false, selection, sportType, true);
    dataSelected.setIsTicket(false);
    dataSelected.setSelection(selection);
    dataSelected.setSportType(sportType);
    dataSelected.setIsSelection(true);

    tabularTopSelectionByTurnover.select(selection, sportType);
    tabularTopSelectionByTicket.deselect(selection, sportType);

    tabularTopCurrencyByTicket.hide();
    tabularTopLeagueByTicket.hide();

    tabularTopSelectionByTicket.show();
    tabularTopSelectionByTurnover.show();

    tabularTopCurrencyByTurnover.show();
    tabularTopLeagueByTurnover.show();

    ReloadSummary();
}

function HideTabulars() {
    tabularTopSelectionByTicket.hide();
    tabularTopCurrencyByTicket.hide();
    tabularTopLeagueByTicket.hide();
    tabularTopSelectionByTurnover.hide();
    tabularTopCurrencyByTurnover.hide();
    tabularTopLeagueByTurnover.hide();
}

function ClearTabulars() {
    tabularTopSelectionByTicket.clear();
    tabularTopCurrencyByTicket.clear();
    tabularTopLeagueByTicket.clear();
    tabularTopSelectionByTurnover.clear();
    tabularTopCurrencyByTurnover.clear();
    tabularTopLeagueByTurnover.clear();
}

function ReloadTabulars(IsTicket) {
    tabularTopSelectionByTicket.reload();
    tabularTopSelectionByTurnover.reload();

    if (IsTicket) {
        tabularTopCurrencyByTicket.reload();
        tabularTopLeagueByTicket.reload();

        tabularTopCurrencyByTurnover.stop();
        tabularTopLeagueByTurnover.stop();
    }
    else {
        tabularTopCurrencyByTurnover.reload();
        tabularTopLeagueByTurnover.reload();

        tabularTopCurrencyByTicket.stop();
        tabularTopLeagueByTicket.stop();
    }
}

function ValueChanged() {
    this.showValueChanged = function (oldTable, _newTable) {
        var table = null;
        //Gets previous table
        switch (oldTable) {
            case 0:
                table = tableSum;
                break;
            case 1:
                table = tableTicket;
                break;
            case 2:
                table = tableTurnover;
                break;
        }
        //Displays blinking background color for value changed
        if (table != null) {
            this.displayValueChanged(table, _newTable);
        }
        //Set old table to the current table
        switch (oldTable) {
            case 0:
                tableSum = _newTable;
                break;
            case 1:
                tableTicket = _newTable;
                break;
            case 2:
                tableTurnover = _newTable;
                break;
        }
    };

    this.displayValueChanged = function (oldTbl, newTbl) {
        if (newTbl != null) {
            for (var i = 0, row; row = newTbl.rows[i]; i++) {
                for (var j = 0, col; col = row.cells[j]; j++) {
                    if (col.innerHTML != oldTbl.rows[i].cells[j].innerHTML) {
                        col.className = 'valueChanged';
                    }
                }
            }
        }
    };
}

SummaryTabular.prototype = new Tabular();