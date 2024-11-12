//using QueryStringAlias.Attributes;
//using System;
//using System.ComponentModel.DataAnnotations;
//using QueryStringAlias.ModelBinders;
//using System.Web.ModelBinding;
//using System.Web.Mvc;


//namespace SeatQ.core.dal.Models
//{
//    public class SMSModel
//    {
//        [Required]
//        public string From { get; set; }
//        [Required]
//        public string To { get; set; }
//        [Required]
//        public string Text { get; set; }
//    }


//    [ModelBinder(typeof(AliasModelBinder))]
//    public class InBoundMessageModel
//    {
//        public string type { get; set; }
//        public string text { get; set; }
//        public string to { get; set; }
//        public string msisdn { get; set; }
//        [BindAlias("network-code")]
//        public string network_code { get; set; }
//        public string messageId { get; set; }
//        [BindAlias("message-timestamp")]
//        public DateTime message_timestamp { get; set; }
//    }


//    [ModelBinder(typeof(AliasModelBinder))]
//    public class DeliveryReceiptModel
//    {
//        public string msisdn { get; set; }
//        public string to { get; set; }
//        [BindAlias("network-code")]
//        public string network_code { get; set; }
//        public string messageId { get; set; }
//        public string price { get; set; }
//        public string status { get; set; }
//        public string scts { get; set; }
//        [BindAlias("err-code")]
//        public string err_code { get; set; }
//        [BindAlias("message-timestamp")]
//        public DateTime message_timestamp { get; set; }
//    }

//}