using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMain : MonoBehaviour
{
    public float speed;
    public Rigidbody2D rdbd2d;

    private Vector2 moveDirection;

    // Update is called once per frame
    void Update()
    {
        ProcessInputs();
    }

    void FixedUpdate()
    {
        //stop movement while dialogue is playing
        if (ScriptReader.GetInstance().dialogueIsPlaying)
        {
            return;
        }

        //Physics Calculations
        Move();
    }

    void ProcessInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(moveX, moveY).normalized;
    }

    void Move()
    {
        rdbd2d.velocity = new Vector2(moveDirection.x * speed, moveDirection.y * speed);
    }
}
