using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ETemizlik.Models
{
    [Table("BeyazEsyaBilgisi")]
    public partial class BeyazEsyaBilgisi
    {
        public BeyazEsyaBilgisi()
        {
            EsyaTemizligiSiparis = new HashSet<EsyaTemizligiSipari>();
        }

        [Key]
        [Column("BeyazEsyaID")]
        public int BeyazEsyaId { get; set; }
        public int? BeyazEsyaSayisi { get; set; }
        [Column("maliyet")]
        public int? Maliyet { get; set; }

        [InverseProperty("BeyazEsyaSayisiNavigation")]
        public virtual ICollection<EsyaTemizligiSipari> EsyaTemizligiSiparis { get; set; }
    }
}
