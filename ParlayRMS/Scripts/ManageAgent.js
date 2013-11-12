ManageAgent = {
    UserName: '',
    CachedDOM: {
        btnAddDangerousAgent: null,
        txtUserName: null,
        btnAgentSearch: null,
        //btnAgentShowAll: null,
        divResult: null
    },
    GetCachedDOM: function () {
        ManageAgent.CachedDOM.btnAddDangerousAgent = $('#btnAddDangerousAgent');
        ManageAgent.CachedDOM.txtUserName = $('#txtUserName');
        ManageAgent.CachedDOM.btnAgentSearch = $('#btnAgentSearch');
        //ManageAgent.CachedDOM.btnAgentShowAll = $('#btnAgentShowAll');
        ManageAgent.CachedDOM.divResult = $('#divResult');
    },
    Init: function () {
        ManageAgent.GetCachedDOM();

        ManageAgent.CachedDOM.btnAddDangerousAgent.addClass('button');
        ManageAgent.CachedDOM.btnAddDangerousAgent.click(function () {
            ManageAgent.GetAddingAgentForm();
        });

        ManageAgent.CachedDOM.txtUserName.bind('blur', function () {
            var userName = $.trim(ManageAgent.CachedDOM.txtUserName.val());
            ManageAgent.CachedDOM.txtUserName.val(userName);
        });

        ManageAgent.CachedDOM.txtUserName.bind('keyup', function (event) {
            if (event.keyCode == 13) {
                ManageAgent.CachedDOM.txtUserName.blur();
                ManageAgent.CachedDOM.btnAgentSearch.click();
            }
        });

        ManageAgent.CachedDOM.btnAgentSearch.button();
        ManageAgent.CachedDOM.btnAgentSearch.click(function () {
            ManageAgent.CachedDOM.divResult.html('');
            ManageAgent.UserName = ManageAgent.CachedDOM.txtUserName.val();
            ManageAgent.GetData(ManageAgent.UserName, 1);
        });

        //        ManageAgent.CachedDOM.btnAgentShowAll.button();
        //        ManageAgent.CachedDOM.btnAgentShowAll.click(function () {
        //            ManageAgent.CachedDOM.txtUserName.val('');
        //            ManageAgent.UserName = '';
        //            ManageAgent.GetData(ManageAgent.UserName, 1);
        //        });

        ManageAgent.GetData(ManageAgent.UserName, 1);
    },
    HandleGridEvents: function () {
        $('.scrollGridContent table.scrollGrid td div.removeIcon', ManageAgent.CachedDOM.divResult).each(function () {
            $(this).click(function () {
                var userName = $(this).attr('data-user-name');
                var userId = $(this).attr('data-user-id');

                ManageAgent.DeleteAgent(userName, userId);
            });
        });
    },
    GetAddingAgentForm: function () {
        $.ajax({
            url: resolveClientUrl("ManageAgent/AddAgent"),
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
    },
    AddAgent: function () {
        if (validator.validateRequired("#UserName")) {
            return;
        }
        var data = {
            UserName: $("#UserName").val()
        };
        $.ajax({
            url: resolveClientUrl("ManageAgent/AddAgent"),
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
    },

    GetData: function (userName, page) {
        var data = {
            UserName: userName,
            Page: page
        }

        $.ajax({
            url: resolveClientUrl('ManageAgent/AgentList'),
            type: 'POST',
            data: data,
            dataType: 'html',
            cache: false,
            beforeSend: function (xhr) {
                loading.show();
            },
            success: function (result) {
                ManageAgent.CachedDOM.divResult.html(result);
                ManageAgent.HandleGridEvents();
            },
            error: function (xhr, status, err) {
                alertBox.showError(xhr);
            },
            complete: function () {
                loading.hide();
            }
        });
    },
    DeleteAgent: function (userName, userId) {
        var title = "Confirmation";
        var content = "Are you sure to remove this agent?";

        confirm(title, content, function () {
            var data = {
                UserName: userName,
                AgentId: userId
            }

            $.ajax({
                url: resolveClientUrl("ManageAgent/DeleteAgent"),
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
                    location.reload();
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
};
function MovePage(pageIndex) {
    ManageAgent.GetData(ManageAgent.UserName, pageIndex);
};

$(document).ready(function () {
    ManageAgent.Init();
});