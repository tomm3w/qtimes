using System;
using System.Collections.Generic;

namespace QTimes.core.dal.Models
{
    public partial class Concert
    {
        public Concert()
        {
            ConcertEvent = new HashSet<ConcertEvent>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public Guid ReservationBusinessId { get; set; }
        public string VirtualNo { get; set; }
        public string PhoneNo { get; set; }
        public string MobileNo { get; set; }
        public DateTime? Date { get; set; }
        public TimeSpan? TimeFrom { get; set; }
        public TimeSpan? TimeTo { get; set; }
        public bool? EnablePrivacyPolicy { get; set; }
        public bool? EnableServiceTerms { get; set; }
        public bool? EnableCommunityGuidelines { get; set; }
        public string PrivacyPolicyFilePath { get; set; }
        public string ServiceTermsFilePath { get; set; }
        public string CummunityGuidelinesFilePath { get; set; }
        public DateTime? DeletedDate { get; set; }

        public virtual ReservationBusiness ReservationBusiness { get; set; }
        public virtual ICollection<ConcertEvent> ConcertEvent { get; set; }
    }
}
