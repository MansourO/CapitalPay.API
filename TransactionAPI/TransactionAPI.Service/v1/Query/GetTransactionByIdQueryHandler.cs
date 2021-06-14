using MediatR;
using Shared.Models;
using System.Threading;
using System.Threading.Tasks;
using TransactionAPI.Data.Repository.v1;

namespace TransactionAPI.Service.v1.Query
{
    public class GetTransactionByIdQueryHandler : IRequestHandler<GetTransactionByIdQuery, Transaction>
    {
        private readonly ITransactionRepository _transactionRepository;

        public GetTransactionByIdQueryHandler(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<Transaction> Handle(GetTransactionByIdQuery request, CancellationToken cancellationToken)
        {
            return await _transactionRepository.GetTransactionByIdAsync(request.Id, cancellationToken);
        }
    }
}