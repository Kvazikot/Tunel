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


*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezieSpline : MonoBehaviour
{
    public BezieKnot knot;

    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DeleteKnots()
    { 
        
    }
}
