using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ABPExample.Domain.Public
{
    public class FullEntity
    {
        [Key]
        public int Id { get; set; }
        
        public DateTime CreationTime { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public bool IsDeleted { get; set; }

    }
}
