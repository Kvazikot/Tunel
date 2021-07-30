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
            float angleZ = Mathf.Atan2(t.x, t.y) * Mathf.Rad2Deg;
            Quaternion q1 = Quaternion.AngleAxis(angleX, Vector3.up);
            Quaternion q2 = Quaternion.AngleAxis(angleZ, Vector3.left);

            //(Vector3.up, t);
            Handles.color = Color.yellow;

            Handles.ArrowHandleCap(
                0,
                transform.position,
                q1 * q2,
                size,
                EventType.Repaint
            );
        
        }
    }
}