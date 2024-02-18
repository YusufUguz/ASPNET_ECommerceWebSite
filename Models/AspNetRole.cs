using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ETemizlik.Models
{
    public partial class AspNetRole
    {
        public AspNetRole()
        {
            AspNetRoleClaims = new HashSet<AspNetRoleClaim>();
            Users = new HashSet<AspNetUser>();
        }

        [Key]
        public string Id { get; set; } = null!;
        [StringLength(256)]
        public string? Name { get; set; }
        [StringLength(256)]
        public string? NormalizedName { get; set; }
        public string? ConcurrencyStamp { get; set; }

        [InverseProperty("Role")]
        public virtual ICollection<AspNetRoleClaim> AspNetRoleClaims { get; set; }

        [ForeignKey("RoleId")]
        [InverseProperty("Roles")]
        public virtual ICollection<AspNetUser> Users { get; set; }
    }
}
