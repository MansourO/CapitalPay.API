using MediatR;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace TransactionAPI.Service.v1.Command
{
    public class UpdateTransactionCommand : IRequest
    {
        public IEnumerable<Transaction> Transactions { get; set; }
    }
}