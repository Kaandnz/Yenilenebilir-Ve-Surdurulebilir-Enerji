using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class AdminTopicsController : Controller
    {
        private readonly ITopicRepository topicRepository;

        public AdminTopicsController(ITopicRepository topicRepository)
        {
            this.topicRepository = topicRepository;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddTopicRequest model)
        {
            if (!ModelState.IsValid) return View(model);

            var topic = new Topic
            {
                Id = Guid.NewGuid(),
                DisplayNameEn = model.DisplayNameEn,
                DisplayNameTr = model.DisplayNameTr,
                FeaturedImageUrl = model.FeaturedImageUrl,
                TopicDetailsEn = model.TopicDetailsEn,
                TopicDetailsTr = model.TopicDetailsTr
                
            };

            await topicRepository.AddAsync(topic);
            return RedirectToAction("List");
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var topics = await topicRepository.GetAllAsync();
            return View(topics);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var topic = await topicRepository.GetAsync(id);
            if (topic == null) return View(null);

            var editModel = new EditTopicRequest
            {
                Id = topic.Id,
                DisplayNameEn = topic.DisplayNameEn,
                DisplayNameTr = topic.DisplayNameTr,
                FeaturedImageUrl = topic.FeaturedImageUrl,
                TopicDetailsEn = topic.TopicDetailsEn,
                TopicDetailsTr = topic.TopicDetailsTr
            };

            return View(editModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditTopicRequest model)
        {
            if (!ModelState.IsValid) return View(model);

            var topic = new Topic
            {
                Id = model.Id,
                DisplayNameEn = model.DisplayNameEn,
                DisplayNameTr = model.DisplayNameTr,
                FeaturedImageUrl = model.FeaturedImageUrl,
                TopicDetailsEn = model.TopicDetailsEn,
                TopicDetailsTr = model.TopicDetailsTr
            };

            var updated = await topicRepository.UpdateAsync(topic);
            if (updated == null)
            {
                return RedirectToAction("Edit", new { id = model.Id });
            }

            return RedirectToAction("List");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EditTopicRequest model)
        {
            var deleted = await topicRepository.DeleteAsync(model.Id);
            if (deleted == null)
            {
                return RedirectToAction("Edit", new { id = model.Id });
            }

            return RedirectToAction("List");
        }
    }
}
