using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ETemizlik.Models;
using ETemizlik.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;

namespace ETemizlik.Controllers
{
    public class EsyaTemizligiSiparisController : Controller
    {
        private readonly AuthUseContext _context;
        private readonly UserManager<AuthUseUser> _userManager;

        public EsyaTemizligiSiparisController(AuthUseContext context, UserManager<AuthUseUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        public IActionResult GetMaliyet(int koltukSayisi, int TASayisi,int yatakSayisi,int beyazEsyaSayisi,int haliSayisi)
        {

            decimal calculatedMaliyet = CalculateMaliyet(koltukSayisi,TASayisi,yatakSayisi,beyazEsyaSayisi,haliSayisi);
            return Json(new { maliyet = calculatedMaliyet });
        }

        private int CalculateMaliyet(int koltukSayisi, int TASayisi, int yatakSayisi, int beyazEsyaSayisi, int haliSayisi)
        {
            int baseCost = 1000;
            int costPerKoltuk = 100;
            int costPerTA = 100;
            int costPerYatak = 100;
            int costPerBeyazEsya = 100;
            int costPerHali = 50;

            int totalCost = baseCost + (koltukSayisi * costPerKoltuk) + (TASayisi * costPerTA) + (yatakSayisi * costPerYatak) + (beyazEsyaSayisi * costPerBeyazEsya) + (haliSayisi * costPerHali);

            return totalCost;
        }

        // GET: EsyaTemizligiSiparis
        public async Task<IActionResult> Index()
        {

            string currentUserId = _userManager.GetUserId(User);
            var authUseContext = _context.EsyaTemizligiSiparis.Where(item => item.UserId == currentUserId).Include(e => e.BeyazEsyaSayisiNavigation).Include(e => e.HaliSayisiNavigation).Include(e => e.Ilce).Include(e => e.KoltukSayisiNavigation).Include(e => e.Saat).Include(e => e.Sehir).Include(e => e.TeknolojikAletSayisiNavigation).Include(e => e.User).Include(e => e.YatakSayisiNavigation);
            return View(await authUseContext.ToListAsync());
        }

        // GET: EsyaTemizligiSiparis/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.EsyaTemizligiSiparis == null)
            {
                return NotFound();
            }

            var esyaTemizligiSipari = await _context.EsyaTemizligiSiparis
                .Include(e => e.BeyazEsyaSayisiNavigation)
                .Include(e => e.HaliSayisiNavigation)
                .Include(e => e.Ilce)
                .Include(e => e.KoltukSayisiNavigation)
                .Include(e => e.Saat)
                .Include(e => e.Sehir)
                .Include(e => e.TeknolojikAletSayisiNavigation)
                .Include(e => e.User)
                .Include(e => e.YatakSayisiNavigation)
                .FirstOrDefaultAsync(m => m.EsyaTemizligiId == id);
            if (esyaTemizligiSipari == null)
            {
                return NotFound();
            }

            return View(esyaTemizligiSipari);
        }

        // GET: EsyaTemizligiSiparis/Create
        public IActionResult Create()
        {
            List<Sehir> sehirlistesi = new List<Sehir>();
            sehirlistesi = (from Sehir in _context.Sehirs
                            select
                            Sehir).ToList();
            sehirlistesi.Insert(0, new Sehir { SehirId = 0, SehirAd = "Şehir Seçiniz" });
            ViewBag.ListofSehir = sehirlistesi;

            ViewData["BeyazEsyaSayisi"] = new SelectList(_context.BeyazEsyaBilgisis, "BeyazEsyaId", "BeyazEsyaSayisi");
            ViewData["HaliSayisi"] = new SelectList(_context.HaliBilgisis, "HaliId", "HaliSayisi");
            ViewData["IlceId"] = new SelectList(_context.Ilces, "IlceId", "IlceAd");
            ViewData["KoltukSayisi"] = new SelectList(_context.KoltukBilgisis, "KoltukBilgisiId", "KoltukSayisi");
            ViewData["SaatId"] = new SelectList(_context.RandevuSaats, "RandevuSaatId", "RandevuSaat1");
            ViewData["SehirId"] = new SelectList(_context.Sehirs, "SehirId", "SehirAd");
            ViewData["TeknolojikAletSayisi"] = new SelectList(_context.TeknolojikAletBilgisis, "TeknolojikAletId", "TaletSayisi");
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id");
            ViewData["YatakSayisi"] = new SelectList(_context.YatakBilgisis, "YatakId", "YatakSayisi");
            return View();
        }

