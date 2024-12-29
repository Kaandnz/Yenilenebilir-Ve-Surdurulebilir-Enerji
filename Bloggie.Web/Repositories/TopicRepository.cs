using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Repositories
{
    public class TopicRepository : ITopicRepository
    {
        private readonly BloggieDbContext db;

        public TopicRepository(BloggieDbContext db)
        {
            this.db = db;
        }

        public async Task<Topic> AddAsync(Topic topic)
        {
            await db.Topics.AddAsync(topic);
            await db.SaveChangesAsync();
            return topic;
        }

        public async Task<Topic?> GetAsync(Guid id)
        {
            return await db.Topics.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<Topic>> GetAllAsync()
        {
            return await db.Topics.ToListAsync();
        }

        public async Task<Topic?> UpdateAsync(Topic topic)
        {
            var existing = await db.Topics.FindAsync(topic.Id);
            if (existing != null)
            {
                existing.DisplayNameEn = topic.DisplayNameEn;
                existing.DisplayNameTr = topic.DisplayNameTr;
                existing.FeaturedImageUrl = topic.FeaturedImageUrl;
                existing.TopicDetailsTr = topic.TopicDetailsTr;
                existing.TopicDetailsEn = topic.TopicDetailsEn;

                await db.SaveChangesAsync();
                return existing;
            }
            return null;
        }

        public async Task<Topic?> DeleteAsync(Guid id)
        {
            var existing = await db.Topics.FindAsync(id);
            if (existing != null)
            {
                db.Topics.Remove(existing);
                await db.SaveChangesAsync();
                return existing;
            }
            return null;
        }
    }
}
