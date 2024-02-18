using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ETemizlik.Models
{
    [Table("TeknolojikAletBilgisi")]
    public partial class TeknolojikAletBilgisi
    {
        public TeknolojikAletBilgisi()
        {
            EsyaTemizligiSiparis = new HashSet<EsyaTemizligiSipari>();
        }

        [Key]
        [Column("TeknolojikAletID")]
        public int TeknolojikAletId { get; set; }
        [Column("TAletSayisi")]
        public int? TaletSayisi { get; set; }
        [Column("maliyet")]
        public int? Maliyet { get; set; }

        [InverseProperty("TeknolojikAletSayisiNavigation")]
        public virtual ICollection<EsyaTemizligiSipari> EsyaTemizligiSiparis { get; set; }
    }
}
