using Shared.Helpers.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Shared.Models.ViewModels
{
    public class PostTransaction
    {
        [Required] public string Description { get; set; }

        [Required] public decimal Amount { get; set; }
        [Required] public DateTimeOffset PostDate { get; set; }
        [Required] public Guid TransactionCategoryId { get; set; }
        [Required] public TransactionTypes TransactionType { get; set; }
    }
}
