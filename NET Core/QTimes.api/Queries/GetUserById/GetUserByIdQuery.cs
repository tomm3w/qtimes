//using iVeew.common.api.Queries;
//using IVeew.common.dal;
//using QTimes.core.dal.Models;
//using System.Linq;

//namespace QTimes.api.Queries.GetUserById
//{
//    public class GetUserByIdQuery : IQuery<GetUserByIdResponse, GetUserByIdRequest>
//    {
//        private readonly IGenericRepository<QTimesContext, UserProfile> _usersRepository;

//        public GetUserByIdQuery(IGenericRepository<QTimesContext, UserProfile> usersRepository)
//        {
//            _usersRepository = usersRepository;
//        }
//        public GetUserByIdResponse Handle(GetUserByIdRequest request)
//        {
//            var user = _usersRepository.FindBy(x => x.UserId == request.UserId).FirstOrDefault();
//            if (user != null)
//            {
//                return new GetUserByIdResponse(user);
//            }
//            return null;
//        }
//    }
//}