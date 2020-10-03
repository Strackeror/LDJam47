using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Borders borders;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        var input = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);
        Vector3 velocity = input.normalized * 10f;

        var target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Debug.LogError(Input.mousePosition);
        var rot_z = Mathf.Atan2(target.y - transform.position.y, target.x - transform.position.x) * Mathf.Rad2Deg;
        transform.position += velocity * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z);

        var (begin, end) = borders.AvailableTerrain();
        var size = end - begin;

        var pos = transform.position;
        var points = new[] {
            pos + new Vector3(size.x, 0),
            pos - new Vector3(size.x, 0),
            pos + new Vector3(0, size.y),
            pos - new Vector3(0, size.y),
        };
        foreach (var npos in points)
        {
            if (npos.x > begin.x && npos.y > begin.y && npos.x < end.x && npos.y < end.y)
            {
                transform.position = npos;
                break;
            }
        }
    }
}
