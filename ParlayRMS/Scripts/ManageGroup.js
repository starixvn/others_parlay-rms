$(document).ready(function () {
    $("#btnNewGroup").addClass("button");
    $("#btnNewUser").addClass("button");
    $("button", ".scrollGrid").addClass("button-disable");
});

function CreateGroup() {
    $.ajax({
        url: resolveClientUrl("ManageGroup/CreateGroup"),
        type: 'GET',
        dataType: 'html',
        cache: false,
        beforeSend: function (xhr) {
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

function CreateGroupPost() {
    var groupNameElement = $("#GroupName");
    if ($.trim(groupNameElement.val()) == '' || $.trim(groupNameElement.val()) == 'This field is required') {
        Validators.show(groupNameElement.get(0), 'This field is required');
        return;
    }

    var data = {
        GroupName: $.trim($("#GroupName").val()),
        Description: $.trim($("#Description").val())
    };

    $.ajax({
        url: resolveClientUrl("ManageGroup/CreateGroup"),
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
                setTimeout("CreateGroupPolicy('" + json.groupid + "','" + $.trim($("#Name").val()) + "');", 2000);
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

function EditGroup(groupid) {
    var data = {
        GroupId: groupid
    };
    $.ajax({
        url: resolveClientUrl("ManageGroup/EditGroup"),
        data: data,
        type: 'GET',
        dataType: 'html',
        cache: false,
        beforeSend: function (xhr) {
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

function EditGroupPost() {
    if ($.trim($("#GroupName").val()) == '' || $.trim($("#GroupName").val()) == 'This field is required') {
        Validators.animateText($('#GroupName').get(0), 'This field is required');
        return;
    }
    var data = {
        GroupId: $('#GroupId').val(),
        GroupName: $.trim($("#GroupName").val()),
        Description: $.trim($("#Description").val())
    };

    $.ajax({
        url: resolveClientUrl('ManageGroup/EditGroup'),
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
                navigateToUrl('ManageGroup', 3000);
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

function CreateGroupPolicy(groupId, groupName) {
    var data = {
        GroupId: groupId,
        GroupName: groupName
    };
    $.ajax({
        url: resolveClientUrl("ManageGroup/CreateGroupPolicy"),
        data: data,
        type: 'GET',
        dataType: 'html',
        cache: false,
        beforeSend: function (xhr) {
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

function CreateGroupPolicyPost() {
    var groupPolicy = getGroupPolicy();
    var data = {
        groupId: $.trim($("#GroupId").val()),
        groupPolicy: groupPolicy
    };

    $.ajax({
        url: resolveClientUrl("ManageGroup/CreateGroupPolicy"),
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
                navigateToUrl('ManageGroup', 2000);
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
