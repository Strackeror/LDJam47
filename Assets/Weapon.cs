using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    Borders borders;
    PlayerController player;
    public bool attached { get; private set; } = true;
    bool hasWrapped;

    public Vector3 velocity;
    private Vector3 dampVelocity;

    public float speed;

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
            transform.position += velocity * Time.deltaTime;
            if (borders.shouldWrap(transform.position))
            {
                transform.position = borders.wrappedPosition(transform.position);
                hasWrapped = true;
            }
            velocity = Vector3.SmoothDamp(velocity, Vector3.zero, ref dampVelocity, 2f);
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
        hasWrapped = false;
        velocity = direction.normalized * speed; 
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Projectile collider hit");
        
        if (col.gameObject.GetComponent<PlayerController>() != null)
        {
            if (hasWrapped)
            {
                attached = true;
            }
        }

        if (col.gameObject.GetComponent<EnemyBehavior>() != null)
        {
            if (!attached)
            {
                col.gameObject.SetActive(false);
            }
        }
    }
}
