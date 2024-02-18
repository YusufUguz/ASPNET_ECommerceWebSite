using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ETemizlik.Models
{
    [Table("HaliBilgisi")]
    public partial class HaliBilgisi
    {
        public HaliBilgisi()
        {
            EsyaTemizligiSiparis = new HashSet<EsyaTemizligiSipari>();
        }

        [Key]
        [Column("haliID")]
        public int HaliId { get; set; }
        [Column("haliSayisi")]
        public int? HaliSayisi { get; set; }
        [Column("maliyet")]
        public int? Maliyet { get; set; }

        [InverseProperty("HaliSayisiNavigation")]
        public virtual ICollection<EsyaTemizligiSipari> EsyaTemizligiSiparis { get; set; }
    }
}
