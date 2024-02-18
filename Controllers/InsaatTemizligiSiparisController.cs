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
using Microsoft.AspNetCore.Identity;
using ETemizlik.Areas.Identity.Data;

namespace ETemizlik.Controllers
{
    public class InsaatTemizligiSiparisController : Controller
    {
        private readonly AuthUseContext _context;
        private readonly UserManager<AuthUseUser> _userManager;

        public InsaatTemizligiSiparisController(AuthUseContext context, UserManager<AuthUseUser> userManager)
        {

            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        public IActionResult GetMaliyet(int KacKatli, int KacOdali)
        {

            decimal calculatedMaliyet = CalculateMaliyet(KacKatli, KacOdali);
            return Json(new { maliyet = calculatedMaliyet });
        }

        private int CalculateMaliyet(int KacKatli, int KacOdali)
        {
            int baseCost = 1000;
            int costPerFloor = 500;
            int costPerRoom = 250;

            int totalCost = baseCost + (KacKatli * costPerFloor) + (KacOdali * costPerRoom);

            return totalCost;
        }

        // GET: InsaatTemizligiSiparis
        public async Task<IActionResult> Index()
        {
            string currentUserId = _userManager.GetUserId(User);
            var authUseContext = _context.InsaatTemizligiSiparis.Where(item => item.UserId == currentUserId).Include(i => i.Ilce).Include(i => i.KacKatliNavigation).Include(i => i.KacOdaliNavigation).Include(i => i.Saat).Include(i => i.Sehir).Include(i => i.User);
            return View(await authUseContext.ToListAsync());
        }

        // GET: InsaatTemizligiSiparis/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.InsaatTemizligiSiparis == null)
            {
                return NotFound();
            }

            var insaatTemizligiSipari = await _context.InsaatTemizligiSiparis
                .Include(i => i.Ilce)
                .Include(i => i.KacKatliNavigation)
                .Include(i => i.KacOdaliNavigation)
                .Include(i => i.Saat)
                .Include(i => i.Sehir)
                .Include(i => i.User)
                .FirstOrDefaultAsync(m => m.InsaatTemizligiId == id);
            if (insaatTemizligiSipari == null)
            {
                return NotFound();
            }

            return View(insaatTemizligiSipari);
        }

