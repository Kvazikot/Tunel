using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Knot))]
public class KnotEditor : Editor
{
    float size = 1.5f;

    protected virtual void OnSceneGUI()
    {
        if (Event.current.type == EventType.Repaint)
        {
            size = 2.5f;
            Transform transform = ((Knot)target).transform;
            Vector3 t = ((Knot)target).t;
            float angleX = Mathf.Atan2(t.x, t.z) * Mathf.Rad2Deg;
            float angleZ = 90 - angleX;
            Quaternion q = Quaternion.AngleAxis(angleX, Vector3.up);
            
                //(Vector3.up, t);
            Handles.color = Color.yellow;

            Handles.ArrowHandleCap(
                0,
                transform.position,
                q,
                size,
                EventType.Repaint
            );
        
        }
    }
}