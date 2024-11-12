using common.api.Queries;
using common.dal;
using SeatQ.core.dal.Enums;
using SeatQ.core.dal.Models;
using System.Collections.Generic;
using System.Linq;

namespace SeatQ.core.api.Queries.GetStaffTypes
{
    public class GetStaffTypesQuery : IQuery<GetStaffTypesResponse, GetStaffTypesRequest>
    {
        private readonly IGenericRepository<SeatQEntities, StaffType> _staffTypeRepository;
        public GetStaffTypesQuery(IGenericRepository<SeatQEntities, StaffType> staffTypeRepository)
        {
            _staffTypeRepository = staffTypeRepository;
        }
        public GetStaffTypesResponse Handle(GetStaffTypesRequest request)
        {
            var staffTypes = _staffTypeRepository.FindBy(x => x.StaffTypeId != (short)StaffTypeEnum.Admin).ToList();
            var types = new List<StaffTypeItem>();
            staffTypes.ForEach(x =>
            {
                types.Add(new StaffTypeItem { StaffTypeId = x.StaffTypeId, Title = x.Title });
            });
            return new GetStaffTypesResponse(types);
        }
    }
}