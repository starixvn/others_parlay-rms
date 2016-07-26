using ParlayRMS.Models;
using ParlayRMS.Models.MemberWinLoss;
using System;

namespace ParlayRMS.Services.Interface.MemberWinLoss
{
    public interface IMemberWinLossService
    {
        MemberWinLossData GetMemberWinLoss(
                              DateTime fromDate,
                              DateTime toDate,
                              string parlayTypes,
                              string sportsType,
                              string combos,
                              string username,
                              bool isCached,
                              out int roleId);
    }
}