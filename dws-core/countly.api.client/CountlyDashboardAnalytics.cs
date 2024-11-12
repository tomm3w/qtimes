using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace countly.api.client
{
	public class CountlyDashboardAnalytics
	{
		public int TotalSessions { get; set; }

		public int TotalUsers { get; set; }

		public int NewUsers { get; set; }

		public static CountlyDashboardAnalytics operator +(CountlyDashboardAnalytics first, CountlyDashboardAnalytics second)
		{
			return new CountlyDashboardAnalytics()
			{
				TotalSessions = first.TotalSessions + second.TotalSessions,
				TotalUsers = first.TotalUsers + second.TotalUsers,
				NewUsers = first.NewUsers + second.NewUsers
			};
		}
	}
}
