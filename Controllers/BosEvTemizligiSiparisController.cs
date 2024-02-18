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
    public class BosEvTemizligiSiparisController : Controller
    {
        private readonly AuthUseContext _context;
        private readonly UserManager<AuthUseUser> _userManager;

        public BosEvTemizligiSiparisController(AuthUseContext context, UserManager<AuthUseUser> userManager)
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

        // GET: BosEvTemizligiSiparis
        public async Task<IActionResult> Index()
        {
            string currentUserId = _userManager.GetUserId(User);
            var authUseContext = _context.BosEvTemizligiSiparis.Where(item => item.UserId == currentUserId).Include(b => b.Ilce).Include(b => b.KacKatliNavigation).Include(b => b.KacOdaliNavigation).Include(b => b.Saat).Include(b => b.Sehir).Include(b => b.User);
            return View(await authUseContext.ToListAsync());
        }

        // GET: BosEvTemizligiSiparis/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.BosEvTemizligiSiparis == null)
            {
                return NotFound();
            }

            var bosEvTemizligiSipari = await _context.BosEvTemizligiSiparis
                .Include(b => b.Ilce)
                .Include(b => b.KacKatliNavigation)
                .Include(b => b.KacOdaliNavigation)
                .Include(b => b.Saat)
                .Include(b => b.Sehir)
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.BosevTemizligiId == id);
            if (bosEvTemizligiSipari == null)
            {
                return NotFound();
            }

            return View(bosEvTemizligiSipari);
        }

        // GET: BosEvTemizligiSiparis/Create
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

        // POST: BosEvTemizligiSiparis/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BosevTemizligiId,SehirId,IlceId,KacKatli,KacOdali,Tarih,SaatId,UserId,Address,PhoneNumber,CartAmount")] BosEvTemizligiSipari bosEvTemizligiSipari)
        {
            if (ModelState.IsValid)
            {
                int kacKatli = (int)bosEvTemizligiSipari.KacKatli;
                int kacOdali = (int)bosEvTemizligiSipari.KacOdali;
                bosEvTemizligiSipari.CartAmount = CalculateMaliyet(kacKatli, kacOdali);
                var userId = _userManager.GetUserId(User);

                // Set the UserId property in the model
                bosEvTemizligiSipari.UserId = userId;
                _context.Add(bosEvTemizligiSipari);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            List<Sehir> sehirlistesi = new List<Sehir>();
            sehirlistesi = (from Sehir in _context.Sehirs
                            select
                            Sehir).ToList();
            sehirlistesi.Insert(0, new Sehir { SehirId = 0, SehirAd = "Şehir Seçiniz" });
            ViewBag.ListofSehir = sehirlistesi;

            ViewData["IlceId"] = new SelectList(_context.Ilces, "IlceId", "IlceId", bosEvTemizligiSipari.IlceId);
            ViewData["KacKatli"] = new SelectList(_context.KatBilgisis, "KatId", "KatId", bosEvTemizligiSipari.KacKatli);
            ViewData["KacOdali"] = new SelectList(_context.OdaBilgisis, "OdaId", "OdaId", bosEvTemizligiSipari.KacOdali);
            ViewData["SaatId"] = new SelectList(_context.RandevuSaats, "RandevuSaatId", "RandevuSaatId", bosEvTemizligiSipari.SaatId);
            ViewData["SehirId"] = new SelectList(_context.Sehirs, "SehirId", "SehirId", bosEvTemizligiSipari.SehirId);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", bosEvTemizligiSipari.UserId);
            return View(bosEvTemizligiSipari);
        }

        // GET: BosEvTemizligiSiparis/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.BosEvTemizligiSiparis == null)
            {
                return NotFound();
            }

            var bosEvTemizligiSipari = await _context.BosEvTemizligiSiparis.FindAsync(id);
            if (bosEvTemizligiSipari == null)
            {
                return NotFound();
            }
            ViewData["IlceId"] = new SelectList(_context.Ilces, "IlceId", "IlceId", bosEvTemizligiSipari.IlceId);
            ViewData["KacKatli"] = new SelectList(_context.KatBilgisis, "KatId", "KatId", bosEvTemizligiSipari.KacKatli);
            ViewData["KacOdali"] = new SelectList(_context.OdaBilgisis, "OdaId", "OdaId", bosEvTemizligiSipari.KacOdali);
            ViewData["SaatId"] = new SelectList(_context.RandevuSaats, "RandevuSaatId", "RandevuSaatId", bosEvTemizligiSipari.SaatId);
            ViewData["SehirId"] = new SelectList(_context.Sehirs, "SehirId", "SehirId", bosEvTemizligiSipari.SehirId);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", bosEvTemizligiSipari.UserId);
            return View(bosEvTemizligiSipari);
        }

        // POST: BosEvTemizligiSiparis/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BosevTemizligiId,SehirId,IlceId,KacKatli,KacOdali,Tarih,SaatId,UserId,Address,PhoneNumber,CartAmount")] BosEvTemizligiSipari bosEvTemizligiSipari)
        {
            if (id != bosEvTemizligiSipari.BosevTemizligiId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bosEvTemizligiSipari);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BosEvTemizligiSipariExists(bosEvTemizligiSipari.BosevTemizligiId))
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
            ViewData["IlceId"] = new SelectList(_context.Ilces, "IlceId", "IlceId", bosEvTemizligiSipari.IlceId);
            ViewData["KacKatli"] = new SelectList(_context.KatBilgisis, "KatId", "KatId", bosEvTemizligiSipari.KacKatli);
            ViewData["KacOdali"] = new SelectList(_context.OdaBilgisis, "OdaId", "OdaId", bosEvTemizligiSipari.KacOdali);
            ViewData["SaatId"] = new SelectList(_context.RandevuSaats, "RandevuSaatId", "RandevuSaatId", bosEvTemizligiSipari.SaatId);
            ViewData["SehirId"] = new SelectList(_context.Sehirs, "SehirId", "SehirId", bosEvTemizligiSipari.SehirId);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", bosEvTemizligiSipari.UserId);
            return View(bosEvTemizligiSipari);
        }

        // GET: BosEvTemizligiSiparis/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.BosEvTemizligiSiparis == null)
            {
                return NotFound();
            }

            var bosEvTemizligiSipari = await _context.BosEvTemizligiSiparis
                .Include(b => b.Ilce)
                .Include(b => b.KacKatliNavigation)
                .Include(b => b.KacOdaliNavigation)
                .Include(b => b.Saat)
                .Include(b => b.Sehir)
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.BosevTemizligiId == id);
            if (bosEvTemizligiSipari == null)
            {
                return NotFound();
            }

            return View(bosEvTemizligiSipari);
        }

        // POST: BosEvTemizligiSiparis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.BosEvTemizligiSiparis == null)
            {
                return Problem("Entity set 'AuthUseContext.BosEvTemizligiSiparis'  is null.");
            }
            var bosEvTemizligiSipari = await _context.BosEvTemizligiSiparis.FindAsync(id);
            if (bosEvTemizligiSipari != null)
            {
                _context.BosEvTemizligiSiparis.Remove(bosEvTemizligiSipari);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BosEvTemizligiSipariExists(int id)
        {
          return (_context.BosEvTemizligiSiparis?.Any(e => e.BosevTemizligiId == id)).GetValueOrDefault();
        }
    }
}
