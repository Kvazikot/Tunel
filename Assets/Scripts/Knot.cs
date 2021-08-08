/*---------------------------------------------------------------------------------------+
  +-+-+---+-+-+ copytight by Kvazikot  +-+-+---+-+-+ 
  +-+-+ email: vsbaranov83@gmail.com 
  + - + - + github: http://github.com/Kvazikot/ 
  + - + - + --- + - + - + 
  Program for building a tunnel with a road. The road passes smoothly 
  into a wormhole.The texture of the walls of a wormhole is changing, examples 
  pictures can be viewed c: \ images \ path_to_images
  The tunnel can be built into the scene with the car. 

  Source creation date: 07/28/2021 
+ ------------------------------------------------- --------------------------------------- +
                +-+--+---+--+-+     MIT LICENSE     +-+--+---+--+-+                                   

Copyright (c) 2021 Kvazikot

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
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
    float[] angle = new float[3]{ 0f, 0f, 0f };
    [Range(0, 360)]
    public float angle1;
    [Range(0, 360)]
    public float angle2;
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
        angle = new float[3];
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
        angle = new float[3];
    }

    // Update is called once per frame
    void Update()
    {
        //return;
        if ((position != transform.position) || 
            (rotation!= transform.rotation) ||
            (scale != transform.localScale) ||
            (angle[0] != angle1) ||
            (angle[1] != angle2))
        {
            float a;
            Vector3 tt;
            transform.rotation.ToAngleAxis(out a, out tt);
            //tt.y = 0;
            t = tt;
            t = tt.normalized * SCALER ;
            Quaternion q;
            q = Quaternion.AngleAxis(angle1, Vector3.up);
            t = q * Vector3.forward * SCALER;

            if (tunnel!=null) tunnel.UpdateGeometry(this);
            //Debug.Log($"localScale = {transform.localScale.x}");
        }
        position = transform.position;
        rotation = transform.rotation;
        scale = transform.localScale;
        angle[0] = angle1;
        angle[1] = angle2;
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
