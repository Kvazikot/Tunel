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

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class MinimalMesh
{
    public Mesh mesh;

    public MinimalMesh(int numVertices = 3)
    {
        mesh = new Mesh();
        Vector3[] vertices = new Vector3[]
        {
            new Vector3(0,0,0),
            new Vector3(0,0,10f+1e-6f),
            new Vector3(10f+231e-6f,0,0)
        };
        mesh.vertices = vertices;

        int[] triangles = new int[]
        {
            0, 1, 2
        };
        mesh.triangles = triangles;

        Vector3[] normals = new Vector3[]
        {
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward
        };
        mesh.normals = normals;

        Vector2[] uv = new Vector2[]
        {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(0, 1)
        };
        mesh.uv = uv;

        if (numVertices > 3)
        {
            vertices = new Vector3[numVertices + 1];
            triangles = new int[numVertices * numVertices + 1];
            normals = new Vector3[numVertices + 1];
            uv = new Vector2[numVertices + 1];
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.normals = normals;

        }

    }

}

public class Tunel : MonoBehaviour
{
    TunelSegment[] segments = null;
    public int n_frame = 0;
    public Tunel()
    {
    

    }
    // Start is called before the first frame update
    void Start()
    {
        //создать LHC
        segments = new TunelSegment[transform.childCount];
        Vector3 t = new Vector3(0, 0, 1);
        Quaternion q = new Quaternion();
        q.eulerAngles = new Vector3(0, 0, 90);
        q = Quaternion.AngleAxis(90, Vector3.up);

        Debug.Log($"LHC ---------START--------");
        for (int i = 1; i < transform.childCount; i++)
        {
            Vector3 t0 =  q * t;
            Vector3 t1 =  q * t0;
            t = t0;
            Debug.Log($"LHC t0 = ${t0} t1 = ${t1}");
            Transform p1 = transform.GetChild(i - 1);
            p1.GetComponent<Knot>().t = t0;
            p1.GetComponent<Knot>().UpdateRotation();
            Transform p2 = transform.GetChild(i);
            p2.GetComponent<Knot>().t = t1;
            p2.GetComponent<Knot>().UpdateRotation();
            Debug.Log(p1.name);
            TunelSegment segment = new TunelSegment();
            segments[i-1] = segment;
            segment.tA = p1;
            segment.tB = p2;
            segment.Start();
        }
        Debug.Log($"LHC ---------END-------");

        // create minimal mesh fro tunel 
        MeshRenderer meshRenderer = transform.GetComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = new Material(Shader.Find("Standard"));
        //MinimalMesh m = new MinimalMesh();
        //MeshFilter mf = transform.GetComponent<MeshFilter>();
        //mf.mesh = m.mesh;

        //DrawSpline(tA.position, tB.position, t0, t1, coef, LOG_SPLINE_VALUES);
        // 2.9 Create all tunel in CPU

        //3. Create Minimal Mesh object.
        // getting vertices with a given segmentation level N_hor_seg
        // between adjacent points of the guide curve

        //3.1 Create shader for minimal mesh object
        // pass guiding curve points as mesh points to shader for tesselation

        //3.2 interpolate t0 and t1 tangent vectors on CPU
        //and pass them to GPU as uniform variables or as uv coordinates

        //4. Constructing a pipe with 0 wall thickness

        //5. Constructing a pipe with a WallThickness of the wall thickness

        //6. Light arrangement

        // 7. animate camera movement thru the tunell
    }


    void drawWireSegments(TunelSegment seg) // on DrawGizmos call
    {
        //1. Tangent of curve t => Euler angles
        if (transform.childCount == 0) return;
        
        //2. rotation of the plane of the base of the tunnel (circle) to EulerAngles
        float pathLen = 0, L = 0, gapLen = 0;

        for (int i = 1; i < seg.resolution; i++)
        {
            float length = (seg.points[i] - seg.points[i - 1]).magnitude;
            L += length;
        }

        pathLen = L;
        float segPerUnit = 1f;
        float n_total_segments = (pathLen * segPerUnit);
        gapLen = pathLen / n_total_segments;

        for (int i = 1; i < seg.resolution; i++)
        {
            Gizmos.color = Color.white;
            float length = (seg.points[i] - seg.points[i - 1]).magnitude;
            //int n_segm = 
            L += length;
            if (L > gapLen)
            {
                Handles.DrawWireDisc(seg.points[i - 1], seg.points[i] - seg.points[i - 1], seg.radius);
                L = 0;
            }
            /*
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
            */
        }

         
     
    }

    void OnDrawGizmos()
    {
        if ((n_frame % 2) != 0)
            return;

        if (segments == null)  return;

        int n_segments = transform.childCount - 1;

        for (int s = 0; s < n_segments; s++)
        {

            TunelSegment seg = segments[s];
            if (seg == null) return;
            Debug.Log("Draw segment " + s);
            // draw tangential vectors at points p (0) and p (1)
            float scaler = 1f;
            //t0 = t0.normalized;
            Vector3 T0 = seg.tA.position + seg.t0 * scaler;
            Vector3 T1 = seg.tB.position + seg.t1 * scaler;
            Gizmos.color = Color.green;
            Gizmos.DrawLine(seg.tA.position, T0);
            Gizmos.DrawLine(seg.tB.position, T1);
            Handles.DrawDottedLine(T1, T1, 20);
            Handles.DrawDottedLine(T0, T0, 20);

            // draw a spline
            Gizmos.color = Color.yellow;
            seg.DrawSpline(seg.tA.position, seg.tB.position, seg.t0, seg.t1, seg.coef);

            // draw the walls
            drawWireSegments(seg);
        }

    }

    public void UpdateGeometry()
    {
        for (int i = 1; i < transform.childCount; i++)
        {
        }
    }

    // Update is called once per frame
    void Update()
    {
        n_frame++;
        if (n_frame > int.MaxValue / 2) n_frame = 0;
    }
}

