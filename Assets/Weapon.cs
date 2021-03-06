﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Weapon : MonoBehaviour
{
    Borders borders;
    PlayerController player;
    public bool attached { get; private set; } = true;
    int wrapCount;

    public Vector3 velocity;
    private Vector3 dampVelocity;

    public float initialSpeed;
    public float maxSpeed;

    private int scoreBonus = 0;

    // Start is called before the first frame update
    void Start()
    {
        borders = FindObjectOfType<Borders>();
        player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!attached)
        {

            if (wrapCount >= 2)
            {
                var targetVelocity = (player.transform.position - transform.position).normalized * initialSpeed;
                velocity = Vector3.SmoothDamp(velocity, targetVelocity, ref dampVelocity, 3f);
            }
            else
            {
                var targetVelocity = velocity.normalized * maxSpeed;
                velocity = Vector3.SmoothDamp(velocity, targetVelocity, ref dampVelocity, 2f);
            }
            transform.position += velocity * Time.deltaTime;
            if (borders.shouldWrap(transform.position))
            {
                player.heavyRun.Stop();
                player.lightRun.Stop();
                transform.position = borders.wrappedPosition(transform.position);
                wrapCount += 1;
                player.heavyRun.Play();
                player.lightRun.Play();
            }

            /*
            var rot_z = Vector2.Angle(velocity, Vector2.right);
            transform.rotation = Quaternion.Euler(0f, 0f, rot_z);
            */

        }
        else
        {
            transform.position = player.transform.position;
            transform.rotation = player.transform.rotation;
        }
    }

    public void Fire(Vector3 direction)
    {
        attached = false;
        wrapCount = 0;
        velocity = direction.normalized * initialSpeed;
        dampVelocity = Vector3.zero;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Projectile collider hit");
        
        if (col.gameObject.GetComponent<PlayerController>() != null)
        {
            if (wrapCount > 0)
            {
                attached = true;
                scoreBonus = 0;
                player.chargeExplosion.Play();
                player.pickup.Play();
                player.heavyRun.Stop();
                FindObjectOfType<ScreenShake>().Shake(0.1f, 0.01f);
            }
        }

        if (col.gameObject.GetComponent<EnemyBehavior>() != null)
        {
            if (!attached)
            {
                var enemy = col.gameObject.GetComponent<EnemyBehavior>();
                if (enemy.spawnTime > 0f) return;
                enemy.Kill();
                Score.scoreValue += (enemy.scoreValue + scoreBonus*50);
                Score.killCount++;
                scoreBonus += 1;
            }
        }
    }
}
