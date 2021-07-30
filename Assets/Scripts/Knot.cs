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

    // Start is called before the first frame update
    void Start()
    {
        t = new Vector3(Random.Range(1,0), Random.Range(1, 0));
        tunel = GameObject.Find("tunel").GetComponent<TunelSegment>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if ((position != transform.position) || (rotation!= transform.rotation))
        {
            tunel.UpdateSpline();
            float a;
            Vector3 tt;
            transform.rotation.ToAngleAxis(out a, out tt);
            tt.y = 0;
            t = tt;
            Debug.Log($"tt = {tt}");
        }
        position = transform.position;
        rotation = transform.rotation;
        //Debug.Log("LateUpdate");
    }
}
