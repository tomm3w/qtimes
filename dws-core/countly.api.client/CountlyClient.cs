using System;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;

namespace countly.api.client
{
	public class CountlyClient : ICountlyClient
	{
		private readonly RestClient _client;

		public CountlyClient()
		{
			_client = new RestClient(ConfigurationManager.AppSettings["countlyApiEndpoint"]);
		}

		public int GetOpenedPassesCount(string apiKey, string countlyAppId, IEnumerable<int> businessIds, DateTime from, DateTime to)
		{
			var result = MakeCountlyRequest(apiKey, countlyAppId, "events", "passopened", "bid", from, to);

			var sum = businessIds.Sum(businessId => GetValueByParam(result, businessId.ToString(), from, to));

			return sum;
		}

		public CountlyDashboardAnalytics GetCountlyDashboardAnalytics(string apiKey, string countlyAppId, DateTime from, DateTime to)
		{
			var result = MakeCountlyRequest(apiKey, countlyAppId, "users", null, null, from, to);

			var totalSessions = GetParamSum(result, "t", from, to);
			var totalUsers = GetParamSum(result, "u", from, to);
			var newUsers = GetParamSum(result, "n", from, to);

			return new CountlyDashboardAnalytics()
			{
				TotalSessions = totalSessions,
				TotalUsers = totalUsers,
				NewUsers = newUsers
			};
		}

		public int GetBusinessView(string apiKey, string countlyAppId, IEnumerable<int> businessIds, DateTime from, DateTime to)
		{
			var result = MakeCountlyRequest(apiKey, countlyAppId, "events", "businessview", "bid", from, to);

			var sum = businessIds.Sum(businessId => GetValueByParam(result, businessId.ToString(), from, to));

			return sum;
		}

		public int GetSearchresult(string apiKey, string countlyAppId, IEnumerable<int> businessIds, DateTime from, DateTime to)
		{
			var result = MakeCountlyRequest(apiKey, countlyAppId, "events", "searchbusiness", "bid", from, to);

			var sum = businessIds.Sum(businessId => GetValueByParam(result, businessId.ToString(), from, to));

			return sum;
		}

		public int GetPhoneCalls(string apiKey, string countlyAppId, IEnumerable<int> businessIds, DateTime from, DateTime to)
		{
			var result = MakeCountlyRequest(apiKey, countlyAppId, "events", "phonecall", "bid", from, to);

			var sum = businessIds.Sum(businessId => GetValueByParam(result, businessId.ToString(), from, to));

			return sum;
		}

		public int GetPushMessDeliveredOutside(string apiKey, string countlyAppId, IEnumerable<int> businessIds, DateTime from, DateTime to)
		{
			var result = MakeCountlyRequest(apiKey, countlyAppId, "events", "pushdeliveredoutsideregion", "bid", from, to);

			var sum = businessIds.Sum(businessId => GetValueByParam(result, businessId.ToString(), from, to));

			return sum;
		}

		public int GetPushMessDeliveredOutside(string apiKey, string countlyAppId, Guid messId, DateTime from, DateTime to)
		{
			var result = MakeCountlyRequest(apiKey, countlyAppId, "events", "pushdeliveredoutsideregion", "messageid", from, to);

			var sum = GetValueByParam(result, messId.ToString(), from, to);

			return sum;
		}

		public int GetPushMessDeliveredInside(string apiKey, string countlyAppId, IEnumerable<int> businessIds, DateTime from, DateTime to)
		{
			var result = MakeCountlyRequest(apiKey, countlyAppId, "events", "pushdeliveredinregion", "bid", from, to);

			var sum = businessIds.Sum(businessId => GetValueByParam(result, businessId.ToString(), from, to));

			return sum;
		}

		public int GetPushMessDeliveredInside(string apiKey, string countlyAppId, Guid messId, DateTime from, DateTime to)
		{
			var result = MakeCountlyRequest(apiKey, countlyAppId, "events", "pushdeliveredinregion", "messageid", from, to);

			var sum = GetValueByParam(result, messId.ToString(), from, to);

			return sum;
		}

		public int GetPushMessOpened(string apiKey, string countlyAppId, IEnumerable<int> businessIds, DateTime from, DateTime to)
		{
			var result = MakeCountlyRequest(apiKey, countlyAppId, "events", "messageopened", "bid", from, to);

			var sum = businessIds.Sum(businessId => GetValueByParam(result, businessId.ToString(), from, to));

			return sum;
		}

