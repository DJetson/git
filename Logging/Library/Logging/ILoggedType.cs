﻿using Logging.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logging
{
    public interface ILoggedType
    {
        MessageType LogMessageType
        {
            get;
            set;
        }
    }
}
