using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TransactionAPI.Data.Repository.v1;

namespace TransactionAPI.Service.v1.Command
{
    public class DeleteTransactionsHandler : IRequestHandler<DeleteTransactionsCommand>
    {
        private readonly ITransactionRepository _transactionRepository;

        public DeleteTransactionsHandler(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<Unit> Handle(DeleteTransactionsCommand request, CancellationToken cancellationToken)
        {
            var getTransactions = _transactionRepository.GetAll();
            var deleteTransactions = getTransactions.ToList().Where(c => request.Guids.Contains(c.Id));
            deleteTransactions.ToList().ForEach(c => c.IsDeleted = true);
            await _transactionRepository.UpdateRangeAsync(deleteTransactions.ToList());

            return Unit.Value;
        }
    }
}