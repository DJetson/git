using Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogRefactorSample
{
    class ClassA : ILogMessage
    {
        private NewMessageType _LogMessageType;
        public NewMessageType LogMessageType
        {
            get { return _LogMessageType; }
            set { _LogMessageType = value; }
        }

        public ClassA()
        {
            LogMessageType = new NewMessageType();
            ClassC c = new ClassC();
        }

    }
}
