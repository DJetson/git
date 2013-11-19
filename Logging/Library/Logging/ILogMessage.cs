using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logging
{
    public interface ILogMessage
    {
        NewMessageType LogMessageType
        {
            get;
            set;
        }
    }
}
