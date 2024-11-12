using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using common.dal;

namespace core.api.client.Models
{
	public class Subscription
	{
		public int Id { get; set; }
		public string SubscriptionPlanId { get; set; }
		public DateTime ActiveFrom { get; set; }
		public DateTime ActiveTo { get; set; }
		public DateTime NextBilling { get; set; }
		public SubscriptionStatus Status { get; set; }
		public string SubscriptionPlan { get; set; }
		public int SubscriptionInterval { get; set; }
		public string SubscriptionPlanSource { get; set; }
		public string SubscriptionPlanMetadata { get; set; }
	}
}
