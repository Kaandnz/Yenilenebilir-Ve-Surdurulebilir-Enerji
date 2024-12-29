using System;
using System.ComponentModel.DataAnnotations;

namespace Bloggie.Web.Models.ViewModels
{
    public class EditTopicRequest
    {
        public Guid Id { get; set; }

        [Required]
        public string DisplayNameEn { get; set; }

        [Required]
        public string DisplayNameTr { get; set; }

        public string FeaturedImageUrl { get; set; }
        public string TopicDetailsTr {  get; set; }
        public string TopicDetailsEn {  get; set; }
    }
}
