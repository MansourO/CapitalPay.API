using MediatR;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TransactionAPI.Data.Repository.v1;

namespace TransactionAPI.Service.v1.Command
{
    public class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, Transaction>
    {
        private readonly ITransactionRepository _transactionRepository;

        public CreateTransactionCommandHandler(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<Transaction> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
        {
            request.Transaction.Id = Guid.NewGuid();
            request.Transaction.Created = DateTimeOffset.UtcNow;
            return await _transactionRepository.AddAsync(request.Transaction);
        }
    }
}