using AutoMapper;
using common.api.Queries;
using common.dal;
using SeatQ.core.common.Dto;
using SeatQ.core.dal.Models;
using System.Collections.Generic;
using System.Linq;

namespace SeatQ.core.api.Queries.GetUsers
{
    public class GetUsersQuery : IQuery<GetUsersResponse, GetUsersRequest>
    {
        private readonly IGenericRepository<SeatQEntities, UserProfile> _userRepository;

        public GetUsersQuery(IGenericRepository<SeatQEntities, UserProfile> userRepository)
        {
            _userRepository = userRepository;
        }

        public GetUsersResponse Handle(GetUsersRequest request)
        {
            var users = _userRepository.GetAll();
            var result = ConvertToDto(users).ToList();
            return new GetUsersResponse(result);
        }

        private IEnumerable<UserDto> ConvertToDto(IEnumerable<UserProfile> users)
        {
            return (from user in users select Mapper.Map<UserProfile, UserDto>(user)).ToList();
        }
    }
}