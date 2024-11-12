using System;
using System.Collections.Generic;

namespace QTimes.core.dal.Models
{
    public partial class ReservationBusiness
    {
        public ReservationBusiness()
        {
            BusinessDetail = new HashSet<BusinessDetail>();
            Concert = new HashSet<Concert>();
            UserInfo = new HashSet<UserInfo>();
        }

        public Guid Id { get; set; }
        public string BusinessName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string TimezoneOffset { get; set; }
        public string TimezoneOffsetValue { get; set; }

        public virtual ICollection<BusinessDetail> BusinessDetail { get; set; }
        public virtual ICollection<Concert> Concert { get; set; }
        public virtual ICollection<UserInfo> UserInfo { get; set; }
    }
}
