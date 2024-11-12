using common.api.Commands;
using common.dal;
using SeatQ.core.dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeatQ.core.api.Commands.DeleteVisitMessage
{
    public class DeleteVisitMessageCommand : ICommand<DeleteVisitMessageRequest>
    {
        private readonly IGenericRepository<SeatQEntities, VisitMessage> _repository;
        public DeleteVisitMessageCommand(IGenericRepository<SeatQEntities, VisitMessage> repository)
        {
            _repository = repository;
        }

        public void Handle(DeleteVisitMessageRequest request)
        {
            var msg = _repository.FindBy(x => x.VisitMessageId == request.VisitMessageId).FirstOrDefault();
            if (msg != null)
            {
                msg.IsDeleted = true;
                msg.ModifiedDate = DateTime.UtcNow;
                _repository.Edit(msg);
                _repository.Save();
            }
        }
    }
}