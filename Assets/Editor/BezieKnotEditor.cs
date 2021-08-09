using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BezieKnot))]
public class BezieKnotEditor : Editor
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected virtual void OnSceneGUI()
    {
        if (Event.current.type == EventType.MouseUp && Event.current.button == 0)
        {
            Debug.Log("mouse up ");
            BezieKnot knot = (BezieKnot)target;
            BezieSpline spline = (BezieSpline)knot.transform.parent.GetComponent<BezieSpline>();
            knot.Update();
            spline.BuildSpline();



        }

        if (Event.current.type == EventType.MouseMove && Event.current.button == 0)
        {
        }
            
    }
}
