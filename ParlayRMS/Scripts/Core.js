/* File Created: May 7, 2012 */
var tooltip;

var kickOutIntId;
var maxRetryTimes = 3;
var curRetryTimes = 0;

String.Empty = ""; // Extend static string

if (ParlayRMS == null || typeof (ParlayRMS) != "object") { var ParlayRMS = new Object(); }

ParlayRMS.Core =
{
    ResolvePermission: function () {
        var elements = $('.disabled');
        elements.unbind('click');
        elements.removeAttr('onclick');
        elements.attr('href', '#');
        elements.filter('span').remove();
    },
    NavigateToUrl: function (url, delayTime) {
        if (delayTime == undefined) {
            delayTime = 2000;
        }
        if (url.indexOf('http') == -1) {
            url = "window.location.replace('" + resolveClientUrl(url) + "');";
        }
        setTimeout(function () {
            location.href = url;
        }, delayTime);
    },
    OpenWindow: function (url) {
        var popH = $(window).height(), popW = $(window).width();
        var formatter = new Array(
                "resizable=no",
                "scrollbars=yes",
                "toolbar=no",
                "menubar=no",
                "location=no",
                "addressbar=no",
                "directories=no",
                "status=yes"
            ).join(',');

        window.open(url, '_blank', '' + ',' + formatter);
    }
};

$(document).ready(function () {
    initValues();

    ReCheckin(); // call immediately after page load complete
    kickOutIntId = setInterval('ReCheckin();', 10000); //call kickout every 10s
});

function initValues() {
    tooltip = $('.tooltip');
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
        },
        complete: function () {
            loading.hide();
        }
    });
}

function isNumeric(value) {
    if (value == null || !value.toString().match(/^[-]?\d*\.?\d*$/)) return false;
    return true;
}
function logOff() {
    loading.show();
    navigateToUrl('Account/SignOut');
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
};

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
};

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

function ResetOwnerPasswordPost(userId) {
    if (validator.validateRequired('#NewPassword') || validator.validateRequired('#ConfirmPassword')) {
        return;
    }

    if ($('#NewPassword').val() != $('#ConfirmPassword').val()) {
        $('#ConfirmPassword').focus();
        validator.show($('#ConfirmPassword').get(0), 'Passwords are not matched');
        return false;
    }

    var password = $('#NewPassword').val();
    var data = {
        UserId: userId,
        NewPassword: password
    };

    $.ajax({
        url: resolveClientUrl("ManageUser/ResetOwnerPassword"),
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
            }
            else {
                navigateToUrl("ManageUser", 2000);
            }
        },
        error: function () {
            ShowErrMsgInPopup('btnSubmit', 'Error on resetting password!');
        },
        complete: function () {
            popup.hideLoading();
        }
    });
}

// Format input number on event e, using: onkeyup=FormatNumber(event)
function FormatNumber(event, isFloat) {
    var evt = event ? event : window.event; // Fix for IE7
    var obj = evt.currentTarget ? evt.currentTarget : evt.srcElement; // Fix for IE7
    var value = obj.value;
    var num;
    var dotPosition = 0;
    var afterDot;
    var caretPosition = GetCaretPosition(obj);

    num = value.toString().replace(/\$|\,/g, String.Empty);
    dotPosition = num.indexOf(".");
    if ((typeof (isFloat) != 'undefined') && isFloat && dotPosition > 0) {
        afterDot = num.substring(dotPosition + 1, num.length).replace(/[^0-9]/g, String.Empty);
        num = num.substring(0, dotPosition);
    }
    num = FormatNumberFloat(num);
    if ((typeof (isFloat) != 'undefined') && isFloat && dotPosition > 0) {
        eval(obj).value = num + "." + afterDot;
        num = obj.value;
    } else {
        eval(obj).value = num;
    }

    if (num.length > value.length)
        caretPosition = caretPosition + 1;
    SetCaretPosition(obj, caretPosition);
    return true;
}

// Sets the caret (cursor) position of the specified text field.
function SetCaretPosition(oField, iCaretPos) {
    // IE Support
    if (document.selection) {
        oField.focus();

        // Create empty selection range
        var oSel = document.selection.createRange();

        // Move selection start and end to desired position
        oSel.moveStart('character', iCaretPos);
        oSel.moveEnd('character', iCaretPos - oField.value.length);
        oSel.select();
    }
    // Firefox support
    else if (oField.selectionStart || oField.selectionStart == '0') {
        oField.selectionStart = iCaretPos;
        oField.selectionEnd = iCaretPos;
        oField.focus();
    }
}

// Returns the caret (cursor) position of the specified text field.
function GetCaretPosition(oField) {
    var iCaretPos = 0;

    // IE Support
    if (document.selection) {
        oField.focus();
        // To get cursor position, get empty selection range
        var oSel = document.selection.createRange();
        // Move selection start to 0 position
        oSel.moveStart('character', -oField.value.length);
        iCaretPos = oSel.text.length;
    }
    // Firefox support
    else if (oField.selectionStart || oField.selectionStart == '0')
        iCaretPos = oField.selectionStart;

    // Return results
    return (iCaretPos);
}

function FormatNumberFloat(number) { // Format a number to 12,345
    if (number != String.Empty) {
        var sign;
        if (isNaN(number)) {
            number = String.Empty;
        }
        sign = (number == (number = Math.abs(number)));
        number = Math.floor(number * 100 + 0.50000000001);
        number = Math.floor(number / 100).toString();
        for (var i = 0; i < Math.floor((number.length - (1 + i)) / 3); i++) {
            number = number.substring(0, number.length - (4 * i + 3)) + ',' + number.substring(number.length - (4 * i + 3));
        }
        return (((sign) ? String.Empty : '-') + number);
    }

    return number;
}

function htmlEncode(value) {
    return $('<div/>').text(value).html();
}

function htmlDecode(value) {
    return $('<div/>').html(value).text();
}

function isNumber(n) {
    return !isNaN(parseFloat(n)) && isFinite(n);
}

function diffDate(oneDate, twoDate) {
    var myDate = new Date(oneDate);
    var otherDate = new Date(twoDate);
    var numberOfDay = Math.floor((otherDate.getTime() - myDate.getTime()) / (1000 * 60 * 60 * 24));
    return numberOfDay;
}

function CreateNoInfoRow(colspan) {
    return '<tr><td class="noinfo" colspan=' + colspan + '>' + Language.NoInformation + '</td></tr>';
}