using Microsoft.Azure.NotificationHubs;
using Microsoft.Toolkit.Uwp.Notifications;
using NetCoreAPI.Data.Interfaces;
using NetCoreAPI.Data.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NetCoreAPI
{
    public class NotificationHub : INotificationHub
    {
        private NotificationHubClient hub;

        public NotificationHub()
        {
            hub = NotificationHubClient.CreateClientFromConnectionString("Endpoint=sb://mynamespacess.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=[sharedkey]", "TestAppNotiHub");
        }

        public string GetActionChangedToast()
        {
            throw new NotImplementedException();
        }

        public NotificationHubClient GetHub()
        {
            return hub;
        }

        public string GetOfficialReplyToast(Channel channel, Feedback feedback, Comment comment)
        {
            ToastContent content = new ToastContent()
            {
                Launch = "app-defined-string",
                Visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        Children =
                        {
                            new AdaptiveText()
                            {
                                Text = $"Phản hồi từ {channel.Name}",
                                HintMaxLines = 1
                            },
                            new AdaptiveText()
                            {
                                Text= feedback.Content,
                            },
                            new AdaptiveText()
                            {
                                Text= comment.Content,
                            },
                        },
                        Attribution = new ToastGenericAttributionText()
                        {
                            Text = "qua Complainer"
                        },
                    }
                   
                },
              
            };

            return content.GetContent();
        }

        public string GetActionChangedToast(Channel channel, Feedback feedback, Comment comment)
        {
            string action = "";
            switch(feedback.Action)
            {
                case 0:
                    action = "đang được xem xét";
                    break;
                case 1:
                    action = "đã được giải quyết";
                    break;
                default:
                    return null;
            }

            ToastContent content = new ToastContent()
            {
                Launch = "app-defined-string",
                Visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        Children =
                        {
                            new AdaptiveText()
                            {
                                Text = $"Phản hồi từ {channel.Name}",
                                HintMaxLines = 1
                            },
                            new AdaptiveText()
                            {
                                Text= feedback.Content,
                            },
                            new AdaptiveText()
                            {
                                Text= $"Phản hồi của bạn {action}",
                            },
                        },
                        Attribution = new ToastGenericAttributionText()
                        {
                            Text = "qua Complainer"
                        },
                    }

                },

            };

            return content.GetContent();
        }


        public async Task SendNotification(string toast, string[] tags)
        {
            //var user = HttpContext.Current.User.Identity.Name;

            Microsoft.Azure.NotificationHubs.NotificationOutcome outcome = null;
            HttpStatusCode ret = HttpStatusCode.InternalServerError;

            outcome = await hub.SendWindowsNativeNotificationAsync(toast, tags);
            if (outcome != null)
            {
                if (!((outcome.State == Microsoft.Azure.NotificationHubs.NotificationOutcomeState.Abandoned) ||
                    (outcome.State == Microsoft.Azure.NotificationHubs.NotificationOutcomeState.Unknown)))
                {
                    ret = HttpStatusCode.OK;
                }
            }
        }
    }
}
