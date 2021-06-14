using MediatR;
using Shared.Models;
using Shared.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TransactionAPI.Service.v1.Models;

namespace TransactionAPI.Service.v1.Services
{
    public interface ITransactionService
    {
        Task<IEnumerable<Transaction>> GetTransactions();
        Task<Transaction> CreateTransaction(PostTransaction postTransaction);
        Task<Transaction> GetTransactionById(Guid guid);
        Task<Unit> DeleteTransaction(IEnumerable<Guid> guids);
        Task<Unit> UpdateTransactions(IEnumerable<Transaction> transactions);
    }
}