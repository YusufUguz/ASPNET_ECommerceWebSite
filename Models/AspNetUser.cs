using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using ETemizlik;

namespace ETemizlik.Models
{
    [Index("NormalizedEmail", Name = "EmailIndex")]
    public partial class AspNetUser
    {
        public AspNetUser()
        {
            AspNetUserClaims = new HashSet<AspNetUserClaim>();
            AspNetUserLogins = new HashSet<AspNetUserLogin>();
            AspNetUserTokens = new HashSet<AspNetUserToken>();
            BosEvTemizligiSiparis = new HashSet<BosEvTemizligiSipari>();
            EsyaTemizligiSiparis = new HashSet<EsyaTemizligiSipari>();
            EvTemizligiSiparis = new HashSet<EvTemizligiSipari>();
            InsaatTemizligiSiparis = new HashSet<InsaatTemizligiSipari>();
            Roles = new HashSet<AspNetRole>();
        }

        [Key]
        public string Id { get; set; } = null!;
        [StringLength(255)]
        public string FirstName { get; set; } = null!;
        [StringLength(255)]
        public string LastName { get; set; } = null!;
        [StringLength(256)]
        public string? UserName { get; set; }
        [StringLength(256)]
        public string? NormalizedUserName { get; set; }
        [StringLength(256)]
        public string? Email { get; set; }
        [StringLength(256)]
        public string? NormalizedEmail { get; set; }
        public bool EmailConfirmed { get; set; }
        public string? PasswordHash { get; set; }
        public string? SecurityStamp { get; set; }
        public string? ConcurrencyStamp { get; set; }
        public string? PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<AspNetUserClaim> AspNetUserClaims { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<AspNetUserLogin> AspNetUserLogins { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<AspNetUserToken> AspNetUserTokens { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<BosEvTemizligiSipari> BosEvTemizligiSiparis { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<EsyaTemizligiSipari> EsyaTemizligiSiparis { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<EvTemizligiSipari> EvTemizligiSiparis { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<InsaatTemizligiSipari> InsaatTemizligiSiparis { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("Users")]
        public virtual ICollection<AspNetRole> Roles { get; set; }
    }
}
