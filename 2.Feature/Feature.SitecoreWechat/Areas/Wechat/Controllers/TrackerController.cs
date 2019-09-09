using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sitecore.Analytics;
using Sitecore.Analytics.Tracking;
using Sitecore.Analytics.Model;
using Sitecore.XConnect;
using Sitecore.XConnect.Client;
namespace Feature.SitecoreWechat.Areas.Wechat.Controllers
{
    public class TrackerController : Controller
    {
        // GET: Wechat/Tracker
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddGoal()
        {
            Guid channelID = Guid.Parse("{3830DD56-6CB3-4ABC-B545-5B429961B4AF}");

            string[] Colors = new string[5];
            Colors[0] = "{CE2CF9FB-8759-4DB0-A0D0-371CAC427C00}"; //white color
            Colors[1] = "{B4728ECA-B8F2-4084-8507-AD0A567A19C5}"; //black color
            Colors[2] = "{7FDBAADE-6565-4610-9B7F-B44E06B0FCE6}"; //blue color
            Colors[3] = "{E69B4B01-F413-4AD0-AFC3-31EBF434F80A}"; //gold color
            Colors[4] = "{E52C0A3F-02CA-4C4F-8CBD-144706F1BFF5}"; //grey color

            Guid goalID = Guid.Parse(Colors[int.Parse(Request.Params["goalID"].ToString())]);

            string userAgent = Request.UserAgent;

            RegisterGoal(channelID, goalID, userAgent);

            return Content("Register Goal:" + channelID + "|" + goalID + "|" + userAgent);
        }
        public void RegisterGoal(Guid channelID, Guid goalID, string userAgent)
        {
            using (Sitecore.XConnect.Client.XConnectClient client = Sitecore.XConnect.Client.Configuration.SitecoreXConnectClientConfiguration.GetClient())
            {
                try
                {
                    // var contact = new Sitecore.XConnect.Contact();
                    //Guid channelID = Guid.NewGuid();
                    //string userAgent = "Sample User Agent";

                    //string globalCookie = Request.Cookies["SC_ANALYTICS_GLOBAL_COOKIE"].Value;
                    //var reference = new IdentifiedContactReference(Sitecore.XConnect.Constants.AliasIdentifierSource, globalCookie.Split('|')[0]);

                    string trackerID = Sitecore.Analytics.Tracker.Current.Contact.ContactId.ToString();

                    //var reference = new IdentifiedContactReference(Sitecore.XConnect.Constants.AliasIdentifierSource,trackerID);
                    var reference = new IdentifiedContactReference("xDB.Tracker", trackerID);

                    Sitecore.XConnect.Contact contact = client.Get<Sitecore.XConnect.Contact>(reference, new ContactExpandOptions() { });

                    if (contact == null)
                    {
                       contact = new Sitecore.XConnect.Contact();
                    }

                    var interaction = new Sitecore.XConnect.Interaction(contact, InteractionInitiator.Brand, channelID, userAgent);

                    // Guid goalID = Guid.Parse("{9B6DC6A2-EB41-47B5-B10F-39AC37DE214F}"); // ID of goal item

                    var goal = new Goal(goalID, DateTime.UtcNow);

                    goal.EngagementValue = 20; // Manually setting engagement value rather than going to defintion item

                    interaction.Events.Add(goal);



                    client.AddContact(contact);
                    client.AddInteraction(interaction);

                    client.Submit();
                }
                catch (XdbExecutionException ex)
                {
                    // Handle exception
                }
            }
        }

    }
}