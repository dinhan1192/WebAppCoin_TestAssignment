using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebAppCoin_TestAssignment.Models;
using WebAppCoin_TestAssignment.Services;
using static WebAppCoin_TestAssignment.Models.Coin;

namespace WebAppCoin_TestAssignment.Controllers
{
    [Authorize]
    public class CoinsController : Controller
    {
        private MyDbContext db = new MyDbContext();

        private ICRUDService<Coin> sqlCoinService;

        public CoinsController()
        {
            sqlCoinService = new SQLCoinService();
        }

        // GET: Coins
        public ActionResult Index(string searchString, string currentFilter, string MarketID)
        {
            if (searchString == null)
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var coins = db.Coins.Where(s => s.Status != CoinStatus.Deleted);

            if (!String.IsNullOrEmpty(searchString))
            {
                coins = coins.Where(s => s.Code.Contains(searchString));
            }

            ViewBag.MarketID = new SelectList(db.Markets, "Id", "Name", MarketID);
            if (String.IsNullOrEmpty(MarketID))
            {
                return View(coins.ToList());
            }

            var id = (int?)Convert.ToInt32(MarketID);

            coins = coins.Where(x => x.Market.Id == id);

            return View(coins.ToList());
        }

        // GET: Coins/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Coin coin = db.Coins.Find(id);
            if (coin == null || coin.IsDeleted())
            {
                return HttpNotFound();
            }
            return View(coin);
        }

        // GET: Coins/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.MarketId = new SelectList(db.Markets, "Id", "Name");
            return View();
        }

        // POST: Coins/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "BaseAsset,QuoteAsset,LastPrice,Volumn24h,MarketId,Status")] Coin coin)
        {
            if (sqlCoinService.Create(coin, ModelState))
            {
                return RedirectToAction("Index");
            }

            ViewBag.MarketId = new SelectList(db.Markets, "Id", "Name", coin.MarketId);
            return View(coin);
        }

        // GET: Coins/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Coin coin = db.Coins.Find(id);
            if (coin == null || coin.IsDeleted())
            {
                return HttpNotFound();
            }
            ViewBag.MarketId = new SelectList(db.Markets, "Id", "Name", coin.MarketId);
            return View(coin);
        }

        // POST: Coins/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "Code,BaseAsset,QuoteAsset,LastPrice,Volumn24h,MarketId,Status")] Coin coin)
        {
            if (coin == null || coin.Code == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var existCoin = db.Coins.Find(coin.Code);
            if (existCoin == null || existCoin.IsDeleted())
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            if (sqlCoinService.Update(existCoin, coin, ModelState))
            {
                return RedirectToAction("Index");
            }

            ViewBag.MarketId = new SelectList(db.Markets, "Id", "Name", coin.MarketId);
            return View(coin);
        }

        // GET: Coins/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Coin coin = db.Coins.Find(id);
            if (coin == null)
            {
                return HttpNotFound();
            }
            return View(coin);
        }

        // POST: Coins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var existCoin = db.Coins.Find(id);
            if (existCoin == null || existCoin.IsDeleted())
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            if (sqlCoinService.Delete(existCoin, ModelState))
            {
                return RedirectToAction("Index");
            }
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
