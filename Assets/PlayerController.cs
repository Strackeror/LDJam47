using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Borders borders;
    public Weapon projectile;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        var input = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);
        Vector3 velocity = input.normalized * 5f;

        var target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        target.z = 0;
        var rot_z = Mathf.Atan2(target.y - transform.position.y, target.x - transform.position.x) * Mathf.Rad2Deg;
        transform.position += velocity * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z);

        if (borders.shouldWrap(transform.position))
        {
            transform.position = borders.wrappedPosition(transform.position);
        }

        if (Input.GetMouseButtonDown(1) && projectile.attached)
        {
            projectile.Fire(target - transform.position);
        }
      }
}
