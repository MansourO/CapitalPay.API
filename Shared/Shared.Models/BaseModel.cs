using System;
using System.ComponentModel.DataAnnotations;

namespace Shared.Models
{
    public class BaseModel
    {
        public BaseModel()
        {
            IsDeleted = false;
        }

        [Key]
        public Guid Id { get; set; }

        //all entities must have these for safe delete
        public bool? IsDeleted { get; set; }

        public DateTimeOffset? Deleted { get; set; }
        public Guid? DeletedBy { get; set; }

        public DateTimeOffset? Modified { get; set; }
        public Guid? ModifiedBy { get; set; }

        public DateTimeOffset Created { get; set; }
        public Guid? CreatedBy { get; set; }

        public string RecordSource { get; set; }
    }
}