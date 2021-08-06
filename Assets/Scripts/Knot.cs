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
        tunel = GameObject.Find("tunel").GetComponent<Tunel>();
        transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
    }

    // Update is called once per frame
    void Update()
    {
        return;
        if ((position != transform.position) || 
            (rotation!= transform.rotation) ||
            (scale != transform.localScale))
        {
            float a;
            Vector3 tt;
            transform.rotation.ToAngleAxis(out a, out tt);
            //tt.y = 0;
            t = tt;
            t = tt.normalized * SCALER ;
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
        Vector3 A = t;
        Vector3 b = new Vector3(0, 0, 1);
        float alfa = Mathf.Rad2Deg * Mathf.Atan2(A.y, Mathf.Sqrt(A.z * A.z + A.x * A.x));
        float beta = Mathf.Rad2Deg * Mathf.Atan2(A.x, A.z);
        Quaternion q1 = Quaternion.AngleAxis(beta, Vector3.up);
        Quaternion q2 = Quaternion.AngleAxis(alfa, Vector3.right);
        Quaternion q = Quaternion.Euler(alfa, beta, 0);
        b = q1 * b;
        b = q2 * b;
        // проверим совпадают ли они с целевым вектором
        A = A.normalized;
        //Debug.Log($"LHC b={b}  A={A}");
        //Debug.Log($"LHC magnitude of diff=(b-A)= {(b - A).magnitude}");
        transform.rotation = q;

    }
}
