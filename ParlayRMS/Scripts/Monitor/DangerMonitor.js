DangerMonitor = {
    Params: {
        AgentIdList: null,
        IsFromConfigList: false,
        Combos: null,
        MatchId: null,
        MaxTransId: null
    },
    CachedDOM: {
        rdbAgentId: null,
        txtUserName: null,
        ddlAgentId: null,
        ddlCombo: null,
        btnSearch: null,
        divResult: null,
        imgWaiting: null
    },
    GetCachedDOM: function () {
        DangerMonitor.CachedDOM.rdbAgentId = $("#rdbAgentId");
        DangerMonitor.CachedDOM.txtUserName = $("#txtUserName");
        DangerMonitor.CachedDOM.ddlAgentId = $("#ddlAgentId");
        DangerMonitor.CachedDOM.ddlCombo = $("#ddlCombo");
        DangerMonitor.CachedDOM.btnSearch = $("#btnSearch");
        DangerMonitor.CachedDOM.divResult = $("#divResult");
        DangerMonitor.CachedDOM.imgWaiting = $('<img src="../Images/Content/icon_wait.gif")" />');
    },
    Init: function () {
        DangerMonitor.GetCachedDOM();

        DangerMonitor.CachedDOM.ddlAgentId.multiselect({
            minWidth: 135,
            height: 136,
            create: function () {
                DangerMonitor.CachedDOM.ddlAgentId.multiselect("checkAll");
            },
            selectedText: function (numChecked, numTotal, checkedItems) {
                if (numChecked == 0) {
                    return "Select options";
                }
                else if (numChecked == 1) {
                    return $(checkedItems).attr('title');
                }
                else if (numChecked == numTotal) {
                    return "All";
                }
                else {
                    return numChecked + ' options';
                }
            }
        });

        DangerMonitor.CachedDOM.ddlCombo.multiselect({
            minWidth: 135,
            height: 136,
            create: function () {
                DangerMonitor.CachedDOM.ddlCombo.multiselect("checkAll");
            },
            selectedText: function (numChecked, numTotal, checkedItems) {
                if (numChecked == 0) {
                    return "Select options";
                }
                else if (numChecked == 1) {
                    return $(checkedItems).attr('title');
                }
                else if (numChecked == numTotal) {
                    return "All";
                }
                else {
                    return numChecked + ' options';
                }
            }
        });

        $('input[name="UserName"]').bind('change', function () {
            if ($(this).val() == 'txtUserName') {
                DangerMonitor.CachedDOM.ddlAgentId.multiselect('disable');
                DangerMonitor.CachedDOM.txtUserName.prop('disabled', false);
            } else {
                DangerMonitor.CachedDOM.txtUserName.prop('disabled', true);
                DangerMonitor.CachedDOM.ddlAgentId.multiselect('enable');
            }
            console.log($('input[name="UserName"]:checked').val());
        });

        DangerMonitor.CachedDOM.txtUserName.bind("blur", function () {
            var username = DangerMonitor.CachedDOM.txtUserName.val().trim();
            DangerMonitor.CachedDOM.txtUserName.val(username);
        });

        DangerMonitor.CachedDOM.txtUserName.bind("keyup", function (event) {
            if (event.keyCode == 13) {
                DangerMonitor.CachedDOM.txtUserName.blur();
                DangerMonitor.CachedDOM.btnSearch.click();
            }
        });

        DangerMonitor.CachedDOM.btnSearch.click(function () {
            DangerMonitor.CachedDOM.divResult.html('');

            var isFromConfigList = false;
            var selectedEle = $('input[name="UserName"]:checked').val();
            var agentIds = '';

            if (selectedEle == 'txtUserName') {
                agentIds = DangerMonitor.CachedDOM.txtUserName.val();
                isFromConfigList = true;
            } else {
                agentIds = DangerMonitor.CachedDOM.ddlAgentId.multiselect("getChecked").map(function () {
                    return this.value;
                }).get().join(',');
            }

            var combos = DangerMonitor.CachedDOM.ddlCombo.multiselect("getChecked").map(function () {
                return this.value;
            }).get().join(',');

            if (combos == '') {
                Validators.show(DangerMonitor.CachedDOM.ddlCombo.multiselect("getButton").get(0), 'Please select your options');
                return;
            }

            DangerMonitor.GetData(agentIds, combos, isFromConfigList);
        });
    },
    RenderData: function (result) {
        DangerMonitor.CachedDOM.divResult.html(result);
        DangerMonitor.Params.MaxTransId = $('#hdfMaxTransId', DangerMonitor.CachedDOM.divResult).val();

        $('a[data-match]', DangerMonitor.CachedDOM.divResult).each(function () {
            $(this).bind('click', function () {
                var matchId = $(this).attr('data-match');
                var matchName = $(this).text().trim();
                var leagueName = $(this).parent().prev().text();

                DangerMonitor.GetDetails(matchId, matchName, leagueName);
            });
        });

        //        $('td[data-match]', DangerMonitor.CachedDOM.divResult).each(function () {
        //            var row = $(this).parent();

        //            $(this).bind('click', function () {
        //                if ($(this).hasClass('grid-collapse')) {
        //                    var matchId = $(this).attr('data-match');
        //                    $('td[class="grid-expand"]', DangerMonitor.CachedDOM.divResult).each(function () {
        //                        $(this).removeClass('grid-expand').addClass('grid-collapse');
        //                        $(this).parent().next().remove();
        //                    });

        //                    $(this).removeClass('grid-collapse').addClass('grid-expand');
        //                    row.after('<tr class="details"><td colspan="11" class="center"></td></tr>');
        //                    DangerMonitor.GetDetails(matchId, row.next().find('td'));
        //                } else if ($(this).hasClass('grid-expand')) {
        //                    $(this).removeClass('grid-expand').addClass('grid-collapse');
        //                    row.next().remove();
        //                }
        //            });
        //        });
    },
    GetData: function (agentIds, combos, isFromConfigList) {
        DangerMonitor.Params.AgentIdList = agentIds;
        DangerMonitor.Params.Combos = combos;
        DangerMonitor.Params.IsFromConfigList = isFromConfigList;
        DangerMonitor.Params.MatchId = null;
        DangerMonitor.Params.MaxTransId = null;

        $.ajax({
            url: resolveClientUrl('DangerMonitor/Data'),
            type: 'POST',
            data: DangerMonitor.Params,
            dataType: 'html',
            cache: false,
            beforeSend: function (xhr) {
                loading.show();
            },
            success: function (result) {
                DangerMonitor.RenderData(result);
            },
            error: function (xhr, status, err) {
                alertBox.showError(xhr);
            },
            complete: function () {
                loading.hide();
            }
        });
    },
    GetDetails: function (matchId, matchName, leagueName) { //, displayArea) {
        DangerMonitor.Params.MatchId = matchId;

        //displayArea.html(DangerMonitor.CachedDOM.imgWaiting);
        $.ajax({
            url: resolveClientUrl('DangerMonitor/Details'),
            type: 'POST',
            data: DangerMonitor.Params,
            dataType: 'html',
            cache: false,
            beforeSend: function (xhr) {
                loading.show();
            },
            success: function (result) {
                //displayArea.html(result);
                modalPopup(result);
                var title = $('#popupTitle.title2').text().trim() + ' ' + leagueName + ' - ' + matchName;

                $('#popupTitle.title2').text(title);
            },
            error: function (xhr, status, err) {
                alertBox.showError(xhr);
            },
            complete: function () {
                loading.hide();
            }
        });
    }
}

String.prototype.trim = function () {
    return this.replace(/^\s+|\s+$/g, '');
};

$(document).ready(function () {
    DangerMonitor.Init();
});