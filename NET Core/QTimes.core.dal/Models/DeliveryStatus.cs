using System;
using System.Collections.Generic;

namespace QTimes.core.dal.Models
{
    public partial class DeliveryStatus
    {
        public int DeliveryStatusId { get; set; }
        public string Msisdn { get; set; }
        public string To { get; set; }
        public string NetworkCode { get; set; }
        public string MessageId { get; set; }
        public string Price { get; set; }
        public string Status { get; set; }
        public string Scts { get; set; }
        public string ErrCode { get; set; }
        public DateTime? MessageTimestamp { get; set; }
        public string Project { get; set; }
    }
}
