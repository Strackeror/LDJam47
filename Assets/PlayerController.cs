using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Player coords: " + transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        var input = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);
        Vector3 velocity = input.normalized * 02f;

        var target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // Debug.LogError(Input.mousePosition);
        var rot_z = Mathf.Atan2(target.y - transform.position.y, target.x - transform.position.x) * Mathf.Rad2Deg;
        transform.position += velocity * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z);
    }
}