		public int GetPushMessOpened(string apiKey, string countlyAppId, Guid messId, DateTime from, DateTime to)
		{
			var result = MakeCountlyRequest(apiKey, countlyAppId, "events", "messageopened", "messageid", from, to);

			var sum = GetValueByParam(result, messId.ToString(), from, to);

			return sum;
		}

		private IDictionary<string, object> MakeCountlyRequest(string apiKey, string appId, string method, string countlyEvent, string segmentation, DateTime? from, DateTime? to)
		{
			var geturl = string.Format("/o?api_key={0}&app_id={1}&method={2}", apiKey, appId, method);

			if (!string.IsNullOrEmpty(countlyEvent))
			{
				geturl += string.Format("&event={0}", countlyEvent);
			}

			if (!string.IsNullOrEmpty(segmentation))
			{
				geturl += string.Format("&segmentation={0}", segmentation);
			}

			if (from.HasValue && to.HasValue)
			{
				geturl += string.Format("&period=[{0},{1}]", from.Value.ToUnixTimeStamp(), to.Value.ToUnixTimeStamp());
			}

			var request = new RestRequest(geturl, Method.GET);
			var response = _client.Get(request);
			if(string.IsNullOrEmpty(response.Content))
				throw new InvalidOperationException("Cannot connect to analytics server.");

			var result = (IDictionary<string, object>)SimpleJson.DeserializeObject(response.Content);
			return result;
		}

		private int GetValueByParam(IDictionary<string, object> data, string paramName, DateTime dFrom, DateTime dTo)
		{
			var range = new Range(dFrom, dTo);
			var sum = 0;
			foreach (var yearKey in data.Keys)
			{
				if (!IsNumber(yearKey))
					continue;
				var year = (IDictionary<string, object>)data[yearKey];

				foreach (var monthKey in year.Keys)
				{
					var monthKeyNum = GetNumber(monthKey);
					if (monthKeyNum == 0 || monthKeyNum > 12)
						continue;

					var month = (IDictionary<string, object>)year[monthKey];

					foreach (var dayKey in month.Keys)
					{
						var dayKeyNum = GetNumber(dayKey);
						if (dayKeyNum == 0 || dayKeyNum > 31)
							continue;

						var day = (IDictionary<string, object>)month[dayKey];

						if (!range.InRange(GetNumber(yearKey), GetNumber(monthKey), GetNumber(dayKey)))
							continue;

						if (!day.ContainsKey(paramName))
							continue;

						var bObj = (IDictionary<string, object>)day[paramName];
						if (!bObj.ContainsKey("c"))
							continue;

						if (!IsNumber(bObj["c"]))
							continue;

						var val = GetNumber(bObj["c"]);

						sum += val;
					}
				}
			}

			return sum;
		}

		private int GetParamSum(IDictionary<string, object> data, string paramName, DateTime dFrom, DateTime dTo)
		{
			var range = new Range(dFrom, dTo);
			var sum = 0;
			foreach (var yearKey in data.Keys)
			{
				if (!IsNumber(yearKey))
					continue;
				var year = (IDictionary<string, object>)data[yearKey];

				foreach (var monthKey in year.Keys)
				{
					var monthKeyNum = GetNumber(monthKey);
					if (monthKeyNum == 0 || monthKeyNum > 12)
						continue;
					var month = (IDictionary<string, object>)year[monthKey];

					foreach (var dayKey in month.Keys)
					{
						var dayKeyNum = GetNumber(dayKey);
						if (dayKeyNum == 0 || dayKeyNum > 31)
							continue;

						var day = (IDictionary<string, object>)month[dayKey];
						if (!range.InRange(GetNumber(yearKey), GetNumber(monthKey), GetNumber(dayKey)))
							continue;

						if (!day.ContainsKey(paramName))
							continue;

						var par = GetNumber(day[paramName]);
						sum += par;
					}
				}
			}

			return sum;
		}

		private bool IsNumber(object value)
		{
			int result;
			return int.TryParse(value.ToString(), out result);
		}

		private int GetNumber(object value)
		{
			int result;
			int.TryParse(value.ToString(), out result);
			return result;
		}

	}

	public class Range
	{
		private DateTime _from;
		private DateTime _to;

		public Range(DateTime from, DateTime to)
		{
			_from = from.Date;
			_to = to.Date;
		}

		public bool InRange(int year, int month, int day)
		{
			var chDate = new DateTime(year, month, day);
			if (chDate >= _from && chDate <= _to)
				return true;
			return false;
		}
	}
}
