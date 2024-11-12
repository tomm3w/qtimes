using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace countly.api.client
{
	public static class DateTimeExt
	{
		public static double ToUnixTimeStamp(this DateTime date)
		{
			var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
			var diff = date - origin;
			return Math.Floor(diff.TotalMilliseconds);
		}
	}
}
