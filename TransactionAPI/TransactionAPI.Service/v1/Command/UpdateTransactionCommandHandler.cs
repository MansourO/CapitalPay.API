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
    public class UpdateTransactionCommandHandler : IRequestHandler<UpdateTransactionCommand>
    {
        private readonly ITransactionRepository _transactionRepository;

        public UpdateTransactionCommandHandler(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<Unit> Handle(UpdateTransactionCommand request, CancellationToken cancellationToken)
        {
            request.Transactions.ToList().ForEach(c =>
            {
                c.Modified = DateTimeOffset.UtcNow;
            });
            await _transactionRepository.UpdateRangeAsync(request.Transactions.ToList());

            return Unit.Value;
        }
    }
}