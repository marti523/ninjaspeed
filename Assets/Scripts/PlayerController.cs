using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : PhysicsObject {

    public float jumpTakeoffSpeed = 7;
    public float maxSpeed = 7;
    public float fallSpeed = .05f;

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    void Awake ()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
	}

    protected override void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;
        move.x = Input.GetAxis("Horizontal");

        if (move.x < 0)
        {
            spriteRenderer.flipX = true;
            moving = true;
            if (moving&&grounded)
                running = true;
        }
        else if (move.x > 0)
        {
            spriteRenderer.flipX = false;
            moving = true;
            if (moving&&grounded)
                running = true;
        }

        if (Input.GetButtonDown ("Jump") && grounded)
        {
            running = false;
            velocity.y = jumpTakeoffSpeed;
        }
        else if (Input.GetButtonUp ("Jump"))
        {
            moving = false;
            if (velocity.y > 0)
                velocity.y = velocity.y * fallSpeed;
        }

        animator.SetBool("grounded", grounded);
        animator.SetBool("moving", moving);
        animator.SetBool("running", running);

        targetVelocity = move * maxSpeed;
    }
}
