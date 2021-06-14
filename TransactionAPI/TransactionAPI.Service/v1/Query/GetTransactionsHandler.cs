using MediatR;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TransactionAPI.Data.Repository.v1;

namespace TransactionAPI.Service.v1.Query
{
    public class GetTransactionsHandler : IRequestHandler<GetTransactionsQuery, IEnumerable<Transaction>>
    {
        private readonly ITransactionRepository _transactionRepository;

        public GetTransactionsHandler(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<IEnumerable<Transaction>> Handle(GetTransactionsQuery request, CancellationToken cancellationToken)
        {
            var result = await _transactionRepository.GetTransactionsAsync(cancellationToken);
            return result;
        }
    }
}