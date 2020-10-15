using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Shape[] m_allshapes;

    Shape GetRandomShapes()
    {
        int i = Random.Range(0, m_allshapes.Length);
        if (m_allshapes[i])
        {
            return m_allshapes[i];
        }
        else
        {
            Debug.Log("INVALID SHAPE!!");
            return null;
        }

    }

    public Shape SpawnShape()
    {
        Shape shape = null;
        shape = Instantiate(GetRandomShapes(), transform.position, Quaternion.identity) as Shape;
        if (shape) return shape;
        else
        {
            Debug.Log("WARNING!! Shape not instantiated!");
            return null;
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
