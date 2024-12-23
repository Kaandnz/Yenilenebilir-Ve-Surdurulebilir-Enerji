namespace Bloggie.Web.Models.ViewModels
{
    public class EditCarouselImageRequest
    {
        public Guid Id { get; set; }
        public int DisplayOrder { get; set; }
        public string CurrentImageUrl { get; set; }
    }
}
