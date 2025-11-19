using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    // private Animator anim;
    private float moveH, moveV;
    [SerializeField] private float moveSpeed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        // anim = GetComponent<Animator>();
        if (GameManager.Instance != null)
        {
            GameManager.Instance.LockInput();
            // Debug.Log("PlayerMovement Start: Locked Input");
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {

        rb = GetComponent<Rigidbody2D>();
    }

    public void ForceStopMovement()
    {
        rb.velocity = Vector2.zero;
    }

    private void FixedUpdate()
    {
        moveH = Input.GetAxis("Horizontal") * moveSpeed;
        moveV = Input.GetAxis("Vertical") * moveSpeed;
        rb.velocity = new Vector2 (moveH, moveV);

        Vector2 direction = new Vector2(moveH, moveV);

        FindObjectOfType<PlayerAnimation>().SetDirection(direction);
    }

    


}
