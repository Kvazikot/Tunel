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
            size = ((Knot)target).SCALER;
            Transform transform = ((Knot)target).transform;
            Vector3 A = ((Knot)target).t;
            Vector3 b = new Vector3(0, 0, 1);
            float gamma = Mathf.Rad2Deg * Mathf.Atan2(A.y, Mathf.Sqrt(A.z * A.z + A.x * A.x));
            float beta = Mathf.Rad2Deg * Mathf.Atan2(A.x, A.z);
            Quaternion q1 = Quaternion.AngleAxis(beta, Vector3.up);
            Quaternion q2 = Quaternion.AngleAxis(gamma, Vector3.right);
            b = q1 * b;
            b = q2 * b;
            // проверим совпадают ли они с целевым вектором
            A = A.normalized;
            //Debug.Log($"LHC magnitude of diff=(b-A)= {(b - A).magnitude}");

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