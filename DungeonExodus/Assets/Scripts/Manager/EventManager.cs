using System;
using System.Collections.Generic;
using UnityEngine;

namespace KPU.Manager
{
    public class EventManager : SingletonBehaviour<EventManager>
    {
        // int temp = a ?? b
        // a가 null이면 temp에 b값 삽입, a가 null이 아니라면 a값 삽입
        private IDictionary<string, List<Action<object>>> EventDatabase =>
            _eventDatabase ?? (_eventDatabase = new Dictionary<string, List<Action<object>>>());

        private IDictionary<string, List<Action<object>>> _eventDatabase;

        public static void On(string eventName, Action<object> subscriber)
        {
            // 만약 없을 경우.
            if (!Instance.EventDatabase.ContainsKey(eventName))
                Instance.EventDatabase.Add(eventName, new List<Action<object>>());

            Instance.EventDatabase[eventName].Add(subscriber);
        }
        // Emit: 방출하다
        public static void Emit(string eventName, object parameter= null)
        {
            if (!Instance.EventDatabase.ContainsKey(eventName))
                Debug.LogWarning($"{eventName}에 해당하는 이벤트는 존재하지 않습니다.");
            else
                foreach (var action in Instance.EventDatabase[eventName])
                {
                    // ?. 문법: action이 null값이라면 null return
                    action?.Invoke(parameter);
                }
        }
    }
}