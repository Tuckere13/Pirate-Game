using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;

    public float amplitude = 1.0f;
    public float length = 2.0f;
    public float speed = 1.0f;
    public float offset = 0.0f;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else if (instance != this)
        {
            Debug.Log("Intance Already Exists, Destroying Object");
            Destroy(this);
        }
    }

    private void Update()
    {
        offset += Time.deltaTime * speed;
    }

    public float GetWaveHeight(float _x)
    {
        return amplitude * Mathf.Sin(_x/ length + offset);
    }

}
