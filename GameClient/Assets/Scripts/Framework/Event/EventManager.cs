using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : Singleton<EventManager>
{
    public delegate void OnEventDlg(object param);

    // Dictionary for proto id to callback list
    Dictionary<int, List<OnEventDlg>> _eventName2CallbackListDict = new Dictionary<int, List<OnEventDlg>>();

    public void RegisterEvent(string eventName, OnEventDlg callback)
    {
          
    }

    public void UnregistEvent(string eventName, OnEventDlg callback)
    {
       
    }

    public void FireEvent(string eventName, object param)
    {

    }
}
