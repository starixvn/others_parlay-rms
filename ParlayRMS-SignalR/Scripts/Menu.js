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
            //_popup.html(data);
            // showPopup('Reset Password', 'auto', 'auto');
            modalPopup(data);
        },
        error: function () {
            showPopup(title, 'auto', 'auto');
        }
    });
}