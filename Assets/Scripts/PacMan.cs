﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacMan : MonoBehaviour
{
    public float moveSpeed = 4.0f;
    public float timeInvincible = 10.0f;

    public Transform movePoint;
    public LayerMask whatStopsMovement;

    // Newly input direction
    private Vector2 newDirection = Vector2.zero;
    // Previous direction
    private Vector2 oldDirection = Vector2.zero;

    private int pelletsConsumed = 0;
    private int score = 0;
    private bool isInvincible = false;
    private float invincibleTimer;

    // Start is called before the first frame update
    void Start()
    {
        movePoint.parent = null;
    }

    // Update is called every frame
    void Update()
    {
        UpdateTimers();
        CheckInput();
        Move();
    }

    void UpdateTimers()
    {
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
            {
                isInvincible = false;
            }
        }
    }

    void CheckInput()
    {
        // Change the direction based on input
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            newDirection = Vector2.left;
        } else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            newDirection = Vector2.right;
        } else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            newDirection = Vector2.up;
        } else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            newDirection = Vector2.down;
        }
    }

    void Move()
    {
        transform.position = Vector2.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);
        if (Vector2.Distance(transform.position, movePoint.position) <= 0.05f)
        {
            // If no collision between movePoint and grid, move the movePoint to the new location
            if (!Physics2D.OverlapCircle(movePoint.position + (Vector3)newDirection, 0.2f, whatStopsMovement))
            {
                UpdateOrientation();
                oldDirection = newDirection;
                movePoint.position += (Vector3)newDirection;
            } 
            // Else if no collision if keep going the old way, then keep going the old way
            else if (!Physics2D.OverlapCircle(movePoint.position + (Vector3)oldDirection, 0.2f, whatStopsMovement))
            {
                movePoint.position += (Vector3)oldDirection;
            }
        }
    }

    void UpdateOrientation()
    {
        if (newDirection == Vector2.left)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else if (newDirection == Vector2.right)
        {
            transform.localScale = new Vector3(1, 1, 1);
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else if (newDirection == Vector2.up)
        {
            transform.localScale = new Vector3(1, 1, 1);
            transform.localRotation = Quaternion.Euler(0, 0, 90);
        }
        else if (newDirection == Vector2.down)
        {
            transform.localScale = new Vector3(1, 1, 1);
            transform.localRotation = Quaternion.Euler(0, 0, 270);
        }
    }

    public void ConsumePellet(string pellet)
    {
        pelletsConsumed++;
        score += 100;
        if (pellet.Equals("SuperPellet"))
        {
            isInvincible = true;
            invincibleTimer = timeInvincible;
        }
    }
}
