using Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogRefactorSample
{
    class ClassB : ILogMessage
    {
        private NewMessageType _LogMessageType;
        public NewMessageType LogMessageType
        {
            get { return _LogMessageType; }
            set { _LogMessageType = value; }
        }

        public ClassB()
        {
            LogMessageType = new NewMessageType();
        }
    }
}
