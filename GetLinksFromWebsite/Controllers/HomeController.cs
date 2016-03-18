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
                        for (var i = 0; i < intDepth; i++) {
                            var innerLink = innerHtmlDocument.DocumentNode.SelectNodes("//a[@href]")[i];
                            var hrefValueInner = innerLink.GetAttributeValue("href", string.Empty);
                            linkList.Add(url + hrefValueInner);
                        }
                        linksList.Add(linkList);
                    }
                    return linksList;
                }
            }

            foreach (var masterLink in htmlDocument.DocumentNode.SelectNodes("//a[@href]")) {
                var linkList = new List <string>();
                var hrefValueMasterLink = masterLink.GetAttributeValue("href", string.Empty);
                var innerHtmlDocument = htmlWeb.Load(url + hrefValueMasterLink);
                linkList.Add(url + hrefValueMasterLink);
                for (var i = 0; i < 5; i++) {
                    var innerLink = innerHtmlDocument.DocumentNode.SelectNodes("//a[@href]")[i];
                    var hrefValueInner = innerLink.GetAttributeValue("href", string.Empty);
                    linkList.Add(url + hrefValueInner);
                }
                linksList.Add(linkList);
            }
            return linksList;
        }

        public class WebClientExtended: WebClient {
            #region Konstruktoren

            public WebClientExtended() {
                CookieContainer = new CookieContainer();
            }

            #endregion

            #region Eigenschaften

            public CookieContainer CookieContainer{get; set;} = new CookieContainer();

            #endregion

            #region Felder

            #endregion

            #region Methoden

            protected override WebRequest GetWebRequest(Uri address) {
                var r = base.GetWebRequest(address);
                var request = r as HttpWebRequest;
                request.AllowAutoRedirect = false;
                request.ServicePoint.Expect100Continue = false;
                if (request != null) {
                    request.CookieContainer = CookieContainer;
                }

                ((HttpWebRequest) r).Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                ((HttpWebRequest) r).UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko"; //IE

                r.Headers.Set("Accept-Encoding", "gzip, deflate, sdch");
                r.Headers.Set("Accept-Language", "de-AT,de;q=0.8,en;q=0.6,en-US;q=0.4,fr;q=0.2");
                r.Headers.Add(HttpRequestHeader.KeepAlive, "1");

                ((HttpWebRequest) r).AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                return r;
            }

            protected override WebResponse GetWebResponse(WebRequest request) {
                var response = base.GetWebResponse(request);

                if (!string.IsNullOrEmpty(response.Headers["Location"])) {
                    request = GetWebRequest(new Uri(response.Headers["Location"]));
                    request.ContentLength = 0;
                    response = GetWebResponse(request);
                }

                return response;
            }

            #endregion
        }
    }
}