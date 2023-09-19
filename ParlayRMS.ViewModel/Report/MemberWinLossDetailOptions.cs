using System;
using System.Web.Mvc;

namespace ParlayRMS.ViewModel.Report
{
	public class MemberWinLossDetailOptions
	{
		public DateTime FromDate { get; set; }

		public DateTime ToDate { get; set; }

		public string ParlayTypes { get; set; }

		public string SportsType { get; set; }

		public byte LiveType { get; set; }

		public string Combos { get; set; }

		[AllowHtml]
		public string Username { get; set; }
	}
}
