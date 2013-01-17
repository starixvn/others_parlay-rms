/* File Created: May 21, 2012 */

var RefreshTime = function () {
    var timeout = 0;
    var counter = 0;
    this.start = function (id) {
        if (filterData.RefreshTime > 0) {
            counter = filterData.RefreshTime / 1000;
            ChangeTime(id);
        }
        else {
            $(id).html("pause");
        }
    };
    this.stop = function () {
        if (timeout != 0) {
            clearTimeout(timeout);
            timeout = 0;
            counter = 0;
        }
    };
    this.restart = function (id) {
        if (counter != 0) {
            this.stop();
        }
        this.start(id);
    };
    var ChangeTime = function (id) {
        timeout = setTimeout(function () {
            counter--;
            $(id).html(counter + "s");
            if (counter <= 0) {
                counter = filterData.RefreshTime / 1000;
            }
            ChangeTime(id);
        }
        , 1000);
    };
};

var setCacheTopSelection = new SetCacheTopSelection(); ;
var setCacheSelectionByTicket = new SetCacheSelectionByTicket();
var setCacheSelectionByTurnover = new SetCacheSelectionByTurnover();
var processCache = new ProcessCache();
var getDataSum;

var loadingHelper = new LoadingHelper();
var seperateDiv = new SeperateDiv();
var refreshTime = new RefreshTime();

var maxTransId = 0;

function UpdateMaxTransId(transId) {
    maxTransId = transId;
};

$(document).ready(function () {
    seperateDiv.Hide();

    $("#selRefreshTime").change(function () {
        filterData.RefreshTime = parseInt($(this).val());
        refreshTime.restart("#spRefreshTime");

        if (filterData.RefreshTime > 0 && filterData.RefreshTime < 30000) {
            filterData.RefreshTime = 30000;
        }

        getDataSum.reload();

        if (filterData.IsChart) {
            ReloadCharts(dataSelected.IsTicket);
        }
        else {
            ReloadTabulars(dataSelected.IsTicket);
        }
    });

    //    $("#btnrefreshnow").click(function () {
    //        refreshTime.restart("#spRefreshTime");

    //        getDataSum.reload();

    //        if (filterData.IsChart) {
    //            ReloadCharts(selectData.IsTicket);
    //        }
    //        else {
    //            ReloadTabulars(selectData.IsTicket);
    //        }
    //    });

    filterData.update();

    getDataSum = new GetDataSum();
    getDataSum.show();

    processCache.cancel();
    processCache.load();

    //refreshTime.start("#spRefreshTime");
});

function LoadingHelper() {
    var count = 1;
    this.Run = function (_count) {
        count = _count;
        if (count < 0) {
            count = 0;
        }
        if (count > 0) {
            loading.show();
            // $("#loadingDetail").showLoading();
        }
    };
    this.Stop = function () {
        loading.hide();
    };
    this.Finish = function () {
        count--;
        if (count < 0) {
            count = 0;
        }
        if (count === 0) {
            loading.hide();
            //$("#loadingDetail").html('');
        }
    };
    this.IsCompleted = function () {
        return (count === 0);
    };
}

function SeperateDiv() {
    this.Show = function () {
        $('#seperateDiv').css('display', 'block');
    };
    this.Hide = function () {
        $('#seperateDiv').css('display', 'none');
    };
}

function RenderChartByFilter(isTicket) {
    if ($('#back')) {
        $('#back').hide();
        $('#favoriteEvent').hide();

        chartTopSelectionByTicketEventBetType.hide();
        chartTopSelectionByTicketEventBetTypeChoice.hide();

        chartTopSelectionByTurnoverEventBetType.hide();
        chartTopSelectionByTurnoverEventBetTypeChoice.hide();
    }

    filterData.update();
    filterData.CurSelectedChart = filterData.IsChart ? 1 : 0;

    //selectData.IsSelection = false;
    dataSelected.reNew();
    dataSelected.setIsSelection(false);

    if (filterData.PreSelectedChart != filterData.CurSelectedChart) {
        filterData.PreSelectedChart = filterData.CurSelectedChart;
        filterData.clearAll();
    }
    else {
        filterData.clearAreas();
    }

    $('#detailTitle').css('display', 'none');
    $('#detailTitle2').css('display', 'none');

    loading.show();

    tabularTopSelectionByTurnover.deselect(dataSelected.Selection, dataSelected.SportType);
    tabularTopSelectionByTicket.deselect(dataSelected.Selection, dataSelected.SportType);

    getDataSum.reload();
    getDataSum.RequestType = 2;
}

function ReloadSummary() {
    if (isCacheEnabled) {
        getDataSum.RequestType = 0;
        getDataSum.reload();

        if (dataSelected.IsTicket) {
            if (filterData.IsChart) {
                chartTopSelectionByTicket.reload();
            }
            else {
                tabularTopSelectionByTicket.reload();
            }
        }
        else {
            if (filterData.IsChart) {
                chartTopSelectionByTurnover.reload();
            }
            else {
                tabularTopSelectionByTurnover.reload();
            }
        }
    }
}