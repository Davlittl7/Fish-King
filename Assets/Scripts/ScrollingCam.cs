using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingCam : MonoBehaviour
{
    private const double V = 0.05;
    public float step = (float)V;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var camPosition = Camera.main.gameObject.transform.position;
        camPosition.y += step;
        Camera.main.gameObject.transform.position = camPosition;
    }
}
