using System;
using System.Collections.Generic;

namespace Bloggie.Web.Models.Domain
{
    public class Topic
    {
        public Guid Id { get; set; }

        // İki dilde isim: İngilizce ve Türkçe
        public string DisplayNameEn { get; set; }
        public string DisplayNameTr { get; set; }

        // Topic sayfasının üstünde göstereceğimiz arkaplan/fon resmi (opsiyonel)
        public string? FeaturedImageUrl { get; set; }
        public string? TopicDetailsTr {  get; set; }
        public string? TopicDetailsEn { get; set; }

        // Bu Topic'e ait Blog Post koleksiyonu
        public ICollection<BlogPost> BlogPosts { get; set; }
    }
}
