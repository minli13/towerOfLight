using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator anim;
    public string[] staticDirections = { "Static N", "Static NW", "Static W", "Static SW", "Static S", "Static SE", "Static E", "Static NE" };
    public string[] runDirections = { "Run N", "Run NW", "Run W", "Run SW", "Run S", "Run SE", "Run E", "Run NE" };
    int lastDirection;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
        float result1 = Vector2.SignedAngle(Vector2.up, Vector2.right); // -90
        Debug.Log("R1 " + result1);

        float result2 = Vector2.SignedAngle(Vector2.up, Vector2.left); // 90
        Debug.Log("R2 " + result2);

        float result3 = Vector2.SignedAngle(Vector2.up, Vector2.down); // 180
        Debug.Log("R3 " + result3);

    }

    // each direction will match one string element
    // we used direction to determine their animation
    
    public void SetDirection(Vector2 _direction)
    {
        string[] directionArray = null;
        if (_direction.magnitude < 0.01) // character is static and its velocity is close to zero
        {
            directionArray = staticDirections;
        } else
        {
            directionArray = runDirections;
            lastDirection = DirectionToIndex(_direction); // get the index of the slice from the direction vector
        }
        anim.Play(directionArray[lastDirection]);
    }

    // converts a Vector2 direction to an index to a slice around a circle
    // goes in a counter-clockwise direction
    private int DirectionToIndex(Vector2 _direction)
    {
        Vector2 norDir = _direction.normalized;
        float step = 360 / 8; // 45; one circle and 8 slices; calculate how many deg a slice is
        float offset = step / 2; // 22.5; offset to make it easy to calculate and get correct index of string array

        float angle = Vector2.SignedAngle(norDir, Vector2.up);

        angle += offset;
        if (angle < 0) // avoid negative numbers
        {
            angle += 360;
        }

        float stepCount = angle / step;
        return Mathf.FloorToInt(stepCount);

    }
}
