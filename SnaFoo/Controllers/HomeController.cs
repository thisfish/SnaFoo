using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SnaFoo.Data;
using System.Data;
using System.Data.Entity;
using System.Net;
using SnaFoo.Models;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace SnaFooWeb.Controllers
{
    public class HomeController : Controller
    {
        private SnaFooEntities db = new SnaFooEntities();

        public ActionResult Index()
        {
            List<Snack> snacks = new List<Snack>();
            List<SuggestedSnack> suggestedSnacks = new List<SuggestedSnack>();
            //get json string containing snacks from web service
            string snackResult = SnacksServiceHelper.GetSnacks();
            if (!string.IsNullOrEmpty(snackResult))
            {
                try
                {
                    snacks = JsonConvert.DeserializeObject<List<Snack>>(snackResult);
                }
                catch(Exception ex)
                {
                    //display error
                }
                if (snacks.Count() > 0)
                {
                    List<Vote> votes = new List<SnaFoo.Models.Vote>();
                    try
                    {
                        //get all votes from database for this month
                        votes = db.Votes.Where(x => x != null && x.VotedOn.Month == DateTime.Now.Month).ToList();
                    }
                    catch(Exception Ex)
                    {
                    }
                    List<Suggestion> suggestions = new List<Suggestion>();
                    try
                    {
                        //get all suggestions from database for this month
                        suggestions = db.Suggestions.Where(x => x != null && x.SuggestedOn.Month == DateTime.Now.Month).ToList();
                    }
                    catch (Exception ex)
                    {
                    }
                    foreach (Snack s in snacks.Where(x => suggestions.Select(y => y.SnackId).Contains(x.Id)))
                    {
                        SuggestedSnack ss = new SuggestedSnack();
                        ss.Id = s.Id;
                        ss.LastPurchaseDate = s.LastPurchaseDate;
                        ss.Name = s.Name;
                        ss.Optional = s.Optional;
                        ss.PurchaseCount = s.PurchaseCount;
                        ss.PurchaseLocations = s.PurchaseLocations;
                        ss.Votes = votes.Count(x => x.SnackId == s.Id);
                        suggestedSnacks.Add(ss);
                    }
                }
            }
            return View(new SnacksAndSuggestedSnacks { Snacks = snacks.Where(x => x.Optional == false).ToList(), SuggestedSnacks = suggestedSnacks });
        }

        public ActionResult Suggestions()
        {
            List<Snack> snacks = new List<Snack>();
            try
            {
                //get snacks from web service
                snacks = JsonConvert.DeserializeObject<List<Snack>>(SnacksServiceHelper.GetSnacks());
            }
            catch(Exception ex)
            {
                //display error
            }
            List<Suggestion> suggestions = new List<Suggestion>();
            if (snacks != null && snacks.Count > 0)
            {
                try
                {
                    suggestions = db.Suggestions.Where(x => x != null && x.SuggestedOn.Month == DateTime.Now.Month).ToList();
                    snacks = snacks.Where(x => !suggestions.Select(y => y.SnackId).Contains(x.Id) && x.Optional == true).ToList();
                    snacks.Insert(0, new Snack { Id = 0, LastPurchaseDate = "", Name = "", Optional = true, PurchaseCount = 0, PurchaseLocations = "" });
                    ViewData["snacks"] = snacks;
                }
                catch(Exception ex)
                {
                    //display error
                }
            }
            else
            {
                ViewData["snacks"] = new List<Snack>();
            }
            return View();
        }

        public ActionResult ShoppingList()
        {
            List<Snack> snacks = new List<Snack>();
            try
            {
                //get snacks from web service
                snacks = JsonConvert.DeserializeObject<List<Snack>>(SnacksServiceHelper.GetSnacks());
            }
            catch (Exception ex)
            {
                //display error
            }
            if (snacks != null && snacks.Count() > 0)
            {
                List<Vote> votes = new List<Vote>();
                List<Suggestion> suggestions = new List<Suggestion>();
                try
                {
                    //get votes from database
                    votes = db.Votes.Where(x => x != null && x.VotedOn.Month == DateTime.Now.Month).ToList();
                }
                catch(Exception ex)
                { }
                try
                {
                    //get suggestions from database
                    suggestions = db.Suggestions.Where(x => x != null && x.SuggestedOn.Month == DateTime.Now.Month).ToList();
                }
                catch(Exception ex)
                { }

                List<SuggestedSnack> suggestedSnacks = new List<SuggestedSnack>();
                foreach (Snack s in snacks.Where(x => suggestions.Select(y => y.SnackId).Contains(x.Id)))
                {
                    SuggestedSnack ss = new SuggestedSnack();
                    ss.Id = s.Id;
                    ss.LastPurchaseDate = s.LastPurchaseDate;
                    ss.Name = s.Name;
                    ss.Optional = s.Optional;
                    ss.PurchaseCount = s.PurchaseCount;
                    ss.PurchaseLocations = s.PurchaseLocations;
                    ss.Votes = votes.Count(x => x.SnackId == s.Id);
                    suggestedSnacks.Add(ss);
                }
                //list of always purchased snacks
                snacks = snacks.Where(x => x.Optional == false).ToList();
                //list of suggested snacks ordered by number of votes and we only take the difference between 10 and the number of always purchased snacks
                suggestedSnacks = suggestedSnacks.OrderByDescending(x => x.Votes).Take(10 - snacks.Count()).ToList();
                foreach (var ss in suggestedSnacks)
                {
                    Snack s = new Snack();
                    s.Id = ss.Id;
                    s.PurchaseLocations = ss.PurchaseLocations;
                    s.Name = ss.Name;
                    snacks.Add(s);
                }
            }

            if (snacks != null)
            {
                return View(snacks);
            }
            else
            {
                return View(new List<Snack>());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Suggest([Bind(Include = "Id,Name,PurchaseLocations")]Snack snack)
        {
            if (ModelState.IsValid)
            {
                dynamic suggestion = new System.Dynamic.ExpandoObject();
                string addedSnack = "";
                if (!string.IsNullOrEmpty(snack.Name) && !string.IsNullOrEmpty(snack.PurchaseLocations))
                {
                    suggestion.name = snack.Name;
                    suggestion.location = snack.PurchaseLocations;
                    suggestion.lattitude = "";
                    suggestion.longitude = "";
                    addedSnack = SnacksServiceHelper.AddSnack(JsonConvert.SerializeObject(suggestion));
                    try
                    {
                        snack = JsonConvert.DeserializeObject<Snack>(addedSnack);
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "");
                        return View();
                    }
                }
                if (snack != null && snack.Id > 0)
                {
                    Suggestion s = new Suggestion();
                    s.SnackId = snack.Id;
                    s.SuggestedOn = DateTime.Now;
                    db.Suggestions.Add(s);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View();
        }

        [HttpPost]
        public ActionResult Vote(string Id)
        {
            if (ModelState.IsValid)
            {
                Vote v = new Vote();
                v.SnackId = Convert.ToInt32(Id);
                v.VotedOn = DateTime.Now;
                //Add vote to database
                try {
                    db.Votes.Add(v);
                    db.SaveChanges();
                }
                catch(Exception ex)
                {
                }
                var redirectUrl = new UrlHelper(Request.RequestContext).Action("Index", "Home");
                return Json(new { Url = redirectUrl });
            }
            return View();
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