using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.Networking.Connectivity;
using Windows.UI.Notifications;
using Windows.UI.StartScreen;
using AwfulRedux.UI.Models.Forums;
using AwfulRedux.UI.Models.Threads;
using Newtonsoft.Json;
using NotificationsExtensions.Tiles;
using NotificationsExtensions.Toasts;

namespace AwfulRedux.Notifications
{
    public class NotifyStatusTile
    {
        public static bool IsInternet()
        {

#if DEBUG
            return true;
#endif
            ConnectionProfile connections = NetworkInformation.GetInternetConnectionProfile();
            bool internet = connections != null &&
                            connections.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess;
            return internet;
        }

        public static async Task<bool> CreateSecondaryForumLinkTile(Forum forumEntity)
        {
            var tileId = forumEntity.ForumId;
            var pinned = SecondaryTile.Exists(tileId.ToString());
            if (pinned)
                return true;

            Uri square150X150Logo = new Uri("ms-appx:///Assets/Logo.png");

            var tile = new SecondaryTile(tileId.ToString())
            {
                DisplayName = forumEntity.Name,
                Arguments = JsonConvert.SerializeObject(forumEntity),
                VisualElements = { Square150x150Logo = square150X150Logo, ShowNameOnSquare150x150Logo = true },
            };
            return await tile.RequestCreateAsync();
        }

        public static void CreateBookmarkLiveTile(Thread forumThread)
        {
            var bindingContent = new TileBindingContentAdaptive()
            {
                Children =
                {
                    new TileText()
                    {
                        Text = forumThread.Name,
                        Style = TileTextStyle.Body
                    },
                    new TileText()
                    {
                        Text = string.Format("Unread Posts: {0}", forumThread.RepliesSinceLastOpened),
                        Wrap = true,
                        Style = TileTextStyle.CaptionSubtle
                    },
                    new TileText()
                    {
                        Text = string.Format("Killed By: {0}", forumThread.KilledBy),
                        Wrap = true,
                        Style = TileTextStyle.CaptionSubtle
                    }
                }
            };
            var binding = new TileBinding()
            {
                Branding = TileBranding.NameAndLogo,
                Content = bindingContent
            };
            var content = new TileContent()
            {
                Visual = new TileVisual()
                {
                    TileMedium = binding,
                    TileWide = binding,
                    TileLarge = binding
                }
            };
            var tileXml = content.GetXml();
            var tileNotification = new TileNotification(tileXml);
            TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);
        }

        public static void CreateToastNotification(Thread forumThread)
        {
            string replyText = forumThread.RepliesSinceLastOpened > 1 ? " has {0} replies." : " has {0} reply.";
            string test = "{" + string.Format("type:'toast', 'threadId':{0}, 'pageNumber':{1}, 'isThreadBookmark':{2}", forumThread.ThreadId, forumThread.CurrentPage, forumThread.IsBookmark.ToString().ToLower()) + "}";
            ToastContent content = new ToastContent()
            {
                Launch = test,
                Visual = new ToastVisual()
                {
                    TitleText = new ToastText()
                    {
                        Text = string.Format("\"{0}\"", forumThread.Name)
                    },
                    BodyTextLine1 = new ToastText()
                    {
                        Text = string.Format(replyText, forumThread.RepliesSinceLastOpened)
                    },
                    AppLogoOverride = new ToastAppLogo()
                    {
                        Source = new ToastImageSource(forumThread.ImageIconLocation)
                    }
                },
                Actions = new ToastActionsCustom()
                {
                    Buttons =
                    {
                        new ToastButton("Open Thread", test)
                        {
                            ActivationType = ToastActivationType.Foreground
                        },
                        new ToastButton("Sleep", "sleep")
                        {
                            ActivationType = ToastActivationType.Background
                        }
                    }
                },
                Audio = new ToastAudio()
                {
                    Src = new Uri("ms-winsoundevent:Notification.Reminder")
                }
            };

            XmlDocument doc = content.GetXml();

            var toastNotification = new ToastNotification(doc);
            var nameProperty = toastNotification.GetType().GetRuntimeProperties().FirstOrDefault(x => x.Name == "Tag");
            nameProperty?.SetValue(toastNotification, forumThread.ThreadId.ToString());
            ToastNotificationManager.CreateToastNotifier().Show(toastNotification);
        }

        public static void CreateToastNotification(string header, string text)
        {
            XmlDocument notificationXml =
    ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText02);
            XmlNodeList toastElements = notificationXml.GetElementsByTagName("text");
            toastElements[0].AppendChild(
                notificationXml.CreateTextNode(header));
            toastElements[1].AppendChild(
                 notificationXml.CreateTextNode(text));
            XmlNodeList imageElement = notificationXml.GetElementsByTagName("image");
            string imageName = string.Empty;
            if (string.IsNullOrEmpty(imageName))
            {
                imageName = @"Assets/Logo.scale-100.png";
            }
            imageElement[0].Attributes[1].NodeValue = imageName;
            IXmlNode toastNode = notificationXml.SelectSingleNode("/toast");
            string test = "{" + string.Format("type:'toast'") + "}";
            var xmlElement = (XmlElement)toastNode;
            xmlElement?.SetAttribute("launch", test);
            var toastNotification = new ToastNotification(notificationXml)
            {
                ExpirationTime = DateTimeOffset.UtcNow.AddSeconds(30)
            };
            ToastNotificationManager.CreateToastNotifier().Show(toastNotification);
        }
    }
}
