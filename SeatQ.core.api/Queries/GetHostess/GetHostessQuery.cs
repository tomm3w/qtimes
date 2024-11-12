using AutoMapper;
using common.api.Queries;
using common.dal;
using SeatQ.core.common.Dto;
using SeatQ.core.dal.Models;
using System.Collections.Generic;
using System.Linq;

namespace SeatQ.core.api.Queries.GetHostess
{
    public class GetUsersQuery : IQuery<GetHostessResponse, GetHostessRequest>
    {
        private readonly IGenericRepository<SeatQEntities, UserProfile> _hostessRepository;

        public GetUsersQuery(IGenericRepository<SeatQEntities, UserProfile> hostessRepository)
        {
            _hostessRepository = hostessRepository;
        }

        public GetHostessResponse Handle(GetHostessRequest request)
        {
            var hostess = _hostessRepository.FindBy(x => x.RestaurantChainId == request.RestaurantChainId && x.RoleName == "User");
            //var result = ConvertToDto(hostess).ToList();
            return new GetHostessResponse(hostess.ToList());

        }

        private IEnumerable<UserDto> ConvertToDto(IEnumerable<UserProfile> users)
        {
            return (from user in users select Mapper.Map<UserProfile, UserDto>(user)).ToList();
        }
    }
}