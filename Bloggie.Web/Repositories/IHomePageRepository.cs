using Bloggie.Web.Models.Domain;

namespace Bloggie.Web.Repositories
{
    public interface IHomePageRepository
    {
        // Carousel Images
        Task<IEnumerable<HomePageCarouselImage>> GetAllCarouselImagesAsync();
        Task<HomePageCarouselImage> AddCarouselImageAsync(HomePageCarouselImage image);
        Task<HomePageCarouselImage?> UpdateCarouselImageAsync(HomePageCarouselImage image);
        Task<HomePageCarouselImage?> DeleteCarouselImageAsync(Guid id);

        // Home Page WYSIWYG
        Task<HomePageSetting?> GetHomePageSettingAsync();
        Task<HomePageSetting> UpdateHomePageSettingAsync(HomePageSetting setting);
    }
}
