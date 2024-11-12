using System;
using System.Collections.Generic;

namespace QTimes.core.dal.Models
{
    public partial class BusinessType
    {
        public BusinessType()
        {
            BusinessDetail = new HashSet<BusinessDetail>();
        }

        public short Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<BusinessDetail> BusinessDetail { get; set; }
    }
}
