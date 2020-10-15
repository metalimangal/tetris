using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    Board m_gameBoard;
    Spawner m_spawner;
    Shape m_activeShape;
    float m_dropInterval = 0.9f;
    float m_timeToDrop;
    float m_timeToNextKey;
    float m_keyRepeatRate = 0.38f;
    bool m_gameOver = false;

    void Start()
    {
        m_timeToNextKey = Time.time;
        m_gameBoard =GameObject.FindWithTag("Board").GetComponent<Board>();
        m_spawner = GameObject.FindWithTag("Spawner").GetComponent<Spawner>();
        if (m_spawner)
        {
            if(m_activeShape == null)
            {
                m_activeShape = m_spawner.SpawnShape();
            }
            m_spawner.transform.position = Vectorf.Round(m_spawner.transform.position);
        }
        if (!m_gameBoard) Debug.Log("WARNING!! Could not find GameBoard");
        if (!m_spawner) Debug.Log("WARNING!! Could not find Spawner");
    }

    void Update()
    {
        if (!m_gameBoard || !m_spawner || !m_activeShape || m_gameOver)
        {
            return;
        }
        else
        {
            PlayerInput();
        }
        
        
    }

    void PlayerInput()
    {
        if (Input.GetButton("MoveRight") && Time.time > m_timeToNextKey || Input.GetButtonDown("MoveRight"))
        {
            m_activeShape.MoveRight();
            m_timeToNextKey = Time.time + m_timeToNextKey;
            if (m_gameBoard.IsValidPosition(m_activeShape))
            {
                Debug.Log("Move right");
            }
            else
            {
                m_activeShape.MoveLeft();
                Debug.Log("Hit the right boundary");
            }
        }
        else if (Input.GetButton("MoveLeft") && Time.time > m_timeToNextKey || Input.GetButtonDown("MoveLeft"))
        {
            m_activeShape.MoveLeft();
            m_timeToNextKey = Time.time + m_timeToNextKey;
            if (m_gameBoard.IsValidPosition(m_activeShape))
            {
                Debug.Log("Move right");
            }
            else
            {
                m_activeShape.MoveRight();
                Debug.Log("Hit the left boundary");
            }
        }
        else if (Input.GetButton("MoveDown") && Time.time > m_timeToNextKey || Input.GetButtonDown("MoveDown") || Time.time > m_timeToDrop)
        {
            m_activeShape.MoveDown();
            m_timeToDrop = m_dropInterval + Time.time;
            if (!m_gameBoard.IsValidPosition(m_activeShape))
            {
                if (m_gameBoard.IsOverLimit(m_activeShape))
                {
                    m_activeShape.MoveUp();
                    m_gameOver = true;
                    Debug.LogWarning(m_activeShape.name + " is over limit");
                }
                else
                {
                    LandShape();
                }
            }
            else
            {
                
                Debug.Log("Hit the bottom boundary");
            }
        }
        else if (Input.GetButton("Rotate") && Time.time > m_timeToNextKey || Input.GetButtonDown("Rotate"))
        {
            m_activeShape.RotateLeft();
            if (m_gameBoard.IsValidPosition(m_activeShape))
            {
                Debug.Log("Rotate");
            }
            else
            {
                m_activeShape.RotateRight();
                Debug.Log("Hit the boundary");
            }
        }
    }

    void LandShape()
    {
        m_timeToNextKey = Time.time + m_timeToNextKey;
        m_activeShape.MoveUp();
        m_gameBoard.StoreShapeInGrid(m_activeShape);
        if (m_spawner)
        {
            m_activeShape = m_spawner.SpawnShape();
        }
        m_gameBoard.ClearAllRows();
    }
}
