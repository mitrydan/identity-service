using System;
using System.Collections.Generic;

namespace IdentityService.BlazorClient.Infrastructure
{
    public class EventAggregator
    {
        private readonly Dictionary<string, IList<Action<EventArgs>>> _events;

        public EventAggregator()
        {
            _events = new Dictionary<string, IList<Action<EventArgs>>>();
        }

        public void Subscribe(string eventName, Action<EventArgs> eventHandler)
        {
            if (_events.ContainsKey(eventName))
            {
                _events[eventName].Add(eventHandler);
            }
            else
            {
                _events.Add(eventName, new List<Action<EventArgs>> { eventHandler });
            }
        }

        public void Unsubscribe(string eventName)
        {
            if (!_events.ContainsKey(eventName))
            {
                return;
            }

            _events.Remove(eventName);
        }

        public void Publish(string eventName, EventArgs eventArgs)
        {
            if (!_events.ContainsKey(eventName))
            {
                return;
            }

            foreach (var eventHandler in _events[eventName])
            {
                eventHandler.Invoke(eventArgs);
            }
        }
    }
}
