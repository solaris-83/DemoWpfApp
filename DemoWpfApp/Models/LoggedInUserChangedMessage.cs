using CommunityToolkit.Mvvm.Messaging.Messages;

namespace DemoWpfApp.Models
{
    public class LoggedInUserChangedMessage : ValueChangedMessage<string>
    {
        public string UserName { get; set; }
        public LoggedInUserChangedMessage(string username) : base(username)
        {
            UserName = username;
        }
    }
}
