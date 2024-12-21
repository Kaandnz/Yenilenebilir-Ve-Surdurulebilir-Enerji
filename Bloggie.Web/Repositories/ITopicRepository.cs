using Bloggie.Web.Models.Domain;

namespace Bloggie.Web.Repositories
{
    public interface ITopicRepository
    {
        Task<Topic> AddAsync(Topic topic);
        Task<Topic?> UpdateAsync(Topic topic);
        Task<Topic?> GetAsync(Guid id);
        Task<IEnumerable<Topic>> GetAllAsync();
        Task<Topic?> DeleteAsync(Guid id);
    }
}
