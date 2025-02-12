//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SeatQ.core.dal.Models
{
    using System;
    
    public partial class GetWaitList_Result
    {
        public long WaitListId { get; set; }
        public string GuestName { get; set; }
        public string MobileNumber { get; set; }
        public Nullable<int> GroupSize { get; set; }
        public Nullable<System.DateTime> ActualDateTime { get; set; }
        public Nullable<System.DateTime> EstimatedDateTime { get; set; }
        public Nullable<byte> MessageReply { get; set; }
        public Nullable<bool> IsSeated { get; set; }
        public Nullable<System.DateTime> SeatedDateTime { get; set; }
        public Nullable<int> NoOfReturn { get; set; }
        public Nullable<int> Visit { get; set; }
        public Nullable<int> GuestTypeId { get; set; }
        public Nullable<bool> IsMessageSent { get; set; }
        public Nullable<int> TableId { get; set; }
        public string TableNumber { get; set; }
        public string RoomNumber { get; set; }
        public Nullable<int> GuestMessageCount { get; set; }
        public Nullable<int> AvgTableTime { get; set; }
        public Nullable<bool> IsLeft { get; set; }
        public Nullable<System.DateTime> LeftDateTime { get; set; }
        public string Comments { get; set; }
        public int RemainingTime { get; set; }
        public int TotalMinuteSat { get; set; }
        public Nullable<int> LateSeating { get; set; }
        public Nullable<int> UnReadMessageCount { get; set; }
    }
}
