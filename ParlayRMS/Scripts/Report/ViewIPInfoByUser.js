$(function () {
    /*Function constant*/
    var ToogleSpeed = 150;

    /*Event binding*/
    /*Choice click */
    $(document).on('click', '.tdChoice', function () {
        if (CheckExpandStage(this.id)) {
            LoadReport(this.id, Expand);
        }
        else {
            Expand(null, this.id);
        }
    });
    /*System Parlay detail click*/
    $(document).on('click', '.tdDetailLink', function () {
        var Ids = this.id.split(',');
        ViewBetDetails(Ids[0], Ids[1]);
    });

    /*Check choice stage, return true if display:none.*/
    function CheckExpandStage(id) {
        return ($('#' + id + ".Expand").css('display') == 'none');
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
                $(".betdetails").css('overflow-y', 'auto');
                $("#simplemodal-container").width($("#simplemodal-container").width() + 20);
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
        var refno = id;

        //Validate input

        //Build query data
        var queryData = {
            refno: refno
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
        var ExpandNode = $('#' + id + '.Expand');
        if (content === '') {
            content = CreateNoInfoRow(1);
        }
        ExpandNode.html(content);
        ExpandNode.slideToggle(ToogleSpeed);
    }
});