using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ETemizlik.Models
{
    [Table("RandevuSaat")]
    public partial class RandevuSaat
    {
        public RandevuSaat()
        {
            BosEvTemizligiSiparis = new HashSet<BosEvTemizligiSipari>();
            EsyaTemizligiSiparis = new HashSet<EsyaTemizligiSipari>();
            EvTemizligiSiparis = new HashSet<EvTemizligiSipari>();
            InsaatTemizligiSiparis = new HashSet<InsaatTemizligiSipari>();
        }

        [Key]
        [Column("randevuSaatID")]
        public int RandevuSaatId { get; set; }
        [Column("randevuSaat")]
        [StringLength(50)]
        [Unicode(false)]
        public string? RandevuSaat1 { get; set; }

        [InverseProperty("Saat")]
        public virtual ICollection<BosEvTemizligiSipari> BosEvTemizligiSiparis { get; set; }
        [InverseProperty("Saat")]
        public virtual ICollection<EsyaTemizligiSipari> EsyaTemizligiSiparis { get; set; }
        [InverseProperty("Saat")]
        public virtual ICollection<EvTemizligiSipari> EvTemizligiSiparis { get; set; }
        [InverseProperty("Saat")]
        public virtual ICollection<InsaatTemizligiSipari> InsaatTemizligiSiparis { get; set; }
    }
}
