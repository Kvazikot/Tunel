using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BezieSpline))]
public class BezieSplineEditor : Editor
{
    const int NUM_DIGITS_IN_NAME = 4;

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
        int a;
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
        if (GUILayout.Button("AddKnot"))
        {
            Vector3 pos = new Vector3(Random.Range(-10,10), 0, Random.Range(-10, 10));
            BezieSpline spline = (BezieSpline)target;

            GameObject newknot = new GameObject();
            newknot.name = get_random_id_without_zeroes(NUM_DIGITS_IN_NAME);
            newknot.transform.parent = spline.transform;
            newknot.transform.position = pos;
            newknot.AddComponent<BezieKnot>();



        }

            //int max_segments = myTarget.tunel.segments.Count;
            //myTarget.n_selected_segment = EditorGUILayout.Slider("n_selected_segment", myTarget.n_selected_segment, 0, max_segments);

            // Show default inspector property editor
            DrawDefaultInspector();
    }

    protected virtual void OnSceneGUI()
    {
        if (Event.current.type == EventType.MouseUp && Event.current.button == 0)
        {
            Debug.Log("mouse up " );
        }
    }
}
