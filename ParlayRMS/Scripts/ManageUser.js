$(document).ready(function () {
    var active = $('#active').html();
    var inactive = $('#inactive').html();

    $('#activeNumber').html(active);
    $('#inactiveNumber').html(inactive);

    $("button", ".scrollGrid").addClass("button-disable");
    $("#btnNewUser").addClass("button");
    $("#btnExistUser").addClass("button");
    $("#btnSubmit").addClass("button");
});

function Search_onclick(UserName, SearchOption) {
    var username = $.trim($('#UserName').val());
    var groupId = $('#SearchOption').val();
    window.location.href = resolveClientUrl("ManageUser/?username=" + username + "&groupId=" + groupId);
}

function ResetPassword(userId, username) {
    var data = {
        UserId: userId,
        UserName: username
    };
    $.ajax({
        url: resolveClientUrl("ManageUser/ResetPassword"),
        data: data,
        type: 'GET',
        dataType: 'html',
        cache: false,
        beforeSend: function (xhr) {
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

function EditUser(userId, username, firstName, lastName, groupId) {
    var data = {
        UserId: userId,
        UserName: username,
        FirstName: firstName,
        LastName: lastName,
        GroupId: groupId
    };
    $.ajax({
        url: resolveClientUrl("ManageUser/EditUser"),
        data: data,
        type: 'GET',
        dataType: 'html',
        cache: false,
        beforeSend: function (xhr) {
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

function EditUserPost(userId) {
    var data = {
        UserId: userId,
        FirstName: $('#FirstName').val(),
        LastName: $('#LastName').val(),
        GroupId: $('#GroupId').val()
    };
    $.ajax({
        url: resolveClientUrl("ManageUser/EditUser"),
        data: data,
        type: 'POST',
        dataType: 'html',
        cache: false,
        beforeSend: function (xhr) {
            popup.showLoading();
        },
        success: function (data) {
            var json = $.parseJSON(data);
            if (json.code != 0) {
                popup.showErrorMessage(json.message);
                return;
            }
            navigateToUrl("ManageUser", 2000);
        },
        error: function (xhr, status, err) {
            alertBox.showError(xhr);
        },
        complete: function () {
            popup.hideLoading();
        }
    });
}

function CreateUser() {
    $.ajax({
        url: resolveClientUrl("ManageUser/CreateUser"),
        type: 'GET',
        dataType: 'html',
        cache: false,
        beforeSend: function (xhr) {
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

function CreateUserPost() {
    if (validator.validateRequired("#popup #UserName") || validator.validateRequired("#Password")) {
        return;
    }
    var data = {
        UserName: $('#popup #UserName').val(),
        Password: $('#Password').val(),
        FirstName: $('#FirstName').val(),
        LastName: $('#LastName').val(),
        GroupId: $('#GroupName').val()
    };
    $.ajax({
        url: resolveClientUrl("ManageUser/CreateUser"),
        type: 'POST',
        data: data,
        dataType: 'html',
        cache: false,
        beforeSend: function (xhr) {
            popup.showLoading();
        },
        success: function (data) {
            var json = $.parseJSON(data);
            if (json.code != 0) {
                popup.showErrorMessage(json.message);
                return;
            }
            navigateToUrl("ManageUser", 2000);
        },
        error: function (xhr, status, err) {
            alertBox.showError(xhr);
        },
        complete: function () {
            popup.hideLoading();
        }
    });
}

function CreateExistingUser() {
    $.ajax({
        url: resolveClientUrl("ManageUser/CreateExistingUser"),
        type: 'GET',
        dataType: 'html',
        cache: false,
        beforeSend: function (xhr) {
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

function CreateExistingUserPost() {
    if (validator.validateRequired("#Username")) {
        return;
    }
    var data = {
        UserName: $('#Username').val(),
        GroupId: $('#GroupName').val()
    };
    $.ajax({
        url: resolveClientUrl("ManageUser/CreateExistingUser"),
        data: data,
        type: 'POST',
        dataType: 'html',
        cache: false,
        beforeSend: function (xhr) {
            popup.showLoading();
        },
        success: function (data) {
            var json = $.parseJSON(data);
            if (json.code != 0) {
                popup.showErrorMessage(json.message);
                return;
            }
            navigateToUrl("ManageUser", 2000);
        },
        error: function (xhr, status, err) {
            alertBox.showError(xhr);
        },
        complete: function () {
            popup.hideLoading();
        }
    });
}

function ToggleStatus(userId, username, div) {
    var isInactive = div.className.indexOf('inactive') > -1;
    var title = '';
    var content = '';
    if (isInactive) {
        title = 'Activate User';
        content = 'Are you sure to activate [' + username + '] user?';
    }
    else {
        title = 'Deactivate User';
        content = 'Are you sure to deactivate [' + username + '] user?';
    }
    confirm(title, content,
    function () {
        var data = {
            UserId: userId,
            IsActive: (div.className.indexOf('inactive') > -1 ? 'true' : 'false')
        };
        $.ajax({
            url: resolveClientUrl('ManageUser/UpdateStatus'),
            data: data,
            type: 'POST',
            dataType: 'html',
            cache: false,
            beforeSend: function (xhr) {
                loading.show();
            },
            success: function (data) {
                var classes = div.className;
                var json = $.parseJSON(data);
                if (json.code != 0) {
                    div.className = classes;
                    messageBox(title, json.Message);
                }
                else {
                    if (classes.indexOf('inactive') > -1) {
                        div.className = classes.replace('inactive', 'active');
                    }
                    else {
                        div.className = classes.replace('active', 'inactive');
                    }

                    var active = $('#activeNumber').html();
                    var inactive = $('#inactiveNumber').html();

                    var numActive = 0;
                    var numInactive = 0;

                    if (isInactive) {
                        numActive = parseInt(active) + 1;
                        numInactive = parseInt(inactive) - 1;
                    }
                    else {
                        numActive = parseInt(active) - 1;
                        numInactive = parseInt(inactive) + 1;
                    }

                    $('#activeNumber').html(numActive);
                    $('#inactiveNumber').html(numInactive);
                }
            },
            error: function (xhr, status, err) {
                alertBox.showError(xhr);
            },
            complete: function () {
                loading.hide();
            }
        });
    });
}

function GetUserByGroup(groupId) {
    window.location.href = resolveClientUrl("ManageUser/?username=&groupId=" + groupId);
}

function EditUserPolicy(userId, userName) {
    var data = {
        UserId: userId,
        UserName: userName
    };
    $.ajax({
        url: resolveClientUrl("ManageUser/EditUserPolicy"),
        data: data,
        type: 'GET',
        dataType: 'html',
        cache: false,
        beforeSend: function (xhr) {
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

function EditUserPolicyPost(userId) {
    var data = {
        UserId: userId,
        UserPolicy: getUserPolicy()
    };
    $.ajax({
        url: resolveClientUrl("ManageUser/EditUserPolicy"),
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
            navigateToUrl('ManageUser', 2000);
        },
        error: function (xhr, status, err) {
            alertBox.showError(xhr);
        },
        complete: function () {
            popup.hideLoading();
        }
    });
}

function checkPermission(isCheck, target) {
    if (isCheck == true) {
        $(target).removeClass('inactive');
        $(target).addClass('active');
    }
    else {
        $(target).removeClass('active');
        $(target).addClass('inactive');
    }
}

function grantPermissionsRead(target) {
    var targetId = $(target).attr('id');
    var isCheck = $(target).hasClass('inactive');
    var parentId = $(target).attr('parent');

    var table = document.getElementById('tblfuntion');
    var divs = table.getElementsByTagName("div");

    checkPermission(isCheck, target);
    $('#' + targetId.replace('r-', 'w-')).attr('val', '1');
    if (!isCheck)
        $('#' + targetId.replace('r-', 'w-')).attr('val', '0');

    for (var i = 0; i < divs.length; i++) {
        if ($(divs[i]).attr("parent") == targetId || (!isCheck && $(divs[i]).attr("parent") == targetId.replace('r-', 'w-') && $(divs[i]).attr("permission") == 2)) {
            checkPermission(false, $(divs[i]).get(0));
            var id = divs[i].id.replace('r-', 'w-');
            $('#' + id).attr('val', '0');
        }

        if ($(divs[i]).attr("parent") == targetId && $(divs[i]).attr("permission") == 1) {
            checkPermission(isCheck, $(divs[i]).get(0));
            var id = divs[i].id.replace('r-', 'w-');
            $('#' + id).attr('val', '1');
            if (!isCheck)
                $('#' + id).attr('val', '0');
        }
    }
}

function grantPermissionsWrite(target) {
    var targetId = $(target).attr('id');
    var isCheck = $(target).hasClass('inactive');
    checkPermission(isCheck, target);
    checkPermission(isCheck, $('#' + targetId.replace('w-', 'r-')).get(0));
    $('#' + targetId).attr('val', '2');
    if (!isCheck)
        $('#' + targetId).attr('val', '0');

    var table = document.getElementById('tblfuntion');
    var divs = table.getElementsByTagName("div");

    for (var i = 0; i < divs.length; i++) {
        if (($(divs[i]).attr("parent") == targetId.replace('w-', 'r-') || $(divs[i]).attr("parent") == targetId.replace('r-', 'w-')) && ($(divs[i]).attr("permission") == 2 || $(divs[i]).attr("permission") == 1)) {
            checkPermission(isCheck, $(divs[i]).get(0));
            $(divs[i]).attr('val', '2');
            if (!isCheck)
                $(divs[i]).attr('val', '0');
        }
    }
}

function grantPermissions(target) {
    var permission = $(target).attr('permission');
    var parentId = $(target).attr('parent');
    var targetId = $(target).attr('id');
    var isCheck = $(target).hasClass('inactive');
    var allUnChecked = false;

    if (permission == 1) // read
    {
        checkPermission(isCheck, target);
        $('#' + targetId.replace('r-', 'w-')).attr('val', '1');

        if (!isCheck) {
            checkPermission(false, $('#' + targetId.replace('r-', 'w-')).get(0));
            $('#' + targetId.replace('r-', 'w-')).attr('val', '0');

            var divs = document.getElementById('tblfuntion').getElementsByTagName("div");

            for (var i = 0; i < divs.length; i++) {
                if ($(divs[i]).attr("id") == parentId) {
                    continue;
                }

                if ($(divs[i]).attr("parent") == parentId) {
                    if ($(divs[i]).hasClass("inactive")) {
                        allUnChecked = true;
                    }
                    else {
                        allUnChecked = false;
                        break;
                    }
                }
            }

            if (allUnChecked) {
                checkPermission(false, $('#' + parentId)); //Uncheck parent node of read permission
                checkPermission(false, $('#' + parentId.replace('r-', 'w-'))); //Uncheck parent node of write permission

                $('#' + parentId.replace('r-', 'w-')).attr('val', '0');
                $('#' + parentId.replace('w-', 'r-')).attr('val', '0');
            }
        }
        else {
            checkPermission(true, $('#' + parentId));
            $('#' + parentId.replace('r-', 'w-')).attr('val', '1');
        }
    }
    else {
        checkPermission(isCheck, target);
        checkPermission(isCheck, $('#' + targetId.replace('w-', 'r-')).get(0));

        if (!isCheck) {
            $('#' + targetId.replace('r-', 'w-')).attr('val', '0');
            $('#' + targetId.replace('w-', 'r-')).attr('val', '0');

            var divs = document.getElementById('tblfuntion').getElementsByTagName("div");

            for (var i = 0; i < divs.length; i++) {
                if ($(divs[i]).attr("id") == parentId) {
                    continue;
                }

                if ($(divs[i]).attr("parent") == parentId) {
                    if ($(divs[i]).hasClass("inactive")) {
                        allUnChecked = true;
                    }
                    else {
                        allUnChecked = false;
                        break;
                    }
                }
            }

            if (allUnChecked) {
                checkPermission(false, $('#' + parentId)); //Uncheck parent node of write permission
                checkPermission(false, $('#' + parentId.replace('w-', 'r-')).get(0)); //Uncheck parent node of read permission

                $('#' + parentId.replace('r-', 'w-')).attr('val', '0');
                $('#' + parentId.replace('w-', 'r-')).attr('val', '0');
            }
        }
        else {
            checkPermission(true, $('#' + parentId)); //Check parent node of write permission
            checkPermission(true, $('#' + parentId.replace('w-', 'r-')).get(0)); //Check parent node of read permission

            $('#' + targetId.replace('r-', 'w-')).attr('val', '2');
            $('#' + targetId.replace('w-', 'r-')).attr('val', '2');

            $('#' + parentId.replace('r-', 'w-')).attr('val', '2');
            $('#' + parentId.replace('w-', 'r-')).attr('val', '2');
        }
    }
}

function getUserPolicy() {
    var divs = document.getElementById('tblfuntion').getElementsByTagName("div");
    var userPolicy = '';
    for (var i = 0; i < divs.length; i++) {
        if ($(divs[i]).attr("permission") == 2) {
            if ($(divs[i]).attr("id")) {
                var val = $(divs[i]).attr("val");
                var id = $(divs[i]).attr("id").replace('r-', '').replace('w-', '');
                userPolicy += id + ',' + val + ';'
            }
        }
    }
    return userPolicy.substring(0, userPolicy.length - 1);
}