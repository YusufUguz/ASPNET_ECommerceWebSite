using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ETemizlik.Models
{
    [Table("Ilce")]
    public partial class Ilce
    {
        public Ilce()
        {
            BosEvTemizligiSiparis = new HashSet<BosEvTemizligiSipari>();
            EsyaTemizligiSiparis = new HashSet<EsyaTemizligiSipari>();
            EvTemizligiSiparis = new HashSet<EvTemizligiSipari>();
            InsaatTemizligiSiparis = new HashSet<InsaatTemizligiSipari>();
        }

        [Key]
        [Column("ilceID")]
        public int IlceId { get; set; }
        [Column("ilceAD")]
        [StringLength(50)]
        [Unicode(false)]
        public string? IlceAd { get; set; }
        [Column("sehirID")]
        public int? SehirId { get; set; }

        [ForeignKey("SehirId")]
        [InverseProperty("Ilces")]
        public virtual Sehir? Sehir { get; set; }
        [InverseProperty("Ilce")]
        public virtual ICollection<BosEvTemizligiSipari> BosEvTemizligiSiparis { get; set; }
        [InverseProperty("Ilce")]
        public virtual ICollection<EsyaTemizligiSipari> EsyaTemizligiSiparis { get; set; }
        [InverseProperty("Ilce")]
        public virtual ICollection<EvTemizligiSipari> EvTemizligiSiparis { get; set; }
        [InverseProperty("Ilce")]
        public virtual ICollection<InsaatTemizligiSipari> InsaatTemizligiSiparis { get; set; }
    }
}
