using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BookRental.Models;

namespace BookRental.Controllers
{
    public class MembershipTypesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: MembershipTypes
        public ActionResult Index()
        {
            return View(db.MembershipTypes.ToList());
        }

        // GET: MembershipTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MembershipType membershipType = db.MembershipTypes.Find(id);
            if (membershipType == null)
            {
                return HttpNotFound();
            }
            return View(membershipType);
        }

        // GET: MembershipTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MembershipTypes/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,SignUpFee,ChargeRateOneMonth,ChargeRateSixMonth")] MembershipType membershipType)
        {
            if (ModelState.IsValid)
            {
                db.MembershipTypes.Add(membershipType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(membershipType);
        }

        // GET: MembershipTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MembershipType membershipType = db.MembershipTypes.Find(id);
            if (membershipType == null)
            {
                return HttpNotFound();
            }
            return View(membershipType);
        }

        // POST: MembershipTypes/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MembershipType membershipType)
        {

			if (!ModelState.IsValid)
			{
                MembershipType membership = new MembershipType
                {
                    Name = membershipType.Name,
                    SignUpFee = membershipType.SignUpFee,
                    ChargeRateOneMonth = membershipType.ChargeRateOneMonth,
                    ChargeRateSixMonth = membershipType.ChargeRateSixMonth
                };
                return View("Edit", membership);
			}
			else
			{
                var membershipInDb = db.MembershipTypes.Single(m => m.Id.Equals(membershipType.Id));
                membershipInDb.Name = membershipType.Name;
                membershipInDb.SignUpFee = membershipType.SignUpFee;
                membershipInDb.ChargeRateOneMonth = membershipType.ChargeRateOneMonth;
                membershipInDb.ChargeRateSixMonth = membershipType.ChargeRateSixMonth;
			}
            db.SaveChanges();
            return RedirectToAction("Index", "MembershipTypes");
        }

        // GET: MembershipTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MembershipType membershipType = db.MembershipTypes.Find(id);
            if (membershipType == null)
            {
                return HttpNotFound();
            }
            return View(membershipType);
        }

        // POST: MembershipTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MembershipType membershipType = db.MembershipTypes.Find(id);
            db.MembershipTypes.Remove(membershipType);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
