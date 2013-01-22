function CreateMenu() {
    $.ajax({
        url: resolveClientUrl("ManageMenu/CreateMenu"),
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

function CreateMenuPost() {
    if (validator.validateRequired("#Name") || validator.validateRequired("#DisplayOrder") || validator.validateInt("#DisplayOrder")) {
        return;
    }
    var data = {
        Name: $.trim($("#Name").val()),
        Url: $.trim($("#Url").val()),
        Description: $.trim($("#Description").val()),
        FunctionId: $.trim($("#Function option:selected").val()),
        ParentId: $.trim($("#ParentId option:selected").val()),
        ParentName: $.trim($("#ParentId option:selected").text()),
        IsActive: false,
        DisplayOrder: $.trim($("#DisplayOrder").val())
    };
    $.ajax({
        url: resolveClientUrl("ManageMenu/CreateMenu"),
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
            navigateToUrl('ManageMenu');
        },
        error: function (xhr, status, err) {
            alertBox.showError(xhr);
        },
        complete: function () {
            popup.hideLoading();
        }
    });
}

function ToggleStatus(target, menuId, menuName) {
    var isActive = $(target).hasClass('active');
    var title = '';
    if (isActive) {
        title = 'Deactive Menu';
        content = 'Are you sure to deactivate [' + menuName + '] menu?';
    }
    else {
        title = 'Active Menu';
        content = 'Are you sure to activate [' + menuName + '] menu?';
    }
    confirm(title, content,
    function () {
        var data = {
            MenuId: menuId,
            IsActive: !isActive
        };
        $.ajax({
            url: resolveClientUrl('ManageMenu/UpdateStatus'),
            type: 'POST',
            data: data,
            dataType: 'html',
            cache: false,
            beforeSend: function (xhr) {
                loading.show();
            },
            success: function (result) {
                var json = $.parseJSON(result);
                if (json.code != 0) {
                    div.className = classes;
                    messageBox(title, json.Message);
                }
                else {
                    if (isActive) {
                        $(target).removeClass('active');
                        $(target).addClass('inactive');
                    }
                    else {
                        $(target).removeClass('inactive');
                        $(target).addClass('active');
                    }
                    window.location.reload(false);
                }
            },
            error: function (xhr, status, err) {
                alertBox.showError(xhr);
            },
            complete: function () {
                loading.hide();
            }
        })
    });
}

$(function () {
    $("#btnNewMenu").addClass("button");
});

function EditMenu(menuId) {
    var data = {
        MenuId: menuId
    };
    $.ajax({
        url: resolveClientUrl("ManageMenu/EditMenu"),
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

function EditMenuPost(menuId) {
    if (validator.validateRequired('#MenuName') || validator.validateRequired('#DisplayOrder') || validator.validateInt('#DisplayOrder')) {
        return;
    }
    var data = {
        MenuId: menuId,
        MenuName: $.trim($("#MenuName").val()),
        Description: $.trim($("#Description").val()),
        Url: $.trim($("#Url").val()),
        FunctionId: $.trim($("#FunctionId option:selected").val()),
        ParentId: $.trim($("#ParentId option:selected").val()),
        ParentName: $.trim($("#ParentId option:selected").text()),
        DisplayOrder: $.trim($("#DisplayOrder").val())
    };
    $.ajax({
        url: resolveClientUrl("ManageMenu/EditMenu"),
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
            navigateToUrl('ManageMenu');
        },
        error: function (xhr, status, err) {
            alertBox.showError(xhr);
        },
        complete: function () {
            popup.hideLoading();
        }
    });
}