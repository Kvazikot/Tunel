using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knot : MonoBehaviour
{
    public TunelSegment tunel;
    public Vector3 t;
    public float angleDeegrees = 90f;
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;
    float  SCALER = 100f;

    // Start is called before the first frame update
    void Start()
    {
        t = new Vector3(Random.Range(1,0), Random.Range(1, 0));
        tunel = GameObject.Find("tunel").GetComponent<TunelSegment>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if ((position != transform.position) || 
            (rotation!= transform.rotation) ||
            (scale != transform.localScale))
        {
            float a;
            Vector3 tt;
            transform.rotation.ToAngleAxis(out a, out tt);
            //tt.y = 0;
            t = tt;
            t = tt.normalized * SCALER * Mathf.Abs(transform.localScale.x);
            tunel.UpdateSpline();
            //Debug.Log($"localScale = {transform.localScale.x}");
        }
        position = transform.position;
        rotation = transform.rotation;
        scale = transform.localScale;
        //Debug.Log("LateUpdate");
    }
}
