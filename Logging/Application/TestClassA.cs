using Logging;
using Logging.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LogSample
{
    class TestClassA : ILoggedType
    {
        private static MessageType _LogMessageType;
        public MessageType LogMessageType
        {
            get { return _LogMessageType; }
            set { _LogMessageType = value; }
        }

    }
}
