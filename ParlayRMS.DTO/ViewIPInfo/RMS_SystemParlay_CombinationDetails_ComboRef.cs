// --------------------------------------------------------------------------
// <copyright file="RMS_SystemParlay_CombinationDetails_ComboRef.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ken.Nguyen" date="2014.04.24">Created</change>
// </history>

namespace ParlayRMS.DTO.ViewIPInfo
{
    /// <summary>
    /// CombinationDetails ComboRef model.
    /// </summary>
    public class RMS_SystemParlay_CombinationDetails_ComboRef
    {
        public int Serial { get; set; }

        public int HomeId { get; set; }

        public int AwayId { get; set; }
    }
}