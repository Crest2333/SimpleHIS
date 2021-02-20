using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ABPExample.Domain.Models
{
    public class DepartmentMapper
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        public long DepartmentId { get; set; }

        public DateTime CreationTime { get; set; }

        public bool IsDeleted { get; set; }
    }
}
