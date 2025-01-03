﻿using Bloggie.Web.Models.Domain;

namespace Bloggie.Web.Repositories
{
    public interface IBlogPostRepository
    {
        Task<IEnumerable<BlogPost>> GetAllAsync();

        Task<BlogPost?> GetAsync(Guid id);

        Task<BlogPost?> GetByUrlHandleAsync(string urlHandle);

        Task<BlogPost> AddAsync(BlogPost blogPost);
        Task<IEnumerable<BlogPost>> GetByTagAsync(string tagName);

        Task<BlogPost?> UpdateAsync(BlogPost blogPost);
        Task<IEnumerable<BlogPost>> GetPostsByTopicAsync(Guid topicId, string lang);
        Task<BlogPost?> DeleteAsync(Guid id);
    }
}
