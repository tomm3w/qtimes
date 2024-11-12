using AutoMapper;
using common.api.Queries;
using common.dal;
using SeatQ.core.common.Dto;
using SeatQ.core.dal.Models;
using System.Linq;

namespace SeatQ.core.api.Queries.GetUserByName
{
    public class GetUserByNameQuery : IQuery<GetUserByNameResponse, GetUserByNameRequest>
    {
        private readonly IGenericRepository<SeatQEntities, UserProfile> _usersRepository;

        public GetUserByNameQuery(IGenericRepository<SeatQEntities, UserProfile> usersRepository)
        {
            _usersRepository = usersRepository;
        }
        public GetUserByNameResponse Handle(GetUserByNameRequest request)
        {
            var user = _usersRepository.FindBy(x => x.UserName == request.Username).FirstOrDefault();
            if (user != null)
            {
                return new GetUserByNameResponse(Mapper.Map<UserProfile, UserDto>(user));
            }
            return null;
        }
    }
}