        public JsonResult GetSehirilce(int sehirId)
        {
            //List<Sehirilce> sehirilcelist = _context.Sehirilces.Where(x => x.SehirId == sehirId).ToList();
            var sehirilcelist = (from sehirilce in _context.Ilces
                                 where sehirilce.SehirId == sehirId
                                 select new
                                 {
                                     Text = sehirilce.IlceAd,
                                     Value = sehirilce.IlceId
                                 }).ToList();

            return Json(sehirilcelist, new System.Text.Json.JsonSerializerOptions());
        }

        // POST: EsyaTemizligiSiparis/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EsyaTemizligiId,KoltukSayisi,TeknolojikAletSayisi,YatakSayisi,BeyazEsyaSayisi,HaliSayisi,SehirId,IlceId,Tarih,SaatId,UserId,Address,PhoneNumber,CartAmount")] EsyaTemizligiSipari esyaTemizligiSipari)
        {
            if (ModelState.IsValid)
            {
                int koltukSayisi = (int)esyaTemizligiSipari.KoltukSayisi;
                int TASayisi = (int)esyaTemizligiSipari.TeknolojikAletSayisi;
                int yatakSayisi = (int)esyaTemizligiSipari.YatakSayisi;
                int haliSayisi = (int)esyaTemizligiSipari.HaliSayisi;
                int beyazEsyaSayisi = (int)esyaTemizligiSipari.BeyazEsyaSayisi;
                esyaTemizligiSipari.CartAmount = CalculateMaliyet(koltukSayisi,TASayisi,yatakSayisi,beyazEsyaSayisi,haliSayisi);
                var userId = _userManager.GetUserId(User);

                // Set the UserId property in the model
                esyaTemizligiSipari.UserId = userId;
                _context.Add(esyaTemizligiSipari);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            List<Sehir> sehirlistesi = new List<Sehir>();
            sehirlistesi = (from Sehir in _context.Sehirs
                            select
                            Sehir).ToList();
            sehirlistesi.Insert(0, new Sehir { SehirId = 0, SehirAd = "Şehir Seçiniz" });
            ViewBag.ListofSehir = sehirlistesi;

            ViewData["BeyazEsyaSayisi"] = new SelectList(_context.BeyazEsyaBilgisis, "BeyazEsyaId", "BeyazEsyaId", esyaTemizligiSipari.BeyazEsyaSayisi);
            ViewData["HaliSayisi"] = new SelectList(_context.HaliBilgisis, "HaliId", "HaliId", esyaTemizligiSipari.HaliSayisi);
            ViewData["IlceId"] = new SelectList(_context.Ilces, "IlceId", "IlceId", esyaTemizligiSipari.IlceId);
            ViewData["KoltukSayisi"] = new SelectList(_context.KoltukBilgisis, "KoltukBilgisiId", "KoltukBilgisiId", esyaTemizligiSipari.KoltukSayisi);
            ViewData["SaatId"] = new SelectList(_context.RandevuSaats, "RandevuSaatId", "RandevuSaatId", esyaTemizligiSipari.SaatId);
            ViewData["SehirId"] = new SelectList(_context.Sehirs, "SehirId", "SehirId", esyaTemizligiSipari.SehirId);
            ViewData["TeknolojikAletSayisi"] = new SelectList(_context.TeknolojikAletBilgisis, "TeknolojikAletId", "TeknolojikAletId", esyaTemizligiSipari.TeknolojikAletSayisi);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", esyaTemizligiSipari.UserId);
            ViewData["YatakSayisi"] = new SelectList(_context.YatakBilgisis, "YatakId", "YatakId", esyaTemizligiSipari.YatakSayisi);
            return View(esyaTemizligiSipari);
        }

        // GET: EsyaTemizligiSiparis/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.EsyaTemizligiSiparis == null)
            {
                return NotFound();
            }

            var esyaTemizligiSipari = await _context.EsyaTemizligiSiparis.FindAsync(id);
            if (esyaTemizligiSipari == null)
            {
                return NotFound();
            }
            ViewData["BeyazEsyaSayisi"] = new SelectList(_context.BeyazEsyaBilgisis, "BeyazEsyaId", "BeyazEsyaId", esyaTemizligiSipari.BeyazEsyaSayisi);
            ViewData["HaliSayisi"] = new SelectList(_context.HaliBilgisis, "HaliId", "HaliId", esyaTemizligiSipari.HaliSayisi);
            ViewData["IlceId"] = new SelectList(_context.Ilces, "IlceId", "IlceId", esyaTemizligiSipari.IlceId);
            ViewData["KoltukSayisi"] = new SelectList(_context.KoltukBilgisis, "KoltukBilgisiId", "KoltukBilgisiId", esyaTemizligiSipari.KoltukSayisi);
            ViewData["SaatId"] = new SelectList(_context.RandevuSaats, "RandevuSaatId", "RandevuSaatId", esyaTemizligiSipari.SaatId);
            ViewData["SehirId"] = new SelectList(_context.Sehirs, "SehirId", "SehirId", esyaTemizligiSipari.SehirId);
            ViewData["TeknolojikAletSayisi"] = new SelectList(_context.TeknolojikAletBilgisis, "TeknolojikAletId", "TeknolojikAletId", esyaTemizligiSipari.TeknolojikAletSayisi);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", esyaTemizligiSipari.UserId);
            ViewData["YatakSayisi"] = new SelectList(_context.YatakBilgisis, "YatakId", "YatakId", esyaTemizligiSipari.YatakSayisi);
            return View(esyaTemizligiSipari);
        }

