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
-------------------------------------------------------------------------------
====================== 08.07.21 ========================= ==========
-----Topic: Implementation of Bezie splines in Tunnel project
Make the point look like a textured quad instead of a sphere.
This will reduce the number of cycles wasted on unnecessary geometry.
Add the ability to build Bezier splines, two more control points.
De Casteljau's algorithm.
The interpolation formula is on Wikipedia. Matrices are not needed there.
Hermitian splines can be retained, or all points can be interpolated with a cubic spline.

-----Topic: Implementation of Bezie splines
1.Create a BezieKnot control point class
Control points are red.
Prepare a texture for a quad point or take a gradient texture
You can write a shader program to do this.
Lines from control points can be displayed by hatching(dotted line)
The spline is colored red.
2. Prepare a prefarb for the bezie knot.
Create a quad and a shader for it with a circle with a border, you can make a gradient fill.
3. Create the BezieSpline class.
This method creates a point by instantiating prefarb.
How many knot points you need?
Transfer point cache logic to BezieSpline.
- Bezie knots can only be shown for the selected segment.
4. The spline interface implements the BezieSplineEditor class with an AddKnot button.
// https://pages.mtu.edu/~shene/COURSES/cs3621/NOTES/spline/Bezier/de-casteljau.html

*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BezieSpline : MonoBehaviour
{
    public BezieKnot knot;
    float[] beta = new float[1];
    float[] coefs_x = new float[1];
    float[] coefs_y = new float[1];
    float[] coefs_z = new float[1];
    const int NUM_CHACHED_POINTS = 1000;
    public Vector3[] points = new Vector3[NUM_CHACHED_POINTS];
    // Start is called before the first frame update
    void Start()
    {
        points = new Vector3[NUM_CHACHED_POINTS];
    }

    public float De_casteljau(float t, ref float[] coefs)
    {
        beta = coefs;
        int n = beta.Length;
        for (int j = 1; j < n; j++)
        for (int k = 0; k < (n - j); k++)
           beta[k] = beta[k] * (1 - t) + beta[k + 1] * t;                
        return beta[0];
    }


    public void BuildSpline()
    {
        int beta_len = transform.childCount;
        if (beta_len != coefs_x.Length)
        {
            coefs_x = new float[beta_len];
            coefs_y = new float[beta_len];
            coefs_z = new float[beta_len];
            beta = new float[beta_len];
        }

        //fill arrays
        for (int i = 0; i < beta_len; i++)
        {
            coefs_x[i] = transform.GetChild(i).transform.position.x;
            coefs_y[i] = transform.GetChild(i).transform.position.y;
            coefs_z[i] = transform.GetChild(i).transform.position.z;
        }

        //compute spline points
        float step = 1f / NUM_CHACHED_POINTS;
        points = new Vector3[NUM_CHACHED_POINTS+1];
        int cnt = 0;
        for (float t = 0; t < 1; t += step)
        {
            points[cnt].x = De_casteljau(t, ref coefs_x);
            points[cnt].y = De_casteljau(t, ref coefs_y);
            points[cnt].z = De_casteljau(t, ref coefs_z);
            cnt++; 
        }



    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnDrawGizmos()
    {
        for (int i = 1; i < points.Length; i++)
        {
            Gizmos.DrawLine(points[i - 1], points[i]);
            //Debug.Log($"points[i - 1] = {points[i - 1]}");
        }
        Gizmos.color = Color.yellow;
        for(int i=1; i < transform.childCount; i++)
            Gizmos.DrawLine(transform.GetChild(i - 1).transform.position,
                            transform.GetChild(i - 0).transform.position);
    }

    public void DeleteKnots()
    { 
        
    }
}
