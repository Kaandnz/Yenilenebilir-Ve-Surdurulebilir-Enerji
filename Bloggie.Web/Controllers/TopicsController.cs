

using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers
{
    public class TopicsController : Controller
    {
        private readonly ITopicRepository topicRepository;
        private readonly IBlogPostRepository blogPostRepository;

        public TopicsController(ITopicRepository topicRepository, IBlogPostRepository blogPostRepository)
        {
            this.topicRepository = topicRepository;
            this.blogPostRepository = blogPostRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var topic = await topicRepository.GetAsync(id);
            if (topic == null)
                return NotFound();

            var allPosts = await blogPostRepository.GetAllAsync();
            var relatedPosts = allPosts.Where(bp => bp.TopicId == id).ToList();

            ViewBag.Topic = topic;
            return View(relatedPosts);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(EditTopicRequest editTopicRequest)
        {
            // Talk to repository to delete this blog post and tags
            var deletedTopic = await topicRepository.DeleteAsync(editTopicRequest.Id);

            if (deletedTopic != null)
            {
                // Show success notification
                return RedirectToAction("List");
            }

            // Show error notification
            return RedirectToAction("Edit", new { id = editTopicRequest.Id });
        }

    }
}
