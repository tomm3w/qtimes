using common.api.Queries;
using common.dal;
using Core.Extensions;
using SeatQ.core.dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using WebGrease.Css.Extensions;

namespace SeatQ.core.api.Queries.Reservation.GetConcertReservations
{
    public class GetConcertReservationsQuery : IQuery<GetConcertReservationsResponse, GetConcertReservationsRequest>
    {
        private readonly IGenericRepository<SeatQEntities, dal.Models.ConcertReservation> _repository;
        private readonly IGenericRepository<SeatQEntities, UserInfo> _userInfoRepository;
        public GetConcertReservationsQuery(IGenericRepository<SeatQEntities, dal.Models.ConcertReservation> repository,
            IGenericRepository<SeatQEntities, UserInfo> userInfoRepository)
        {
            _repository = repository;
            _userInfoRepository = userInfoRepository;
        }

        public GetConcertReservationsResponse Handle(GetConcertReservationsRequest request)
        {

            var reservations = _repository.FindBy(x => x.ConcertId == request.ConcertId);

            if (!string.IsNullOrEmpty(request.SearchText))
                reservations = reservations.Where(x => x.ConcertGuests.Any(y => y.Name == request.SearchText || y.MobileNo == request.SearchText || y.SeatNo == request.SearchText));

            var data = new List<ConcertReservationData>();
            reservations.ForEach(x =>
            {
                var reserve = new ConcertReservationData
                {
                    Id = x.Id,
                    Size = x.Size,
                    GuestTypeId = x.GuestTypeId,
                    GuestType = x.GuestType.GuestType1,
                    ConfirmationNo = x.ConfirmationNo
                };

                var mainGuest = x.ConcertGuests.Where(y => y.IsMainGuest == true).FirstOrDefault();
                if (mainGuest != null)
                {
                    reserve.MainGuestId = mainGuest.Id;
                    reserve.MainGuestName = mainGuest.Name;
                    reserve.MainGuestMobileNo = mainGuest.MobileNo;
                    reserve.Photo = mainGuest.Photo;
                    reserve.PhotoFullPath = SeatQ.core.api.Helpers.ImageHelper.ContentAbsUrl("/Content/Uploads/Reservation/" + mainGuest.Photo);
                    reserve.SeatNo = mainGuest.SeatNo;
                    reserve.DOB = mainGuest.DOB != null ? mainGuest.DOB.Value.ToString("yyyy-MM-dd") : "";
                    reserve.MainGuestAddress = mainGuest.Address;
                    reserve.MainGuestCity = mainGuest.City;
                    reserve.MainGuestState = mainGuest.State;
                    reserve.MainGuestZip = mainGuest.Zip;
                    reserve.MainGuestEmail = mainGuest.Email;
                }

                x.ConcertGuests.ForEach(z =>
                {
                    reserve.Guests.Add(new ConcertReservationGuest
                    {
                        ConcertReservationId = (int)z.ConcertReservationId,
                        ConcertGuestId = z.Id,
                        Name = z.Name,
                        MobileNo = z.MobileNo,
                        Email = z.Email,
                        SeatNo = z.SeatNo,
                        Temperature = z.Temperature ?? "",
                        CheckInTime = z.CheckInTime != null && z.CheckInTime != default(DateTime) ? z.CheckInTime.Value.ToUniversalTimeString() : null,
                        Address = z.Address,
                        City = z.City,
                        State = z.State,
                        Zip = z.Zip,
                        IsMainGuest = z.IsMainGuest,
                        SpotValue = z.SeatNo,
                        IsCheckedIn = z.CheckInTime != null,
                        IsSolo = z.IsSolo
                    });
                });

                data.Add(reserve);

            });

            int numOfRecords = reservations.Count();
            var paging = new PagingModel
            {
                Page = request.Page,
                PageSize = request.PageSize,
                TotalData = numOfRecords,
                TotalPages = (int)Math.Ceiling((decimal)numOfRecords / (decimal)request.PageSize)
            };

            return new GetConcertReservationsResponse(data, paging);

        }

    }
}