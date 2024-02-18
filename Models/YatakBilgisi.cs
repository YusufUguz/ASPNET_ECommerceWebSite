using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ETemizlik.Models
{
    [Table("YatakBilgisi")]
    public partial class YatakBilgisi
    {
        public YatakBilgisi()
        {
            EsyaTemizligiSiparis = new HashSet<EsyaTemizligiSipari>();
        }

        [Key]
        [Column("yatakID")]
        public int YatakId { get; set; }
        [Column("yatakSayisi")]
        public int? YatakSayisi { get; set; }
        [Column("maliyet")]
        public int? Maliyet { get; set; }

        [InverseProperty("YatakSayisiNavigation")]
        public virtual ICollection<EsyaTemizligiSipari> EsyaTemizligiSiparis { get; set; }
    }
}
