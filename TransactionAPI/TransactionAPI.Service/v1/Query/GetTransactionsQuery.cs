using MediatR;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace TransactionAPI.Service.v1.Query
{
    public class GetTransactionsQuery : IRequest<IEnumerable<Transaction>>
    {
    }
}