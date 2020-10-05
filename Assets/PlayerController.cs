using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Borders borders;
    public Weapon projectile;

    public float minChargeTime = 0.5f;
    public float maxChargeTime = 3f;

    float currentChargedTime = 0f;

    [Header("Particles")]
    public ParticleSystem lightRun;
    public ParticleSystem heavyRun;
    public ParticleSystem chargeExplosion;
    public ParticleSystem bigChargeExplosion;

    public ParticleSystem deathExplosion;

    [Header("Sounds")]
    public AudioSource fire1;
    public AudioSource pickup;
    public AudioSource charged;
    public AudioSource deathExplosionSound;

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
            if (velocity.magnitude > 0f || Input.GetMouseButtonUp(0))
            {
                heavyRun.Stop();
                lightRun.Stop();
                transform.position = borders.wrappedPosition(transform.position);
                heavyRun.Play();
                lightRun.Play();
            }
        }

        float projectilePower = (currentChargedTime - minChargeTime) / (maxChargeTime - minChargeTime);

        if (Input.GetMouseButtonUp(0))
        {
            if (projectile.attached && currentChargedTime >= minChargeTime)
            {
                fire1.Play();

                projectile.initialSpeed = Mathf.Lerp(0.5f, 6f, projectilePower);
                projectile.maxSpeed = Mathf.Lerp(6f, 15f, projectilePower);
                projectile.Fire(target - transform.position);
                FindObjectOfType<ScreenShake>().Shake(0.15f, 0.03f);
            }
        }

        var sprite = GetComponentInChildren<SpriteRenderer>();
        if (Input.GetMouseButton(0) && projectile.attached)
        {
            sprite.color = Color.Lerp(Color.white, new Color(1f, 0.5f, 0f), currentChargedTime / minChargeTime);

            currentChargedTime += Time.deltaTime;
            velocity = Vector2.Lerp(velocity, velocity * 0.5f, currentChargedTime / minChargeTime);
            if (currentChargedTime > minChargeTime)
            {
                if (currentChargedTime - Time.deltaTime < minChargeTime)
                {
                    charged.Play();
                    chargeExplosion.Play();
                    heavyRun.Play();
                    FindObjectOfType<ScreenShake>().Shake(0.1f, 0.01f);
                }
                if (currentChargedTime - Time.deltaTime < maxChargeTime && currentChargedTime > maxChargeTime) {
                    bigChargeExplosion.Play();
                    charged.Play();
                    FindObjectOfType<ScreenShake>().Shake(0.2f, 0.02f);
                }
                {
                    var main = heavyRun.main;
                    main.startSpeed = Mathf.Lerp(0.5f, 4, projectilePower);

                    var emission = heavyRun.emission;
                    emission.rateOverTime = Mathf.Lerp(50, 400, projectilePower);
                    sprite.color = Color.Lerp(new Color(1f, 0.5f, 0f), Color.red, projectilePower);
                }

                velocity = Vector2.Lerp(velocity, velocity * 0.2f, projectilePower);
            }



        }
        else
        {
            currentChargedTime = 0f;
            sprite.color = Color.white;
        }

        if (projectile.attached) {
            var rot_z = Mathf.Atan2(target.y - transform.position.y, target.x - transform.position.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rot_z);
            transform.position += velocity * Time.deltaTime;
        }

        if (borders.Diameter() < 0.5)
        {
            Kill();
        }

        lightRun.gameObject.SetActive(velocity.magnitude > 0.1);

    }

    void Kill()
    {
        deathExplosion.Play();
        deathExplosionSound.Play();
        GetComponent<Collider2D>().enabled = false;
        GetComponentInChildren<SpriteRenderer>().gameObject.SetActive(false);
        enabled = false;
        projectile.gameObject.SetActive(false);
        EndGame.GameOver();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<EnemyBehavior>())
        {
            Kill();
        }
    }
}
