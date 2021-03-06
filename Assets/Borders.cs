﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Borders : MonoBehaviour
{

    public GameObject circle;
    public float shrinkRatio = 1f;
    public float shrinkTime = 20f;
    public float startDiameter = 4f;

    public float reverseTime = 0f;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (EndGame.isGameOver) return;
        if (reverseTime > 0f) {
            shrinkRatio += Time.deltaTime / shrinkTime;
            reverseTime -= Time.deltaTime;
        } else {
            shrinkRatio -= Time.deltaTime / shrinkTime;
        }

        shrinkRatio = Mathf.Clamp(shrinkRatio, 0.05f, 1f);
        reverseTime = Mathf.Min(reverseTime, 5f);

        circle.transform.localScale = Vector2.one * shrinkRatio;
        var rot_z = Time.time;
        circle.transform.rotation = Quaternion.Euler(0f, 0f, rot_z);

    }

    public bool shouldWrap(Vector3 pos)
    {
        var diameter = startDiameter * shrinkRatio;
        return pos.magnitude > diameter / 2;
    }

    public Vector3 wrappedPosition(Vector3 pos)
    {
        var diameter = startDiameter * shrinkRatio;
        return pos.normalized * -1 * (diameter - pos.magnitude);
    }

    public float Diameter()
    {
        return 4 * shrinkRatio;
    }
}
