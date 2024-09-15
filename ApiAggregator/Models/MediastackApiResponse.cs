using System;
using System.Collections.Generic;

namespace ApiAggregator.Models
{
    // represents the JSON response structure about the field topic which returns topic related news from external API
    public class MediastackApiResponse
    {
        public Pagination ?Pagination { get; set; }
        public List<MediastackArticle> ?Data { get; set; }
    }

    // Defines the structure of the pagination data in the API response
    public class Pagination
    {
        public int Limit { get; set; } // Maximum number of articles per page
        public int Offset { get; set; } // The offset for pagination, indicating the starting point of the next page
        public int Total { get; set; } // Total number of available articles for the given topic
    }
}