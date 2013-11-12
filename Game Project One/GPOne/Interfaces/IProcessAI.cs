using GPOne.BaseClasses;
using GPOne.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace GPOne.Interfaces
{
    /// <summary>
    /// IProcessAI is used to create objects with a consistent AI Architecture. It requires implementation of IPositionInfo as most
    /// if not all State Decisions will require some spacial awareness on the part of the object. It also requires implementation of 
    /// IGameObject to ensure that it has a valid reference to the world in which it resides.
    /// </summary>
    interface IProcessAI : IGameObject, IPositionInfo
    {
        /// <summary>
        /// A dictionary containing all of the available AI states available to this object
        /// </summary>
        Dictionary<String, AIState> AIStates
        {
            get;
            set;
        }

        /// <summary>
        /// The current active state of the AI
        /// </summary>
        AIState ActiveState
        {
            get;
            set;
        }

        /// <summary>
        /// The Next AI State that will be employed by this object. It is not assigned immediately after it is chosen to allow for
        /// last minute changes as well as timed transitions from the current state
        /// </summary>
        AIState NextState
        {
            get;
            set;
        }

        /// <summary>
        /// The default state of the AI. Unless otherwise specified, this is the objects initial state as well as the fallback
        /// state which will be activated if no other states' prerequisites have been satisfied.
        /// </summary>
        AIState DefaultState
        {
            get;
            set;
        }

        /// <summary>
        /// A list of all objects which may have an effect on the state decision of this object
        /// </summary>
        List<CharacterBase> KeyObjects
        {
            get;
            set;
        }

        /// <summary>
        /// A collection for storing any timers created and used by this AI object
        /// </summary>
        List<DispatcherTimer> StateTiming
        {
            get;
            set;
        }

        /// <summary>
        /// Determine the next state that will be assigned to this object
        /// </summary>
        void CalculateNextState();

        /// <summary>
        /// Transition to the next AI state.
        /// </summary>
        void TransitionToNextState();

        /// <summary>
        /// Execute any special behavior associated with this state
        /// </summary>
        void ExecuteAI();
    }
}
