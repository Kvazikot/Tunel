using UnityEngine;

public class Knot : MonoBehaviour
{
    public Tunnel tunnel = null;
    public enum KnotType
    {
        Starting=0,
        Ending=1,
    }
    public Knot base_knot = null;
    public KnotType type;
    public Vector3 t;
    public Vector3 position;
    Quaternion rotation;
    Vector3 scale;
    [Range(0, 100)]
    public float SCALER = 1f;
    [Range(0, 360)]
    public float angle1;
    public int n_selected_segment = 0;
    public int n_segment = 0;
    public int seg_switch = 1;

    public Knot(Knot A, KnotType _type=0)
    {
        base_knot = A;
        seg_switch = 1;
        t = A.t;
        type = _type;
        SCALER = A.SCALER;
        tunnel = GameObject.Find("tunnel").GetComponent<Tunnel>();
        position = A.transform.position;
        rotation = A.transform.rotation;
        scale = A.transform.localScale;
    }

    public void SetSelectedSegment(int n)
    {
        n_selected_segment = n;
        if( base_knot!=null) base_knot.n_selected_segment = n;
        if (tunnel != null)
        {
            tunnel.n_selected_segment = n;
            //Debug.Log("SET SELECTED SEGMENT");
        }
        else
            Debug.Log("TUNNEL OBJECT REFERENCE IS NOT SET UP CORRECTLY");
    }

    public void SetMySegment(int n)
    {
        n_segment = n;
        if (base_knot != null) base_knot.n_segment = n;
        Debug.Log($"LHC SetMySegment = {n}");
    }

    public void SwitchSegment() { seg_switch = -seg_switch;  }

    public int GetMySegment() { return n_segment; }
    public int GetSelectedSegment() { return n_selected_segment; }

    // Start is called before the first frame update
    void Start()
    {
        tunnel = GameObject.Find("tunnel").GetComponent<Tunnel>();
        transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
        position = transform.position;
        rotation = transform.rotation;
        scale = transform.localScale;
        base_knot = this;
        seg_switch = 1;
    }

    // Update is called once per frame
    void Update()
    {
        //return;
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
            if(tunnel!=null) tunnel.UpdateGeometry(this);
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
        rotation = q;

    }
}
