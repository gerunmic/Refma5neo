
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Refma5neo.Models;
using System.Collections.Generic;
using System.Net;

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
        public ActionResult Read(int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (GraphDbContext db = new GraphDbContext())
            {
                WebArticle webarticle = db.getWebArticle(id);
            if (webarticle == null)
                {
                    return HttpNotFound();
                }

               
                ArticleDecorator decorator = new ArticleDecorator(webarticle, User.Identity.GetUserId());
                List<ViewArticleElement> viewElements = decorator.GetAllViewElements();

      

                ViewBag.ViewElements = viewElements;

                return View(webarticle);
            }
 
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


        public JsonResult UpdateElementJSON(string value, string langcode, int knowledge)
        {
            string currentUserId = User.Identity.GetUserId();
            using (GraphDbContext db  = new GraphDbContext())
            {
                db.updateRating(value, langcode, knowledge, currentUserId);
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }
    }
}
