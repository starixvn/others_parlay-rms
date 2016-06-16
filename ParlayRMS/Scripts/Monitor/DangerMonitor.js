DangerMonitor = {
	Params: {
		AgentIdList: null,
		IsFromConfigList: false,
		Combos: null,
		MatchId: null,
		MaxPayout: null,
		MaxTransId: null
	},
	CachedDOM: {
		rdbAgentId: null,
		txtUserName: null,
		ddlAgentId: null,
		ddlCombo: null,
		txtMaxPayout: null,
		btnSearch: null,
		divResult: null,
		imgWaiting: null
	},
	GetCachedDOM: function () {
		DangerMonitor.CachedDOM.rdbAgentId = $("#rdbAgentId");
		DangerMonitor.CachedDOM.txtUserName = $("#txtUserName");
		DangerMonitor.CachedDOM.ddlAgentId = $("#ddlAgentId");
		DangerMonitor.CachedDOM.ddlCombo = $("#ddlCombo");
		DangerMonitor.CachedDOM.txtMaxPayout = $("#txtMaxPayout");
		DangerMonitor.CachedDOM.btnSearch = $("#btnSearch");
		DangerMonitor.CachedDOM.divResult = $("#divResult");
		DangerMonitor.CachedDOM.imgWaiting = $('<img src="../Content/Images/Content/icon_wait.gif" />');
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
		}).multiselectfilter();

		DangerMonitor.CachedDOM.ddlCombo.multiselect({
			minWidth: 135,
			height: 136,
			create: function () {
				DangerMonitor.CachedDOM.ddlCombo.multiselect();
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
		});
		$('#rdbUserName[value="txtUserName"]').prop('checked', true);
		$('#rdbUserName[value="txtUserName"]').change();

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

		DangerMonitor.CachedDOM.txtMaxPayout.bind('keyup', function (event) {
			FormatNumber(event, true);
			var maxPayout = parseInt($.trim($(this).val().replace(/,/g, '')));
			if (maxPayout > 1000000000000) {
				$(this).val("1,000,000,000,000");
			}

			if (event.keyCode == 13) {
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

			var maxPayout = $.trim(DangerMonitor.CachedDOM.txtMaxPayout.val().replace(/,/g, ''));
			if (maxPayout == '') {
				maxPayout = '0';
			}

			if (!isNumeric(maxPayout) || parseInt(maxPayout) < 0) {
				Validators.show(DangerMonitor.CachedDOM.txtMaxPayout.get(0), 'Max Payout must be greater or than 0');
				return null;
			}

			DangerMonitor.GetData(agentIds, combos, maxPayout, isFromConfigList);
		});
	},
	RenderData: function (result) {
		DangerMonitor.CachedDOM.divResult.html(result);
		DangerMonitor.Params.MaxTransId = $('#hdfMaxTransId', DangerMonitor.CachedDOM.divResult).val();
		DangerMonitor.Params.AgentIdList = $('#hdfAgentIdList', DangerMonitor.CachedDOM.divResult).val();

		$('a[data-match]', DangerMonitor.CachedDOM.divResult).each(function () {
			$(this).bind('click', function () {
				var matchId = $(this).attr('data-match');
				var matchName = $(this).text().trim();
				var leagueName = $(this).parent().prev().text();

				DangerMonitor.GetDetails(matchId, matchName, leagueName);
			});
		});
	},
	GetData: function (agentIds, combos, maxPayout, isFromConfigList) {
		DangerMonitor.Params.AgentIdList = agentIds;
		DangerMonitor.Params.Combos = combos;
		DangerMonitor.Params.IsFromConfigList = isFromConfigList;
		DangerMonitor.Params.MatchId = null;
		DangerMonitor.Params.MaxTransId = null;
		DangerMonitor.Params.MaxPayout = maxPayout;

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
	GetDetails: function (matchId, matchName, leagueName) {
		DangerMonitor.Params.MatchId = matchId;

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