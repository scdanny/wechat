using Sitecore.Globalization;
using Sitecore.Links;
using Sitecore.Mvc.Presentation;
using Sitecore.Web.UI.WebControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WechatFans.Models;

namespace WechatFans.Controllers
{
    public class OverviewController : Controller
    {
        // GET: Overview
        public ActionResult Index()
        {
            var model = new OverviewList()
            {
                ReadMore = Translate.Text("Read More")
            };

            model.AddRange(RenderingContext.Current.Rendering.Item.GetChildren(Sitecore.Collections.ChildListOptions.SkipSorting).OrderByDescending(i => i.Statistics.Created).Select(i => new OverviewItem()
            {
                title = new HtmlString(FieldRenderer.Render(i, "title")),
                image = new HtmlString(FieldRenderer.Render(i, "banner", "mw=500&mh=333")),
                URL = LinkManager.GetItemUrl(i)
            }
            ));
            return View(model);
        }
    }
}