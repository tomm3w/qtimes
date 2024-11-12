using iVeew.common.api.Queries;
using IVeew.common.dal;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QTimes.api.Infrastructure;
using QTimes.core.dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QTimes.api.Queries.Reservation.GetReservationBusinessById
{
    public class GetReservationBusinessByIdQuery : IQuery<GetReservationBusinessByIdResponse, GetReservationBusinessByIdRequest>
    {
        private readonly IGenericRepository<QTimesContext, BusinessDetail> _reservationRepository;
        private readonly IBlobService _blobService;
        public GetReservationBusinessByIdQuery(IGenericRepository<QTimesContext, BusinessDetail> reservationRepository, IBlobService blobService)
        {
            _reservationRepository = reservationRepository;
            _blobService = blobService;
        }

        public GetReservationBusinessByIdResponse Handle(GetReservationBusinessByIdRequest request)
        {

            var business = _reservationRepository.FindBy(x => x.Id == request.Id).FirstOrDefault();

            if (business == null)
                throw new Exception("Reservation business not registered.");
            GetReservationBusinessByIdResponse result = new GetReservationBusinessByIdResponse(_blobService);
            result.Address = business.ReservationBusiness.Address;
            result.BusinessName = business.BusinessName;
            result.City = business.ReservationBusiness.City;
            result.LimitSizePerGroup = business.LimitSizePerGroup;
            result.LimitSizePerHour = business.LimitSizePerHour;
            result.LogoPath = business.LogoPath;
            result.State = business.ReservationBusiness.State;
            result.Zip = business.ReservationBusiness.Zip;
            result.TimeSlots = GetAvilableTimeSlots(request);
            result.EnableCommunityGuidelines = business.EnableCommunityGuidelines;
            result.EnablePrivacyPolicy = business.EnablePrivacyPolicy;
            result.EnableServiceTerms = business.EnableServiceTerms;
            result.CummunityGuidelinesFilePath = business.CummunityGuidelinesFilePath;
            result.PrivacyPolicyFilePath = business.PrivacyPolicyFilePath;
            result.ServiceTermsFilePath = business.ServiceTermsFilePath;
            result.IsDlscanEnabled = business.IsDlscanEnabled;
            result.IsSeatmapEnabled = business.IsSeatmapEnabled;
            result.IsChargeEnabled = business.IsChargeEnabled;
            result.PrivacyPolicyFileFullPath = _blobService.GetFileUrl(result.PrivacyPolicyFilePath) ?? "";
            result.ServiceTermsFileFullPath = _blobService.GetFileUrl(result.ServiceTermsFilePath) ?? "";
            result.CummunityGuidelinesFileFullPath = _blobService.GetFileUrl(result.CummunityGuidelinesFilePath) ?? "";
            result.Description = business.Description;
            return result;
        }

        private List<TimeSlot> GetAvilableTimeSlots(GetReservationBusinessByIdRequest request)
        {
            var timeslots = new List<TimeSlot>();
            using (var context = new QTimesContext())
            {
                var param = new SqlParameter[] {
                        new SqlParameter() {
                            ParameterName = "@BusinessId",
                            SqlDbType =  System.Data.SqlDbType.UniqueIdentifier,
                            Direction = System.Data.ParameterDirection.Input,
                            Value = request.Id
                        },
                        new SqlParameter() {
                            ParameterName = "@Date",
                            SqlDbType =  System.Data.SqlDbType.DateTime,
                            Direction = System.Data.ParameterDirection.Input,
                            Value = request.Date
                        }};
                var result = context.TimeSlots.FromSqlRaw($"[dbo].[GetTimeSlots] @BusinessId, @Date", param).ToList();
                timeslots.AddRange(result.Select(x => new TimeSlot
                {
                    OpenHourFrom = x.OpenHourFrom,
                    OpenHourTo = x.OpenHourTo,
                    SpotLeft = x.SpotLeft
                }));
            }

            return timeslots;
        }
    }
}