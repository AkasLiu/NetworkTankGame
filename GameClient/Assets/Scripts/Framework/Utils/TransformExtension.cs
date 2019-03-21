using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformExtension
{
    public static void AddClickEventListener(this Transform t, ClickEventTriggerListener.EventPosDelegate action)
    {
        ClickEventTriggerListener ev = ClickEventTriggerListener.Get(t);
        if (ev != null)
        {
            ev.onClick = action;
        }
    }
}
