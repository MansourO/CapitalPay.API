using MediatR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace TransactionAPI.Service.v1.Command
{
    public class DeleteTransactionsCommand : IRequest
    {
        public IEnumerable<Guid> Guids { get; set; }
    }
}