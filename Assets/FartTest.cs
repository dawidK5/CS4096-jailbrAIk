using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FartTest : MonoBehaviour
{
    public float maxFartometer = 1f;
    public float currentFartometer = 0f;

    public Fartometer fartometer;

    void Start()
    {
        currentFartometer = maxFartometer;
        fartometer.setMaxFartometer(maxFartometer);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //takeFartometerDamage(.20f);
        }
    }

    void takeFartometerDamage(float damage)
    {
        currentFartometer -= damage;
        fartometer.setFartometer(currentFartometer);
    }
}
