using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIMPlatform.Application.Contracts.Events
{
    public class UserSubscriptionDataInfo
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string UserDisplayName { get; set; }
        public string Email { get; set; }
        public string NotifyType { get; set; }
    }

    public enum NotificationType
    {
        Email,
        Notification,
        EmailAndNotification
    }
}
