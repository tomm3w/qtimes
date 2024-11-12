using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SeatQ.core.dal.Models
{

    [MetadataType(typeof(ReadyMessageMetaType))]
    public partial class ReadyMessage
    {
        public class ReadyMessageMetaType
        {
            [Required]
            public Nullable<int> RestaurantChainId { get; set; }

            [Required]
            [Display(Name = "Ready Message")]
            public string ReadyMessage1 { get; set; }
        }
    }

    [MetadataType(typeof(VisitMessageMetaType))]
    public partial class VisitMessage
    {
        public class VisitMessageMetaType
        {
            [Required]
            public Nullable<int> RestaurantChainId { get; set; }

            [Required]
            public Nullable<int> Visit { get; set; }

            [Required]
            [Display(Name="Visit Message")]
            public string VisitMessage1 { get; set; }
        }
    }

    public class MessageModel
    {
        [Required]
        public int? RestaurantChainId { get; set; }
        public List<ReadyMessage> ReadyMessage { get; set; }
        public List<VisitMessage> VisitMessage { get; set; }
    }

    public class AllMessagesModel
    {
        public Nullable<int> RestaurantChainId { get; set; }
        public List<ReadyMessage> ReadyMessage { get; set; }
        public List<VisitMessage> VisitMessage { get; set; }
    }

    public class SendMessageToGuestModel
    {
        [Required]
        public int? WaitListId { get; set; }
        [Required]
        public string Message { get; set; }
    }

    public class SendLoyaltyMessageToGuestModel
    {
        [Required]
        public int? GuestId { get; set; }
        [Required]
        public int? RestaurantChainId { get; set; }

        [Required]
        public string Message { get; set; }
    }

}