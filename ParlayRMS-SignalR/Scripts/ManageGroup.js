$(document).ready(function () {
    $("#btnNewGroup").addClass("button");
    $("#btnNewUser").addClass("button");
    $("button", ".scrollGrid").addClass("button-disable");
});

function CreateNewGroup() {
    var url = "ManageGroup/CreateGroup";
    var title = "Create New Group";
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

function EditGroup(groupid) {
    var url = "ManageGroup/EditGroup?groupid=" + groupid;
    var title = "Edit Group Info";
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

function CreateNewUser() {
    var url = "ManageUser/CreateUser";
    var title = "Create User";
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

function CreateNewGroupPolicy(groupId, groupName) {
    var url = "ManageGroup/CreateGroupPolicy?groupid=" + groupId + "&groupname=" + groupName;
    $.ajax({
        url: resolveClientUrl(url),
        type: 'GET',
        dataType: 'html',
        cache: false,
        beforesend: function (xhr) {
        },
        success: function (data) {
            modalPopup(data);
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
} function grantPermissions(target) {
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
function getGroupPolicy() {
    var divs = document.getElementById('tblfuntion').getElementsByTagName("div");
    var groupPolicy = '';
    for (var i = 0; i < divs.length; i++) {
        if ($(divs[i]).attr("permission") == 2) {
            var val = $(divs[i]).attr("val");
            var id = $(divs[i]).attr("id").replace('r-', '').replace('w-', '');
            groupPolicy += id + ',' + val + ';'
        }
    }
    return groupPolicy.substring(0, groupPolicy.length - 1);
}
function saveGroupPolicy() {
    var groupPolicy = getGroupPolicy();

    var url = "ManageGroup/SaveUpdateGroupPolicy";
    var data = {
        groupId: $.trim($("#GroupId").val()),
        groupPolicy: groupPolicy
    };
    loadingInPopup.show();
    messagePopup.hide();
    //ShowLoadingInPopup('btnSubmitNewGroupPolicy');
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
                //ShowErrMsgInPopup('btnSubmitNewGroupPolicy', json.message);
                loadingInPopup.hide();
                messagePopup.show(json.message);
            }
            else {
                navigateToUrl('ManageGroup/GroupList', 2000);
            }
        },
        error: function () {
            //ShowLoadingInPopup('btnSubmitNewGroupPolicy', 'Error on granting permission!');
        }
    });
}

function saveCreateGroup() {
    if ($.trim($("#Name").val()) == '' || $.trim($("#Name").val()) == 'This field is required') {
        Validators.show($('#Name').get(0), 'This field is required');
        return;
    }
    var url = "ManageGroup/SaveNewGroup";
    var data = {
        Name: $.trim($("#Name").val()),
        Description: $.trim($("#Description").val())
    };
    messagePopup.hide();
    loadingInPopup.show();
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
                //ShowErrMsgInPopup('btnSubmitNewGroup', json.message);
                //alert(json.message);
                loadingInPopup.hide();
                messagePopup.show(json.message);
            }
            else {
                setTimeout("CreateNewGroupPolicy('" + json.groupid + "','" + $.trim($("#Name").val()) + "');", 2000);
            }
        },
        error: function () {
            //ShowLoadingInPopup('btnSubmitNewGroup', 'Error on creating new group!');
        }
    });
}

function saveEditGroup() {
    if ($.trim($("#Name").val()) == '' || $.trim($("#Name").val()) == 'This field is required') {
        Validators.animateText($('#Name').get(0), 'This field is required');
        return;
    }
    var url = "ManageGroup/SaveUpdateGroup?groupid=" + $('#GroupId').val();
    var data = {
        Name: $.trim($("#Name").val()),
        Description: $.trim($("#Description").val())
    };
    messagePopup.hide();
    loadingInPopup.show();
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
                navigateToUrl('ManageGroup/GroupList', 3000);
            }
        },
        error: function () {
            alert('Error on create new group!');
        }
    });
}