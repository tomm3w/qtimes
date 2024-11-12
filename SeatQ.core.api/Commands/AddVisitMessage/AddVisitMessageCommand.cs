using common.api.Commands;
using common.dal;
using SeatQ.core.api.Commands.AddHostess;
using SeatQ.core.dal.Models;
using System;

namespace SeatQ.core.api.Commands.AddVisitMessage
{
    public class AddVisitMessageCommand : ICommand<AddVisitMessageRequest>
    {
        private readonly IGenericRepository<SeatQEntities, VisitMessage> _repository;
        public AddVisitMessageCommand(IGenericRepository<SeatQEntities, VisitMessage> repository)
        {
            _repository = repository;
        }

        public void Handle(AddVisitMessageRequest request)
        {
            var data = new VisitMessage
            {
                RestaurantChainId=request.Model.RestaurantChainId,
                Visit = request.Model.Visit,
                VisitMessage1 = request.Model.VisitMessage1,
                IsEnabled = request.Model.IsEnabled,
                IsDeleted = false,
                ModifiedDate = DateTime.UtcNow
            };
            _repository.Add(data);
            _repository.Save();
        }
    }
}