using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameObjects.Controls.DeveloperConsole
{
    public class CommandObject
    {
        public String Name;
        public int Id;
        public String CallType;
        public String CallTypeAssembly;
//        public String CallProperty;
        public String CallFunction;
//        public String Arguments;
        public String Description;

        public CommandObject()
        {
        }

        public CommandObject(String name, int id, String callFunction, String description)
        {
            Name = name;
            Id = id;
            CallFunction = callFunction;
            Description = description;
        }
    }
}
