using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    private Transform target;

    public float spawnTime = 2.0f;
    public float disappearTime = 1.0f;

    bool killed = false;
    float killTime = 0f;

    [Header("Particles")]
    public ParticleSystem explode;
    public ParticleSystem spawnWarning;

    [Header("Sounds")]
    public AudioClip explosionSound;

    public enum AIType {
        Follow,
        Straight,

    }
    public float value;
    public int scoreValue = 100;

    [Header("AI")]
    public AIType type;

    [Header("AI : Follow")]

    public float speed;
    public float turnSpeed;

    [Header("AI : Straight")]
    public float straightSpeed = 2f;

    public Vector2 velocity;
    Vector2 smoothVelocity;

    EnemyBehavior[] children = {};

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindObjectOfType<PlayerController>()?.transform;
        spawnWarning.Play();

        children = GetComponentsInChildren<EnemyBehavior>(true);

        if (velocity.magnitude != 0) return;
        switch(type) {
            case AIType.Follow:
                velocity = (target.transform.position - transform.position).normalized * speed;
                TurnToTarget(Vector3.zero);
                break;
            case AIType.Straight:
                velocity = (target.transform.position - transform.position).normalized * straightSpeed;
                TurnToTarget(target.transform.position);
                break;
        }

    }

    void TurnToTarget(Vector3 pos) {
        var targetPos = pos;
        var thisPos = transform.position;
        targetPos.x = targetPos.x - thisPos.x;
        targetPos.y = targetPos.y - thisPos.y;
        if (targetPos.magnitude > 0) {
            var angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }

    public void Kill() {
        if (spawnTime > 0f) {
            return;
        }
        FindObjectOfType<ScreenShake>().Shake(0.1f, 0.01f);
        this.GetComponent<CircleCollider2D>().enabled = false;
        explode.Play();
        killed = true;
        killTime = Time.time;
        FindObjectOfType<Borders>().reverseTime += value;
        GetComponent<AudioSource>().clip = explosionSound;
        GetComponent<AudioSource>().Play();
        foreach (var enemy in children) {
            if (enemy == this) continue;
            enemy.spawnTime = 0.0f;
            enemy.transform.SetParent(transform.parent);
            enemy.velocity = (enemy.transform.position - transform.position).normalized * 1.0f;
            enemy.gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (EndGame.isGameOver) return;
        var borders = FindObjectOfType<Borders>();

        if (spawnTime > 0f) {
            spawnTime -= Time.deltaTime;
            transform.position = transform.position.normalized * borders.Diameter() / 2.02f;
            return;
        }

        var sprite = GetComponentInChildren<SpriteRenderer>();
        spawnWarning.Stop();
        if (killed) {
            sprite.color = Color.Lerp(Color.white, Color.clear, (Time.time - killTime) / 0.1f);
            if (Time.time - killTime > disappearTime) 
            {
                Destroy(gameObject);
            }
            return;
        }

        sprite.enabled = true;
        switch (type)
        {
            case AIType.Follow:
                var targetVelocity = (target.position - transform.position).normalized * speed;
                velocity = Vector2.SmoothDamp(velocity, targetVelocity, ref smoothVelocity, 1f, turnSpeed);
                TurnToTarget(transform.position + (Vector3) velocity);
                break;
            case AIType.Straight:
                if (borders.shouldWrap(transform.position)) {
                    transform.position = borders.wrappedPosition(transform.position);
                }
                break;

        }
        transform.position += (Vector3) velocity * Time.deltaTime;
    }
}
