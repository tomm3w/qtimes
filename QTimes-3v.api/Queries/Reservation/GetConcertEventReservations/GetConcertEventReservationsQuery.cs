using iVeew.common.api.Queries;
using iVeew.Core.Extensions;
using IVeew.common.dal;
using QTimes.api.Infrastructure;
using QTimes.core.dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QTimes.api.Queries.Reservation.GetConcertEventReservations
{
    public class GetConcertEventReservationsQuery : IQuery<GetConcertEventReservationsResponse, GetConcertEventReservationsRequest>
    {
        private readonly IGenericRepository<QTimesContext, ConcertEventReservation> _repository;
        private readonly IGenericRepository<QTimesContext, UserInfo> _userInfoRepository;
        private readonly IBlobService _blobService;

        public GetConcertEventReservationsQuery(IGenericRepository<QTimesContext, ConcertEventReservation> repository,
            IGenericRepository<QTimesContext, UserInfo> userInfoRepository, IBlobService blobService)
        {
            _repository = repository;
            _userInfoRepository = userInfoRepository;
            _blobService = blobService;
        }

        public GetConcertEventReservationsResponse Handle(GetConcertEventReservationsRequest request)
        {
            IQueryable<ConcertEventReservation> reservations;
            if (request.ConcertEventId.HasValue)
            {
                reservations = _repository.FindBy(x => x.ConcertEventId == request.ConcertEventId && x.ConcertEvent.DeletedDate == null && x.ConcertEvent.Concert.DeletedDate == null);
            }
            else
            {
                reservations = _repository.FindBy(x => x.ConcertEvent.ConcertId == request.ConcertId && x.ConcertEvent.DeletedDate == null && x.ConcertEvent.Concert.DeletedDate == null);
            }
            if (!string.IsNullOrEmpty(request.SearchText))
                reservations = reservations.Where(x => x.ConcertEventGuest.Any(y => y.Name == request.SearchText || y.MobileNo == request.SearchText || y.SeatNo == request.SearchText));

            var data = new List<ConcertEventReservationData>();

            foreach (var x in reservations.ToList())
            {
                var reserve = new ConcertEventReservationData
                {
                    Id = x.Id,
                    Size = x.Size,
                    GuestTypeId = x.GuestTypeId,
                    GuestType = x.GuestType.GuestType1,
                    ConfirmationNo = x.ConfirmationNo,
                    PassUrl = x.PassUrl
                };

                var mainGuest = x.ConcertEventGuest.Where(y => y.IsMainGuest == true).FirstOrDefault();

                if (mainGuest != null)
                {
                    reserve.MainGuestId = mainGuest.Id;
                    reserve.MainGuestName = mainGuest.Name;
                    reserve.MainGuestMobileNo = mainGuest.MobileNo;
                    reserve.Photo = mainGuest.Photo;
                    //reserve.PhotoFullPath = _blobService.GetFileUrl(mainGuest.Photo);
                    reserve.SeatNo = mainGuest.SeatNo;
                    reserve.DOB = mainGuest.Dob != null ? mainGuest.Dob.Value.ToString("yyyy-MM-dd") : "";
                    reserve.MainGuestAddress = mainGuest.Address;
                    reserve.MainGuestCity = mainGuest.City;
                    reserve.MainGuestState = mainGuest.State;
                    reserve.MainGuestZip = mainGuest.Zip;
                    reserve.MainGuestEmail = mainGuest.Email;
                    reserve.FacePhotoImageBase64 = mainGuest.FacePhotoImageBase64;
                }

                foreach (var z in x.ConcertEventGuest)
                {
                    reserve.Guests.Add(new ConcertEventReservationGuest
                    {
                        ConcertEventReservationId = z.ConcertEventReservationId.Value,
                        ConcertGuestId = z.Id,
                        Name = z.Name,
                        MobileNo = z.MobileNo,
                        Email = z.Email,
                        SeatName = z.SeatNo.Split('-')[0],
                        SeatNo = z.SeatNo.Split('-')[1],
                        Temperature = z.Temperature ?? "",
                        CheckInTime = z.CheckInTime != null && z.CheckInTime != default(DateTime) ? z.CheckInTime.Value.ToUniversalTimeString() : null,
                        Address = z.Address,
                        City = z.City,
                        State = z.State,
                        Zip = z.Zip,
                        IsMainGuest = z.IsMainGuest,
                        SpotValue = z.SeatNo,
                        IsCheckedIn = z.CheckInTime != null,
                        IsSolo = z.IsSolo ?? true
                    });
                };

                data.Add(reserve);

            };

            int numOfRecords = reservations.Count();
            var paging = new PagingModel
            {
                Page = request.Page,
                PageSize = request.PageSize,
                TotalData = numOfRecords,
                TotalPages = (int)Math.Ceiling((decimal)numOfRecords / (decimal)request.PageSize)
            };

            return new GetConcertEventReservationsResponse(data, paging);

        }

    }
}