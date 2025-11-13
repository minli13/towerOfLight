using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrangeInCircle : MonoBehaviour
{
    public float radius = 100f;

    void Start()
    {
        int count = transform.childCount;
        for (int i = 0; i < count; i++)
        {
            float angle = i * Mathf.PI * 2f / count;
            Vector3 pos = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;
            transform.GetChild(i).localPosition = pos;
        }
    }
}
