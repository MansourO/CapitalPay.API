using System;
using System.Collections.Generic;
using System.Text;

namespace TransactionAPI.Service.v1.Models
{
    public class UpdataTransactionModel
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
    }
}