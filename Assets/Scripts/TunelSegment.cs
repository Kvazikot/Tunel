/*---------------------------------------------------------------------------------------+
  +-+--+---+--+-+  copytight by Kvazikot (twitter.com/Kvazikot) +-+--+---+--+-+          |
  +-+--+ email: vsbaranov83@gmail.com                                                    |  
  +-+--+ github: http://github.com/Kvazikot/                                             |  
  +-+--+---+--+-+ ѕрограмма построени€ тоннел€ с дорогой. ƒорога плавно переходит        |
  в кротовую нору. “екстура стенок кротовой норы мен€етс€, примеры                       |
  картинок можно посмотреть  c:\images\путь_к_кратинкам                                  |
  “оннель можно будет встроить в сцену с автомобилем.                                    |
  ƒата создани€ исходника: 28.07.2021                                                    |
+----------------------------------------------------------------------------------------+
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

public class TunelSegment : MonoBehaviour
{
    public Transform tA;
    public Transform tB;
    public Matrix4x4 coef;
    public Vector3 t0 = new Vector3(0.1f, 0, 0.5f);
    public Vector3 t1 = new Vector3(0.9f, 0, -0.5f);
    public float _seconds;
    const bool LOG_SPLINE_VALUES = true; 
   
    [Range(1, 1000)]
    public int resolution = 1000;

    // Start is called before the first frame update
    void Start()
    {
        //copy tangent vectors from rotation of tA and tB       
        t0 = tA.rotation.eulerAngles.normalized;
        t1 = tB.rotation.eulerAngles.normalized;

        coef = CalculateSpline(tA.position, tB.position, t0, t1);
        Debug.Log("result of function: ");
        Debug.Log("+-+--+---+ Tunel::CalculateSpline() +-+--+---+");
        Debug.Log($"A={tA.position} B={tB.position}");
        Debug.Log($"t0={t0} t1={t1}");
        Debug.Log($"coefficient of solution Matrix for Hermit spline");
        Debug.Log($"{coef}");
        Debug.Log("result of function: ");
        Debug.Log("+-+--+---+ Tunel::DrawSpline() +-+--+---+");
        
        DrawSpline(tA.position, tB.position, t0, t1, coef, LOG_SPLINE_VALUES);


    }

    public void UpdateSpline()
    {
        coef = CalculateSpline(tA.position, tB.position, t0, t1);
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

        //set x(1) and y(1)
        float x1 = tB.position.x;
        float y1 = tB.position.z;

        // set x'(0) and y'(0)
        float dUx = 1f / (x1 - x0);
        float dUy = 1f / (y1 - y0);
        float xx0 = t0.x * dUx;
        float yy0 = t0.z * dUy;
        float xx1 = t1.x * dUx;
        float yy1 = t1.z * dUy;

        Matrix4x4 solution = new Matrix4x4();

        Matrix<double> A = DenseMatrix.OfArray(new double[,] {
        {1, 0, 0, 0},
        {1, 1, 1, 1},
        {0, 1, 0, 0},
        {0, 1, 2, 3} });

        Matrix<double> v = DenseMatrix.OfArray(new double[,] {
            {x0, y0},
            {x1, y1},
            {xx0, yy0},
            {xx1, yy1}});

        var y = A.Solve(v);
        Debug.Log($"y={ y }");
        solution[0, 0] = (float)y[0, 0];
        solution[0, 1] = (float)y[0, 1];
        solution[1, 0] = (float)y[1, 0];
        solution[1, 1] = (float)y[1, 1];
        solution[2, 0] = (float)y[2, 0];
        solution[2, 1] = (float)y[2, 1];
        solution[3, 0] = (float)y[3, 0];
        solution[3, 1] = (float)y[3, 1];

        return solution;
    }

    // –асстановка источников света внутри тоннел€
    // цвет подсветки тунел€ мен€етс€ от зеленого к синему
    // и от синего к красному
    void setupLights()
    { 
    
    }

    void DrawSpline(Vector3 startP, Vector3 endP, Vector3 tangentVec1, Vector3 tangentVec2,
                    Matrix4x4 coeff, bool bLogSpline=false)
    {
        
        float U = 0;         //parameter U lies in interval [0,1]
        float ax, bx, cx, dx;
        float ay, by, cy, dy;

        ax = coeff[0, 0];
        ay = coeff[0, 1];
        bx = coeff[1, 0];
        by = coeff[1, 1];
        cx = coeff[2, 0];
        cy = coeff[2, 1];
        dx = coeff[3, 0];
        dy = coeff[3, 1];

        Vector3 p0 = tA.position, p = p0;
        float ustep = 1f / resolution;
        if ( bLogSpline ) Debug.Log($"[DrawSpline] ustep={ustep}");
        for (int i = 0; i < resolution; i++)
        {
            //equation for Hermitt spline for 2D case
            //when add additional dimension for 3D case
            p.x = ax + bx * U + cx * U * U + U * U * U * dx;
            p.z = ay + by * U + cy * U * U + U * U * U * dy;
            U += ustep;
            if ( !bLogSpline ) Gizmos.DrawLine(p0, p);
            p0 = new Vector3(p.x, 0, p.z);
            if( bLogSpline )
                Debug.Log($"[DrawSpline] p={p} U={U}");


        }
    }

    void drawWalls(Vector3 p, float R, Vector3 t)
    { 
        //1. Tangent of curve t = > Euler angles

        //2. поворот плоскости основани€ тонел€ (окружности) на EulerAngles

        //3. получение вершин с заданным уровенем сегментации N_hor_seg
        // между соседними точками направл€ющей кривой

        //4. ѕостроение трубы с 0 толщиной стенки
        
        //5. ѕостроение трубы с WallThickness толщиной стенки

        //6. –асстановка света
    }

    void OnDrawGizmos()
    {


        // нарисовать тангенциальные векторы в точках p(0) и p(1)
        float scale = 2;
        //t0 = t0.normalized;
        Vector3 T0 = tA.position + t0 * scale;
        Vector3 T1 = tB.position + t1 * scale;
        Gizmos.color = Color.green;
        Gizmos.DrawLine(tA.position, T0);
        Gizmos.DrawLine(tB.position, T1);
        Handles.DrawDottedLine(T1, T1, 20);
        Handles.DrawDottedLine(T0, T0, 20);

        // нарисовать сплайн
        Gizmos.color = Color.yellow;
        DrawSpline(tA.position, tB.position, t0, t1, coef);

        // нарисовать обмотку
        float radius = 3;
        Gizmos.color = Color.white;
        Handles.DrawWireDisc(tA.position, new Vector3(1, 0, 0), radius);

    }

    void Update()
    {
        //copy tangent vectors from rotation of tA and tB       
        if ((t0 != tA.rotation.eulerAngles.normalized) ||
             (t1 != tB.rotation.eulerAngles.normalized) )
            {
            t0.x = tA.rotation.eulerAngles.normalized.x;
            t0.y = 0;
            t0.z = tA.rotation.eulerAngles.normalized.z;
            t1.x = tB.rotation.eulerAngles.normalized.x;
            t1.y = 0;
            t1.z = tB.rotation.eulerAngles.normalized.z;
            UpdateSpline();
        }

    }
}
