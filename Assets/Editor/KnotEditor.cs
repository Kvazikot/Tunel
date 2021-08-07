using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Knot))]
public class KnotEditor : Editor
{
    float size = 1.5f;
    int seg_switch = 1;


    public override void OnInspectorGUI() 
    {
        EditorGUILayout.Slider(size, 0, 100);
        Knot myTarget = (Knot)target;
        //int max_segments = myTarget.tunel.segments.Count;
        //myTarget.n_selected_segment = EditorGUILayout.Slider("n_selected_segment", myTarget.n_selected_segment, 0, max_segments);

        // Show default inspector property editor
        DrawDefaultInspector();
    }

    protected virtual void OnSceneGUI()
    {
        if (Event.current.type == EventType.MouseUp && Event.current.button == 0)
        {
            seg_switch = -seg_switch;
            Knot knot = (Knot)target;
            knot.SetSelectedSegment(knot.GetSelectedSegment() + seg_switch );
            Debug.Log("selected segment " + knot.GetSelectedSegment());
        }

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