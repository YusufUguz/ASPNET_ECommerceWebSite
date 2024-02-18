using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ETemizlik.Models
{
    public partial class EsyaTemizligiSipari
    {
        [Key]
        [Column("EsyaTemizligiID")]
        public int EsyaTemizligiId { get; set; }
        [Column("koltukSayisi")]
        public int? KoltukSayisi { get; set; }
        [Column("teknolojikAletSayisi")]
        public int? TeknolojikAletSayisi { get; set; }
        [Column("yatakSayisi")]
        public int? YatakSayisi { get; set; }
        [Column("beyazEsyaSayisi")]
        public int? BeyazEsyaSayisi { get; set; }
        [Column("haliSayisi")]
        public int? HaliSayisi { get; set; }
        [Column("sehirID")]
        public int? SehirId { get; set; }
        [Column("ilceID")]
        public int? IlceId { get; set; }
        [Column("tarih", TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Tarih { get; set; }
        [Column("saatID")]
        public int? SaatId { get; set; }
        [Column("userID")]
        [StringLength(450)]
        public string? UserId { get; set; }
        [Column("address")]
        public string? Address { get; set; }
        [Column("phoneNumber")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Telefon numarası 10 haneli olmalıdır.")]
        [StringLength(10, ErrorMessage = "Telefon numarası 10 haneli olmalıdır.", MinimumLength = 10)]
        public string? PhoneNumber { get; set; }
        [Column("cartAmount")]
        public int? CartAmount { get; set; }

        [ForeignKey("BeyazEsyaSayisi")]
        [InverseProperty("EsyaTemizligiSiparis")]
        public virtual BeyazEsyaBilgisi? BeyazEsyaSayisiNavigation { get; set; }
        [ForeignKey("HaliSayisi")]
        [InverseProperty("EsyaTemizligiSiparis")]
        public virtual HaliBilgisi? HaliSayisiNavigation { get; set; }
        [ForeignKey("IlceId")]
        [InverseProperty("EsyaTemizligiSiparis")]
        public virtual Ilce? Ilce { get; set; }
        [ForeignKey("KoltukSayisi")]
        [InverseProperty("EsyaTemizligiSiparis")]
        public virtual KoltukBilgisi? KoltukSayisiNavigation { get; set; }
        [ForeignKey("SaatId")]
        [InverseProperty("EsyaTemizligiSiparis")]
        public virtual RandevuSaat? Saat { get; set; }
        [ForeignKey("SehirId")]
        [InverseProperty("EsyaTemizligiSiparis")]
        public virtual Sehir? Sehir { get; set; }
        [ForeignKey("TeknolojikAletSayisi")]
        [InverseProperty("EsyaTemizligiSiparis")]
        public virtual TeknolojikAletBilgisi? TeknolojikAletSayisiNavigation { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("EsyaTemizligiSiparis")]
        public virtual AspNetUser? User { get; set; }
        [ForeignKey("YatakSayisi")]
        [InverseProperty("EsyaTemizligiSiparis")]
        public virtual YatakBilgisi? YatakSayisiNavigation { get; set; }
    }
}
