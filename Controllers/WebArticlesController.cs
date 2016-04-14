using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Refma5neo.Models;

namespace Refma5neo.Controllers
{
    public class WebArticlesController : Controller
    {
        // GET: WebArticles
        public ActionResult Index()
        {
            string userId = User.Identity.GetUserId();
            if (userId != null)
            {
                using (GraphDbContext db = new GraphDbContext())
                {
                    //  ViewBag.LangCode = currentUser.TargetLang.Code;
                
                    var articles = db.getWebArticles(userId);

                    return View(articles);
                }
            }
            return View();// show nothing  return View();
        }

        // GET: WebArticles/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: WebArticles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: WebArticles/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: WebArticles/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: WebArticles/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: WebArticles/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: WebArticles/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
