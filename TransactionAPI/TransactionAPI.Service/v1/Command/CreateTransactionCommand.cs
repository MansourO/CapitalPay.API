using MediatR;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace TransactionAPI.Service.v1.Command
{
    public class CreateTransactionCommand : IRequest<Transaction>
    {
        public Transaction Transaction { get; set; }
    }
}