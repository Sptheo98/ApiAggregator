using System;

namespace ApiAggregator.Models
{
    // represents the JSON response structure(together with Pagination) about the field topic from external API
    public class MediastackArticle
    {
        public string Title { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string Source { get; set; } = string.Empty;
        public string published_at { get; set; } = string.Empty;
    }
}