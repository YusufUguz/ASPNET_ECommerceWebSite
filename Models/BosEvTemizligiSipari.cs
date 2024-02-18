﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ETemizlik.Models
{
    public partial class BosEvTemizligiSipari
    {
        [Key]
        [Column("bosevTemizligiID")]
        public int BosevTemizligiId { get; set; }
        [Column("sehirID")]
        public int? SehirId { get; set; }
        [Column("ilceID")]
        public int? IlceId { get; set; }
        [Column("kacKatli")]
        public int? KacKatli { get; set; }
        [Column("kacOdali")]
        public int? KacOdali { get; set; }
        [Column("tarih", TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Tarih { get; set; }
        [Column("saatID")]
        public int? SaatId { get; set; }
        [Column("userID")]
        [StringLength(450)]
        public string? UserId { get; set; }
        [Column("address")]
        [Unicode(false)]
        public string? Address { get; set; }
        [Column("phoneNumber")]
        [Unicode(false)]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Telefon numarası 10 haneli olmalıdır.")]
        [StringLength(10, ErrorMessage = "Telefon numarası 10 haneli olmalıdır.", MinimumLength = 10)]
        public string? PhoneNumber { get; set; }
        [Column("cartAmount")]
        public int? CartAmount { get; set; }

        [ForeignKey("IlceId")]
        [InverseProperty("BosEvTemizligiSiparis")]
        public virtual Ilce? Ilce { get; set; }
        [ForeignKey("KacKatli")]
        [InverseProperty("BosEvTemizligiSiparis")]
        public virtual KatBilgisi? KacKatliNavigation { get; set; }
        [ForeignKey("KacOdali")]
        [InverseProperty("BosEvTemizligiSiparis")]
        public virtual OdaBilgisi? KacOdaliNavigation { get; set; }
        [ForeignKey("SaatId")]
        [InverseProperty("BosEvTemizligiSiparis")]
        public virtual RandevuSaat? Saat { get; set; }
        [ForeignKey("SehirId")]
        [InverseProperty("BosEvTemizligiSiparis")]
        public virtual Sehir? Sehir { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("BosEvTemizligiSiparis")]
        public virtual AspNetUser? User { get; set; }
    }
}
