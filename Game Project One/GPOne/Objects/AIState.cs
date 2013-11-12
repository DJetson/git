using GPOne.BaseClasses;
using GPOne.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPOne.Objects
{
    class StateCondition
    {
        private IProcessAI _TargetObject;
        public IProcessAI TargetObject
        {
            get { return _TargetObject; }
        }

        private WorldObject _World;
        public WorldObject World
        {
            get { return _World; }
        }

        private Func<IProcessAI, WorldObject, Boolean> EntranceCondition;
        private Func<IProcessAI, WorldObject, Boolean> MaintenanceCondition;
        private Func<IProcessAI, WorldObject, Boolean> ExitCondition;

        /// <summary>
        /// Returns a Boolean indicating whether or not the Entry Condition has been satisfied
        /// </summary>
        public Boolean AllowEntry
        {
            get { return (EntranceCondition != null) ? EntranceCondition(TargetObject, World) : true; }
        }
        
        /// <summary>
        /// Returns a Boolean indicating whether or not the Maintenance Condition has been satisfied
        /// </summary>
        public Boolean CanContinue
        {
            get { return (MaintenanceCondition != null) ? MaintenanceCondition(TargetObject, World) : true; }
        }

        /// <summary>
        /// Returns a Boolean indicating whether or not the Exit Condition has been satisfied
        /// </summary>
        public Boolean AllowExit
        {
            get { return (ExitCondition != null) ? ExitCondition(TargetObject, World) : true; }
        }

        /// <summary>
        /// State Condition Constructor
        /// </summary>
        /// <param name="targetObject">The object which owns the AIState for which this condition is being evaluated</param>
        /// <param name="world">A reference to the world which contains the circumstances being assessed by this StateCondition</param>
        /// <param name="entranceCondition">The condition which must be satisfied in order for this state to be entered</param>
        /// <param name="maintenanceCondition">The condition which must be maintained in order for this state to continue</param>
        /// <param name="exitCondition">The condition which must be met in order for this state to be allowed to exit</param>
        public StateCondition(IProcessAI targetObject, WorldObject world,
            Func<IProcessAI, WorldObject, Boolean> entranceCondition = null,
            Func<IProcessAI, WorldObject, Boolean> maintenanceCondition = null,
            Func<IProcessAI, WorldObject, Boolean> exitCondition = null)
        {
            _TargetObject = targetObject;
            _World = world;
            EntranceCondition = entranceCondition;
            MaintenanceCondition = maintenanceCondition;
            ExitCondition = exitCondition;
        }
    }

    ///TODO: Finish Implementing AIState. Uncomment Precedents, Antecedents, and Behaviors
    class AIState
    {
        /// <summary>
        /// Conditions required to enter, maintain, or exit this state.
        /// </summary>
        private List<StateCondition> Prerequisites;

        /// <summary>
        /// States from which this state may be transitioned to.
        /// </summary>
        //private Dictionary<String, AIState> Precedents;

        /// <summary>
        /// States to which this state may transition
        /// </summary>
        //private Dictionary<String, AIState> Antecedents;

        /// <summary>
        /// A Dictionary of functions which handle response functionality for the state
        /// </summary>
        //Dictionary<String, Action<IProcessAI>> StateBehavior;

        public Boolean CanEnterState
        {
            get
            {
                if (Prerequisites == null || Prerequisites.Count == 0)
                    return true;
                foreach (StateCondition Condition in Prerequisites)
                    if (Condition.AllowEntry == false)
                        return false;
                return true;
            }
        }

        public Boolean CanContinueState
        {
            get
            {
                if (Prerequisites == null || Prerequisites.Count == 0)
                    return true;
                foreach (StateCondition Condition in Prerequisites)
                    if (Condition.CanContinue == false)
                        return false;
                return true;
            }
        }

        public Boolean CanExitState
        {
            get
            {
                if (Prerequisites == null || Prerequisites.Count == 0)
                    return true;
                foreach (StateCondition Condition in Prerequisites)
                    if (Condition.AllowExit == false)
                        return false;
                return true;
            }
        }

        public AIState()
        {
        }

        public void AddPrerequisite(StateCondition NewStateConditions)
        {
            Prerequisites = Prerequisites ?? new List<StateCondition>();

            if (Prerequisites.Contains(NewStateConditions))
                return;

            Prerequisites.Add(NewStateConditions);
        }

        public void RemovePrerequisite(StateCondition ToRemove)
        {
            Prerequisites = Prerequisites ?? new List<StateCondition>();
            Prerequisites.Remove(ToRemove);
        }

        /// <summary>
        /// Create and start any timers needed by this AIState
        /// </summary>
        //void InitializeState(IGameObject AIObject)
        //{

        //}
    }
}
