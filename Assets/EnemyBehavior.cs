using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public float speed=0.5f;

    private Transform target;

    bool killed = false;
    float killTime = 0f;

    [Header("Particles")]
    public ParticleSystem explode;

    [Header("Sounds")]
    public AudioClip explosionSound;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindObjectOfType<PlayerController>()?.transform;
    }

    public void Kill() {
        this.GetComponent<CircleCollider2D>().enabled = false;
        explode.Play();
        killed = true;
        killTime = Time.time;
        FindObjectOfType<Borders>().reverseTime += 0.5f;
        GetComponent<AudioSource>().clip = explosionSound;
        GetComponent<AudioSource>().Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (killed) {
            var sprite = GetComponentInChildren<SpriteRenderer>();
            sprite.color = Color.Lerp(sprite.color, Color.clear, (Time.time - killTime) / 1.0f);

            if (Time.time - killTime > 1.0f)  {
                Destroy(gameObject);
            }
        }
        else if (target && !EndGame.isGameOver)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            var targetPos = target.position;
            var thisPos = transform.position;
            targetPos.x = targetPos.x - thisPos.x;
            targetPos.y = targetPos.y - thisPos.y;
            var angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }

        
    }
}
