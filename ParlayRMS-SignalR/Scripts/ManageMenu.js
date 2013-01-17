function createMenu() {
    var url = "ManageMenu/CreateMenu";
    var title = "Create Menu";
    $.ajax({
        url: resolveClientUrl(url),
        type: 'GET',
        dataType: 'html',
        cache: false,
        beforeSend: function () {
        },
        success: function (data) {
            modalPopup(data);
        },
        error: function () {
            ShowErrorMessage(arguments[0].responseText);
        }
    });
}

function SaveNewMenu() {
    if ($.trim($('#Name').val()) == '' || $.trim($('#Name').val()) == 'This field is required') {
        Validators.show($('#Name').get(0), 'This field is required');
        return;
    };
    if ($.trim($('#Url').val()) == '' || $.trim($('#Url').val()) == 'This field is required') {
        Validators.show($('#Url').get(0), 'This field is required');
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

    var url = "ManageMenu/SaveNewMenu";
    var data = {
        Name: $.trim($("#Name").val()),
        Url: $.trim($("#Url").val()),
        Description: $.trim($("#Description").val()),
        FunctionId: $.trim($("#Function option:selected").val()),
        ParentId: $.trim($("#Parent option:selected").val()),
        ParentName: $.trim($("#Parent option:selected").text()),
        IsActive: false,
        DisplayOrder: $.trim($("#DisplayOrder").val())
    };

    loadingInPopup.show();
    messagePopup.hide();
    $.ajax({
        url: resolveClientUrl(url),
        type: 'POST',
        data: data,
        dataType: 'html',
        cache: false,
        beforesend: function (xhr) {
        },
        success: function (result) {
            var json = $.parseJSON(result);
            if (json.code != 0) {
                //alert(json.message);
                loadingInPopup.hide();
                messagePopup.show(json.message);
            }
            else {
                navigateToUrl('ManageMenu/MenuList');
            }
        },
        error: function () {
            messageBox('Error', 'Error on create new menu!');
        }
    });
}

function ToggleActive(target, menuId, menuName) {
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
        var url = 'ManageMenu/SaveUpdateMenuStatus';
        var data = {
            Id: menuId,
            IsActive: !isActive
        };
        $.ajax({
            url: resolveClientUrl(url),
            type: 'POST',
            data: data,
            dataType: 'html',
            cache: false,
            beforesend: function (xhr) {
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
                    //messageBox(title, "Updated successfully!");
                    window.location.reload(false);
                }
            },
            error: function () {
                //ShowErrorMessage();
                ShowErrorMessage(arguments[0].responseText);
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
    //$("input:submit, a, button", "#barFunction").button({ icons: { primary: "ui-icon-plus" }, text: true });
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
        },
        success: function (data) {
            modalPopup(data);
        },
        error: function () {
            ShowErrorMessage(arguments[0].responseText);
        }
    });
}

function SaveUpdateMenu() {
    if ($.trim($('#Name').val()) == '' || $.trim($('#Name').val()) == 'This field is required') {
        Validators.show($('#Name').get(0), 'This field is required');
        return;
    };
    if ($.trim($('#Url').val()) == '' || $.trim($('#Url').val()) == 'This field is required') {
        Validators.show($('#Url').get(0), 'This field is required');
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
    loadingInPopup.show();
    messagePopup.hide();
    $.ajax({
        url: resolveClientUrl(url),
        type: 'POST',
        data: data,
        dataType: 'html',
        cache: false,
        beforesend: function (xhr) {
        },
        success: function (result) {
            var json = $.parseJSON(result);
            if (json.code != 0) {
                //alert(json.message);
                loadingInPopup.hide();
                messagePopup.show(json.message);
            }
            else {
                navigateToUrl('ManageMenu/MenuList');
            }
        },
        error: function () {
            messageBox('Error', 'Error on edit menu!');
        }
    });
}