using System;
using System.Collections.Generic;

namespace Labyrinth.API.Services
{
    public class QuestEventDispatcher
    {
        private readonly Dictionary<string, Action<object>> _eventHandlers = new Dictionary<string, Action<object>>();

        public void RegisterEvent(string eventType, Action<object> callback)
        {
            if (!_eventHandlers.ContainsKey(eventType))
            {
                _eventHandlers[eventType] = callback;
            }
        }

        public void DispatchEvent(string eventType, object eventData)
        {
            if (_eventHandlers.ContainsKey(eventType))
            {
                _eventHandlers[eventType](eventData);
            }
        }
    }
}
