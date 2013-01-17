// -----------------------------------------------------------------------
// <copyright file="Config.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ParlayRMS.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class Config
    {
        public static DateTime WinlostDate
        {
            get
            {
                if (Convert.ToBoolean(ConfigurationManager.AppSettings["IsDebug"]))
                {
                    return new DateTime(2012, 11, 6).Date;
                }
                else
                {
                    return DateTime.Now.AddDays(-1).Date;
                }
            }
        }
    }
}