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
                TopicId = addBlogPostRequest.SelectedTopicId
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

            // Seçili tag ID'lerini alıyoruz
            var selectedTagIds = new HashSet<Guid>(blogPost.Tags.Select(t => t.Id));

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

                // Tags listesi ve seçili tag'lar
                Tags = tags.Select(tag => new SelectListItem
                {
                    Text = tag.Name,
                    Value = tag.Id.ToString(),
                    Selected = selectedTagIds.Contains(tag.Id) // Doğru karşılaştırma
                })
            };

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditBlogPostRequest editBlogPostRequest)
        {
            if (!ModelState.IsValid)
            {
                // ModelState hatalarını logla
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine("Validation error: " + error.ErrorMessage);
                }

                // Topics ve Tags'i yeniden doldur
                var topics = await topicRepository.GetAllAsync();
                var tags = await tagRepository.GetAllAsync();

                // Seçili tag ID'lerini tekrar al
                var selectedTagIds = new HashSet<Guid>(blogPostRepository.GetAsync(editBlogPostRequest.Id).Result.Tags.Select(t => t.Id));

                editBlogPostRequest.Topics = topics.Select(t => new SelectListItem
                {
                    Text = t.DisplayNameEn,
                    Value = t.Id.ToString(),
                    Selected = (t.Id == editBlogPostRequest.SelectedTopicId)
                });

                editBlogPostRequest.Tags = tags.Select(tag => new SelectListItem
                {
                    Text = tag.Name,
                    Value = tag.Id.ToString(),
                    Selected = selectedTagIds.Contains(tag.Id)
                });

                return View(editBlogPostRequest);
            }

            // Tag’ları yok sayıyoruz, blogPost’a dahil etmiyoruz.
            var blogPostDomainModel = new BlogPost
            {
                Id = editBlogPostRequest.Id,
                Heading = editBlogPostRequest.Heading,
                PageTitle = editBlogPostRequest.PageTitle,
                Content = editBlogPostRequest.Content,
                Author = editBlogPostRequest.Author,
                ShortDescription = editBlogPostRequest.ShortDescription,
                FeaturedImageUrl = editBlogPostRequest.FeaturedImageUrl,
                PublishedDate = editBlogPostRequest.PublishedDate,
                UrlHandle = editBlogPostRequest.UrlHandle,
                Visible = editBlogPostRequest.Visible,
                TopicId = editBlogPostRequest.SelectedTopicId
            };

            var updatedBlog = await blogPostRepository.UpdateAsync(blogPostDomainModel);

            if (updatedBlog == null)
            {
                return RedirectToAction("Edit", new { id = editBlogPostRequest.Id });
            }

            return RedirectToAction("List");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(EditBlogPostRequest editBlogPostRequest)
        {
            var deletedBlogPost = await blogPostRepository.DeleteAsync(editBlogPostRequest.Id);

            if (deletedBlogPost != null)
            {
                return RedirectToAction("List");
            }

            return RedirectToAction("Edit", new { id = editBlogPostRequest.Id });
        }
    }
}
