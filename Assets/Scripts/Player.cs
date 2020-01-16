using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool m_playerDead = false;

    private float m_movementSpeed = 5.0f; 

    void Update()
    {
        if (m_playerDead) return;        

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector2 movementVector = new Vector2(m_movementSpeed * horizontalInput * Time.deltaTime, m_movementSpeed * verticalInput * Time.deltaTime);
        
        transform.position = transform.position + new Vector3(movementVector.x, movementVector.y, 0.0f);

        // Hardcoded size of screen
        const float kMaxWidth = 9.0f;
        const float kMaxHeight = 5.0f;

        if (Mathf.Abs(transform.position.x) > kMaxWidth)
        {
            float newX = kMaxWidth * Mathf.Sign(transform.position.x);
            transform.position = new Vector3(newX, transform.position.y, 0.0f);
        }

        if (Mathf.Abs(transform.position.y) > kMaxHeight)
        {
            float newY = kMaxHeight * Mathf.Sign(transform.position.y);
            transform.position = new Vector3(transform.position.x, newY, 0.0f);
        }

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        m_playerDead = true;
    }
}