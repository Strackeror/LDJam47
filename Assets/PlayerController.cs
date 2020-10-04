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
    public ParticleSystem lightRun, heavyRun, chargeExplosion;

    void Start()
    {
        Debug.Log("Player coords: " + transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        var input = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);
        Vector3 velocity = input.normalized * 2f;

        var target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        target.z = 0;


        if (borders.shouldWrap(transform.position))
        {
            if (velocity.magnitude > 0f) {
                transform.position = borders.wrappedPosition(transform.position);
            }
        }


        if (Input.GetMouseButtonUp(0)) {
            Debug.Log(("MouseUP", projectile.attached, currentChargedTime));
            if (projectile.attached && currentChargedTime >= chargeTime)
            {
                projectile.Fire(target - transform.position);
            }
        }

        if (Input.GetMouseButton(0) && projectile.attached)
        {
            currentChargedTime += Time.deltaTime;
            if (currentChargedTime > chargeTime)
            {
                if (currentChargedTime - Time.deltaTime < chargeTime) {
                    chargeExplosion.Play();
                    heavyRun.Play();
                }
            }

            velocity *= 1 - 0.5f * (Mathf.Clamp(currentChargedTime, 0f, chargeTime) / chargeTime);
        }
        else
        {
            if (projectile.attached) {
                heavyRun.Stop();
            }
            currentChargedTime = 0f;
        }


        if (projectile.attached)
        {
            var rot_z = Mathf.Atan2(target.y - transform.position.y, target.x - transform.position.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rot_z);
            transform.position += velocity * Time.deltaTime;
        }

        lightRun.gameObject.SetActive(velocity.magnitude > 0.1);

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<EnemyBehavior>())
        {
            this.gameObject.SetActive(false);
        }
    }
}
