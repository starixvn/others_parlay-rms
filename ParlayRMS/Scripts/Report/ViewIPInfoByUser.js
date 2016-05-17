$(function () {
    /*Function constant*/
    var toogleSpeed = 150;

    /*Event binding*/
    /*Choice click */
    $(document).on('click', '.tdChoice', function () {
        var id = this.getAttribute('data-id');
        if (CheckExpandStage(id)) {
            var betType = this.getAttribute('data-bet-type');
            var betCheck = this.getAttribute('data-bet-check');
            var betTeam = this.getAttribute('data-bet-team');
            var hdp1 = this.getAttribute('data-hdp1');
            var hdp2 = this.getAttribute('data-hdp2');
            LoadReport(id, betType, betCheck, betTeam, hdp1, hdp2, Expand);
        }
        else {
            Expand(null, id);
        }
    });
    /*System Parlay detail click*/
    $(document).on('click', '.tdDetailLink', function () {
        var id = this.getAttribute('data-id');
        ViewBetDetails(id);
    });

    /*Check choice stage, return true if display:none.*/
    function CheckExpandStage(id) {
        return ($('#exp_' + id + ".Expand").css('display') == 'none');
    }

    /*View bet details*/
    function ViewBetDetails(refNo) {
        var data = {
            RefNo: refNo
        }
        $.ajax({
            url: resolveClientUrl('ViewIPInfo/ParlayBetDetails'),
            type: 'POST',
            data: data,
            dataType: 'html',
            cache: false,
            beforeSend: function () {
                loading.show();
            },
            success: function (result) {
                modalPopup(result);
                $("#simplemodal-container").width($("#simplemodal-container").width() + 12);
            },
            error: function (xhr, status, err) {
                alertBox.showError(xhr.responseText);
            },
            complete: function () {
                loading.hide();
            }
        });
    }

    /*Load report from server and expand choice. */
    function LoadReport(refNo, betType, betCheck, betTeam, hdp1, hdp2, expand) {
        var queryData = {
            RefNo: refNo,
            betType: betType,
            betCheck: betCheck,
            betTeam: betTeam,
            hdp1: hdp1,
            hdp2: hdp2
        };

        $.ajax({
            url: resolveClientUrl('ViewIPInfo/ParlayBetSubTickets'),
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
                expand(json.data, refNo);
            },
            error: function (xhr, status, err) {
                alertBox.showError(xhr);
            },
            complete: function () {
                loading.hide();
            }
        });
    }

    /*Expand choice*/
    var Expand = function (content, id) {
        var expandNode = $('#exp_' + id + '.Expand');
        if (content === '') {
            content = '</br><div class="noinfo" style="text-align: right">' + Language.NoInformation + '</div>';
        }
        expandNode.html(content);
        expandNode.slideToggle(toogleSpeed);
        expandNode.parent().find('.combo-type').slideToggle(toogleSpeed);
    }
});