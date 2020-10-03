using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Borders borders;
    public Weapon projectile;

    public float chargeTime = 0.5f;
    float currentChargedTime = 0f;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Player coords: " + transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        var input = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);
        Vector3 velocity = input.normalized * 5f;

        var target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        target.z = 0;


        if (borders.shouldWrap(transform.position))
        {
            transform.position = borders.wrappedPosition(transform.position);
        }

        if (Input.GetMouseButton(0) && projectile.attached)
        {
            currentChargedTime += Time.deltaTime;
            if (currentChargedTime > chargeTime)
            {
                currentChargedTime = chargeTime;
                projectile.Fire(target - transform.position);
            }
            velocity *= 1 - (currentChargedTime / chargeTime);
        }
        else
        {
            currentChargedTime = 0f;
        }

        if (!projectile.attached)
        {
            velocity = Vector3.zero;
        }



        transform.position += velocity * Time.deltaTime;

        var rot_z = Mathf.Atan2(target.y - transform.position.y, target.x - transform.position.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<EnemyBehavior>())
        {
            this.gameObject.SetActive(false);
        }
    }
}
