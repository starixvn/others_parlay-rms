var reLoginId = null;

$(document).ready(function () {
    //ReCheckinOffline(); // call immediately after page load complete
    //reLoginId = setInterval('ReCheckinOffline();', 30000); //call kickout every 10s
});

function ReCheckinOffline() {
    $.ajax({
        url: resolveClientUrl('Account/ReCheckInOffline'),
        type: 'GET',
        dataType: 'html',
        cache: false,
        success: function (data) {
            if (data.code == 0) {
                clearInterval(reLoginId);
                navigateToUrl('Account/Login');
            }
        }
    });
}