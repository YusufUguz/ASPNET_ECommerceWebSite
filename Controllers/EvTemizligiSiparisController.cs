using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ETemizlik.Models;
using System.Net.NetworkInformation;
using Microsoft.AspNetCore.Identity;
using ETemizlik.Areas.Identity.Data;
using System.Security.Claims;

namespace ETemizlik.Controllers
{

    public class EvTemizligiSiparisController : Controller
    {
        private readonly UserManager<AuthUseUser> _userManager;
        private readonly SignInManager<AuthUseUser> _signInManager;
        private readonly AuthUseContext _context;

        public EvTemizligiSiparisController(AuthUseContext context, UserManager<AuthUseUser> userManager, SignInManager<AuthUseUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
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

        // GET: EvTemizligiSiparis
        public async Task<IActionResult> Index()
        {
            string currentUserId = _userManager.GetUserId(User);
            var authUseContext = _context.EvTemizligiSiparis.Where(item=>item.UserId == currentUserId).Include(e => e.Ilce).Include(e => e.KacKatliNavigation).Include(e => e.KacOdaliNavigation).Include(e => e.Saat).Include(e => e.Sehir).Include(e => e.User);
            return View(await authUseContext.ToListAsync());
        }

        // GET: EvTemizligiSiparis/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.EvTemizligiSiparis == null)
            {
                return NotFound();
            }

            var evTemizligiSipari = await _context.EvTemizligiSiparis
                .Include(e => e.Ilce)
                .Include(e => e.KacKatliNavigation)
                .Include(e => e.KacOdaliNavigation)
                .Include(e => e.Saat)
                .Include(e => e.Sehir)
                .Include(e => e.User)
                .FirstOrDefaultAsync(m => m.EvTemizligiId == id);
            if (evTemizligiSipari == null)
            {
                return NotFound();
            }

            return View(evTemizligiSipari);
        }

