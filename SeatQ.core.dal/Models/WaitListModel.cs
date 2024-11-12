using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SeatQ.core.dal.Models
{
    [MetadataType(typeof(WaitListMetaType))]
    public partial class WaitList
    {
        public class WaitListMetaType
        {
            [Required]
            public Nullable<int> RestaurantChainId { get; set; }
            [Required]
            [Display(Name = "Group Size")]
            public Nullable<int> GroupSize { get; set; }
            [Required]
            public Nullable<int> EnteredBy { get; set; }
            [Required]
            [Display(Name = "Estimated Time")]
            public Nullable<DateTime> EstimatedDateTime { get; set; }
            [Required]
            [Display(Name = "Guest Type")]
            public Nullable<int> GuestTypeId { get; set; }
        }
    }

    public class AddWaitListModel : WaitList
    {
        [Required]
        [Display(Name = "Guest Name")]
        public string GuestName { get; set; }
        [Required]
        [Display(Name = "Cellphone No.")]
        public string MobileNumber { get; set; }
    }

    public class InsertWaitList
    {
        public Nullable<int> WaitListId { get; set; }
        public Nullable<int> RestaurantChainId { get; set; }

        [Required]
        [Display(Name = "Guest Name")]
        public string GuestName { get; set; }
        [Required]
        [Display(Name = "Cellphone No.")]
        public string MobileNumber { get; set; }
        [Required]
        [Display(Name = "Group Size")]
        public Nullable<int> GroupSize { get; set; }
        [Display(Name = "Estimated Time")]
        public Nullable<DateTime> EstimatedDateTime { get; set; }
        [Display(Name = "Actual Time")]
        public Nullable<DateTime> ActualDateTime { get; set; }
        [Required]
        [Display(Name = "Guest Type")]
        public Nullable<int> GuestTypeId { get; set; }
        public string RoomNumber { get; set; }
        public int? TableId{ get; set; }
        public string Comments { get; set; }
        public string DeviceId { get; set; }

    }

    public class WaitListModel
    {
        public List<GetWaitList_Result> Businesses { get; set; }
        public PagingModel Paging { get; set; }
    }

    public class WaitListIdModel
    {
        [Required]
        public Nullable<long> WaitListId { get; set; }
    }
    public class WaitListSeatedModel
    {
        [Required]
        public Nullable<long> WaitListId { get; set; }
        [Required]
        public Nullable<int> TableId { get; set; }
    }

    public class ReturnGuestModel
    {
        public List<GetReturnGuest_Result> Businesses { get; set; }
        public PagingModel Paging { get; set; }
    }

    public class LoyaltyModel
    {
        public List<GetLoyalty_Result> Businesses { get; set; }
        public PagingModel Paging { get; set; }
    }
}