using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication5.Models;

namespace WebApplication5.Controllers
{
    public class TagsController : Controller
    {
        private BlogDbContext db = new BlogDbContext();

        // GET: Tags
        public ActionResult Index()
        {
            var tags = db.Tags.Include(t => t.Blog);
            return View(tags.ToList());
        }

        public ActionResult BlogsOftag(string tagName)
        {
            List<Blog> BlogsWithtag = new List<Blog>();

            foreach(var blog in db.Blogs)
            {
                foreach(var BlogTag in blog.Tags)
                {
                    if (BlogTag.Name == tagName)
                    {
                        BlogsWithtag.Add(blog);
                    }
                }
            }
            return View(BlogsWithtag);
        }

        public ActionResult NumberOfBlogs()
        {
            Dictionary<string, int> TagCounts = new Dictionary<string, int>();
            var DistinctTags = db.Tags.Select(x => x.Name).Distinct().ToList();
            foreach(string tagName in DistinctTags)
            {
                int counter = 0;
                foreach(var blog in db.Blogs)
                {
                    if(blog.Tags.Any(x => x.Name == tagName))
                    {
                        counter++;
                    }
                }
                if (counter > 0)
                {
                    TagCounts.Add(tagName, counter);
                }
            }

            ViewBag.Tags = TagCounts;
            return View();
        }

        public ActionResult MostPopularTag()
        {
            Dictionary<string, int> TagCounts = new Dictionary<string, int>();
            var DistinctTags = db.Tags.Select(x => x.Name).Distinct().ToList();
            foreach (string tagName in DistinctTags)
            {
                int counter = 0;
                foreach (var blog in db.Blogs)
                {
                    if (blog.Tags.Any(x => x.Name == tagName))
                    {
                        counter++;
                    }
                }
                if (counter > 0)
                {
                    TagCounts.Add(tagName, counter);
                }
            }

            string mostPopular = TagCounts.OrderByDescending(x => x.Value).First().Key;
            return View(db.Blogs.Where(x => x.Tags.Any(t => t.Name == mostPopular)));
        }

        // GET: Tags/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tag tag = db.Tags.Find(id);
            if (tag == null)
            {
                return HttpNotFound();
            }
            return View(tag);
        }

        // GET: Tags/Create
        public ActionResult Create()
        {
            ViewBag.BlogId = new SelectList(db.Blogs, "Id", "Title");
            return View();
        }

        // POST: Tags/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,BlogId")] Tag tag)
        {
            if (ModelState.IsValid || !db.Tags.Any(x => x.Name == tag.Name))
            {
                db.Tags.Add(tag);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BlogId = new SelectList(db.Blogs, "Id", "Title", tag.BlogId);
            return View(tag);
        }

        // GET: Tags/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tag tag = db.Tags.Find(id);
            if (tag == null)
            {
                return HttpNotFound();
            }
            ViewBag.BlogId = new SelectList(db.Blogs, "Id", "Title", tag.BlogId);
            return View(tag);
        }

        // POST: Tags/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,BlogId")] Tag tag)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tag).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BlogId = new SelectList(db.Blogs, "Id", "Title", tag.BlogId);
            return View(tag);
        }

        // GET: Tags/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tag tag = db.Tags.Find(id);
            if (tag == null)
            {
                return HttpNotFound();
            }
            return View(tag);
        }

        // POST: Tags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tag tag = db.Tags.Find(id);
            db.Tags.Remove(tag);
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
