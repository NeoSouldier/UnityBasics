using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float m_moveSpeed = 1.0f;
    public Vector3 m_moveDirection = new Vector3(0.0f, 0.0f, 0.0f);
    public float m_timeAlive = 0.0f;
    public const float m_kTimeToLive = 5.0f;

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + 
            new Vector3(m_moveDirection.x * m_moveSpeed * Time.deltaTime, 
                        m_moveDirection.y * m_moveSpeed * Time.deltaTime, 0.0f);
        
        m_timeAlive += Time.deltaTime;
        if (m_timeAlive > m_kTimeToLive)
        {
            Destroy(gameObject);
        }
    }
}
