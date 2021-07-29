using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knot : MonoBehaviour
{
    public TunelSegment tunel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if ( Input.GetMouseButton(1) )
        {
            tunel.UpdateSpline();
            Debug.Log("Knot mouse event");
        }
    }
}
