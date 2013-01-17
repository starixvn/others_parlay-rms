function createFunction() {
    var url = "ManageFunction/CreateFunction";
    var title = "Create Function";
    $.ajax({
        url: resolveClientUrl(url),
        type: 'GET',
        dataType: 'html',
        cache: false,
        beforeSend: function () {
            loading.show();
        },
        success: function (data) {
            modalPopup(data);
        },
        error: function (xhr, status, err) {
            alertBox.showError(xhr);
        },
        complete: function () {
            loading.hide();
        }
    });
}

function SaveNewFunction() {
    if ($.trim($('#Name').val()) == '' || $.trim($('#Name').val()) == 'This field is required') {
        Validators.show($('#Name').get(0), 'This field is required');
        return;
    };
    if ($.trim($('#Key').val()) == '' || $.trim($('#Key').val()) == 'This field is required') {
        Validators.show($('#Key').get(0), 'This field is required');
        return;
    };
    if ($.trim($('#DisplayOrder').val()) == '' || $.trim($('#DisplayOrder').val()) == 'This field is required') {
        Validators.show($('#DisplayOrder').get(0), 'This field is required');
        return;
    };
    var displayOrder = $.trim($('#DisplayOrder').val());
    if (!isNumeric(displayOrder) || parseInt(displayOrder) < 0) {
        Validators.show($('#DisplayOrder').get(0), 'Order value must be greater or than 0');
        return;
    };

    var url = "ManageFunction/SaveNewFunction";
    var data = {
        Name: $.trim($("#Name").val()),
        Key: $.trim($("#Key").val()),
        Description: $.trim($("#Description").val()),
        ParentId: $.trim($("#Parent option:selected").val()),
        ParentName: $.trim($("#Parent option:selected").text()),
        DisplayOrder: $.trim($("#DisplayOrder").val())
    };

    $.ajax({
        url: resolveClientUrl(url),
        type: 'POST',
        data: data,
        dataType: 'html',
        cache: false,
        beforeSend: function (xhr) {
            loadingInPopup.show();
            messagePopup.hide();
        },
        success: function (result) {
            var json = $.parseJSON(result);
            if (json.code != 0) {
                loadingInPopup.hide();
                messagePopup.show(json.message);
            }
            else {
                navigateToUrl('ManageFunction');
            }
        },
        error: function (xhr, status, err) {
            alertBox.showError(xhr);
        }
    });
}

function EditFunction(functionId) {
    var url = "ManageFunction/EditFunction?functionId=" + functionId;
    var title = "Edit Function";
    $.ajax({
        url: resolveClientUrl(url),
        type: 'GET',
        dataType: 'html',
        cache: false,
        beforeSend: function () {
            loading.show();
        },
        success: function (data) {
            modalPopup(data);
        },
        error: function (xhr, status, err) {
            alertBox.showError(xhr);
        },
        complete: function () {
            loading.hide();
        }
    });
}

function ShowErrorMessage(responseText) {
    var json = $.parseJSON(responseText);
    if (json.code == 401) {
        messageBox('Error', json.message);
    }
    else {
        messageBox('Error', 'Error on edit menu!');
    }
}

function SaveUpdateFunction() {
    if ($.trim($('#Name').val()) == '' || $.trim($('#Name').val()) == 'This field is required') {
        Validators.show($('#Name').get(0), 'This field is required');
        return;
    };
    if ($.trim($('#DisplayOrder').val()) == '' || $.trim($('#DisplayOrder').val()) == 'This field is required') {
        Validators.show($('#DisplayOrder').get(0), 'This field is required');
        return;
    };
    var displayOrder = $.trim($('#DisplayOrder').val());
    if (!isNumeric(displayOrder) || parseInt(displayOrder) < 0) {
        Validators.show($('#DisplayOrder').get(0), 'Order value must be greater or than 0');
        return;
    };

    var url = "ManageFunction/SaveUpdateFunction";
    var data = {
        Id: $.trim($("#Id").val()),
        Name: $.trim($("#Name").val()),
        Key: $.trim($("#Key").val()),
        Description: $.trim($("#Description").val()),
        ParentId: $.trim($("#ParentId option:selected").val()),
        ParentName: $.trim($("#ParentId option:selected").text()),
        DisplayOrder: $.trim($("#DisplayOrder").val())
    };

    $.ajax({
        url: resolveClientUrl(url),
        type: 'POST',
        data: data,
        dataType: 'html',
        cache: false,
        beforeSend: function (xhr) {
            loadingInPopup.show();
            messagePopup.hide();
        },
        success: function (result) {
            var json = $.parseJSON(result);
            if (json.code != 0) {
                loadingInPopup.hide();
                messagePopup.show(json.message);
            }
            else {
                navigateToUrl('ManageFunction');
            }
        },
        error: function (xhr, status, err) {
            alertBox.showError(xhr);
        }
    });
}

$(function () {
    $("#btnNewFunction").addClass("button");
});