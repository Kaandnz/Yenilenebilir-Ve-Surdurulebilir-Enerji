using System.ComponentModel.DataAnnotations;

namespace Bloggie.Web.Models.ViewModels
{
    public class AddTopicRequest
    {
        [Required]
        public string DisplayNameEn { get; set; }

        [Required]
        public string DisplayNameTr { get; set; }

        public string FeaturedImageUrl { get; set; }
    }
}
