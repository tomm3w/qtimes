using common.api.Queries;
using SeatQ.core.dal.Models;
using System.Collections.Generic;

namespace SeatQ.core.api.Queries.GetStaffTypes
{
    public class GetStaffTypesResponse: IQueryResponse
    {
        public GetStaffTypesResponse(List<StaffTypeItem> staffTypeItems)
        {
            StaffTypeItems = staffTypeItems;
        }
        public List<StaffTypeItem> StaffTypeItems { get; set; }
    }

    public class StaffTypeItem
    {
        public int StaffTypeId { get; set; }
        public string Title { get; set; }

    }
}