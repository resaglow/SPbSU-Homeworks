using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TrainTickets.Models;

namespace TrainTickets.Controllers
{
    public class HomeController : Controller
    {
        private TicketsDbContext db = new TicketsDbContext();

        private static object Lock = new object();

        public UserManager<ApplicationUser> UserManager { get; private set; }
        public UserStore<ApplicationUser> Store { get; private set; }

        public HomeController()
        {
            Store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            UserManager = new UserManager<ApplicationUser>(Store);
        }

        // GET: Home
        public ActionResult Index()
        {
            var tickets = from ticket in db.Tickets
                          select ticket;

            tickets = tickets.Where(x => x.OwnerLogin == null && x.DateFrom > DateTime.Now);

            ViewBag.NotLoggedIn = TempData["NotLoggedIn"];
            ViewBag.NotEnoughMoney = TempData["NotEnoughMoney"];

            return View(tickets);
        }

        [HttpPost]
        public ActionResult Buy()
        {
            lock (Lock)
            {
                if (!User.Identity.IsAuthenticated)
                {
                    TempData["NotLoggedIn"] = "Not";
                }
                else
                {
                    var username = Request["username"];
                    var ticketId = Request["ticketId"];
                    var price = Convert.ToDecimal(Request["price"]);

                    var user = UserManager.FindById(User.Identity.GetUserId());

                    if (user.UserMoney < price)
                    {
                        TempData["NotEnoughMoney"] = "Not";
                        return RedirectToAction("Index");
                    }

                    user.UserMoney -= price;

                    var boughtTicket = db.Tickets.Find(Convert.ToInt64(ticketId));
                    boughtTicket.OwnerLogin = username;
                    db.SaveChanges();

                    var result = UserManager.UpdateAsync(user);
                    Store.Context.SaveChanges();
                }

                return RedirectToAction("Index");
            }
        }

        // GET: Home/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Home/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Number,OwnerLogin,From,To,DateFrom,DateTo,Price")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                db.Tickets.Add(ticket);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ticket);
        }

        // GET: Home/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // POST: Home/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Number,OwnerLogin,From,To,DateFrom,DateTo,Price")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ticket);
        }

        // GET: Home/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // POST: Home/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Ticket ticket = db.Tickets.Find(id);
            db.Tickets.Remove(ticket);
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
