using GraphQL_HotChoclate_EFCore.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using static GraphQL_HotChoclate_EFCore.Enums.StatusEnum;

namespace GraphQL_HotChoclate_EFCore.Models
{
    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 Id { get; set; }

        [MaxLength(128)]
        [EmailAddress]
        public String Email { get; set; }
        
        [MaxLength(128)]
        public String Name { get; set; }

        public int? Code { get; set; }

        [EnumDataType(typeof(StatusEnum))]
        public Status Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool IsBlocked { get; set; }

    }
}
