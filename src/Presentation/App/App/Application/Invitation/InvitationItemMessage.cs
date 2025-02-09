using System;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace TaSked.App;

public class InvitationItemMessage : ValueChangedMessage<String>
{ 
        public InvitationItemMessage(string value) : base(value) {
        }
}
