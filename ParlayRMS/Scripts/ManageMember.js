/* File Created: October 24, 2012 */
var isUpdateLevel = false;
$(document).ready(function () {
    $("#btnAddSpecialMember").addClass("button");
    // Show menu when a list item is clicked
    $("#levelColor").contextMenu({
        menu: 'myColor'
    }, function (elOption, elMenu, pos) {
        $("#searchLevelName").text(elOption.html());
        var currentClass = $(elMenu).parent().attr('class');
        $(elMenu).parent().removeClass(currentClass);
        $(elMenu).parent().addClass("level " + elOption.attr('color'));
        $(elMenu).attr('vallevel', elOption.attr('level'));
    });
    LoadSpecialMember($("#txtUserName").val(), $("#levelColor").attr('vallevel'), null);
});

function LoadSpecialMember(username, level, page) {
    var data = {
        Username: username,
        Level: level,
        Page: page
    };
    $.ajax({
        url: resolveClientUrl("ManageMember/MemberList"),
        type: 'POST',
        data: data,
        dataType: 'html',
        cache: false,
        beforeSend: function (xhr) {
            loading.show();
        },
        success: function (result) {
            $("#content").html(result);

            $(".arrowDown_div").contextMenu({
                menu: 'LevelList'
            }, function (elOption, elMenu, pos) {
                UpdateLevel(elOption, elMenu);
            });
        },
        error: function (xhr, status, err) {
            alertBox.showError(xhr);
        },
        complete: function () {
            loading.hide();
        }
    });
}

function UpdateLevel(elOption, elMenu) {
    var level = $(elOption).attr('level');
    var custId = $(elMenu).attr('custid');
    var title = "Confirmation";
    var content = "Are you sure you want to change level to " + (level == 0 ? "None" : level) + " ?";
    confirm(title, content, function () {
        var data = {
            Custid: custId,
            Level: level
        };
        $.ajax({
            url: resolveClientUrl("ManageMember/UpdateLevel"),
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
                    alertBox.show(json.message);
                }
                else {
                    var currentClass = $(elMenu).parent().attr('class');
                    $("#levelName_" + custId).text(elOption.html());
                    $(elMenu).parent().removeClass(currentClass);
                    $(elMenu).parent().addClass("divcolor " + elOption.attr('color'));
                    $(elMenu).parent().attr('level', level);
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

function AddMember() {
    $.ajax({
        url: resolveClientUrl("ManageMember/AddMember"),
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

function AddMemberPost() {
    if (validator.validateRequired("#UserName")) {
        return;
    }
    var data = {
        UserName: $.trim($("#UserName").val()),
        Level: $.trim($("#getColor").attr('valcolor'))
    };
    $.ajax({
        url: resolveClientUrl("ManageMember/AddMember"),
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
            location.reload();
        },
        error: function (xhr, status, err) {
            alertBox.showError(xhr);
        },
        complete: function () {
            popup.hideLoading();
        }
    });
}

function SearchMember_onclick() {
    var username = $.trim($('#txtUserName').val());
    var level = $('#levelColor').attr("vallevel");
    LoadSpecialMember(username, level, null);
}

function SearchAllMember_onclick() {
    $('#txtUserName').val('');
    $('#searchLevelName').html('All level');
    $('#levelColor').attr('vallevel', '');
    $('#levelColor').parent().addClass('level alllevel');
    LoadSpecialMember(null, null, null);
}

function ExportExcel() {
    var username = $.trim($('#txtUserName').val());
    var level = $('#levelColor').attr("vallevel");
    var url = "ManageMember/ExportExcel?username=" + username + "&level=" + level;
    window.location.href = resolveClientUrl(url);
}

function MovePage(pageNumber) {
    var username = $.trim($('#txtUserName').val());
    var level = $('#levelColor').attr("vallevel");
    LoadSpecialMember(username, level, pageNumber);
}