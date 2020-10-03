using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Vector3 velocity;
    private Vector3 dampVelocity;

    public PlayerController attached;

    Borders borders;

    // Start is called before the first frame update
    void Start()
    {
        borders = FindObjectOfType<Borders>();
    }

    public bool isAttached() => attached != null;

    // Update is called once per frame
    void Update()
    {
        if (attached == null)
        {
            transform.position += velocity * Time.deltaTime;
            transform.position = borders.wrappedPosition(transform.position);

            velocity = Vector3.SmoothDamp(velocity, Vector3.zero, ref dampVelocity, 0.5f);
        }
        else
        {
            transform.position = attached.transform.position;
            transform.rotation = attached.transform.rotation;
        }
    }
}
