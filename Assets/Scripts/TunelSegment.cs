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


using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using UnityEditor;
using UnityEngine;



public class Tunel
{
    public Tunel(Transform parentNode)
    { 
        for (int i = 1; i < parentNode.childCount; i++)
        {
            Transform p1 = parentNode.GetChild(i-1  );
            Transform p2 = parentNode.GetChild(i);
            Debug.Log(p1.name);
            TunelSegment segment = new TunelSegment();
            segment.tA = p1;
            segment.tB = p2;
            segment.Start();
        }

    }

}


public class TunelSegment : MonoBehaviour
{
    static Tunel tunel_singleton = null;

    [Range(1, 1000)]
    public int resolution = 1000;
    [Range(1, 1000)]
    public float radius = 1f;
    [Range(1, 1000)]
    public int num_base_segments = 20;
    [Range(1, 1000)]
    public int num_side_segments = 20;

    public Transform tA;
    public Transform tB;
    public int n_frame = 0;
    public Matrix4x4 coef;
    public Vector3 t1 = new Vector3(0f, 0, 1.5f);
    public Vector3 t0 = new Vector3(1f, 0, -1f);
    public Vector3[] points = new Vector3[1000 + 1];
    public float _seconds;
    const bool LOG_SPLINE_VALUES = false; 
   
    // Start is called before the first frame update
    public void Start()
    {
        if (tunel_singleton == null)
        {
            tunel_singleton = new Tunel(transform);
            return;
        }
        //set up sphere rotation equal to t0
        t1 = new Vector3(0f, 0, 1.5f);
        t0 = new Vector3(1f, 0, -1f);
        Knot A = tA.GetComponent<Knot>();
        Knot B = tB.GetComponent<Knot>();

        points = new Vector3[resolution+1];
        A.t = t0;
        B.t = t1;

        //copy tangent vectors from rotation of tA and tB       
        coef = CalculateSpline(tA.position, tB.position, t0, t1);
        Debug.Log("result of function: ");
        Debug.Log("+-+--+---+ Tunel::CalculateSpline() +-+--+---+");
        Debug.Log($"A={tA.position} B={tB.position}");
        Debug.Log($"t0={t0} t1={t1}");
        Debug.Log($"coefficient of solution Matrix for Hermit spline");
        Debug.Log($"{coef}");
        Debug.Log("result of function: ");
        Debug.Log("+-+--+---+ Tunel::DrawSpline() +-+--+---+"); // new line

      


            //DrawSpline(tA.position, tB.position, t0, t1, coef, LOG_SPLINE_VALUES);


        }

    public void UpdateSpline()
    {
        if ((n_frame % 10) == 0)
        {
            //set up sphere rotation equal to t0
            Knot A = tA.GetComponent<Knot>();
            Knot B = tB.GetComponent<Knot>();
            t0 = A.t;
            t1 = B.t;

            //Debug.Log($"UpdateSpline() t0={t0} t1={t1} ");
            coef = CalculateSpline(tA.position, tB.position, t0, t1);
        }
    }

    Matrix4x4 CalculateSpline(Vector3 startP, Vector3 endP, 
                              Vector3 tangentVec1, Vector3 tangentVec2)
    {
        // 0 is a first point startP
        // 1 is a second point endP

        // calculate tangent vectors
        //set x(0) and y(0)
        float x0 = tA.position.x;
        float y0 = tA.position.z;
        float z0 = Mathf.Max(1e-5f, tA.position.y);

        //set x(1) and y(1)
        float x1 = tB.position.x;
        float y1 = tB.position.z;
        float z1 = Mathf.Max(1e-5f, tB.position.y);

        // set x'(0) and y'(0)
        float dUx = 1f / (x1 - x0);
        float dUy = 1f / (y1 - y0);
        float dUz = 1f / (z1 - z0);
        float xx0 = t0.x * dUx;
        float yy0 = t0.z * dUy;
        float zz0 = t0.y * dUz;
        float xx1 = t1.x * dUx;
        float yy1 = t1.z * dUy;
        float zz1 = t1.y * dUz;

        Matrix4x4 solution = new Matrix4x4();

        Matrix<double> A = DenseMatrix.OfArray(new double[,] {
        {1, 0, 0, 0},
        {1, 1, 1, 1},
        {0, 1, 0, 0},
        {0, 1, 2, 3} });

        Matrix<double> v = DenseMatrix.OfArray(new double[,] {
            {x0, y0, z0},
            {x1, y1, z1},
            {xx0, yy0, zz0},
            {xx1, yy1, zz1}});

        var y = A.Solve(v);

        //Debug.Log($"y={ y }");
        solution[0, 0] = (float)y[0, 0];
        solution[0, 1] = (float)y[0, 1];
        solution[0, 2] = (float)y[0, 2];
        solution[1, 0] = (float)y[1, 0];
        solution[1, 1] = (float)y[1, 1];
        solution[1, 2] = (float)y[1, 2];
        solution[2, 0] = (float)y[2, 0];
        solution[2, 1] = (float)y[2, 1];
        solution[2, 2] = (float)y[2, 2];
        solution[3, 0] = (float)y[3, 0];
        solution[3, 1] = (float)y[3, 1];
        solution[3, 2] = (float)y[3, 2];

        float U = 0;         //parameter U lies in interval [0,1]
        float ax, bx, cx, dx;
        float ay, by, cy, dy;
        float az, bz, cz, dz;

        ax = solution[0, 0];
        ay = solution[0, 1];
        az = solution[0, 2];
        bx = solution[1, 0];
        by = solution[1, 1];
        bz = solution[1, 2];
        cx = solution[2, 0];
        cy = solution[2, 1];
        cz = solution[2, 2];
        dx = solution[3, 0];
        dy = solution[3, 1];
        dz = solution[3, 2];

        Vector3 p0 = tA.position, p = p0;
        float ustep = 1f / resolution;
        for (int i = 0; i < resolution; i++)
        {
            //equation for Hermitt spline for 2D case
            //when add additional dimension for 3D case
            p.x = ax + bx * U + cx * U * U + U * U * U * dx;
            p.z = ay + by * U + cy * U * U + U * U * U * dy;
            p.y = az + bz * U + cz * U * U + U * U * U * dz;
            U += ustep;
            p0 = p;
            points[i] = p0;
        }

        return solution;
    }

