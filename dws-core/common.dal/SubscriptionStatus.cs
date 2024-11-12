using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace common.dal
{
	public enum SubscriptionStatus
	{
		Unknown = 0,
		Pending = 1,
		Active = 2,
		Hold = 3,
		InActive = 4
	}
}