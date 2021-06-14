using Shared.Interfaces;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TransactionAPI.Data.Repository.v1
{
    public interface ITransactionRepository : IRepository<Transaction>
    {
        Task<IEnumerable<Transaction>> GetTransactionsAsync(CancellationToken cancellationToken);

        Task<Transaction> GetTransactionByIdAsync(Guid transactionId, CancellationToken cancellationToken);
    }
}