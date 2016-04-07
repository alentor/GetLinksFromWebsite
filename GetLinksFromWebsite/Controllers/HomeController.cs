using System.Collections.Generic;
using System.Web.Mvc;
using GetLinksFromWebsite.Models;
using HtmlAgilityPack;

namespace GetLinksFromWebsite.Controllers {
    public class HomeController: Controller {
        // GET: Home
        public ActionResult Index() {
            var model = new IndexViewModel();
            model.WebsiteUrl = "https://angular.io/docs/ts/latest/tutorial/";
            model.DepthWebsiteUrls = "5";
            return View(model);
        }

        // Post: Home
        [HttpPost]
        public PartialViewResult Index(IndexViewModel model) {
            List <List <string>> linksList = GetLinks(model.WebsiteUrl, model.DepthWebsiteUrls);
            return PartialView("HomeIndexPartialView", linksList);
        }

        public List <List <string>> GetLinks(string url, string depth) {
            var htmlWeb = new HtmlWeb();
            var htmlDocument = htmlWeb.Load(url);
            var linksList = new List <List <string>>();

            if (depth != null) {
                int num;
                if (int.TryParse(depth, out num)) {
                    var intDepth = int.Parse(depth);
                    foreach (var masterLink in htmlDocument.DocumentNode.SelectNodes("//a[@href]")) {
                        var linkList = new List <string>();
                        var hrefValueMasterLink = masterLink.GetAttributeValue("href", string.Empty);
                        var innerHtmlDocument = htmlWeb.Load(url + hrefValueMasterLink);
                        linkList.Add(url + hrefValueMasterLink);
                        if (intDepth != 0) {
                            for (var i = 0; i < intDepth; i++) {
                                var innerLink = innerHtmlDocument.DocumentNode.SelectNodes("//a[@href]")[i];
                                var hrefValueInner = innerLink.GetAttributeValue("href", string.Empty);
                                linkList.Add(url + hrefValueInner);
                            }
                        }

                        linksList.Add(linkList);
                    }
                    return linksList;
                }
            }
            var err = new List <string> {"error"};
            linksList.Add(err);
            return linksList;
        }
    }
}