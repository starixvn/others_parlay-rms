// -----------------------------------------------------------------------
// <copyright file="ICommonService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ParlayRMS.Services.Interface
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using ParlayRMS.DTO;
    using ParlayRMS.DTO.Common;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ICommonService
    {
        /// <summary>
        /// Gets the balance up day info.
        /// </summary>
        /// <returns></returns>
        RMS_SelBalanceUpDay GetBalanceUpDayInfo();
    }
}
