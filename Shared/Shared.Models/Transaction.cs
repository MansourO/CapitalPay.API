using Shared.Helpers.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Shared.Models
{
    public class Transaction : BaseModel
    {
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public DateTimeOffset? PostDate { get; set; }
        public TransactionTypes TransactionType { get; set; }

        public Guid? FK_TransactionCategoryId { get; set; }

        [ForeignKey(nameof(FK_TransactionCategoryId))]
        public TransactionCategory TransactionCategory { get; set; }
    }
}