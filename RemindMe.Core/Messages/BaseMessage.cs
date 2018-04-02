using MvvmCross.Plugins.Messenger;
using System;
using System.Collections.Generic;
using System.Text;

namespace RemindMe.Core.Messages
{
    public class BaseMessage : MvxMessage
    {
        public BaseMessage(object sender) : base(sender)
        {

        }
    }
}
