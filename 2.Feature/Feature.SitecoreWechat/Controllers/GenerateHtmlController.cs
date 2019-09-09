
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using Sitecore.Web.UI.WebControls;
using Sitecore.Mvc.Pipelines;
using Sitecore.Mvc.Pipelines.Response.RenderRendering;
using Feature.SitecoreWechat.Models;

namespace Feature.SitecoreWechat.Controllers
{
    public class GenerateHtmlController : Controller
    {
        // GET: GenerateHtml
        public ActionResult Index()
        {
            if (Sitecore.Context.PageMode.IsPreview)
            {
                //string phKey = "/main/pageContent";
                string phKey = RenderingContext.Current.Rendering.Parameters["phkey"];
                return View(CreateModel(phKey));
            }
            else
            {
                //return View("EmptyView");
                return Content("The html source will be generated in Preview Mode!");
            }
        }

        public GenPhHtml CreateModel(string placeholderKey)
        {
            string returnHtml = null;
            Item currentItem = RenderingContext.Current.Rendering.Item;
            var renderings = new List<Sitecore.Mvc.Presentation.Rendering>();
            var renderingCount = currentItem.Visualization.GetRenderings(Sitecore.Context.Device, false).ToList();

            if (renderingCount.Count > 0)
            {
                renderings.AddRange(renderingCount.Select(r => new Sitecore.Mvc.Presentation.Rendering
                {
                    RenderingItemPath = r.RenderingID.ToString(),
                    Parameters = new RenderingParameters(r.Settings.Parameters),
                    DataSource = r.Settings.DataSource,
                    Placeholder = r.Placeholder,

                }));



                foreach (var r in renderings.Where(r => r.Placeholder.Contains(placeholderKey)))
                {

                    using (var stringWriter = new StringWriter())
                    {
                        PipelineService.Get()
                            .RunPipeline("mvc.renderRendering", new RenderRenderingArgs(r, stringWriter));
                        returnHtml += stringWriter.ToString();
                    }
                }

            }

            //var imageRenderings = new List<string>();
            //imageRenderings.Add(FieldRenderer.Render(currentItem, "Image", "mw=400"));

            return new GenPhHtml
            {
                PlaceholderKey = placeholderKey,
                GenHtml = returnHtml,
                ItemID = RenderingContext.Current.Rendering.Item.ID.ToString()
            };

        }
    }
}