/* File Created: May 8, 2012 */

$(document).ready(function () {
    $('.menu .nav li.tab').hover(
        function () { // on mouse over
            $('.menu .nav ul.sub-nav').hide();
            $(this).addClass('hover').find('ul.sub-nav').eq(0).slideDown('fast');
        },
        function () { // on mouse out
            $(this).removeClass('hover').find('ul.sub-nav').eq(0).hide();
        });

    $('.menu .content .welcome').hover(
            function () { // on mouse over
                $('.menu .content .welcome div.welcomebody').hide();
                $(this).addClass('hover').find('div.welcomebody').eq(0).slideDown('fast');
            },
            function () { // on mouse out
                $(this).removeClass('hover').find('div.welcomebody').eq(0).hide();
            });
});

function ChangePassword(adminId, userId, username) {
    var url = "ManageUser/ResetOwnerPassword/?userId=" + userId + "&username=" + username;
    $.ajax({
        url: resolveClientUrl(url),
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

function navigateToUrl(url, delayTime) {
    if (delayTime == undefined) delayTime = 2000;
    setInterval("window.location.replace('" + resolveClientUrl(url) + "');", delayTime);
}

function logOff() {
    $('#loading').show('fade');
    navigateToUrl('Auth/LogOut');
}