using Microsoft.Azure.NotificationHubs;
using NetCoreAPI.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreAPI.Data.Interfaces
{
    public interface INotificationHub
    {
        NotificationHubClient GetHub();
        string GetOfficialReplyToast(Channel channel, Feedback feedback, Comment comment);
        string GetActionChangedToast(Channel channel, Feedback feedback, Comment comment);
        Task SendNotification(string msg, string[] tags);
    }
}
