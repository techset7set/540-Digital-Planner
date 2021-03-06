﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Digital_Planner.Models;

namespace Digital_Planner.Controllers
{
    public class EventsController : Controller
    {
        private calendarEntities db = new calendarEntities();

        // GET: Events
        public ActionResult Index()
        {
            var events = db.Events.Include(e => e.Category).Include(e => e.User);
            return View(events.ToList());
        }

        // GET: Events/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // GET: Events/Create
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Description");
            ViewBag.UserID = new SelectList(db.Users, "ID", "FirstName");
            return View(new Event());
        }

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Title,OccursAt,Duration,Priority,CompleteBy,IsComplete,Location,UserID,CategoryID")] Event @event, int recurrence)
        {
            if (ModelState.IsValid)
            {
                db.Events.Add(@event);
                db.SaveChanges();

                if (recurrence > 24)
                {
                    recurrence = 24;
                }
                var Occurs = @event.OccursAt;
                var Complete = @event.CompleteBy;
                while (recurrence > 0)
                {
                    var re_event = new Event();
                    Occurs = Occurs.AddDays(7);
                    Complete = Complete.AddDays(7);

                    re_event.OccursAt = Occurs;
                    re_event.CompleteBy = Complete;
                    re_event.IsComplete = @event.IsComplete;
                    re_event.Priority = @event.Priority;
                    re_event.Title = @event.Title;
                    re_event.UserID = @event.UserID;
                    re_event.CategoryID = @event.CategoryID;
                    re_event.Location = @event.Location;
                    re_event.AutoAssign = @event.AutoAssign;
                    re_event.Duration = @event.Duration;
                 
                    db.Events.Add(re_event);
                    db.SaveChanges();

                    recurrence--;
                }
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Description", @event.CategoryID);
            ViewBag.UserID = new SelectList(db.Users, "ID", "FirstName", @event.UserID);
            return View(@event);
        }

        // GET: Events/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Description", @event.CategoryID);
            ViewBag.UserID = new SelectList(db.Users, "ID", "FirstName", @event.UserID);
            return View(@event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Title,OccursAt,Duration,Priority,CompleteBy,IsComplete,Location,UserID,CategoryID")] Event @event)
        {
            if (ModelState.IsValid)
            {
                db.Entry(@event).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Description", @event.CategoryID);
            ViewBag.UserID = new SelectList(db.Users, "ID", "FirstName", @event.UserID);
            return View(@event);
        }

        // POST: Events/5
        // Changes the attribute of an event to the specified value
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ToggleCompletion(int? id)//[Bind(Include = "ID,Title,OccursAt,Duration,Priority,CompleteBy,IsComplete,Location,UserID,CategoryID")] Event @event)
        {
            if (id != null)
            {
                var evt = db.Events.Where(e => e.ID == id);
                //Shouldn't be more than one, but just to make sure....
                foreach(var item in evt)
                {
                    item.IsComplete = !item.IsComplete;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index");
            /*
            if (ModelState.IsValid)
            {
                //db.Entry(@event).State = EntityState.Modified;
                db.Entry(@event).Entity.IsComplete = !db.Entry(@event).Entity.IsComplete;
                db.SaveChanges();
                //Response.Redirect(Request.UrlReferrer.ToString());
                return RedirectToAction("Index");
            }
            //ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Description", @event.CategoryID);
            //ViewBag.UserID = new SelectList(db.Users, "ID", "FirstName", @event.UserID);
            return View(@event);
            */
        }

        // GET: Events/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Event @event = db.Events.Find(id);
            db.Events.Remove(@event);
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
