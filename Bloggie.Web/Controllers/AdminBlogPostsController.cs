using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace Bloggie.Web.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class AdminBlogPostsController : Controller
    {
        private readonly IBlogPostRepository blogPostRepository;
        private readonly ITopicRepository topicRepository;
        private readonly ITagRepository tagRepository;

        public AdminBlogPostsController(
            IBlogPostRepository blogPostRepository,
            ITopicRepository topicRepository,
            ITagRepository tagRepository)
        {
            this.blogPostRepository = blogPostRepository;
            this.topicRepository = topicRepository;
            this.tagRepository = tagRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            // Topic ve Tag listesini çek, formda göster
            var topics = await topicRepository.GetAllAsync();
            var tags = await tagRepository.GetAllAsync();

            var model = new AddBlogPostRequest
            {
                Topics = topics.Select(t => new SelectListItem { Text = t.DisplayNameEn, Value = t.Id.ToString() }),
                Tags = tags.Select(tag => new SelectListItem { Text = tag.Name, Value = tag.Id.ToString() })
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(AddBlogPostRequest addBlogPostRequest)
        {
            if (!ModelState.IsValid)
            {
                // ModelState hatalarını logla
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine("Validation error: " + error.ErrorMessage);
                }

                // Formu yeniden doldur
                var topics = await topicRepository.GetAllAsync();
                var tags = await tagRepository.GetAllAsync();
                addBlogPostRequest.Topics = topics.Select(t => new SelectListItem { Text = t.DisplayNameEn, Value = t.Id.ToString() });
                addBlogPostRequest.Tags = tags.Select(tag => new SelectListItem { Text = tag.Name, Value = tag.Id.ToString() });

                return View(addBlogPostRequest);
            }

            // Tag’ları yok sayıyoruz, blogPost’a dahil etmiyoruz.
            var blogPost = new BlogPost
            {
                Heading = addBlogPostRequest.Heading,
                PageTitle = addBlogPostRequest.PageTitle,
                Content = addBlogPostRequest.Content,
                ShortDescription = addBlogPostRequest.ShortDescription,
                FeaturedImageUrl = addBlogPostRequest.FeaturedImageUrl,
                UrlHandle = addBlogPostRequest.UrlHandle,
                PublishedDate = addBlogPostRequest.PublishedDate,
                Author = addBlogPostRequest.Author,
                Visible = addBlogPostRequest.Visible,
                TopicId = addBlogPostRequest.SelectedTopicId,
                Lang = addBlogPostRequest.Lang
            };

            await blogPostRepository.AddAsync(blogPost);
            return RedirectToAction("List");
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var blogPosts = await blogPostRepository.GetAllAsync();
            return View(blogPosts);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var blogPost = await blogPostRepository.GetAsync(id);
            if (blogPost == null)
            {
                return View(null);
            }

            var topics = await topicRepository.GetAllAsync();
            var tags = await tagRepository.GetAllAsync();

            var model = new EditBlogPostRequest
            {
                Id = blogPost.Id,
                Heading = blogPost.Heading,
                PageTitle = blogPost.PageTitle,
                Content = blogPost.Content,
                ShortDescription = blogPost.ShortDescription,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                UrlHandle = blogPost.UrlHandle,
                PublishedDate = blogPost.PublishedDate,
                Author = blogPost.Author,
                Visible = blogPost.Visible,

                SelectedTopicId = blogPost.TopicId,
                Topics = topics.Select(t => new SelectListItem
                {
                    Text = t.DisplayNameEn,
                    Value = t.Id.ToString(),
                    Selected = (t.Id == blogPost.TopicId)
                }),
                Lang = blogPost.Lang,


                Tags = tags.Select(tag => new SelectListItem { Text = tag.Name, Value = tag.Id.ToString() }),
                
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditBlogPostRequest editBlogPostRequest)
        {
            if (!ModelState.IsValid)
            {
                // ModelState hatalarını logla ve formu yeniden doldur
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine("Validation error: " + error.ErrorMessage);
                }

                // Topics ve Tags'i yeniden doldur
                var topics = await topicRepository.GetAllAsync();
                var tags = await tagRepository.GetAllAsync();

                editBlogPostRequest.Topics = topics.Select(t => new SelectListItem
                {
                    Text = t.DisplayNameEn,
                    Value = t.Id.ToString(),
                    Selected = (t.Id == editBlogPostRequest.SelectedTopicId)
                });

                editBlogPostRequest.Tags = tags.Select(tag => new SelectListItem { Text = tag.Name, Value = tag.Id.ToString() });

                return View(editBlogPostRequest);
            }

            // Var olan blog post'u veritabanından çek
            var existingBlogPost = await blogPostRepository.GetAsync(editBlogPostRequest.Id);
            if (existingBlogPost == null)
            {
                return NotFound();
            }

            // Özellikleri güncelle
            existingBlogPost.Heading = editBlogPostRequest.Heading;
            existingBlogPost.PageTitle = editBlogPostRequest.PageTitle;
            existingBlogPost.Content = editBlogPostRequest.Content;
            existingBlogPost.ShortDescription = editBlogPostRequest.ShortDescription;
            existingBlogPost.FeaturedImageUrl = editBlogPostRequest.FeaturedImageUrl;
            existingBlogPost.UrlHandle = editBlogPostRequest.UrlHandle;
            existingBlogPost.PublishedDate = editBlogPostRequest.PublishedDate;
            existingBlogPost.Author = editBlogPostRequest.Author;
            existingBlogPost.Visible = editBlogPostRequest.Visible;
            existingBlogPost.TopicId = editBlogPostRequest.SelectedTopicId;
            existingBlogPost.Lang = editBlogPostRequest.Lang;

            // Güncellemeyi kaydet
            await blogPostRepository.UpdateAsync(existingBlogPost);

            return RedirectToAction("List");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EditBlogPostRequest editBlogPostRequest)
        {
            // Talk to repository to delete this blog post and tags
            var deletedBlogPost = await blogPostRepository.DeleteAsync(editBlogPostRequest.Id);

            if (deletedBlogPost != null)
            {
                // Show success notification
                return RedirectToAction("List");
            }

            // Show error notification
            return RedirectToAction("Edit", new { id = editBlogPostRequest.Id });
        }

        


    }
}
