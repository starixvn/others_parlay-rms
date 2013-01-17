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
    loading.show();
    var username = $.trim($('#UserName').val());
    var groupId = $('#SearchOption').val();

    var url = "ManageUser/UserList/?username=" + username + "&groupId=" + groupId;

    window.location.href = resolveClientUrl(url);
}

function ResetPassword(userId, username) {
    var url = "ManageUser/ResetPassword/?userId=" + userId + "&username=" + username;
    $.ajax({
        url: resolveClientUrl(url),
        type: 'GET',
        dataType: 'html',
        cache: false,
        beforesend: function (xhr) {
            loading.show();
        },
        success: function (data) {
            modalPopup(data);
        },
        error: function () {
            showPopup(title, 'auto', 'auto');
        }
    });
}

function EditInfo(userId, username, firstName, lastName, groupId) {
    var url = "ManageUser/EditUserInfo/?userId=" + userId + "&username=" + username + "&firstName=" + firstName + "&lastName=" + lastName + "&groupId=" + groupId;
    var title = "Edit User Information";
    $.ajax({
        url: resolveClientUrl(url),
        type: 'GET',
        dataType: 'html',
        cache: false,
        beforesend: function (xhr) {
            loading.show();
        },
        success: function (data) {
            modalPopup(data);
        },
        error: function () {
            showPopup(title, 'auto', 'auto');
        }
    });
}

function SaveInfo(userId) {
    var firstName = $('#FirstName').val();
    var lastName = $('#LastName').val();
    var groupId = $('#GroupName').val();

    var url = "ManageUser/SaveUserInfo/?userId=" + userId + "&firstName=" + firstName + "&lastName=" + lastName + "&groupId=" + groupId;
    loadingInPopup.show();
    messagePopup.hide();
    $.ajax({
        url: resolveClientUrl(url),
        type: 'POST',
        dataType: 'html',
        cache: false,
        success: function (data) {
            var json = $.parseJSON(data);
            if (json.code != 0) {
                loadingInPopup.hide();
                messagePopup.show(json.message);
            }
            else {
                navigateToUrl("ManageUser/UserList", 2000);
            }
        },
        error: function () {
            ShowErrMsgInPopup('btnSubmit', 'Error on updating user information!');
        }
    });
}

function GetUserInfo() {
    var title = "Create New User";
    $.ajax({
        url: resolveClientUrl("ManageUser/CreateUser"),
        type: 'GET',
        dataType: 'html',
        cache: false,
        beforesend: function (xhr) {
            loading.show();
        },
        success: function (data) {
            modalPopup(data);
        },
        error: function () {
            showPopup(title, 'auto', 'auto');
        }
    });
}

function GetExistUserInfo() {
    var title = "Add Existing User";
    $.ajax({
        url: resolveClientUrl("ManageUser/CreateExistUser"),
        type: 'GET',
        dataType: 'html',
        cache: false,
        beforesend: function (xhr) {
            loading.show();
        },
        success: function (data) {
            modalPopup(data);
        },
        error: function () {
            showPopup(title, 'auto', 'auto');
        }
    });
}

function CreateUserInfo() {
    if ($('#Username').val() == '') {
        $('#Username').focus();
        Validators.show($('#Username').get(0), 'This field is required');
        return false;
    }

    if ($('#Password').val() == '') {
        $('#Password').focus();
        Validators.show($('#Password').get(0), 'This field is required');
        return false;
    }

    loadingInPopup.show();
    messagePopup.hide();
    var password = $('#Password').val();
    var username = $('#Username').val();
    var firstName = $('#FirstName').val();
    var lastName = $('#LastName').val();
    var groupId = $('#GroupName').val();
    var url = "ManageUser/SaveNewUser/?username=" + username + "&password=" + password + "&firstName=" + firstName + "&lastName=" + lastName + "&groupId=" + groupId;

    $.ajax({
        url: resolveClientUrl(url),
        type: 'POST',
        dataType: 'html',
        cache: false,
        success: function (data) {
            var json = $.parseJSON(data);
            if (json.code != 0) {
                loadingInPopup.hide();
                messagePopup.show(json.message);
            }
            else {
                navigateToUrl("ManageUser/UserList", 2000);
            }
        },
        error: function () {
            ShowErrMsgInPopup('btnSubmit', 'Error on creating user!');
        }
    });
}

function CreateExistUserInfo() {
    if ($('#Username').val() == '') {
        $('#Username').focus();
        Validators.show($('#Username').get(0), 'This field is required');
        return false;
    }

    loadingInPopup.show();
    messagePopup.hide();
    var username = $('#Username').val();
    var groupId = $('#GroupName').val();
    var url = "ManageUser/SaveExistUser/?username=" + username + "&groupId=" + groupId;

    $.ajax({
        url: resolveClientUrl(url),
        type: 'POST',
        dataType: 'html',
        cache: false,
        success: function (data) {
            var json = $.parseJSON(data);
            if (json.code != 0) {
                loadingInPopup.hide();
                messagePopup.show(json.message);
            }
            else {
                navigateToUrl("ManageUser/UserList", 2000);
            }
        },
        error: function () {
            ShowErrMsgInPopup('btnSubmit', 'Error on creating user!');
        }
    });
}

function ProcessCheckBox(userId, username, div, actionUrl) {
    var classes = div.className;
    var isInactive = div.className.indexOf('inactive') > -1;

    actionUrl += '/?userId=' + userId + '&active=' + (div.className.indexOf('inactive') > -1 ? 'true' : 'false');

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
        $.ajax({
            url: resolveClientUrl(actionUrl),
            type: 'POST',
            dataType: 'html',
            cache: false,
            success: function (data) {
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
                div.className = classes;
                messageBox(title, 'Error on updating!');
            }
        });
    });
}

function GetUserByGroup(groupId) {
    var url = "ManageUser/UserList/?username=&groupId=" + groupId;

    window.location.href = resolveClientUrl(url);
}

function GetUserPermission(userId, userName) {
    var title = "Edit Permission: <span style=\"color:#ff6300\">" + userName + "</span>";
    $.ajax({
        url: resolveClientUrl("ManageUser/UserPolicy/?userId=" + userId + "&userName=" + userName),
        type: 'GET',
        dataType: 'html',
        cache: false,
        beforesend: function (xhr) {
            loading.show();
        },
        success: function (data) {
            modalPopup(data);
        },
        error: function () {
            showPopup(title, 'auto', 'auto');
        }
    });
}

/*
===================
*/

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
function saveUserPolicy(userId) {
    var userPolicy = getUserPolicy();

    var url = "ManageUser/SaveUserPolicy";
    var data = {
        userId: userId,
        policy: userPolicy
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
                loadingInPopup.hide();
                messagePopup.show(json.message);
            }
            else {
                navigateToUrl('ManageUser/UserList', 2000);
            }
        },
        error: function () {
            ShowErrMsgInPopup('btnSubmit', 'Error on editting user permission');
        }
    });
}