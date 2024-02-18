using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ETemizlik.Models
{
    [Table("Sehir")]
    public partial class Sehir
    {
        public Sehir()
        {
            BosEvTemizligiSiparis = new HashSet<BosEvTemizligiSipari>();
            EsyaTemizligiSiparis = new HashSet<EsyaTemizligiSipari>();
            EvTemizligiSiparis = new HashSet<EvTemizligiSipari>();
            Ilces = new HashSet<Ilce>();
            InsaatTemizligiSiparis = new HashSet<InsaatTemizligiSipari>();
        }

        [Key]
        [Column("sehirID")]
        public int SehirId { get; set; }
        [Column("sehirAD")]
        [StringLength(50)]
        [Unicode(false)]
        public string? SehirAd { get; set; }

        [InverseProperty("Sehir")]
        public virtual ICollection<BosEvTemizligiSipari> BosEvTemizligiSiparis { get; set; }
        [InverseProperty("Sehir")]
        public virtual ICollection<EsyaTemizligiSipari> EsyaTemizligiSiparis { get; set; }
        [InverseProperty("Sehir")]
        public virtual ICollection<EvTemizligiSipari> EvTemizligiSiparis { get; set; }
        [InverseProperty("Sehir")]
        public virtual ICollection<Ilce> Ilces { get; set; }
        [InverseProperty("Sehir")]
        public virtual ICollection<InsaatTemizligiSipari> InsaatTemizligiSiparis { get; set; }
    }
}
