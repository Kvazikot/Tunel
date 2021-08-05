using UnityEngine;

public class Knot : MonoBehaviour
{
    public Tunel tunel = null;
    public Vector3 t;
    public float angleDeegrees = 90f;
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;
    public float SCALER = 1f;

    // Start is called before the first frame update
    void Start()
    {
        t = new Vector3(Random.Range(1,0), Random.Range(1, 0));
        tunel = GameObject.Find("tunel").GetComponent<Tunel>();
        transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
    }

    // Update is called once per frame
    void Update()
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
            if(tunel!=null) tunel.UpdateGeometry();
            //Debug.Log($"localScale = {transform.localScale.x}");
        }
        position = transform.position;
        rotation = transform.rotation;
        scale = transform.localScale;
        //Debug.Log("LateUpdate");
    }

    public void UpdateRotation()
    {
        //transform.rotation = q;
        //float a;
    }
}
