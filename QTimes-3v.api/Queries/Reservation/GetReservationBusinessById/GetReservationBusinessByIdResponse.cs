using iVeew.common.api.Queries;
using QTimes.api.Infrastructure;
using QTimes.core.dal.Models;
using System.Collections.Generic;

namespace QTimes.api.Queries.Reservation.GetReservationBusinessById
{
    public class GetReservationBusinessByIdResponse : IQueryResponse
    {
        private readonly IBlobService _blobService;
        public GetReservationBusinessByIdResponse(IBlobService blobService)
        {
            TimeSlots = new List<TimeSlot>();
            _blobService = blobService;
        }

        public int? LimitSizePerHour { get; set; }
        public int? LimitSizePerGroup { get; set; }
        public string BusinessName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string LogoPath { get; set; }
        public string LogoFullPath
        {
            get
            {
                return _blobService.GetFileUrl(LogoPath);
            }
        }
        public string FullAddress
        {
            get
            {
                return $"{Address}, {City}, {State} {Zip}";
            }
        }
        public int LimitSize
        {
            get
            {
                var limitSize = 0;
                if ((LimitSizePerGroup ?? 0) > (LimitSizePerHour ?? 0))
                    limitSize = LimitSizePerHour ?? 0;
                else
                    limitSize = LimitSizePerGroup ?? 0;

                return limitSize;
            }
        }
        public List<TimeSlot> TimeSlots { get; set; }
        public bool? EnableCommunityGuidelines { get; set; }
        public bool? EnablePrivacyPolicy { get; set; }
        public bool? EnableServiceTerms { get; set; }
        public string CummunityGuidelinesFilePath { get; set; }
        public string PrivacyPolicyFilePath { get; set; }
        public string ServiceTermsFilePath { get; set; }
        public string CummunityGuidelinesFileFullPath { get; set; }
        public string ServiceTermsFileFullPath { get; set; }
        public string PrivacyPolicyFileFullPath { get; set; }
        public string Description { get; set; }
        public bool? IsDlscanEnabled { get; set; }
        public bool? IsSeatmapEnabled { get; set; }
        public bool? IsChargeEnabled { get; set; }
    }
}