$(function () {
    $("#btnSearch").addClass("button");

    $("#betId").keypress(function (e) {
        switch (e.keyCode) {
            case 13: // enter
                $("#btnSearch").click();
                break;
        }
    });

    $('#btnSearch').click(function () {
        var transId = $.trim($('#betId').val().replace(/,/g, ''));
        if (transId == '') {
            Validators.show($('#betId').get(0), 'This field is required');
            return;
        }

        if (!isNumber(transId) || parseInt(transId) < 0) {
            Validators.show($("#betId").get(0), 'BetID must be greater or than 0');
            return;
        }

        var data = {
            TransId: transId
        }
        var container = $('#divResult');

        $.ajax({
            url: resolveClientUrl('SimilarBets/Data'),
            type: 'POST',
            data: data,
            dataType: 'html',
            cache: false,
            beforeSend: function (xhr) {
                loading.show();
            },
            success: function (result) {
                container.html(result);
                var table = $('#similarbetlist').find('table');
                var theadTable = table.filter('.thead');
                var tbodyTable = table.filter('.tbody');
                tbodyTable.children('tbody').children("tr:even").addClass("even");
                tableSorter.bind2(theadTable, tbodyTable, {
                    type: 'float',
                    onFinish: function () {
                        var trs = tbodyTable.children('tbody').children("tr");
                        trs.removeClass("even");
                        trs.filter(':even').addClass("even");
                        trs.each(function (index, tr) {
                            $(tr).children('td:first').text(index + 1);
                        });
                    }
                });
            },
            error: function () {
                var messageError = "<div class='error'>Ticket " + transId + " is not found</div>";
                container.html(messageError);
            },
            complete: function () {
                container.show();
                loading.hide();
            }
        });
    });

    if ($('#betId').val() != '') {
        $('#btnSearch').click();
    }
});

function ViewSimilarBets() {
    if ($('#similarbetlist').is(":visible")) {
        $('#similarbetlist').hide();
    }
    else {
        $('#similarbetlist').hide().slideToggle("slow");
    }
}

function CloseSimilarBets() {
    $('#similarbetlist').hide();
}

function ViewBetDetails(username, transId, refNo, originalRefNo) {
    var data = {
        BetId: transId,
        Username: username,
        RefNo: refNo,
        OriginalRefNo: originalRefNo
    }

    $.ajax({
        url: resolveClientUrl('SimilarBets/BetDetails'),
        type: 'POST',
        data: data,
        dataType: 'html',
        cache: false,
        beforeSend: function () {
            loading.show();
        },
        success: function (data) {
            modalPopup(data);
            $(".betdetails").css('overflow-y', 'auto');
            $("#simplemodal-container").width($("#simplemodal-container").width() + 20);
        },
        error: function (xhr, status, err) {
            loading.hide();
            alertBox.showError(xhr);
        },
        complete: function () {
            loading.hide();
        }
    });
}