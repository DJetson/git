using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logging
{
    public class NewMessageType
    {
        private Guid _TypeId;
        public Guid TypeId
        {
            get { return _TypeId; }
            set { _TypeId = value; }
        }

        private NewMessageType _Parent;
        public NewMessageType Parent
        {
            get { return _Parent; }
            set { _Parent = value; }
        }

        private List<NewMessageType> _Children;
        public List<NewMessageType> Children
        {
            get { return _Children; }
            set { _Children = value; }
        }

        public NewMessageType()
        {
            System.Diagnostics.StackFrame Frame = new System.Diagnostics.StackFrame(2);
            if (Frame != null)
            {
                if (Frame.GetMethod() != null)
                {
                    if (Frame.GetMethod().DeclaringType != null)
                    {
                        object t = Frame.GetMethod().;
                        if (t as ILogMessage != null)
                        {
                            Parent = (t as ILogMessage).LogMessageType;
                            Parent.Children = Parent.Children ?? new List<NewMessageType>();
                            Parent.Children.Add(this);
                        }
                    }
                }
            }
            Children = new List<NewMessageType>();
            TypeId = new Guid();

            
        }

        public List<NewMessageType> GetAllSubTypes(List<NewMessageType> list)
        {
            list = list ?? new List<NewMessageType>();

            list.Add(this);
            
            if (Children == null || Children.Count == 0)
                return list;

            foreach (NewMessageType m in Children)
            {
                list = GetAllSubTypes(list);
            }

            return list;
        }

    }
}