        // GET: EvTemizligiSiparis/Create
        public IActionResult Create()
        {
            //if (!_signInManager.IsSignedIn(User))
            //{
            //    // Kullanıcı giriş yapmamışsa, bir uyarı göster ve giriş yapma sayfasına yönlendir
            //    TempData["ErrorMessage"] = "Üzgünüz, işlem yapabilmek için önce giriş yapmalısınız.";
            //    return RedirectToAction("Login", "Account");// "Login" ve "Account" kısmını kendi proje yapınıza göre güncelleyin.
            //}

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

        // POST: EvTemizligiSiparis/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EvTemizligiId,SehirId,IlceId,KacKatli,KacOdali,Tarih,SaatId,UserId,Address,PhoneNumber,CartAmount")] EvTemizligiSipari evTemizligiSipari)
        {
            if (ModelState.IsValid)
            {

                int kacKatli = (int)evTemizligiSipari.KacKatli;
                int kacOdali = (int)evTemizligiSipari.KacOdali;
                evTemizligiSipari.CartAmount = CalculateMaliyet(kacKatli, kacOdali);
                var userId = _userManager.GetUserId(User);

                // Set the UserId property in the model
                evTemizligiSipari.UserId = userId;
                _context.Add(evTemizligiSipari);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            List<Sehir> sehirlistesi = new List<Sehir>();
            sehirlistesi = (from Sehir in _context.Sehirs
                            select
                            Sehir).ToList();
            sehirlistesi.Insert(0, new Sehir { SehirId = 0, SehirAd = "Şehir Seçiniz" });
            ViewBag.ListofSehir = sehirlistesi;

            ViewData["IlceId"] = new SelectList(_context.Ilces, "IlceId", "IlceId", evTemizligiSipari.IlceId);
            ViewData["KacKatli"] = new SelectList(_context.KatBilgisis, "KatId", "KatId", evTemizligiSipari.KacKatli);
            ViewData["KacOdali"] = new SelectList(_context.OdaBilgisis, "OdaId", "OdaId", evTemizligiSipari.KacOdali);
            ViewData["SaatId"] = new SelectList(_context.RandevuSaats, "RandevuSaatId", "RandevuSaatId", evTemizligiSipari.SaatId);
            ViewData["SehirId"] = new SelectList(_context.Sehirs, "SehirId", "SehirId", evTemizligiSipari.SehirId);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", evTemizligiSipari.UserId);
            return View(evTemizligiSipari);
        }

        // GET: EvTemizligiSiparis/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.EvTemizligiSiparis == null)
            {
                return NotFound();
            }

            var evTemizligiSipari = await _context.EvTemizligiSiparis.FindAsync(id);
            if (evTemizligiSipari == null)
            {
                return NotFound();
            }
            ViewData["IlceId"] = new SelectList(_context.Ilces, "IlceId", "IlceId", evTemizligiSipari.IlceId);
            ViewData["KacKatli"] = new SelectList(_context.KatBilgisis, "KatId", "KatId", evTemizligiSipari.KacKatli);
            ViewData["KacOdali"] = new SelectList(_context.OdaBilgisis, "OdaId", "OdaId", evTemizligiSipari.KacOdali);
            ViewData["SaatId"] = new SelectList(_context.RandevuSaats, "RandevuSaatId", "RandevuSaatId", evTemizligiSipari.SaatId);
            ViewData["SehirId"] = new SelectList(_context.Sehirs, "SehirId", "SehirId", evTemizligiSipari.SehirId);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", evTemizligiSipari.UserId);
            return View(evTemizligiSipari);
        }

        // POST: EvTemizligiSiparis/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EvTemizligiId,SehirId,IlceId,KacKatli,KacOdali,Tarih,SaatId,UserId,Address,PhoneNumber,CartAmount")] EvTemizligiSipari evTemizligiSipari)
        {
            if (id != evTemizligiSipari.EvTemizligiId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(evTemizligiSipari);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EvTemizligiSipariExists(evTemizligiSipari.EvTemizligiId))
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
            ViewData["IlceId"] = new SelectList(_context.Ilces, "IlceId", "IlceId", evTemizligiSipari.IlceId);
            ViewData["KacKatli"] = new SelectList(_context.KatBilgisis, "KatId", "KatId", evTemizligiSipari.KacKatli);
            ViewData["KacOdali"] = new SelectList(_context.OdaBilgisis, "OdaId", "OdaId", evTemizligiSipari.KacOdali);
            ViewData["SaatId"] = new SelectList(_context.RandevuSaats, "RandevuSaatId", "RandevuSaatId", evTemizligiSipari.SaatId);
            ViewData["SehirId"] = new SelectList(_context.Sehirs, "SehirId", "SehirId", evTemizligiSipari.SehirId);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", evTemizligiSipari.UserId);
            return View(evTemizligiSipari);
        }

        // GET: EvTemizligiSiparis/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.EvTemizligiSiparis == null)
            {
                return NotFound();
            }

            var evTemizligiSipari = await _context.EvTemizligiSiparis
                .Include(e => e.Ilce)
                .Include(e => e.KacKatliNavigation)
                .Include(e => e.KacOdaliNavigation)
                .Include(e => e.Saat)
                .Include(e => e.Sehir)
                .Include(e => e.User)
                .FirstOrDefaultAsync(m => m.EvTemizligiId == id);
            if (evTemizligiSipari == null)
            {
                return NotFound();
            }

            return View(evTemizligiSipari);
        }

        // POST: EvTemizligiSiparis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.EvTemizligiSiparis == null)
            {
                return Problem("Entity set 'AuthUseContext.EvTemizligiSiparis'  is null.");
            }
            var evTemizligiSipari = await _context.EvTemizligiSiparis.FindAsync(id);
            if (evTemizligiSipari != null)
            {
                _context.EvTemizligiSiparis.Remove(evTemizligiSipari);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EvTemizligiSipariExists(int id)
        {
          return (_context.EvTemizligiSiparis?.Any(e => e.EvTemizligiId == id)).GetValueOrDefault();
        }
    }
}
