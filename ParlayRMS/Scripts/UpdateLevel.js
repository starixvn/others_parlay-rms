function changeLevel(elOption, elMenu) {
    var currentClass = $(elMenu).parent().attr('class');
    var custId = $(elMenu).attr('custid');
    var level = elOption.attr('level');
    var username = $(elMenu).attr('username');

    var data = {
        UserName: $.trim(username),
        Level: level,
        CustId: custId
    };
    var title = "Confirmation";
    var content = "Are you sure you want to change level to " + (level == 0 ? "None" : level) + " ?";
    confirm(title, content, function () {
        $.ajax({
            url: resolveClientUrl('ManageMember/ChangeCustomerLevel'),
            type: 'POST',
            data: data,
            dataType: 'html',
            cache: false,
            beforeSend: function (xhr) {
                loading.show();
            },
            success: function (result) {
                loading.hide();
                var json = $.parseJSON(result);
                if (json.code != 0) {
                    alert(json.message);
                }
                else {
                    $(elMenu).parent().removeClass(currentClass);
                    $(elMenu).parent().addClass(elOption.attr('color'));
                }
            },
            error: function (xhr, status, err) {
                loading.hide();
                alertBox.showError(xhr);
            }
        });
    });
}