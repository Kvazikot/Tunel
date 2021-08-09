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
using UnityEngine;
using UnityEditor;

public class BezieKnot : MonoBehaviour
{
    public bool lockYTranslate;
    Vector3 positionState0 = new Vector3();
    // Start is called before the first frame update
    void Start()
    {
        lockYTranslate = true;


    }

    public int getMyIndex()
    {
        for (int i = 0; i < transform.parent.childCount; i++)
            if (transform.parent.GetChild(i) == this.transform)
                return i;
        return -1;
    }

    // Update is called once per frame
    public void Update()
    {
        if (lockYTranslate)
        {
            transform.position = new Vector3(transform.position.x,
                                             positionState0.y,
                                              transform.position.z);
            positionState0 = transform.position;
        }


    }
    public void SetlockYAxis(bool flag)
    {
        lockYTranslate = flag;
        positionState0 = transform.position;
    }
}

// The icon has to be stored in Assets/Gizmos

public class BezieKnotGizmoDrawer
{
    [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected | GizmoType.Pickable)]
    static void DrawGizmoForBezieKnot(BezieKnot scr, GizmoType gizmoType)
    {
        Vector3 position = scr.transform.position;

        if (Vector3.Distance(position, Camera.current.transform.position) > 10f)
        {
            Vector3 scale = Gizmos.matrix.lossyScale;
            //Debug.Log($"lossyScale={scale}");
            //TODO: get Editor main window projection matrix and translate the label
            // or second option - scale the icon according to the zoom of editor window
            Gizmos.DrawIcon(position, "red_knot.png", true);
            Handles.Label(position * scale.x, $"{scr.getMyIndex()}");
        }
    }
}