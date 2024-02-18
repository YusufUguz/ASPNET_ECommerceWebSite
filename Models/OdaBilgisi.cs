using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ETemizlik.Models
{
    [Table("OdaBilgisi")]
    public partial class OdaBilgisi
    {
        public OdaBilgisi()
        {
            BosEvTemizligiSiparis = new HashSet<BosEvTemizligiSipari>();
            EvTemizligiSiparis = new HashSet<EvTemizligiSipari>();
            InsaatTemizligiSiparis = new HashSet<InsaatTemizligiSipari>();
        }

        [Key]
        [Column("odaID")]
        public int OdaId { get; set; }
        [Column("odaBilgisi")]
        [StringLength(50)]
        [Unicode(false)]
        public string? OdaBilgisi1 { get; set; }
        [Column("maliyet")]
        public int? Maliyet { get; set; }

        [InverseProperty("KacOdaliNavigation")]
        public virtual ICollection<BosEvTemizligiSipari> BosEvTemizligiSiparis { get; set; }
        [InverseProperty("KacOdaliNavigation")]
        public virtual ICollection<EvTemizligiSipari> EvTemizligiSiparis { get; set; }
        [InverseProperty("KacOdaliNavigation")]
        public virtual ICollection<InsaatTemizligiSipari> InsaatTemizligiSiparis { get; set; }
    }
}
