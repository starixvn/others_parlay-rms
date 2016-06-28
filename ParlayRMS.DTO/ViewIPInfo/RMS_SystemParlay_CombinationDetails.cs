// --------------------------------------------------------------------------
// <copyright file="RMS_SystemParlay_CombinationDetails.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ken.Nguyen" date="2014.04.24">Created</change>
// </history>

namespace ParlayRMS.DTO.ViewIPInfo
{
    using System.Collections.Generic;

    /// <summary>
    /// CombinationDetails model.
    /// </summary>
    public class RMS_SystemParlay_CombinationDetails
    {
        public decimal Outstanding { get; set; }

        public IEnumerable<RMS_SystemParlay_CombinationDetails_Combo> Combo { get; set; }

        public IEnumerable<RMS_SystemParlay_CombinationDetails_ComboRef> ComboRef { get; set; }
    }
}