    // Arrange the light sources inside the tunnel
    // tunnel lighting color changes from green to blue
    // and from blue to red
    void setupLights()
    { 
    
    }

    void DrawSpline(Vector3 startP, Vector3 endP, Vector3 tangentVec1, Vector3 tangentVec2,
                    Matrix4x4 coeff, bool bLogSpline=false)
    {
        for (int i = 1; i < resolution; i++)
           Gizmos.DrawLine(points[i-1], points[i]);
    }

    
    void drawWireSegments() // on DrawGizmos call
    {
        //1. Tangent of curve t => Euler angles

        //2. rotation of the plane of the base of the tunnel (circle) to EulerAngles
        float pathLen = 0, L = 0, gapLen = 0;

        for (int i = 1; i < resolution; i++)
        {
            float length = (points[i] - points[i - 1]).magnitude;
            L += length;
        }
        
        pathLen = L;
        float segPerUnit = 1f;
        float n_total_segments =  (pathLen * segPerUnit);
        gapLen = pathLen / n_total_segments;

        for (int i = 1; i < resolution; i++)
        {
            Gizmos.color = Color.white;
            float length = (points[i] - points[i - 1]).magnitude;
            //int n_segm = 
            L += length;
            if (L > gapLen)
            {
                Handles.DrawWireDisc(points[i - 1], points[i] - points[i - 1], radius);
                L = 0;
            }
            if (length > gapLen)
            {
                float n_sub_segments = length / gapLen;
                for (float j = 0; j < n_sub_segments; j+=1f)
                {
                    // interpolate coordinate
                    Vector3 p = Vector3.Lerp(points[i - 1], points[i], j / n_sub_segments);
                    // interpolate normal
                    Handles.DrawWireDisc(p, points[i] - points[i - 1], radius);
                    Debug.Log("n_sub_segments branch!");
                }            
            }
        }

        //3. Create Minimal Mesh object.
        // getting vertices with a given segmentation level N_hor_seg
        // between adjacent points of the guide curve

        //3.1 Create shader for minimal mesh object
        // pass guiding curve points to shader for tesselation

        //4. Constructing a pipe with 0 wall thickness

        //5. Constructing a pipe with a WallThickness of the wall thickness

        //6. Light arrangement
    }

    void OnDrawGizmos()
    {
        if ((n_frame % 2) != 0)
            return;
        
            // draw tangential vectors at points p (0) and p (1)
        float scaler = 1f;
        //t0 = t0.normalized;
        Vector3 T0 = tA.position + t0 * scaler;
        Vector3 T1 = tB.position + t1 * scaler;
        Gizmos.color = Color.green;
        Gizmos.DrawLine(tA.position, T0);
        Gizmos.DrawLine(tB.position, T1);
        Handles.DrawDottedLine(T1, T1, 20);
        Handles.DrawDottedLine(T0, T0, 20);

        // draw a spline
        Gizmos.color = Color.yellow;
        DrawSpline(tA.position, tB.position, t0, t1, coef);

        // draw the walls
        drawWireSegments();

    }

    void Update()
    {      
        n_frame++;        
        if (n_frame > int.MaxValue/2) n_frame = 0;
    }
}
