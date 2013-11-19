using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Windows;

namespace Logging.Objects
{
	public class MessageType : INotifyPropertyChanged
	{

		#region PropertyChanged EventHandler

		public event PropertyChangedEventHandler PropertyChanged;

		void NotifyPropertyChanged(String Property)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(Property));
		}

		#endregion

		//The Default TraceLevel for new MessageTypes
		private TraceLevel DefaultTraceLevel = TraceLevel.Info;

		private static MessageType _Root;
		public static MessageType Root
		{
			get { return _Root; }
			set { _Root = value; }
		}

		private String _Name;
		public String Name
		{
			get { return _Name; }
			set { _Name = value; NotifyPropertyChanged("Name"); }
		}

		private Type _Type;
		public Type Type
		{
			get { return _Type; }
			set { _Type = value; NotifyPropertyChanged("Type"); }
		}

		private String _Description;
		public String Description
		{
			get { return _Description; }
			set { _Description = value; NotifyPropertyChanged("Description"); }
		}

		private MessageType _Parent;
		public MessageType Parent
		{
			get { return _Parent; }
			set { _Parent = value; NotifyPropertyChanged("Parent"); if(Parent != null) Parent.NotifyPropertyChanged("Children"); }
		}

		private List<MessageType> _Children;
		public List<MessageType> Children
		{
			get { return _Children; }
			set { _Children = value; NotifyPropertyChanged("Children"); foreach (MessageType m in Children) m.NotifyPropertyChanged("Parent"); }
		}

		private Boolean _HasPriority = true;
		public Boolean HasPriority
		{
			get { return _HasPriority; }
			set { _HasPriority = value; NotifyPropertyChanged("HasPriority"); }
		}

		//TODO: Implement a function to see if a particular type already exists but only as an ancestor

		public static Boolean SetRootMessageType(Type RootType)
		{
			if (Root != null)
				return false;

			Root = new MessageType()
			{
				Children = new List<MessageType>(),
				Description = "All Message Types",
				Type = RootType,
				Name = RootType.Name,
				Parent = null
			};

			return true;
		}

		public static MessageType GetRootMessageType()
		{
			if (Root == null)
			{
				Type RootType = Application.Current.MainWindow.GetType();
				Root = new MessageType()
				{
					Name = RootType.Name,
					Description = String.Format("All {0} Messages Types", RootType.Name),
					Type = RootType,
					Parent = null,
					Children = new List<MessageType>()
				};
			}

			return Root;
		}

		//Beginning with the invoking object, traverse the tree depth-first and
		//return the first instance of a LogMessageType with the given type
		public MessageType GetMessageType(Type t)
		{
			if (Type == t)
				return this;

			if (Children == null)
				return null;

			foreach (MessageType Child in Children)
			{
				MessageType Result = Child.GetMessageType(t);
				if (Result != null)
					return Result;
			}

			return null;
		}

		//Beginning with the invoking object, traverse the tree depth-first and
		//return the first instance of a LogMessageType with a given name
		public MessageType GetMessageType(String n)
		{
			if (Name.CompareTo(n) == 0)
				return this;

			foreach (MessageType Child in Children)
			{
				MessageType Result = Child.GetMessageType(n);
				if (Result != null)
					return Result;
			}

			return null;
		}

		public List<MessageType> GetAllTypes(List<MessageType> AllTypes)
		{
			if (AllTypes == null)
				AllTypes = new List<MessageType>();
			AllTypes.Add(this);
			foreach (MessageType Child in Children)
				Child.GetAllTypes(AllTypes);

			//If this is the root element of the tree, return the completed list
			if (Parent == null)
				return AllTypes;

			//Otherwise return nothing
			return null;
		}

		/// <summary>
		/// Register a new MessageType with the invoking object
		/// </summary>
		/// <param name="m">The new MessageType to register</param>
		public void RegisterSubType(MessageType m)
		{
			m.Parent = this;

			if (Children == null)
				Children = new List<MessageType>();

			Children.Add(m);
		}

		/// <summary>
		/// From a given source type, get the type of the class that originally declared it.
		/// </summary>
		/// <param name="Source"></param>
		/// <returns></returns>
		public static Type GetUnregisteredParentType(Type Source)
		{
			StackFrame s = new StackFrame(GetFrameOffsetForType(Source) + 2);
			if (s == null)
				return null;
			MethodBase m = s.GetMethod();
			if (m == null)
				return null;

			return m.DeclaringType;
		}

		public static int GetFrameOffsetForType(Type Source)
		{
			Type t = null;
			StackFrame s = null;
			MethodBase m = null;

			int Offset = 0;
			while (true)
			{
				Offset += 2;
				if ((s = new StackFrame(Offset)) == null)
					break;
				if ((m = s.GetMethod()) == null)
					break;
				if ((t = m.DeclaringType) == null)
					break;
				if (t == Source)
					return Offset;
			}
			return -1;
		}

		public static MessageType FindRegisteredAncestor(Type Source)
		{
			//Get a Parent type
			//Check if the parent type is registered
			Type Parent = GetUnregisteredParentType(Source);
			if (IsRegistered(Parent))
				return GetRootMessageType().GetMessageType(Parent);
			return FindRegisteredAncestor(Parent);

			//If yes return the registered type
			//if noreturn result of find registered ancestor
		}

		public static Boolean IsRegistered(Type ToValidate)
		{
			//If the type we are checking exists already, return true
			if (Root.GetMessageType(ToValidate) != null)
				return true;
			else
				return false;
		}

		public static MessageType Register(Type Source)
		{
			Type ParentType;
			MessageType ParentMessageType = null;
			//1.Check if a source type has been registered
			//2A.If yes, go to step 3
			if (IsRegistered(Source) != true && Source.Name != GetRootMessageType().Name)
			{
				//2B.Get the ParentType that instantiated the source type
				ParentType = GetUnregisteredParentType(Source);
				if (ParentType == null)
					return null;
				//2C.If the ParentType is null then treat General(All Message Types) as the ParentType
				//2D.Check if the ParentType has been registered,
				if (IsRegistered(ParentType) == false)
				{
					//2E.if no go to step 1 treating the ParentType as the source type
					ParentMessageType = Register(ParentType);
				}

				//2F. If yes Register the SourceType with the Parent set to ParentMessageType

				String NewDescription;
				if (ParentMessageType == null)
					NewDescription = "Application";
				else
					NewDescription = ParentType.Name;

				MessageType SourceType;
				if (ParentMessageType == null)
				{
					SourceType = new MessageType()
					{
						Children = new List<MessageType>(),
						Name = Source.Name,
						Description = String.Format("All {0} Messages posted by the {1}", Source.Name, NewDescription),
						Type = Source,
						Parent = MessageType.GetRootMessageType()
					};
				}
				else
				{
					SourceType = new MessageType()
					{
						Children = new List<MessageType>(),
						Name = Source.Name,
						Description = String.Format("All {0} Messages posted by the {1}", Source.Name, NewDescription),
						Type = Source,
						Parent = MessageType.GetRootMessageType()
					};
				}
				if (ParentMessageType == null)
					GetRootMessageType().RegisterSubType(SourceType);
				else
					ParentMessageType.RegisterSubType(SourceType);
				return SourceType;
			}

			//3. Return the SourceMessageType
			return GetRootMessageType().GetMessageType(Source);
		}
	}
}
