using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Models
{
    public class TransactionCategory : BaseModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}