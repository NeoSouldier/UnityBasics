using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameDirector : MonoBehaviour
{
    //Public
    public GameObject m_enemyObject;
    public Player m_player;    
    public Text m_scoreText;
    public Text m_startText;
    public Text m_gameOverText;

    //Private
    enum GameState
    {
        kMenuState = 0,
        kPlayingState = 1,
        kDeadState = 2
    }

    private GameState m_currGameState;
    private float m_score;
    private float m_timeSinceLastEnemy;
    private bool m_readyToStart;
    private float m_timeDied;    

    // Start is called before the first frame update
    void Start()
    {
        m_currGameState = GameState.kMenuState;
        m_scoreText.gameObject.SetActive(false);
        m_startText.gameObject.SetActive(true);
        m_gameOverText.gameObject.SetActive(false);
        m_readyToStart = true;
        m_score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_currGameState == GameState.kMenuState)
        {
            if (m_readyToStart && Input.anyKeyDown)
            {
                m_currGameState = GameState.kPlayingState;
                m_scoreText.gameObject.SetActive(true);
                m_startText.gameObject.SetActive(false);
                m_gameOverText.gameObject.SetActive(false);
                m_score = 0;
            }
        }

        if (m_currGameState == GameState.kPlayingState)
        {
            UpdatePlayingState();

            if (m_player.m_playerDead)
            {
                m_currGameState = GameState.kDeadState;
                m_scoreText.gameObject.SetActive(true);
                m_startText.gameObject.SetActive(false);
                m_gameOverText.gameObject.SetActive(true);

                m_readyToStart = false;
                m_timeDied = Time.time;
            }
        }

        if (m_currGameState == GameState.kDeadState)
        {
            UpdateDeadState();

            if (m_readyToStart && Input.anyKeyDown)
            {
                m_currGameState = GameState.kPlayingState;
                m_scoreText.gameObject.SetActive(true);
                m_startText.gameObject.SetActive(false);
                m_gameOverText.gameObject.SetActive(false);

                m_score = 0;
                m_player.m_playerDead = false;
                m_player.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
            }
        }
    }

    void UpdatePlayingState()
    {
        m_score += Time.deltaTime;
        m_scoreText.text = "SCORE: " + (int)m_score;

        m_timeSinceLastEnemy += Time.deltaTime;
        const float kFreqOfNewEnemy = 1.0f;
        const float kBaseProbabilityOfNewEnemy = 0.01f;
        if (m_timeSinceLastEnemy > kFreqOfNewEnemy 
                && Random.Range(0.0f, 1.0f) < (kBaseProbabilityOfNewEnemy + m_score/100.0f))
        {
            m_timeSinceLastEnemy = 0.0f;

            GameObject newEnemyObject = Instantiate(m_enemyObject);
            Enemy newEnemy = (Enemy) newEnemyObject.GetComponent("Enemy");
                        
            newEnemy.m_moveSpeed = Random.Range(3.0f, 10.0f);

            // Hardcoded size of screen
            const float kMaxWidth = 9.0f;
            const float kMaxHeight = 5.0f;

            bool kSpawnedFromVertical = Random.Range(0.0f, 1.0f) > 0.5f;
            bool kSpawnedFromPositiveSide = Random.Range(0.0f, 1.0f) > 0.5f;

            Vector3 spawnPos = new Vector3(0.0f,0.0f,0.0f);
            if (kSpawnedFromVertical && kSpawnedFromPositiveSide)
            {
                spawnPos.x = Random.Range(-kMaxWidth, kMaxWidth);
                spawnPos.y = kMaxHeight;                            
            }
            else if (kSpawnedFromVertical && !kSpawnedFromPositiveSide)
            {
                spawnPos.x = Random.Range(-kMaxWidth, kMaxWidth);
                spawnPos.y = -kMaxHeight;                
            }
            else if (!kSpawnedFromVertical && kSpawnedFromPositiveSide)
            {
                spawnPos.x = kMaxWidth;
                spawnPos.y = Random.Range(-kMaxHeight, kMaxHeight);                        
            }
            else 
            {
                spawnPos.x = -kMaxWidth;
                spawnPos.y = Random.Range(-kMaxHeight, kMaxHeight);
            }
            
            newEnemyObject.transform.position = spawnPos;

            Vector3 moveDir = new Vector3(0.0f,0.0f,0.0f);
            moveDir = m_player.transform.position - spawnPos;
            newEnemy.m_moveDirection = Vector3.Normalize(moveDir);
        }
    }

    void UpdateDeadState()
    {
        const float kTimeToWaitAfterDeath = 2.5f;
        if (Time.time - m_timeDied > kTimeToWaitAfterDeath)
        {
            m_readyToStart = true;
            m_startText.gameObject.SetActive(true);            
        }
    }
}