        // GET: InsaatTemizligiSiparis/Create
        public IActionResult Create()
        {
            List<Sehir> sehirlistesi = new List<Sehir>();
            sehirlistesi = (from Sehir in _context.Sehirs
                            select
                            Sehir).ToList();
            sehirlistesi.Insert(0, new Sehir { SehirId = 0, SehirAd = "Şehir Seçiniz" });
            ViewBag.ListofSehir = sehirlistesi;

            ViewData["IlceId"] = new SelectList(_context.Ilces, "IlceId", "IlceAd");
            ViewData["KacKatli"] = new SelectList(_context.KatBilgisis, "KatId", "KatBilgisi1");
            ViewData["KacOdali"] = new SelectList(_context.OdaBilgisis, "OdaId", "OdaBilgisi1");
            ViewData["SaatId"] = new SelectList(_context.RandevuSaats, "RandevuSaatId", "RandevuSaat1");
            ViewData["SehirId"] = new SelectList(_context.Sehirs, "SehirId", "SehirAd");
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id");
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

        // POST: InsaatTemizligiSiparis/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InsaatTemizligiId,SehirId,IlceId,KacKatli,KacOdali,Tarih,SaatId,UserId,Address,PhoneNumber,CartAmount")] InsaatTemizligiSipari insaatTemizligiSipari)
        {
            if (ModelState.IsValid)
            {
                int kacKatli = (int)insaatTemizligiSipari.KacKatli;
                int kacOdali = (int)insaatTemizligiSipari.KacOdali;
                insaatTemizligiSipari.CartAmount = CalculateMaliyet(kacKatli, kacOdali);
                var userId = _userManager.GetUserId(User);
                insaatTemizligiSipari.UserId = userId;
                _context.Add(insaatTemizligiSipari);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            List<Sehir> sehirlistesi = new List<Sehir>();
            sehirlistesi = (from Sehir in _context.Sehirs
                            select
                            Sehir).ToList();
            sehirlistesi.Insert(0, new Sehir { SehirId = 0, SehirAd = "Şehir Seçiniz" });
            ViewBag.ListofSehir = sehirlistesi;

            ViewData["IlceId"] = new SelectList(_context.Ilces, "IlceId", "IlceId", insaatTemizligiSipari.IlceId);
            ViewData["KacKatli"] = new SelectList(_context.KatBilgisis, "KatId", "KatId", insaatTemizligiSipari.KacKatli);
            ViewData["KacOdali"] = new SelectList(_context.OdaBilgisis, "OdaId", "OdaId", insaatTemizligiSipari.KacOdali);
            ViewData["SaatId"] = new SelectList(_context.RandevuSaats, "RandevuSaatId", "RandevuSaatId", insaatTemizligiSipari.SaatId);
            ViewData["SehirId"] = new SelectList(_context.Sehirs, "SehirId", "SehirId", insaatTemizligiSipari.SehirId);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", insaatTemizligiSipari.UserId);
            return View(insaatTemizligiSipari);
        }

        // GET: InsaatTemizligiSiparis/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.InsaatTemizligiSiparis == null)
            {
                return NotFound();
            }

            var insaatTemizligiSipari = await _context.InsaatTemizligiSiparis.FindAsync(id);
            if (insaatTemizligiSipari == null)
            {
                return NotFound();
            }
            ViewData["IlceId"] = new SelectList(_context.Ilces, "IlceId", "IlceId", insaatTemizligiSipari.IlceId);
            ViewData["KacKatli"] = new SelectList(_context.KatBilgisis, "KatId", "KatId", insaatTemizligiSipari.KacKatli);
            ViewData["KacOdali"] = new SelectList(_context.OdaBilgisis, "OdaId", "OdaId", insaatTemizligiSipari.KacOdali);
            ViewData["SaatId"] = new SelectList(_context.RandevuSaats, "RandevuSaatId", "RandevuSaatId", insaatTemizligiSipari.SaatId);
            ViewData["SehirId"] = new SelectList(_context.Sehirs, "SehirId", "SehirId", insaatTemizligiSipari.SehirId);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", insaatTemizligiSipari.UserId);
            return View(insaatTemizligiSipari);
        }

        // POST: InsaatTemizligiSiparis/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("InsaatTemizligiId,SehirId,IlceId,KacKatli,KacOdali,Tarih,SaatId,UserId,Address,PhoneNumber,CartAmount")] InsaatTemizligiSipari insaatTemizligiSipari)
        {
            if (id != insaatTemizligiSipari.InsaatTemizligiId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(insaatTemizligiSipari);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InsaatTemizligiSipariExists(insaatTemizligiSipari.InsaatTemizligiId))
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
            ViewData["IlceId"] = new SelectList(_context.Ilces, "IlceId", "IlceId", insaatTemizligiSipari.IlceId);
            ViewData["KacKatli"] = new SelectList(_context.KatBilgisis, "KatId", "KatId", insaatTemizligiSipari.KacKatli);
            ViewData["KacOdali"] = new SelectList(_context.OdaBilgisis, "OdaId", "OdaId", insaatTemizligiSipari.KacOdali);
            ViewData["SaatId"] = new SelectList(_context.RandevuSaats, "RandevuSaatId", "RandevuSaatId", insaatTemizligiSipari.SaatId);
            ViewData["SehirId"] = new SelectList(_context.Sehirs, "SehirId", "SehirId", insaatTemizligiSipari.SehirId);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", insaatTemizligiSipari.UserId);
            return View(insaatTemizligiSipari);
        }

        // GET: InsaatTemizligiSiparis/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.InsaatTemizligiSiparis == null)
            {
                return NotFound();
            }

            var insaatTemizligiSipari = await _context.InsaatTemizligiSiparis
                .Include(i => i.Ilce)
                .Include(i => i.KacKatliNavigation)
                .Include(i => i.KacOdaliNavigation)
                .Include(i => i.Saat)
                .Include(i => i.Sehir)
                .Include(i => i.User)
                .FirstOrDefaultAsync(m => m.InsaatTemizligiId == id);
            if (insaatTemizligiSipari == null)
            {
                return NotFound();
            }

            return View(insaatTemizligiSipari);
        }

        // POST: InsaatTemizligiSiparis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.InsaatTemizligiSiparis == null)
            {
                return Problem("Entity set 'AuthUseContext.InsaatTemizligiSiparis'  is null.");
            }
            var insaatTemizligiSipari = await _context.InsaatTemizligiSiparis.FindAsync(id);
            if (insaatTemizligiSipari != null)
            {
                _context.InsaatTemizligiSiparis.Remove(insaatTemizligiSipari);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InsaatTemizligiSipariExists(int id)
        {
          return (_context.InsaatTemizligiSiparis?.Any(e => e.InsaatTemizligiId == id)).GetValueOrDefault();
        }
    }
}
