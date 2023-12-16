using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using jomi.vis;

public class Test : MonoBehaviour
{
    [SerializeField] Color color;
    [SerializeField] Vector2 a,b,c,d;


    private void Update()
    {
        Draw.Point(transform.position, transform.localScale.x, color);
        Draw.Quad(a,b,c,d,color);
    }
}
