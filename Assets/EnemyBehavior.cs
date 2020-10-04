using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public float speed = 0.5f;
    public float value = 0.5f;

    private Transform target;

    float spawnTime = 2.0f;

    bool killed = false;
    float killTime = 0f;

    [Header("Particles")]
    public ParticleSystem explode;
    public ParticleSystem spawnWarning;

    [Header("Sounds")]
    public AudioClip explosionSound;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindObjectOfType<PlayerController>()?.transform;

        TurnToTarget(Vector3.zero);
        spawnWarning.Play();
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
    }

    // Update is called once per frame
    void Update()
    {
        if (EndGame.isGameOver) return;

        if (spawnTime > 0f) {
            spawnTime -= Time.deltaTime;
        }
        else
        {
            var sprite = GetComponentInChildren<SpriteRenderer>();
            spawnWarning.Stop();
            if (killed) {
                sprite.color = Color.Lerp(sprite.color, Color.clear, (Time.time - killTime) / 1.0f);
                if (Time.time - killTime > 1.0f)
                {
                    Destroy(gameObject);
                }
            }
            else {
                sprite.enabled = true;
                transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
                TurnToTarget(target.position);
            }
        }
    }
}
