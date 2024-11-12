using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace countly.api.client
{
	public interface ICountlyClient
	{
		int GetOpenedPassesCount(string apiKey, string countlyAppId, IEnumerable<int> businessIds, DateTime from, DateTime to);

		CountlyDashboardAnalytics GetCountlyDashboardAnalytics(string apiKey, string countlyAppId, DateTime from, DateTime to);

		int GetBusinessView(string apiKey, string countlyAppId, IEnumerable<int> businessIds, DateTime from, DateTime to);

		int GetSearchresult(string apiKey, string countlyAppId, IEnumerable<int> businessIds, DateTime from, DateTime to);

		int GetPhoneCalls(string apiKey, string countlyAppId, IEnumerable<int> businessIds, DateTime from, DateTime to);

		int GetPushMessDeliveredOutside(string apiKey, string countlyAppId, IEnumerable<int> businessIds, DateTime from, DateTime to);

		int GetPushMessDeliveredOutside(string apiKey, string countlyAppId, Guid messId, DateTime from, DateTime to);

		int GetPushMessDeliveredInside(string apiKey, string countlyAppId, IEnumerable<int> businessIds, DateTime from, DateTime to);

		int GetPushMessDeliveredInside(string apiKey, string countlyAppId, Guid messId, DateTime from, DateTime to);

		int GetPushMessOpened(string apiKey, string countlyAppId, IEnumerable<int> businessIds, DateTime from, DateTime to);

		int GetPushMessOpened(string apiKey, string countlyAppId, Guid messId, DateTime from, DateTime to);
	}
}
