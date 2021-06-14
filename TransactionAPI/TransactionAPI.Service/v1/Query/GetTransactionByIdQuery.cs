using MediatR;
using Shared.Models;
using System;

namespace TransactionAPI.Service.v1.Query
{
    public class GetTransactionByIdQuery : IRequest<Transaction>
    {
        public Guid Id { get; set; }
    }
}