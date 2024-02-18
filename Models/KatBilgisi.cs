using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ETemizlik.Models
{
    [Table("KatBilgisi")]
    public partial class KatBilgisi
    {
        public KatBilgisi()
        {
            BosEvTemizligiSiparis = new HashSet<BosEvTemizligiSipari>();
            EvTemizligiSiparis = new HashSet<EvTemizligiSipari>();
            InsaatTemizligiSiparis = new HashSet<InsaatTemizligiSipari>();
        }

        [Key]
        [Column("katID")]
        public int KatId { get; set; }
        [Column("katBilgisi")]
        [StringLength(50)]
        [Unicode(false)]
        public string? KatBilgisi1 { get; set; }
        [Column("maliyet")]
        public int? Maliyet { get; set; }

        [InverseProperty("KacKatliNavigation")]
        public virtual ICollection<BosEvTemizligiSipari> BosEvTemizligiSiparis { get; set; }
        [InverseProperty("KacKatliNavigation")]
        public virtual ICollection<EvTemizligiSipari> EvTemizligiSiparis { get; set; }
        [InverseProperty("KacKatliNavigation")]
        public virtual ICollection<InsaatTemizligiSipari> InsaatTemizligiSiparis { get; set; }
    }
}
