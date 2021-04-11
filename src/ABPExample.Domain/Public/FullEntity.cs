using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;

namespace ABPExample.Domain.Public
{
    public class FullEntity:  ISoftDelete,IHasCreationTime,IHasModificationTime
    {
        [Key]
        public int Id { get; set; }
        
        public DateTime CreationTime { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public bool IsDeleted { get; set; }

    }
}
