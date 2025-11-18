using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrangeInCircle : MonoBehaviour
{
    public float radius = 100f;

    void Start()
    {
        
    }

    // Call this manually when adding new tools too
    public void ArrangeChildren()
    {
        int count = transform.childCount;

        // Only arrange if we have children
        if (count == 0)
        {
            return;
        }

        for (int i = 0; i < count; i++)
        {
            Transform child = transform.GetChild(i);

            float angle = i * Mathf.PI * 2f / count;
            Vector3 pos = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;
            child.localPosition = pos;
        }
    }

    // Call this manually when add new tools dynamically
    public void RefreshArrangement()
    {
        ArrangeChildren();
    }
}
