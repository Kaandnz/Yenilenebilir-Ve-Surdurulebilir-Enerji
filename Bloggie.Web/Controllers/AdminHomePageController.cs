using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminHomePageController : Controller
    {
        private readonly IHomePageRepository homePageRepo;
        private readonly IImageRespository imageRepo;

        public AdminHomePageController(IHomePageRepository homePageRepo,
                                       IImageRespository imageRepo)
        {
            this.homePageRepo = homePageRepo;
            this.imageRepo = imageRepo;
        }

        // 1) Carousel List
        [HttpGet]
        public async Task<IActionResult> CarouselList()
        {
            var images = await homePageRepo.GetAllCarouselImagesAsync();
            return View(images); 
        }

        
        [HttpGet]
        public IActionResult CarouselAdd()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CarouselAdd(List<IFormFile> files)
        {
          
            if (files != null && files.Count > 0)
            {
              
                var allExisting = await homePageRepo.GetAllCarouselImagesAsync();
                int nextOrder = (allExisting.Any())
                                ? allExisting.Max(x => x.DisplayOrder) + 1
                                : 1;

                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        var url = await imageRepo.UploadAsync(file);

                        var newImage = new HomePageCarouselImage
                        {
                            Id = Guid.NewGuid(),
                            ImageUrl = url,
                            DisplayOrder = nextOrder++
                        };
                        await homePageRepo.AddCarouselImageAsync(newImage);
                    }
                }
            }
            return RedirectToAction("CarouselList");
        }


        [HttpGet]
        public async Task<IActionResult> CarouselEdit(Guid id)
        {
            var existing = (await homePageRepo.GetAllCarouselImagesAsync())
                           .FirstOrDefault(x => x.Id == id);
            if (existing == null) return NotFound();

            var vm = new EditCarouselImageRequest
            {
                Id = existing.Id,
                DisplayOrder = existing.DisplayOrder,
                CurrentImageUrl = existing.ImageUrl
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> CarouselEdit(EditCarouselImageRequest model, IFormFile? file)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var imageUrl = model.CurrentImageUrl;
            if (file != null && file.Length > 0)
            {
                imageUrl = await imageRepo.UploadAsync(file);
            }

            var updated = await homePageRepo.UpdateCarouselImageAsync(new HomePageCarouselImage
            {
                Id = model.Id,
                DisplayOrder = model.DisplayOrder,
                ImageUrl = imageUrl
            });

            if (updated == null)
                return RedirectToAction("CarouselEdit", new { id = model.Id });

            return RedirectToAction("CarouselList");
        }


        [HttpPost]
        public async Task<IActionResult> CarouselDelete(Guid id)
        {
            await homePageRepo.DeleteCarouselImageAsync(id);
            return RedirectToAction("CarouselList");
        }

        [HttpGet]
        public async Task<IActionResult> EditBody()
        {
            var setting = await homePageRepo.GetHomePageSettingAsync();
            if (setting == null)
            {
      
                setting = new HomePageSetting
                {
                    Id = Guid.NewGuid(),
                    BodyHtml = ""
                };
            }

            var vm = new EditHomePageBodyRequest
            {
                Id = setting.Id,
                BodyHtml = setting.BodyHtml
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> EditBody(EditHomePageBodyRequest model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var updated = await homePageRepo.UpdateHomePageSettingAsync(new HomePageSetting
            {
                Id = model.Id,
                BodyHtml = model.BodyHtml
            });


            return RedirectToAction("EditBody");
        }
    }
}
