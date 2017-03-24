using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

/// <summary>
/// SSG Eventsystem
/// </summary>
public class EventManager : SingletonBehaviour<EventManager>
{
    public delegate void EventDelegate(IEvent EventObject);

    private Dictionary<int, List<EventDelegate>> EventListenerMap = new Dictionary<int, List<EventDelegate>>();
    private Queue<KeyValuePair<float, IEvent>> EventQueue = new Queue<KeyValuePair<float, IEvent>>();

    public override void AwakeSingleton()
    {
        DontDestroyOnLoad(gameObject);
    }

    public bool AddEventListener(EventDelegate Function, int EventType)
    {
        List<EventDelegate> EventList;
        if (!EventListenerMap.ContainsKey(EventType))
        {
            EventList = new List<EventDelegate>();
            EventListenerMap[EventType] = EventList;
        }
        else {
            EventList = EventListenerMap[EventType];
        }

        if (EventList.Contains(Function))
        {
            UnityEngine.Debug.LogError("Attempting to double-register a delegate");
            return false;
        }
        else
        {
            EventList.Add(Function);
            return true;
        }

    }

    public bool RemoveListener(EventDelegate Function, int EventType)
    {
        if (EventListenerMap.ContainsKey(EventType))
        {
            var findIt = EventListenerMap[EventType];
            if (findIt != null)
            {
                foreach (EventDelegate delegateFunction in findIt.ToArray())
                {
                    if (delegateFunction == Function)
                    {
                        findIt.Remove(delegateFunction);
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public void TriggerEvent(IEvent EventObject, float delayTime = 0)
    {
        EventQueue.Enqueue(new KeyValuePair<float, IEvent>(delayTime, EventObject));
    }

    void Update()
    {
        try
        {
            Queue<KeyValuePair<float, IEvent>> tempQueue = new Queue<KeyValuePair<float, IEvent>>();
            while (EventQueue.Count > 0)
            {
                KeyValuePair<float, IEvent> EventObject = EventQueue.Dequeue();
                if (EventObject.Key > 0)
                {
                    tempQueue.Enqueue(new KeyValuePair<float, IEvent>(EventObject.Key - Time.deltaTime, EventObject.Value));
                }
                else
                {
                    Execute(EventObject.Value);
                }
            }
            EventQueue = tempQueue;
        }
        catch (Exception ex)
        {
            // Get stack trace for the exception with source file information
            var st = new StackTrace(ex, true);
            // Get the top stack frame
            var frame = st.GetFrame(0);
            // Get the line number from the stack frame
            var line = frame.GetFileLineNumber();
            var filename = frame.GetFileName();

            UnityEngine.Debug.LogError(ex.GetType().Name + ": " + ex.Message + "\n\t at " + filename + ", Line " + line);
        }
    }

    private void Execute(IEvent EventObject)
    {
        if (EventListenerMap.ContainsKey(EventObject.GetIdentity()))
        {
            var findIt = EventListenerMap[EventObject.GetIdentity()];
            if (findIt != null)
            {
                foreach (EventDelegate delegateFunction in findIt.ToArray())
                {
                    try
                    {
                        delegateFunction(EventObject);
                    }
                    catch (Exception ex)
                    {
                        // Get stack trace for the exception with source file information
                        var st = new StackTrace(ex, true);
                        // Get the top stack frame
                        var frame = st.GetFrame(0);
                        // Get the line number from the stack frame
                        var line = frame.GetFileLineNumber();
                        var filename = frame.GetFileName();

                        UnityEngine.Debug.LogError(ex.GetType().Name + ": " + ex.Message + "\nFor event " + EventObject.GetType().Name + " in " + delegateFunction.Target.GetType().Name + "." + delegateFunction.Method.Name + "\n\t at " + filename + ", Line " + line);

                    }
                }
            }
        }
    }
}
