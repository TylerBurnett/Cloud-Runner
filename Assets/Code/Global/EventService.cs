using System;
using System.Linq;
using UnityEngine;

namespace Game.Global
{
    /// <summary>
    /// This Service is globally accessible and will work regardless of what scripts are avaliable at runtime.
    /// It serves the utterly vital role of being the event fabric of which all services need to communicate upon.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class EventService<T> where T : Delegate
    {
        // Internal testing hook for monitoring game events
        private static readonly bool _debug = true;
        private static readonly string[] _debugEventFilter = new string[] { "GameEnterEvent", "GameplayEndEvent", "GameplayStartEvent", "DeathObjectCollisionEvent" };

        private static T _handle;

        /// <summary>
        /// Registers Delegates for an event type.
        /// </summary>
        /// <param name="callback"></param>
        public static void Register(T callback)
        {
            _handle = Delegate.Combine(_handle, callback) as T;
        }

        /// <summary>
        /// De-Register's events for a delegate type.
        /// </summary>
        /// <param name="callback"></param>
        public static void Unregister(T callback)
        {
            _handle = Delegate.Remove(_handle, callback) as T;
        }

        public static T Trigger
        {
            get            {
                if (_debug)
                {
                    string eventName = typeof(T).Name;
                    if (_debugEventFilter.Contains(eventName))
                    {
                        Debug.Log(string.Format("{0} Triggered", eventName));
                    }
                }

                return _handle;            }
        }
    }
}
