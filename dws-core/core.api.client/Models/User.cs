using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.api.client.Models
{
	public class User
	{
		public string Id { get; set; }
		public string UserName { get; set; }
		public string Email { get; set; }
		public string FullName { get; set; }
		public string Phone { get; set; }
		public int? Avatar { get; set; }
		public string Address { get; set; }
		public string City { get; set; }
		public object State { get; set; }
		public string Zip { get; set; }
		public string StripeId { get; set; }
		public IList<Subscription> Subscriptions { get; set; }
	}
}
