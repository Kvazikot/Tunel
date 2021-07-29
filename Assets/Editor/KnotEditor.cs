using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Knot))]
public class KnotEditor : Editor
{
    float size = 5f;

    protected virtual void OnSceneGUI()
    {
        if (Event.current.type == EventType.Repaint)
        {
            Transform transform = ((Knot)target).transform;
            Handles.color = Handles.xAxisColor;
            Handles.ArrowHandleCap(
                0,
                transform.position ,
                transform.rotation ,
                size,
                EventType.Repaint
            );
        
        }
    }
}