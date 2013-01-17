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

    LoadSpecialMember(null, null, null);
});

function LoadSpecialMember(username, level, page) {
    var url = "ManageMember/SpecialMemberList";
    var data = {
        Username: username,
        Level: level,
        Page: page
    };
    $.ajax({
        url: resolveClientUrl(url),
        type: 'POST',
        data: data,
        dataType: 'html',
        cache: false,
        beforeSend: function (xhr) {
            loading.show();
        },
        success: function (result) {
            loading.hide();
            $("#content").html(result);

            $(".arrowDown_div").contextMenu({
                menu: 'LevelList'
            }, function (elOption, elMenu, pos) {
                updateLevel(elOption, elMenu);
            });
        },
        error: function (xhr, status, err) {
            loading.hide();
            alertBox.showError(xhr);
        }
    });
}

function updateLevel(elOption, elMenu) {
    var currentClass = $(elMenu).parent().attr('class');
    var custId = $(elMenu).attr('custid');
    var level = $(elOption).attr('level');

    var url = "ManageMember/UpdateLevel";
    var data = {
        custid: custId,
        Level: level
    };

    var title = "Confirmation";
    var content = "Are you sure you want to change level to " + (level == 0 ? "None" : level) + " ?";
    confirm(title, content, function () {
        $.ajax({
            url: resolveClientUrl(url),
            type: 'POST',
            data: data,
            dataType: 'html',
            cache: false,
            beforeSend: function (xhr) {
                loading.show();
            },
            success: function (result) {
                var json = $.parseJSON(result);
                loading.hide();
                if (json.code != 0) {
                    alertBox.show('Error Message', json.message);
                }
                else {
                    $("#levelName_" + custId).text(elOption.html());
                    $(elMenu).parent().removeClass(currentClass);
                    $(elMenu).parent().addClass("divcolor " + elOption.attr('color'));
                    $(elMenu).parent().attr('level', level);
                }
            },
            error: function (xhr, status, err) {
                loading.hide();
                alertBox.showError(xhr);
            }
        });
    });
}

function addSpecialMember() {
    var url = "ManageMember/AddMember";
    var title = "Add Special Member";
    $.ajax({
        url: resolveClientUrl(url),
        type: 'GET',
        dataType: 'html',
        cache: false,
        beforeSend: function (xhr) {
            loading.show();
        },
        success: function (data) {
            modalPopup(data);
            loading.hide();
        },
        error: function (xhr, status, err) {
            loading.hide();
            alertBox.showError(xhr);
        }
    });
}

function saveSpecialMember() {
    if ($.trim($("#UserName").val()) == '' || $.trim($("#UserName").val()) == 'This field is required') {
        Validators.show($('#UserName').get(0), 'This field is required');
        return;
    }
    var url = "ManageMember/SaveMember";
    var data = {
        UserName: $.trim($("#UserName").val()),
        Level: $.trim($("#getColor").attr('valcolor'))
    };

    $.ajax({
        url: resolveClientUrl(url),
        type: 'POST',
        data: data,
        dataType: 'html',
        cache: false,
        beforeSend: function (xhr) {
            messagePopup.hide();
            loadingInPopup.show();
        },
        success: function (result) {
            var json = $.parseJSON(result);
            if (json.code != 0) {
                loadingInPopup.hide();
                messagePopup.show(json.message);
            }
            else {
                modalClose();
                LoadSpecialMember(null, null, null);
            }
        },
        error: function (xhr, status, err) {
            loadingInPopup.hide();
            alertBox.showError(xhr);
        }
    });
}

function SearchMember_onclick() {
    loading.show();
    var username = $.trim($('#txtUserName').val());
    var level = $('#levelColor').attr("vallevel");
    LoadSpecialMember(username, level, null);
}
function SearchAllMember_onclick() {
    $('#txtUserName').val('');
    $('#searchLevelName').html('All level');
    $('#levelColor').attr('vallevel', '-1');
    $('#levelColor').parent().addClass('level alllevel');
    LoadSpecialMember(null, null, null);
}
function ExportExcel() {
    var username = $.trim($('#txtUserName').val());
    var level = $('#levelColor').attr("vallevel");

    var url = "ManageMember/ExportSpecialMembers/?username=" + username + "&level=" + level;

    window.location.href = resolveClientUrl(url);
}

function MovePage(pageNumber) {
    var username = $.trim($('#txtUserName').val());
    var level = $('#levelColor').attr("vallevel");
    LoadSpecialMember(username, level, pageNumber);
}