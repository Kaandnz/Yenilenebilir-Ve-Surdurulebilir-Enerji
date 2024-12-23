using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Repositories
{
    public class HomePageRepository : IHomePageRepository
    {
        private readonly BloggieDbContext db;

        public HomePageRepository(BloggieDbContext db)
        {
            this.db = db;
        }

        // Carousel Methods
        public async Task<IEnumerable<HomePageCarouselImage>> GetAllCarouselImagesAsync()
        {
            return await db.HomePageCarouselImages
                .OrderBy(x => x.DisplayOrder)
                .ToListAsync();
        }

        public async Task<HomePageCarouselImage> AddCarouselImageAsync(HomePageCarouselImage image)
        {
            await db.HomePageCarouselImages.AddAsync(image);
            await db.SaveChangesAsync();
            return image;
        }

        public async Task<HomePageCarouselImage?> UpdateCarouselImageAsync(HomePageCarouselImage image)
        {
            var existing = await db.HomePageCarouselImages.FindAsync(image.Id);
            if (existing != null)
            {
                existing.ImageUrl = image.ImageUrl;
                existing.DisplayOrder = image.DisplayOrder;
                await db.SaveChangesAsync();
                return existing;
            }
            return null;
        }

        public async Task<HomePageCarouselImage?> DeleteCarouselImageAsync(Guid id)
        {
            var existing = await db.HomePageCarouselImages.FindAsync(id);
            if (existing != null)
            {
                db.HomePageCarouselImages.Remove(existing);
                await db.SaveChangesAsync();
                return existing;
            }
            return null;
        }

        // WYSIWYG
        public async Task<HomePageSetting?> GetHomePageSettingAsync()
        {
            // Tek kaydı kullanacağımızı varsayıyoruz. 
            // Gerekirse FirstOrDefaultAsync vb. de kullanılabilir.
            return await db.HomePageSettings.FirstOrDefaultAsync();
        }

        public async Task<HomePageSetting> UpdateHomePageSettingAsync(HomePageSetting setting)
        {
            // Tek satır mantığı: Mevcut kaydı bul, güncelle.
            var existing = await db.HomePageSettings.FindAsync(setting.Id);
            if (existing != null)
            {
                existing.BodyHtml = setting.BodyHtml;
                await db.SaveChangesAsync();
                return existing;
            }
            else
            {
                // Hiç yoksa ekle
                await db.HomePageSettings.AddAsync(setting);
                await db.SaveChangesAsync();
                return setting;
            }
        }
    }
}
