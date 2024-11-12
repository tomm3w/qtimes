using AutoMapper;
using common.api.Queries;
using common.dal;
using SeatQ.core.common.Dto;
using SeatQ.core.dal.Models;
using System.Linq;

namespace SeatQ.core.api.Queries.GetUserById
{
    public class GetUserByIdQuery : IQuery<GetUserByIdResponse, GetUserByIdRequest>
    {
        private readonly IGenericRepository<SeatQEntities, UserProfile> _usersRepository;

        public GetUserByIdQuery(IGenericRepository<SeatQEntities, UserProfile> usersRepository)
        {
            _usersRepository = usersRepository;
        }
        public GetUserByIdResponse Handle(GetUserByIdRequest request)
        {
            var user = _usersRepository.FindBy(x => x.UserId == request.UserId).FirstOrDefault();
            if (user != null)
            {
                return new GetUserByIdResponse(user);
            }
            return null;
        }
    }
}