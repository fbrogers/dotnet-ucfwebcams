using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using SDES___Webcams.Areas.admin.Models;
using SDES___Webcams.Code;
using SDES___Webcams.Models;

namespace SDES___Webcams.Areas.admin.Controllers
{
    public class camController : AuthorizeBaseController
    {
        //
        // GET: /admin/cam/

        public ActionResult Index()
        {
            var db = new IT_WebcamsEntities();
            var all = db.cams.OrderBy(x => x.camName).ToList();
            return View(all);
        }

        //
        // GET: /admin/cam/Details/5

        public ActionResult Details(int id)
        {
            var db = new IT_WebcamsEntities();
            var single = db.cams.Find(id);
            return View(single);
        }

        //
        // GET: /admin/cam/Create

        public ActionResult Create()
        {
            var model = new CamViewModel{ camInterval = 1, camJpegQuality = 60 };
            return View(model);
        } 

        //
        // POST: /admin/cam/Create

        [HttpPost]
        public ActionResult Create(CamViewModel submitted)
        {
            try
            {
                using(var db = new IT_WebcamsEntities())
                {
                    var model = CamViewModel.ModelConvert(submitted);
                    model.created = DateTime.Now;
                    db.cams.Add(model);
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            catch(Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
                return View(submitted);
            }
        }
        
        //
        // GET: /admin/cam/Edit/5
 
        public ActionResult Edit(int id)
        {
            var db = new IT_WebcamsEntities();
            var single = db.cams.Find(id);
            var viewmodel = CamViewModel.ModelConvert(single);
            return View(viewmodel);
        }

        //
        // POST: /admin/cam/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, CamViewModel submitted)
        {
            try
            {
                using (var db = new IT_WebcamsEntities())
                {
                    var model = CamViewModel.ModelConvert(submitted);
                    db.Entry(model).State = EntityState.Modified;
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            catch(Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
                return View(submitted);
            }
        }

        //
        // GET: /admin/cam/Delete/5
 
        public ActionResult Delete(int id)
        {
            var db = new IT_WebcamsEntities();
            var single = db.cams.Find(id);
            return View(single);
        }

        //
        // POST: /admin/cam/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, cam submitted)
        {
            try
            {
                using (var db = new IT_WebcamsEntities())
                {
                    var cam = db.cams.Find(id);
                    db.Entry(cam).State = EntityState.Deleted;
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult Blurred(FormCollection collection)
        {
            try
            {
                var db = new IT_WebcamsEntities();
                var single = db.cams.Find(int.Parse(collection["cam.camId"]));
                single.isBlurred = !single.isBlurred;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult Enabled(FormCollection collection)
        {
            try
            {
                var db = new IT_WebcamsEntities();
                var single = db.cams.Find(int.Parse(collection["cam.camId"]));
                single.isEnabled = !single.isEnabled;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }

        }
    }
}
