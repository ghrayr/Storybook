using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public class StoryController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetStories([Bind(Include = "page")] int page = 1)
        {
            const int pageSize = 15;
            return PartialView("_StoriesPartial", StoryManager.GetStories(User.Identity.GetUserId<int>(), page, pageSize));
        }

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Story story = await StoryManager.FindAsync(id.Value);
            if (story == null)
                return HttpNotFound();

            return View(story);
        }

        public ActionResult Create()
        {
            ViewBag.Groups = new MultiSelectList(GroupManager.GetAllGroups(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Save([Bind(Include = "Id,Title,Description,Content,GroupIds")] Story story)
        {
            story.PostedOn = DateTime.Now;
            story.UserId = User.Identity.GetUserId<int>();

            if (ModelState.IsValid)
            {
                await StoryManager.SaveStoryAsync(story);
                return RedirectToAction("Index");
            }

            ViewBag.Groups = new MultiSelectList(await GroupManager.GetAllGroupsAsync(), "Id", "Name");

            return story.Id > 0 ? View("Edit", story) : View("Create", story);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Story story = await StoryManager.FindAsync(id.Value, true);
            if (story == null)
                return HttpNotFound();

            ViewBag.Groups = new MultiSelectList(GroupManager.GetAllGroups(), "Id", "Name", story.Groups.Select(x => x.Id).ToList());
            return View(story);
        }
    }
}