        // POST: EsyaTemizligiSiparis/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EsyaTemizligiId,KoltukSayisi,TeknolojikAletSayisi,YatakSayisi,BeyazEsyaSayisi,HaliSayisi,SehirId,IlceId,Tarih,SaatId,UserId,Address,PhoneNumber,CartAmount")] EsyaTemizligiSipari esyaTemizligiSipari)
        {
            if (id != esyaTemizligiSipari.EsyaTemizligiId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(esyaTemizligiSipari);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EsyaTemizligiSipariExists(esyaTemizligiSipari.EsyaTemizligiId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BeyazEsyaSayisi"] = new SelectList(_context.BeyazEsyaBilgisis, "BeyazEsyaId", "BeyazEsyaId", esyaTemizligiSipari.BeyazEsyaSayisi);
            ViewData["HaliSayisi"] = new SelectList(_context.HaliBilgisis, "HaliId", "HaliId", esyaTemizligiSipari.HaliSayisi);
            ViewData["IlceId"] = new SelectList(_context.Ilces, "IlceId", "IlceId", esyaTemizligiSipari.IlceId);
            ViewData["KoltukSayisi"] = new SelectList(_context.KoltukBilgisis, "KoltukBilgisiId", "KoltukBilgisiId", esyaTemizligiSipari.KoltukSayisi);
            ViewData["SaatId"] = new SelectList(_context.RandevuSaats, "RandevuSaatId", "RandevuSaatId", esyaTemizligiSipari.SaatId);
            ViewData["SehirId"] = new SelectList(_context.Sehirs, "SehirId", "SehirId", esyaTemizligiSipari.SehirId);
            ViewData["TeknolojikAletSayisi"] = new SelectList(_context.TeknolojikAletBilgisis, "TeknolojikAletId", "TeknolojikAletId", esyaTemizligiSipari.TeknolojikAletSayisi);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", esyaTemizligiSipari.UserId);
            ViewData["YatakSayisi"] = new SelectList(_context.YatakBilgisis, "YatakId", "YatakId", esyaTemizligiSipari.YatakSayisi);
            return View(esyaTemizligiSipari);
        }

        // GET: EsyaTemizligiSiparis/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.EsyaTemizligiSiparis == null)
            {
                return NotFound();
            }

            var esyaTemizligiSipari = await _context.EsyaTemizligiSiparis
                .Include(e => e.BeyazEsyaSayisiNavigation)
                .Include(e => e.HaliSayisiNavigation)
                .Include(e => e.Ilce)
                .Include(e => e.KoltukSayisiNavigation)
                .Include(e => e.Saat)
                .Include(e => e.Sehir)
                .Include(e => e.TeknolojikAletSayisiNavigation)
                .Include(e => e.User)
                .Include(e => e.YatakSayisiNavigation)
                .FirstOrDefaultAsync(m => m.EsyaTemizligiId == id);
            if (esyaTemizligiSipari == null)
            {
                return NotFound();
            }

            return View(esyaTemizligiSipari);
        }

        // POST: EsyaTemizligiSiparis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.EsyaTemizligiSiparis == null)
            {
                return Problem("Entity set 'AuthUseContext.EsyaTemizligiSiparis'  is null.");
            }
            var esyaTemizligiSipari = await _context.EsyaTemizligiSiparis.FindAsync(id);
            if (esyaTemizligiSipari != null)
            {
                _context.EsyaTemizligiSiparis.Remove(esyaTemizligiSipari);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EsyaTemizligiSipariExists(int id)
        {
          return (_context.EsyaTemizligiSiparis?.Any(e => e.EsyaTemizligiId == id)).GetValueOrDefault();
        }
    }
}
