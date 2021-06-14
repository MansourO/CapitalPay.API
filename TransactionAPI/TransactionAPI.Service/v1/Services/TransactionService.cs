using AutoMapper;
using MediatR;
using Shared.Models;
using Shared.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransactionAPI.Service.v1.Command;
using TransactionAPI.Service.v1.Query;

namespace TransactionAPI.Service.v1.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public TransactionService(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<Transaction> CreateTransaction(PostTransaction postTransaction)
        {
            try
            {
                return await _mediator.Send(new CreateTransactionCommand
                {
                    Transaction = _mapper.Map<Transaction>(postTransaction)
                });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Unit> DeleteTransaction(IEnumerable<Guid> guids)
        {
            try
            {
                return await _mediator.Send(new DeleteTransactionsCommand { 
                    Guids = guids 
                });
             
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Transaction> GetTransactionById(Guid guid)
        {
            try
            {
                return await _mediator.Send(new GetTransactionByIdQuery
                {
                    Id = guid
                });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Shared.Models.Transaction>> GetTransactions()
        {
            try
            {
                var result = await _mediator.Send(new GetTransactionsQuery());
                if (result.Any())
                {
                    return result;
                }
                else
                {
                    return new List<Shared.Models.Transaction>();
                }
            }
            catch (Exception)
            {
                return new List<Shared.Models.Transaction>();
            }
        }

        public async Task<Unit> UpdateTransactions(IEnumerable<Transaction> transactions)
        {
            try
            {
                return await _mediator.Send(new UpdateTransactionCommand { 
                    Transactions = transactions 
                });
   
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}