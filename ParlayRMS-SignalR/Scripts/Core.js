/* File Created: May 7, 2012 */
var tooltip;

var kickOutIntId;
var maxRetryTimes = 3;
var curRetryTimes = 0;
var loadingInPopup = null;
var messagePopup = null;
$(document).ready(function () {
    initValues();

    ReCheckin(); // call immediately after page load complete
    kickOutIntId = setInterval('ReCheckin();', 10000); //call kickout every 10s
});

function initValues() {
    tooltip = $('.tooltip');

    loadingInPopup = new LoadingInPopup();
    messagePopup = new ErrorMessageInPopup();
}
function navigateToUrl(url, delayTime) {
    if (delayTime == undefined) delayTime = 2000;
    setInterval("window.location.replace('" + resolveClientUrl(url) + "');", delayTime);
}

function ReCheckin() {
    $.ajax({
        url: resolveClientUrl('Account/ReCheckIn'),
        type: 'GET',
        dataType: 'html',
        cache: false,
        success: function (data) {
            curRetryTimes = 0;
            // alert(data);
            if (data == -1) {
                navigateToUrl('Account/Login');
                alert('You have logged in from another session or your login session has expired!');
            }
            else if (data == 2) {
                navigateToUrl('Account/Login');
            }
        },
        error: function (xhr, status, err) {
            curRetryTimes++;

            if (curRetryTimes >= maxRetryTimes) {
                clearInterval(kickOutIntId);
                window.location.replace(resolveClientUrl('Account/Login'));
            }
        }
    });
}

function limitInput(target) {
    var $target = $(target);

    var maxlength = $target.attr('maxlength');
    var val = $target.val();

    $target.val(val.slice(0, maxlength));
}

function messageBox(title, content) {
    $.ajax({
        url: resolveClientUrl("Shared/MessageBox/?title=" + title + "&content=" + content),
        type: 'GET',
        dataType: 'html',
        cache: false,
        beforesend: function (xhr) {
            loading.show();
        },
        success: function (data) {
            if ($('.ui-dialog') != null && $('.ui-dialog') != undefined && $('.ui-dialog') != 'undefined') {
                $('.ui-dialog').remove();
            }
            _popup.html(data);
            $("#dialog:ui-dialog").dialog("destroy");

            $("#dialog-message").dialog({
                modal: true,
                resizable: false,
                buttons: {
                    Ok: function () {
                        $(this).dialog("close");
                    }
                }
            });
        },
        error: function () {
            alert('Error on processing message box');
        }
    });
    loading.hide();
}

function isNumeric(value) {
    if (value == null || !value.toString().match(/^[-]?\d*\.?\d*$/)) return false;
    return true;
}
function logOff() {
    loading.show();
    navigateToUrl('Account/LogOff');
}

function Minimize(target, minimizeId, divTitle) {
    var width = $("#" + divTitle).width();

    if ($('#' + target).css('display') == 'block') {
        $("#" + target).slideToggle("slow");
        $('#' + minimizeId).html('+');
    }
    else {
        $("#" + target).slideToggle("slow");
        $('#' + minimizeId).html('-');
    }

    $("#" + divTitle).width(width);
}

Number.prototype.round = function (dec) {
    return Math.round(this * Math.pow(10, dec)) / Math.pow(10, dec);
}

Number.prototype.format2d = function () {
    text = this.round(2).toString();
    x = text.split(".");
    x1 = x[0];
    x2 = x.length > 1 ? "." + x[1] : "";
    rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, "$1" + ',' + "$2");
    }
    return x1 + x2;
}

function Button() {
    this.disabled = function (id) {
        $(id).attr("disabled", "true");
        $(id).removeClass("button");
        $(id).addClass("button-disable");
    };
    this.enabled = function (id) {
        $(id).removeAttr("disabled");
        $(id).addClass("button");
        $(id).removeClass("button-disable");
    };
}

function ResetPwd(userId) {
    if ($('#NewPassword').val() == '') {
        $('#NewPassword').focus();
        Validators.show($('#NewPassword').get(0), 'This field is required');
        return false;
    }

    if ($('#ConfirmNewPassword').val() == '') {
        $('#ConfirmNewPassword').focus();
        Validators.show($('#ConfirmNewPassword').get(0), 'This field is required');
        return false;
    }

    if ($('#NewPassword').val() != $('#ConfirmNewPassword').val()) {
        $('#ConfirmNewPassword').focus();
        Validators.show($('#ConfirmNewPassword').get(0), 'Passwords are not matched');
        return false;
    }

    loadingInPopup.show();
    messagePopup.hide();
    var password = $('#NewPassword').val();
    var url = "ManageUser/ResetPwd/?userId=" + userId + "&password=" + password;

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
            ShowErrMsgInPopup('btnSubmit', 'Error on resetting password!');
        }
    });
}