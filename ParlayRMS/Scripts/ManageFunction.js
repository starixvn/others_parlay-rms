$(function () {
    $("#btnNewFunction").addClass("button");
    ParlayRMS.Core.ResolvePermission();
});

function CreateFunction() {
    $.ajax({
        url: resolveClientUrl("ManageFunction/CreateFunction"),
        type: 'GET',
        dataType: 'html',
        cache: false,
        beforeSend: function () {
            loading.show();
        },
        success: function (data) {
            popup.show(data);
        },
        error: function (xhr, status, err) {
            alertBox.showError(xhr);
        },
        complete: function () {
            loading.hide();
        }
    });
}

function CreateFunctionPost() {
    if (validator.validateRequired("#Name") || validator.validateRequired("#Key") || validator.validateRequired("#DisplayOrder") || validator.validateInt("#DisplayOrder")) {
        return;
    }
    var data = {
        Name: $.trim($("#Name").val()),
        Key: $.trim($("#Key").val()),
        Description: $.trim($("#Description").val()),
        ParentId: $.trim($("#ParentId option:selected").val()),
        ParentName: $.trim($("#ParentId option:selected").text()),
        DisplayOrder: $.trim($("#DisplayOrder").val())
    };
    $.ajax({
        url: resolveClientUrl("ManageFunction/CreateFunction"),
        type: 'POST',
        data: data,
        dataType: 'html',
        cache: false,
        beforeSend: function (xhr) {
            popup.showLoading();
        },
        success: function (result) {
            var json = $.parseJSON(result);
            if (json.code != 0) {
                popup.showErrorMessage(json.message);
                return;
            }
            navigateToUrl('ManageFunction');
        },
        error: function (xhr, status, err) {
            alertBox.showError(xhr);
        },
        complete: function () {
            popup.hideLoading();
        }
    });
}

function EditFunction(functionId) {
    var data = {
        FunctionId: functionId
    }
    $.ajax({
        url: resolveClientUrl("ManageFunction/EditFunction"),
        data: data,
        type: 'GET',
        dataType: 'html',
        cache: false,
        beforeSend: function () {
            loading.show();
        },
        success: function (data) {
            popup.show(data);
        },
        error: function (xhr, status, err) {
            alertBox.showError(xhr);
        },
        complete: function () {
            loading.hide();
        }
    });
}

function EditFunctionPost() {
    if (validator.validateRequired("#Name") || validator.validateRequired("#DisplayOrder") || validator.validateInt("#DisplayOrder")) {
        return;
    }
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
        url: resolveClientUrl("ManageFunction/EditFunction"),
        type: 'POST',
        data: data,
        dataType: 'html',
        cache: false,
        beforeSend: function (xhr) {
            popup.showLoading();
        },
        success: function (result) {
            var json = $.parseJSON(result);
            if (json.code != 0) {
                popup.showErrorMessage(json.message);
                return;
            }
            navigateToUrl('ManageFunction');
        },
        error: function (xhr, status, err) {
            alertBox.showError(xhr);
        },
        complete: function () {
            popup.hideLoading();
        }
    });
}