using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ETemizlik.Models
{
    [Table("KoltukBilgisi")]
    public partial class KoltukBilgisi
    {
        public KoltukBilgisi()
        {
            EsyaTemizligiSiparis = new HashSet<EsyaTemizligiSipari>();
        }

        [Key]
        [Column("koltukBilgisiID")]
        public int KoltukBilgisiId { get; set; }
        [Column("koltukSayisi")]
        public int? KoltukSayisi { get; set; }
        [Column("maliyet")]
        public int? Maliyet { get; set; }

        [InverseProperty("KoltukSayisiNavigation")]
        public virtual ICollection<EsyaTemizligiSipari> EsyaTemizligiSiparis { get; set; }
    }
}
