$(function () {
    /*Function constant*/
    var toogleSpeed = 150;

    /*Event binding*/
    /*Choice click */
    $(document).on('click', '.tdChoice', function () {
        var id = this.getAttribute('data-id');
        if (CheckExpandStage(id)) {
            LoadReport(id, Expand);
        }
        else {
            Expand(null, id);
        }
    });
    /*System Parlay detail click*/
    $(document).on('click', '.tdDetailLink', function () {
        var id = this.getAttribute('data-id');
        var combo = this.getAttribute('data-combo');
        ViewBetDetails(id, combo);
    });

    /*Check choice stage, return true if display:none.*/
    function CheckExpandStage(id) {
        return ($('#exp_' + id + ".Expand").css('display') == 'none');
    }

    /*View bet details*/
    function ViewBetDetails(refNo, comboType) {
        var data = {
            RefNo: refNo,
            ComboType: comboType
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
    function LoadReport(id, expand) {
        //get input
        var refNo = id;

        //Validate input

        //Build query data
        var queryData = {
            RefNo: refNo
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
                expand(json.data, id);

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
    }
});