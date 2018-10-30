using Sitecore.Diagnostics;
using Sitecore.XConnect;
using Sitecore.XConnect.Client;
using Sitecore.XConnect.Collection.Model;
using System;
using System.Web;

namespace WeChat.Service
{
    public class ContactService
    {
        public static void AddContact(UserInfo userInfo)
        {
            using (XConnectClient client = Sitecore.XConnect.Client.Configuration.SitecoreXConnectClientConfiguration.GetClient())
            {
                try
                {
                    //Identify contact
                    Sitecore.Analytics.Tracker.Current.Session.IdentifyAs("wechat", userInfo.Openid);

                    //Retrieve contact
                    Contact existingContact = client.Get(new IdentifiedContactReference("wechat", userInfo.Openid), new ContactExpandOptions(PersonalInformation.DefaultFacetKey));

                    if (existingContact != null)
                    {
                        //Retrieve facet by name
                        var personalInfoFacet = existingContact.GetFacet<PersonalInformation>(PersonalInformation.DefaultFacetKey);

                        if (personalInfoFacet != null)
                        {
                            //Change facet properties
                            personalInfoFacet.FirstName = userInfo.Nickname;
                            personalInfoFacet.Nickname = userInfo.Nickname;
                            personalInfoFacet.Gender = !string.IsNullOrWhiteSpace(userInfo.Sex) ? (userInfo.Sex.Equals("1") ? "Male" : (userInfo.Sex.Equals("2") ? "Female" : string.Empty)) : string.Empty;
                            personalInfoFacet.PreferredLanguage = userInfo.Language;

                            //Set the updated facet
                            client.SetFacet(existingContact, personalInfoFacet);
                        }
                        else
                        {
                            //Create facet
                            personalInfoFacet = new PersonalInformation()
                            {
                                FirstName = userInfo.Nickname,
                                Nickname = userInfo.Nickname,
                                Gender = !string.IsNullOrWhiteSpace(userInfo.Sex) ? (userInfo.Sex.Equals("1") ? "Male" : (userInfo.Sex.Equals("2") ? "Female" : string.Empty)) : string.Empty,
                                PreferredLanguage = userInfo.Language
                            };

                            client.SetFacet(existingContact, personalInfoFacet);
                        }

                        //AddInteraction(client, existingContact);

                        client.Submit();
                    }                    
                    //else
                    //{
                    //    Contact contact = new Contact(new ContactIdentifier("wechat", userInfo.Openid, ContactIdentifierType.Known));

                    //    client.AddContact(contact);

                    //    PersonalInformation personalInfoFacet = new PersonalInformation()
                    //    {
                    //        FirstName = userInfo.Nickname,
                    //        Nickname = userInfo.Nickname,
                    //        Gender = !string.IsNullOrWhiteSpace(userInfo.Sex) ? (userInfo.Sex.Equals("1") ? "Male" : (userInfo.Sex.Equals("2") ? "Female" : string.Empty)) : string.Empty,
                    //        PreferredLanguage = userInfo.Language
                    //    };

                    //    client.SetFacet(contact, personalInfoFacet);

                    //    // Facet without a reference, key is specified
                    //    AddressList addresses = new AddressList(new Address() { City = userInfo.City, StateOrProvince = userInfo.Province }, "Home");

                    //    client.SetFacet(contact, AddressList.DefaultFacetKey, addresses);

                    //    AddInteraction(client, contact);

                    //    // Submit operations as batch
                    //    client.Submit();
                    //}
                }
                catch (XdbExecutionException ex)
                {
                    Log.Error("Contact Service - Add Contact", ex, "");
                }
            }
        }

        private static void AddInteraction(XConnectClient client, Contact contact)
        {
            // Create a new interaction for the contact
            Guid channelId = new Guid("{1DA15267-B0DB-4BE1-B44F-E57C2EEB8A6B}");

            string userAgent = HttpContext.Current.Request.UserAgent;

            Interaction webInteraction = new Interaction(contact, InteractionInitiator.Contact, channelId, userAgent);

            // Create a new web visit facet model
            var webVisitFacet = new WebVisit();

            // Populate data about the web visit
            HttpBrowserCapabilities browser = HttpContext.Current.Request.Browser;
            webVisitFacet.Browser = new BrowserData() { BrowserMajorName = browser.Browser, BrowserMinorName = browser.Platform, BrowserVersion = browser.Version };
            webVisitFacet.Language = Sitecore.Context.Language.Name;
            webVisitFacet.OperatingSystem = new OperatingSystemData() { Name = Environment.OSVersion.VersionString, MajorVersion = Environment.OSVersion.Version.Major.ToString(), MinorVersion = Environment.OSVersion.Version.Minor.ToString() };
            webVisitFacet.Referrer = HttpContext.Current.Request.UrlReferrer != null ? HttpContext.Current.Request.UrlReferrer.AbsoluteUri : string.Empty;
            webVisitFacet.Screen = new ScreenData() { ScreenHeight = browser.ScreenPixelsHeight, ScreenWidth = browser.ScreenPixelsWidth };
            webVisitFacet.SearchKeywords = "wechat";
            webVisitFacet.SiteName = Sitecore.Context.Site.Name;

            var item = Sitecore.Context.Item;

            //page view
            PageViewEvent pageView = new PageViewEvent(DateTime.UtcNow, item.ID.ToGuid(), item.Version.Number, item.Language.Name)
            {
                Duration = new TimeSpan(0, 0, 30),
                Url = HttpContext.Current.Request.Url.PathAndQuery
            };

            webInteraction.Events.Add(pageView);

            // Set web visit facet on interaction
            client.SetWebVisit(webInteraction, webVisitFacet);

            // Add interaction
            client.AddInteraction(webInteraction);
        }                       
    }
}

