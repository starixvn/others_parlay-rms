///* File Created: October 24, 2012 */
var isUpdateGroup = false;
$(document).ready(function () {
    $("#btnAddSpecialLeague").addClass("button");
    // Show menu when a list item is clicked
    $("#groupColor").contextMenu({
        menu: 'myColor'
    }, function (elOption, elMenu, pos) {
        $("#searchGroupName").text(elOption.html());
        $(elMenu).parent().css("background-color", elOption.attr('color'));
        $(elMenu).attr('valgroup', elOption.attr('group'));
    });
    LoadSpecialLeague($("#txtLeague").val(), $("#groupColor").attr('valgroup'), 1);
});

function LoadSpecialLeague(league, group, page) {
    var data = {
        League: league,
        Group: group,
        Page: page
    };
    $.ajax({
        url: resolveClientUrl("ManageLeague/LeagueList"),
        type: 'POST',
        data: data,
        dataType: 'html',
        cache: false,
        beforeSend: function (xhr) {
            loading.show();
        },
        success: function (result) {
            $("#content").html(result);
            ParlayRMS.Core.ResolvePermission();
            $(".arrowDown_div").contextMenu({
                menu: 'GroupList'
            }, function (elOption, elMenu, pos) {
                UpdateGroup(elOption, elMenu);
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

function UpdateGroup(elOption, elMenu) {
    var group = $(elOption).attr('group');
    var groupname = $(elOption).html();
    var league = $(elMenu).attr('league');
    var leaguename = $("#league-name-" + league).text().trim();
    var title = "Confirmation";
    var content = "Are you sure you want to change '" + leaguename + "' to group '" + groupname + "' ?";
    confirm(title, content, function () {
        var data = {
            league: league,
            group: group
        };
        $.ajax({
            url: resolveClientUrl("ManageLeague/UpdateGroup"),
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
                    LoadSpecialLeague($("#txtLeague").val(), $("#groupColor").attr('valgroup'), $(".Pager .ActiveNumber").text().trim());
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

function AddLeague() {
    $.ajax({
        url: resolveClientUrl("ManageLeague/AddLeague"),
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

function AddLeaguePost() {
    if (validator.validateRequired("#League")) {
        return;
    }
    var data = {
        League: $.trim($("#League").val()),
        Group: $.trim($("#getColor").attr('valcolor'))
    };
    $.ajax({
        url: resolveClientUrl("ManageLeague/AddLeague"),
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
            LoadSpecialLeague($("#txtLeague").val(), $("#groupColor").attr('valgroup'), $(".Pager .ActiveNumber").text().trim());
        },
        error: function (xhr, status, err) {
            alertBox.showError(xhr);
        },
        complete: function () {
            popup.hideLoading();
        }
    });
}

function SearchLeague_onclick() {
    LoadSpecialLeague($("#txtLeague").val(), $("#groupColor").attr('valgroup'), 1);
}

function SearchAllLeague_onclick() {
    $('#txtLeague').val('');
    $('#searchGroupName').html('All group');
    $('#groupColor').attr('valgroup', '0');
    
    $('#groupColor').css("background-color", "#000000");

    LoadSpecialLeague($("#txtLeague").val(), $("#groupColor").attr('valgroup'), $(".Pager .ActiveNumber").text().trim());
}

function ExportExcel() {
    var url = "ManageLeague/ExportExcel?league=" + $("#txtLeague").val() + "&group=" + $("#groupColor").attr('valgroup');
    window.location.href = resolveClientUrl(url);
}

function MovePage(pageNumber) {    
    LoadSpecialLeague($("#txtLeague").val(), $("#groupColor").attr('valgroup'), pageNumber);
}