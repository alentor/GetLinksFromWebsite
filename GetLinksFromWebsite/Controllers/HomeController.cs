using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using GetLinksFromWebsite.Models;
using HtmlAgilityPack;

namespace GetLinksFromWebsite.Controllers {
    public class HomeController: Controller {
        // GET: Home
        public ActionResult Index() {
            return View();
        }

        // Post: Home
        [HttpPost]
        public PartialViewResult Index(IndexViewModel model) {
            //WebClientExtended client = new WebClientExtended(); 
            //HtmlDocument doc = new HtmlDocument(); 
            //doc.LoadHtml(System.Text.Encoding.UTF8.GetString(client.DownloadData(model.WebsteiUrl)));

            //HtmlWeb hw = new HtmlWeb();
            //HtmlDocument doc = hw.Load(model.WebsteiUrl);

            ////List <List <string>> linksList = new List <List <string>>();
            ////foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//a[@href]")) {
            ////    List <string> innerList = new List <string>();
            ////    string hrefValue = link.GetAttributeValue("href", string.Empty);
            ////    HtmlDocument innerDoc = hw.Load(model.WebsteiUrl + hrefValue);
            ////    innerList.Add(model.WebsteiUrl + hrefValue);
            ////    for (int i = 0; i < 1; i++) {
            ////        HtmlNode innerLink = innerDoc.DocumentNode.SelectNodes("//a[@href]")[i];
            ////        string hrefValueInner = innerLink.GetAttributeValue("href", string.Empty);
            ////        innerList.Add(model.WebsteiUrl + hrefValueInner);
            ////    }
            ////    linksList.Add(innerList);
            ////}
            ////return PartialView("HomeIndexPartialView", linksList);

            ////for (int index = 0; index < doc.DocumentNode.SelectNodes("//a[@href]").Count; index++)
            ////{
            ////    HtmlNode link = doc.DocumentNode.SelectNodes("//a[@href]")[index];
            ////    string hrefValue = link.GetAttributeValue("href", string.Empty);

            ////    links.Add(model.WebsteiUrl + hrefValue);
            ////}

            //List <string> links = new List <string>();
            //foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//a[@href]")) {
            //    string hrefValue = link.GetAttributeValue("href", string.Empty);

            //    links.Add(model.WebsteiUrl + hrefValue);
            //}
            //return PartialView("HomeIndexPartialView", links);

            ////return Json(model);

            ////string linksString = string.Join(",", links.ToArray());
            ////return Content(linksString);
            //////return Json(linksString, JsonRequestBehavior.AllowGet);

            ////return View();
            /// 
            List <List <string>> linksList = GetLinks(model.WebsteiUrl, model.DepthWebsiteUrls);
            return PartialView("HomeIndexPartialView", linksList);
        }

        public List <List <string>> GetLinks(string url, string depth) {
            HtmlWeb htmlWeb = new HtmlWeb();
            HtmlDocument htmlDocument = htmlWeb.Load(url);
            List <List <string>> linksList = new List <List <string>>();

            if (depth != null) {
                int num;
                if (int.TryParse(depth, out num)) {
                    int intDepth = int.Parse(depth);
                    foreach (HtmlNode masterLink in htmlDocument.DocumentNode.SelectNodes("//a[@href]")) {
                        List <string> linkList = new List <string>();
                        string hrefValueMasterLink = masterLink.GetAttributeValue("href", string.Empty);
                        HtmlDocument innerHtmlDocument = htmlWeb.Load(url + hrefValueMasterLink);
                        linkList.Add(url + hrefValueMasterLink);
                        for (int i = 0; i < intDepth; i++) {
                            HtmlNode innerLink = innerHtmlDocument.DocumentNode.SelectNodes("//a[@href]")[i];
                            string hrefValueInner = innerLink.GetAttributeValue("href", string.Empty);
                            linkList.Add(url + hrefValueInner);
                        }
                        linksList.Add(linkList);
                    }
                    return linksList;
                }
            }

            foreach (HtmlNode masterLink in htmlDocument.DocumentNode.SelectNodes("//a[@href]")) {
                List <string> linkList = new List <string>();
                string hrefValueMasterLink = masterLink.GetAttributeValue("href", string.Empty);
                HtmlDocument innerHtmlDocument = htmlWeb.Load(url + hrefValueMasterLink);
                linkList.Add(url + hrefValueMasterLink);
                for (int i = 0; i < 5; i++) {
                    HtmlNode innerLink = innerHtmlDocument.DocumentNode.SelectNodes("//a[@href]")[i];
                    string hrefValueInner = innerLink.GetAttributeValue("href", string.Empty);
                    linkList.Add(url + hrefValueInner);
                }
                linksList.Add(linkList);
            }
            return linksList;
        }
    }
}