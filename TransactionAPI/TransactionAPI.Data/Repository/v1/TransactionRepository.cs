using Microsoft.EntityFrameworkCore;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TransactionAPI.Data.Database;

namespace TransactionAPI.Data.Repository.v1
{
    public class TransactionRepository : Repository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(CapitalTransactionContext capitalTransactionContext) : base(capitalTransactionContext)
        {
        }

        public async Task<Transaction> GetTransactionByIdAsync(Guid transactionId, CancellationToken cancellationToken)
        {
            return await CapitalTransactionContext.Transactions.Include(c => c.TransactionCategory).FirstOrDefaultAsync(c => c.Id == transactionId, cancellationToken);
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsAsync(CancellationToken cancellationToken)
        {
            return await CapitalTransactionContext.Transactions.Include(c => c.TransactionCategory).ToListAsync(cancellationToken);
        }
    }
}