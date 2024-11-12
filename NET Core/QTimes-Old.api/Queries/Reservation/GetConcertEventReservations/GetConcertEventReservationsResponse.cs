using iVeew.common.api.Queries;
using QTimes.core.dal.Models;
using System;
using System.Collections.Generic;

namespace QTimes.api.Queries.Reservation.GetConcertEventReservations
{
    public class GetConcertEventReservationsResponse : IQueryResponse
    {
        public List<ConcertEventReservationData> Model { get; set; }
        public PagingModel PagingModel { get; set; }
        public GetConcertEventReservationsResponse(List<ConcertEventReservationData> model, PagingModel pagingModel)
        {
            Model = model;
            PagingModel = pagingModel;
        }
    }

    public class ConcertEventReservationData
    {
        public ConcertEventReservationData()
        {
            Guests = new List<ConcertEventReservationGuest>();
        }
        public int Id { get; set; }
        public Guid ConcertId { get; set; }
        public Nullable<short> Size { get; set; }
        public string ConfirmationNo { get; set; }
        public string MainGuestName { get; set; }
        public string MainGuestMobileNo { get; set; }
        public string DOB { get; set; }
        public Nullable<int> GuestTypeId { get; set; }
        public string GuestType { get; set; }
        public string SeatNo { get; set; }
        public DateTime CheckInTime { get; set; }
        public string Photo { get; set; }
        public string PhotoFullPath { get; set; }
        public List<ConcertEventReservationGuest> Guests { get; set; }
        public int MainGuestId { get; set; }
        public string MainGuestAddress { get; set; }
        public string MainGuestCity { get; set; }
        public string MainGuestState { get; set; }
        public string MainGuestZip { get; set; }
        public string MainGuestEmail { get; set; }
        public Guid ConcertEventId { get; set; }
        public string FacePhotoImageBase64 { get; set; }
        public string PassUrl { get; set; }
    }

    public class ConcertEventReservationGuest
    {
        public int ConcertEventReservationId { get; set; }
        public int ConcertGuestId { get; set; }
        public string Name { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string SeatNo { get; set; }
        public string SeatName { get; set; }
        public string SpotValue { get; set; }
        public string CheckInTime { get; set; }
        public string Temperature { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public Nullable<bool> IsMainGuest { get; set; }
        public bool IsCheckedIn { get; set; }
        public bool? IsSolo { get; set; }
    }
}