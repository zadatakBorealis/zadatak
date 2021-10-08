using Borealis.Data;
using Borealis.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Borealis.Controllers
{
    public class TopTimeController : Controller
    {
        private readonly ApplicationDbContext _db;

        public TopTimeController(ApplicationDbContext db)
        {
            _db = db;
        }
        
        public IActionResult Index()
        {
            IEnumerable<TopTime> objList = _db.TopTimes;
            
            return View();
        }

        public IActionResult List()
        {
            IEnumerable<TopTime> objList = _db.TopTimes.
                OrderBy(time => time.TimeAsDate.TimeOfDay).
                Where(isApproved => isApproved.IsApproved == true);
            return View(objList);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult UnproccessedTimes()
        {
            IEnumerable<TopTime> objList = _db.TopTimes.
                OrderBy(time => time.TimeAsDate.TimeOfDay).
                Where(isApproved => isApproved.IsApproved == false &&
                    isApproved.IsReadyToDelete == false);
            return View(objList);
        }

        public IActionResult UnproccessedTimesReadyToDelete()
        {
            IEnumerable<TopTime> objList = _db.TopTimes.
                OrderBy(time => time.TimeAsDate.TimeOfDay).
                Where(isApproved => isApproved.IsReadyToDelete == true && 
                    isApproved.CreatedBy == User.Claims.ToList()[0].Value);
            return View(objList);
        }

        public IActionResult SendToAcceptApproval(int id)
        {
            var topTime = _db.TopTimes.Find(id);
            if (topTime == null)
            {
                return NotFound();
            }
            topTime.IsReadyToDelete = false;
            topTime.IsApproved = false;
            _db.SaveChanges();
            return RedirectToAction("List");
        }

        public IActionResult Accept(int id)
        {
            var topTime = _db.TopTimes.Find(id);
            if (topTime == null)
            {
                return NotFound();
            }
            topTime.IsApproved = true;
            _db.SaveChanges();
            return RedirectToAction("List");
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TopTime topTime)
        {
            if (ModelState.IsValid && DateTime.TryParse(topTime.Time, out DateTime result))
            {
                topTime.CreatedBy = User.Claims.ToList()[0].Value;
                _db.TopTimes.Add(topTime);
                _db.SaveChanges();
                return RedirectToAction("List");
            }
            TempData["Error"] = @"Greška! Uneseno je pogrešno vrijeme. Vrijeme mora biti u formatu" +
                " HH:MM:SS npr., 12:34:56 (12 sati, 34 minute i 56 sekundi)!";
            return View(topTime);
        }

        public IActionResult SendToDeleteApproval(int id)
        {
            var topTime = _db.TopTimes.Find(id);
            if (topTime == null)
            {
                return NotFound();
            }
            topTime.IsReadyToDelete = true;
            topTime.IsApproved = false;
            _db.SaveChanges();
            return RedirectToAction("List");
        }

        public IActionResult Delete(int id)
        {
            var topTime = _db.TopTimes.Find(id);
            if(topTime == null)
            {
                return NotFound();
            }
            _db.TopTimes.Remove(topTime);
            _db.SaveChanges();
            return RedirectToAction("List");
        }
    }
}
