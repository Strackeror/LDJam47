using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    Vector2 initialPosition;

    float intensity = 0f;
    float time = 0f;

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
    }

    public void Shake(float time, float intensity) {
        this.time = time;
        this.intensity = intensity;
    }

    // Update is called once per frame
    void Update()
    {
        if (time > 0f) {
            time -= Time.deltaTime;
            transform.position = initialPosition + Random.insideUnitCircle * intensity;
        } else {
            transform.position = initialPosition;
        }
        transform.position -= Vector3.forward;
    }
}
