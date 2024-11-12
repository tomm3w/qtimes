﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class SeatQEntities : DbContext
    {
        public SeatQEntities()
            : base("name=SeatQEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<GuestType> GuestTypes { get; set; }
        public virtual DbSet<MessageReply> MessageReplies { get; set; }
        public virtual DbSet<MessageReplyType> MessageReplyTypes { get; set; }
        public virtual DbSet<MessageSent> MessageSents { get; set; }
        public virtual DbSet<PlanType> PlanTypes { get; set; }
        public virtual DbSet<ReadyMessage> ReadyMessages { get; set; }
        public virtual DbSet<Restaurant> Restaurants { get; set; }
        public virtual DbSet<VisitMessage> VisitMessages { get; set; }
        public virtual DbSet<VisitMessageSent> VisitMessageSents { get; set; }
        public virtual DbSet<DeliveryStatu> DeliveryStatus { get; set; }
        public virtual DbSet<SeatedMessageSent> SeatedMessageSents { get; set; }
        public virtual DbSet<UserInfo> UserInfoes { get; set; }
        public virtual DbSet<UsersInRestaurant> UsersInRestaurants { get; set; }
        public virtual DbSet<UserProfile> UserProfiles { get; set; }
        public virtual DbSet<RestaurantChain> RestaurantChains { get; set; }
        public virtual DbSet<RestaurantClosedDay> RestaurantClosedDays { get; set; }
        public virtual DbSet<TableType> TableTypes { get; set; }
        public virtual DbSet<RestaurantTable> RestaurantTables { get; set; }
        public virtual DbSet<WaitList> WaitLists { get; set; }
        public virtual DbSet<GuestMessage> GuestMessages { get; set; }
        public virtual DbSet<LoyaltyMessage> LoyaltyMessages { get; set; }
        public virtual DbSet<GuestInfo> GuestInfoes { get; set; }
        public virtual DbSet<StaffType> StaffTypes { get; set; }
        public virtual DbSet<ReservationMessage> ReservationMessages { get; set; }
        public virtual DbSet<ConcertReservation> ConcertReservations { get; set; }
        public virtual DbSet<ConcertGuest> ConcertGuests { get; set; }
        public virtual DbSet<ConcertMessageRule> ConcertMessageRules { get; set; }
        public virtual DbSet<ConcertSeating> ConcertSeatings { get; set; }
        public virtual DbSet<ConcertReservationMessage> ConcertReservationMessages { get; set; }
        public virtual DbSet<ConcertSeatLock> ConcertSeatLocks { get; set; }
        public virtual DbSet<BusinessType> BusinessTypes { get; set; }
        public virtual DbSet<Reservation> Reservations { get; set; }
        public virtual DbSet<ReservationGuest> ReservationGuests { get; set; }
        public virtual DbSet<ReservationMessageRule> ReservationMessageRules { get; set; }
        public virtual DbSet<BusinessDetail> BusinessDetails { get; set; }
        public virtual DbSet<ReservationBusiness> ReservationBusinesses { get; set; }
        public virtual DbSet<Concert> Concerts { get; set; }
    
        public virtual int GetAverageWaitTime(Nullable<int> restaurantChainId, ObjectParameter averageWaitTime)
        {
            var restaurantChainIdParameter = restaurantChainId.HasValue ?
                new ObjectParameter("RestaurantChainId", restaurantChainId) :
                new ObjectParameter("RestaurantChainId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SeatQEntities.GetAverageWaitTime", restaurantChainIdParameter, averageWaitTime);
        }
    
        public virtual int GetMetrics(Nullable<int> restaurantChainId, string type, Nullable<System.DateTime> startDate, Nullable<System.DateTime> endDate, ObjectParameter seated, ObjectParameter noShow, ObjectParameter promotions, ObjectParameter guest, ObjectParameter groupSize, ObjectParameter averageWaitTime)
        {
            var restaurantChainIdParameter = restaurantChainId.HasValue ?
                new ObjectParameter("RestaurantChainId", restaurantChainId) :
                new ObjectParameter("RestaurantChainId", typeof(int));
    
            var typeParameter = type != null ?
                new ObjectParameter("Type", type) :
                new ObjectParameter("Type", typeof(string));
    
            var startDateParameter = startDate.HasValue ?
                new ObjectParameter("StartDate", startDate) :
                new ObjectParameter("StartDate", typeof(System.DateTime));
    
            var endDateParameter = endDate.HasValue ?
                new ObjectParameter("EndDate", endDate) :
                new ObjectParameter("EndDate", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SeatQEntities.GetMetrics", restaurantChainIdParameter, typeParameter, startDateParameter, endDateParameter, seated, noShow, promotions, guest, groupSize, averageWaitTime);
        }
    
        public virtual int UpdateUserEmail(Nullable<System.Guid> userId, string email)
        {
            var userIdParameter = userId.HasValue ?
                new ObjectParameter("UserId", userId) :
                new ObjectParameter("UserId", typeof(System.Guid));
    
            var emailParameter = email != null ?
                new ObjectParameter("Email", email) :
                new ObjectParameter("Email", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SeatQEntities.UpdateUserEmail", userIdParameter, emailParameter);
        }
    
        public virtual ObjectResult<GetSeatedMessageList_Result> GetSeatedMessageList()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetSeatedMessageList_Result>("SeatQEntities.GetSeatedMessageList");
        }
    
        public virtual ObjectResult<GetVisitedMessageList_Result> GetVisitedMessageList()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetVisitedMessageList_Result>("SeatQEntities.GetVisitedMessageList");
        }
    
        public virtual ObjectResult<GetReturnGuest_Result> GetReturnGuest(Nullable<int> restaurantChainId, string type, Nullable<System.DateTime> startDate, Nullable<System.DateTime> endDate, Nullable<int> startIndex, Nullable<int> pageSize, ObjectParameter totalCount)
        {
            var restaurantChainIdParameter = restaurantChainId.HasValue ?
                new ObjectParameter("RestaurantChainId", restaurantChainId) :
                new ObjectParameter("RestaurantChainId", typeof(int));
    
            var typeParameter = type != null ?
                new ObjectParameter("Type", type) :
                new ObjectParameter("Type", typeof(string));
    
            var startDateParameter = startDate.HasValue ?
                new ObjectParameter("StartDate", startDate) :
                new ObjectParameter("StartDate", typeof(System.DateTime));
    
            var endDateParameter = endDate.HasValue ?
                new ObjectParameter("EndDate", endDate) :
                new ObjectParameter("EndDate", typeof(System.DateTime));
    
            var startIndexParameter = startIndex.HasValue ?
                new ObjectParameter("StartIndex", startIndex) :
                new ObjectParameter("StartIndex", typeof(int));
    
            var pageSizeParameter = pageSize.HasValue ?
                new ObjectParameter("PageSize", pageSize) :
                new ObjectParameter("PageSize", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetReturnGuest_Result>("SeatQEntities.GetReturnGuest", restaurantChainIdParameter, typeParameter, startDateParameter, endDateParameter, startIndexParameter, pageSizeParameter, totalCount);
        }
    
        public virtual ObjectResult<string> SendVisitMessage(Nullable<int> restaurantChainId, Nullable<int> guestId, Nullable<int> visit)
        {
            var restaurantChainIdParameter = restaurantChainId.HasValue ?
                new ObjectParameter("RestaurantChainId", restaurantChainId) :
                new ObjectParameter("RestaurantChainId", typeof(int));
    
            var guestIdParameter = guestId.HasValue ?
                new ObjectParameter("GuestId", guestId) :
                new ObjectParameter("GuestId", typeof(int));
    
            var visitParameter = visit.HasValue ?
                new ObjectParameter("Visit", visit) :
                new ObjectParameter("Visit", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("SeatQEntities.SendVisitMessage", restaurantChainIdParameter, guestIdParameter, visitParameter);
        }
    
        public virtual ObjectResult<GetHostess_Result> GetHostess(Nullable<int> restaurantChainId)
        {
            var restaurantChainIdParameter = restaurantChainId.HasValue ?
                new ObjectParameter("RestaurantChainId", restaurantChainId) :
                new ObjectParameter("RestaurantChainId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetHostess_Result>("SeatQEntities.GetHostess", restaurantChainIdParameter);
        }
    
        public virtual ObjectResult<GetLoyalty_Result> GetLoyalty(Nullable<int> restaurantChainId, Nullable<int> startIndex, Nullable<int> pageSize, ObjectParameter totalCount)
        {
            var restaurantChainIdParameter = restaurantChainId.HasValue ?
                new ObjectParameter("RestaurantChainId", restaurantChainId) :
                new ObjectParameter("RestaurantChainId", typeof(int));
    
            var startIndexParameter = startIndex.HasValue ?
                new ObjectParameter("StartIndex", startIndex) :
                new ObjectParameter("StartIndex", typeof(int));
    
            var pageSizeParameter = pageSize.HasValue ?
                new ObjectParameter("PageSize", pageSize) :
                new ObjectParameter("PageSize", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetLoyalty_Result>("SeatQEntities.GetLoyalty", restaurantChainIdParameter, startIndexParameter, pageSizeParameter, totalCount);
        }
    
        public virtual ObjectResult<GetWaitList_Result> GetWaitList(Nullable<int> restaurantChainId, Nullable<int> startIndex, Nullable<int> pageSize, ObjectParameter totalCount)
        {
            var restaurantChainIdParameter = restaurantChainId.HasValue ?
                new ObjectParameter("RestaurantChainId", restaurantChainId) :
                new ObjectParameter("RestaurantChainId", typeof(int));
    
            var startIndexParameter = startIndex.HasValue ?
                new ObjectParameter("StartIndex", startIndex) :
                new ObjectParameter("StartIndex", typeof(int));
    
            var pageSizeParameter = pageSize.HasValue ?
                new ObjectParameter("PageSize", pageSize) :
                new ObjectParameter("PageSize", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetWaitList_Result>("SeatQEntities.GetWaitList", restaurantChainIdParameter, startIndexParameter, pageSizeParameter, totalCount);
        }
    
        public virtual ObjectResult<GetTablesWithSeating_Result> GetTablesWithSeating(Nullable<int> restaurantChainId)
        {
            var restaurantChainIdParameter = restaurantChainId.HasValue ?
                new ObjectParameter("RestaurantChainId", restaurantChainId) :
                new ObjectParameter("RestaurantChainId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetTablesWithSeating_Result>("SeatQEntities.GetTablesWithSeating", restaurantChainIdParameter);
        }
    
        public virtual ObjectResult<Nullable<int>> CloseTable(Nullable<int> restaurantChainId, Nullable<int> tableId)
        {
            var restaurantChainIdParameter = restaurantChainId.HasValue ?
                new ObjectParameter("RestaurantChainId", restaurantChainId) :
                new ObjectParameter("RestaurantChainId", typeof(int));
    
            var tableIdParameter = tableId.HasValue ?
                new ObjectParameter("TableId", tableId) :
                new ObjectParameter("TableId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("SeatQEntities.CloseTable", restaurantChainIdParameter, tableIdParameter);
        }
    
        public virtual ObjectResult<GetTableSummary_Result> GetTableSummary(Nullable<int> restaurantChainId)
        {
            var restaurantChainIdParameter = restaurantChainId.HasValue ?
                new ObjectParameter("RestaurantChainId", restaurantChainId) :
                new ObjectParameter("RestaurantChainId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetTableSummary_Result>("SeatQEntities.GetTableSummary", restaurantChainIdParameter);
        }
    
        public virtual ObjectResult<GetWaitListFromGuestReply_Result> GetWaitListFromGuestReply(string restaurantNumber, string mobileNumber, Nullable<System.DateTime> message_timestamp)
        {
            var restaurantNumberParameter = restaurantNumber != null ?
                new ObjectParameter("RestaurantNumber", restaurantNumber) :
                new ObjectParameter("RestaurantNumber", typeof(string));
    
            var mobileNumberParameter = mobileNumber != null ?
                new ObjectParameter("MobileNumber", mobileNumber) :
                new ObjectParameter("MobileNumber", typeof(string));
    
            var message_timestampParameter = message_timestamp.HasValue ?
                new ObjectParameter("message_timestamp", message_timestamp) :
                new ObjectParameter("message_timestamp", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetWaitListFromGuestReply_Result>("SeatQEntities.GetWaitListFromGuestReply", restaurantNumberParameter, mobileNumberParameter, message_timestampParameter);
        }
    
        public virtual ObjectResult<GetWaitListFromReply_Result> GetWaitListFromReply(string restaurantNumber, string mobileNumber, Nullable<System.DateTime> message_timestamp)
        {
            var restaurantNumberParameter = restaurantNumber != null ?
                new ObjectParameter("RestaurantNumber", restaurantNumber) :
                new ObjectParameter("RestaurantNumber", typeof(string));
    
            var mobileNumberParameter = mobileNumber != null ?
                new ObjectParameter("MobileNumber", mobileNumber) :
                new ObjectParameter("MobileNumber", typeof(string));
    
            var message_timestampParameter = message_timestamp.HasValue ?
                new ObjectParameter("message_timestamp", message_timestamp) :
                new ObjectParameter("message_timestamp", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetWaitListFromReply_Result>("SeatQEntities.GetWaitListFromReply", restaurantNumberParameter, mobileNumberParameter, message_timestampParameter);
        }
    
        public virtual ObjectResult<GetTimeSlots_Result> GetTimeSlots(Nullable<System.Guid> businessId, Nullable<System.DateTime> date)
        {
            var businessIdParameter = businessId.HasValue ?
                new ObjectParameter("BusinessId", businessId) :
                new ObjectParameter("BusinessId", typeof(System.Guid));
    
            var dateParameter = date.HasValue ?
                new ObjectParameter("Date", date) :
                new ObjectParameter("Date", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetTimeSlots_Result>("SeatQEntities.GetTimeSlots", businessIdParameter, dateParameter);
        }
    }
}
