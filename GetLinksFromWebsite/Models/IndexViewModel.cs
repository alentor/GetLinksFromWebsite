using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GetLinksFromWebsite.Models
{
    public class IndexViewModel
    {
        [Display(Name = "Website URL")]
        public string WebsteiUrl{get; set;}

        [Display(Name = "Depth lvl for each link")]
        public string DepthWebsiteUrls { get; set;}
    }
}