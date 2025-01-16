using System;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace TaSked.App;

public class InvintationItemMessage : ValueChangedMessage<String>
{ 
        public InvintationItemMessage(string value) : base(value) {
        }
}
