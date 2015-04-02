using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Storybook.DataModel.Models;
using Storybook.DAL.Managers;

namespace Storybook.Controllers
{
    [Authorize]
    public class GroupController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetGroups([Bind(Include = "page")] int page = 1)
        {
            const int pageSize = 14;
            return PartialView("_GroupsPartial", GroupManager.GetGroups(page, pageSize));
        }

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Group group = await GroupManager.FindAsync(id.Value);
            if (group == null)
                return HttpNotFound();

            return View(group);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Description")] Group group)
        {
            if (ModelState.IsValid)
            {
                await GroupManager.SaveGroupAsync(group);
                return RedirectToAction("Index");
            }

            return View(group);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Group group = await GroupManager.FindAsync(id.Value);
            if (group == null)
                return HttpNotFound();

            return View(group);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Description")] Group group)
        {
            if (ModelState.IsValid)
            {
                await GroupManager.SaveGroupAsync(group);
                return RedirectToAction("Index");
            }
            return View(group);
        }

        [HttpPost]
        public async Task<ActionResult> Join([Bind(Include = "id,page")]int id, int page)
        {
            await GroupManager.JoinAsync(id, User.Identity.GetUserId<int>());
            return GetGroups(page);
        }
    }
}