using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebImageGallery.Models
{
    public class Image
    {
        public Guid Id { get; set; }
        public string Url { get; set; }
    }
}