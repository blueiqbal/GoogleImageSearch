using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using System.ComponentModel;

// Model view for GoogleImageSearch App
namespace GoogleImageSearchViewModel
{
    public class RootObject
    {
        public responseData responseData { get; set; }
    }

    public class responseData
    {
        public IEnumerable<results> results { get; set; }
    }

    public class results
    {
        public string tbUrl { get; set; }
        public string contentNoFormatting { get; set; }
        public string url { get; set; }
    }

    public class ImageSearchItem
    {
        public ImageSearchItem(string thumbnailUrl, string description, string fullSizeUrl)
        {
            tbUrl = thumbnailUrl;
            contentNoFormatting = description;
            url = fullSizeUrl;

        }
        public string tbUrl { get; set; }
        public string contentNoFormatting { get; set; }
        public string url { get; set; }

    }

    public class ImagePreview
    {
        public string url { get; set; }
    }
};
