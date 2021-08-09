using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BezieSpline))]
public class BezieSplineEditor : Editor
{
    const int NUM_DIGITS_IN_NAME = 4;
    bool lockYAxis = false;

    // Start is called before the first frame update
    void Start()
    {       
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    string get_random_id_without_zeroes(int id_len)
    {
        string output="knot";
        for (int i = 0; i < id_len; i++)
        {
            int digit = Random.Range(1,9);
            output +=$"{digit}";
        }
        return output;
    }

    public static void SafeDestory(GameObject obj)
    {
        obj.transform.parent = null;
        obj.name = "$disposed";
        UnityEngine.Object.DestroyImmediate(obj);
        obj.SetActive(false);
    }

    public override void OnInspectorGUI()
    {
        BezieSpline spline = (BezieSpline)target;
        if (GUILayout.Button("AddKnot"))
        {
            
            Vector3 pos = new Vector3(Random.Range(-10,10), 0, Random.Range(-10, 10));
            GameObject newknot = new GameObject();
            newknot.name = get_random_id_without_zeroes(NUM_DIGITS_IN_NAME);
            newknot.transform.parent = spline.transform;
            newknot.transform.position = pos;
            newknot.AddComponent<BezieKnot>();
            if (spline.transform.childCount > 4)
                spline.BuildSpline();
        }
        if (GUILayout.Button("BuildSpline"))
            spline.BuildSpline();
        
        lockYAxis = GUILayout.Toggle(spline.flags.LockYAxis, "Lock translation in Y axis");
        spline.setFlags(lockYAxis);


        //int max_segments = myTarget.tunel.segments.Count;
        //myTarget.n_selected_segment = EditorGUILayout.Slider("n_selected_segment", myTarget.n_selected_segment, 0, max_segments);

            // Show default inspector property editor
        DrawDefaultInspector();
    }

}
