using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace SimpleBT 
{
	public class Optional<T> {
		private T _value;

		public bool HasValue { get; private set; }

		public Optional()
		{
			_value = default;
			HasValue = false;
		}

		public Optional(T value){
			_value = value;
			HasValue = true;
		}

		public static implicit operator Optional<T>(T value)
        {
            return new Optional<T>(value);
        }

		public T Value {
			get {
				if(!HasValue){
					throw new Exception("Optional has no value");
				}
				return _value;
			}

			set {
				_value = value;
				HasValue = true;
			}
		}

		public void Clear(){
			HasValue = false;
			_value = default;
		}
	}

	public class BTEvent
	{
		public Optional<object> Arg1 {get; set;} = new Optional<object>();
		public Optional<object> Arg2 {get; set;} = new Optional<object>();
		public Optional<object> Arg3 {get; set;} = new Optional<object>();
	}

	public class EventBus
	{
		private Dictionary<string, UnityEvent<BTEvent>> _events = new Dictionary<string, UnityEvent<BTEvent>>();
		
		public void RegisterCallback(string eventName, UnityAction<BTEvent> action)
		{
			// TODO: RegisterEvent/DeregisterEvent
			if(!_events.TryGetValue(eventName, out var ev)){
				ev = new UnityEvent<BTEvent>();
				_events[eventName] = ev;
			}

			ev.RemoveListener(action);
			ev.AddListener(action);
		}

		public void DeregisterCallback(string eventName, UnityAction<BTEvent> action)
		{
			if(!_events.TryGetValue(eventName, out var ev)){
				// Debug.LogWarning($"No event handlers for \"{eventName}\" are registered");
				return;
			}

			ev.RemoveListener(action);
		}

		public void SendEvent(string eventName, BTEvent btEvent){
			if(!_events.TryGetValue(eventName, out var ev)){
				// Debug.LogWarning($"No event handlers for \"{eventName}\" are registered");
				return;
			}

			ev.Invoke(btEvent);
		}

	}

}