using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sube1.HelloMVC.Models;
using Sube1.HelloMVC.Models.ViewModels;
using System.Text.Json;
using System.Threading.Tasks;

namespace Sube1.HelloMVC.Controllers
{
    public class OgrenciController : Controller
    {
        readonly OkulDbContext ctx;

        public OgrenciController(OkulDbContext context)
        {
            ctx = context;
        }

        public IActionResult Index()
        {
            return View("AnaSayfa");
        }

        public IActionResult OgrenciDetay(int id)
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> StudentDetail(int id)
        {
            var ogrenci = await ctx.Ogrenciler.FindAsync(id);
            return View(ogrenci);
        }

        [HttpPost]
        public async Task<IActionResult> StudentDetail(Ogrenci ogr)
        {
            ctx.Entry(ogr).State = EntityState.Modified;
            await ctx.SaveChangesAsync();
            return RedirectToAction("OgrenciListe");
        }

        public async Task<ViewResult> OgrenciListe()
        {
            var lst = await ctx.Ogrenciler.ToListAsync();
            return View(lst);
        }

        [HttpGet]
        public ViewResult OgrenciEkle()
        {
            return View();
        }

        [HttpPost]
        public async Task<ViewResult> OgrenciEkle(Ogrenci ogr)
        {
            ctx.Ogrenciler.Add(ogr);
            int sonuc = await ctx.SaveChangesAsync();
            TempData["sonuc"] = sonuc > 0;
            return View();
        }

        public async Task<IActionResult> OgrenciSil(int id)
        {
            var ogr = await ctx.Ogrenciler.FindAsync(id);
            if (ogr != null)
            {
                ctx.Ogrenciler.Remove(ogr);
                await ctx.SaveChangesAsync();
            }
            return RedirectToAction(nameof(OgrenciListe));
        }

        [HttpGet]
        public async Task<IActionResult> OgrenciListeAjax()
        {
            var ogrenciler = await ctx.Ogrenciler.ToListAsync();
            return Json(ogrenciler);
        }

        [HttpGet]
        public async Task<IActionResult> OgrenciGetirAjax(int id)
        {
            var ogrenci = await ctx.Ogrenciler.FindAsync(id);
            if (ogrenci == null)
            {
                return NotFound();
            }
            return Json(ogrenci);
        }

        [HttpPost]
        public async Task<IActionResult> OgrenciEkleAjax([FromBody] Ogrenci ogrenci)
        {
            if (ogrenci == null)
            {
                return BadRequest();
            }

            ctx.Ogrenciler.Add(ogrenci);
            int sonuc = await ctx.SaveChangesAsync();
            if (sonuc > 0)
            {
                return Json(new { success = true, message = "Öğrenci başarıyla eklendi.", data = ogrenci });
            }
            else
            {
                return Json(new { success = false, message = "Öğrenci eklenemedi." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> OgrenciGuncelleAjax([FromBody] Ogrenci ogrenci)
        {
            if (ogrenci == null)
            {
                return BadRequest();
            }

            var mevcutOgrenci = await ctx.Ogrenciler.FindAsync(ogrenci.Ogrenciid);
            if (mevcutOgrenci == null)
            {
                return NotFound();
            }

            mevcutOgrenci.Ad = ogrenci.Ad;
            mevcutOgrenci.Soyad = ogrenci.Soyad;

            ctx.Entry(mevcutOgrenci).State = EntityState.Modified;
            int sonuc = await ctx.SaveChangesAsync();

            if (sonuc > 0)
            {
                return Json(new { success = true, message = "Öğrenci başarıyla güncellendi.", data = mevcutOgrenci });
            }
            else
            {
                return Json(new { success = false, message = "Öğrenci güncellenemedi." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> OgrenciSilAjax([FromBody] int id)
        {
            var ogrenci = await ctx.Ogrenciler.FindAsync(id);
            if (ogrenci == null)
            {
                return NotFound();
            }

            ctx.Ogrenciler.Remove(ogrenci);
            int sonuc = await ctx.SaveChangesAsync();

            if (sonuc > 0)
            {
                return Json(new { success = true, message = "Öğrenci başarıyla silindi." });
            }
            else
            {
                return Json(new { success = false, message = "Öğrenci silinemedi." });
            }
        }
    }
}
