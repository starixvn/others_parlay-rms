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

function CreateMenuPost() {
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
            loading.show();
        },
        success: function (result) {
            var json = $.parseJSON(result);
            if (json.code != 0) {
                messagePopup.show(json.message);
            }
            else {
                navigateToUrl('ManageMenu');
            }
        },
        error: function (xhr, status, err) {
            alertBox.showError(xhr);
        },
        complete: function () {
            loading.hide();
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
            Id: menuId,
            IsActive: !isActive
        };
        $.ajax({
            url: resolveClientUrl('ManageMenu/UpdateStatus'),
            type: 'POST',
            data: data,
            dataType: 'html',
            cache: false,
            beforeSend: function (xhr) {
            },
            success: function (result) {
                var json = $.parseJSON(result);
                if (json.code != 0) {
                    messagePopup.show(json.message);
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
            }
        })
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

$(function () {
    $("#btnNewMenu").addClass("button");
});

function EditMenu(menuId) {
    var url = "ManageMenu/EditMenu?menuId=" + menuId;
    var title = "Edit Menu";
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
            loading.hide();
        },
        error: function (xhr, status, err) {
            alertBox.showError(xhr);
        }
    });
}

function SaveUpdateMenu() {
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

    var url = "ManageMenu/SaveUpdateMenu";
    var data = {
        Id: $.trim($("#Id").val()),
        Name: $.trim($("#Name").val()),
        Description: $.trim($("#Description").val()),
        Url: $.trim($("#Url").val()),
        FunctionId: $.trim($("#FunctionId option:selected").val()),
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
                navigateToUrl('ManageMenu');
            }
        },
        error: function (xhr, status, err) {
            alertBox.showError(xhr);
        }
    });
}