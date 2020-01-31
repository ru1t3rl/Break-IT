using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinusScale : MonoBehaviour
{
    [SerializeField] private float amplitude = 1f;
    [SerializeField] private float frequentie = 0.1f;


    public bool fixX = false;
    public bool fixY = false;
    public bool fixZ = false;

    private float angle = 0.0f;
    private Vector3 localScale = Vector3.zero;
    [SerializeField] private Vector3 center = Vector3.zero;

    private void Start()
    {

    }

    void Update()
    {
        angle += frequentie;

        if (!fixX)
        {
            localScale.x = (amplitude * Mathf.Sin(angle) + center.x);
        }
        if (!fixY)
        {
            localScale.y =  (amplitude *  Mathf.Sin(angle) + center.y);
        }
        if (!fixZ)
        {
            localScale.z = (amplitude * Mathf.Sin(angle) + center.z);
        }

        transform.localScale = localScale;
    }
}
