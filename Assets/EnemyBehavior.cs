﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public float speed=0.5f;

    private Transform target;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindObjectOfType<PlayerController>().transform;
        Debug.Log(target);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        Debug.Log("Enemy coord: " + transform.position);
        Debug.Log("Player coord: " + target.position);
        var targetPos = target.position;
        var thisPos = transform.position;
        targetPos.x = targetPos.x - thisPos.x;
        targetPos.y = targetPos.y - thisPos.y;
        var angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
