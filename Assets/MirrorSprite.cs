using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorSprite : MonoBehaviour
{
    Borders borders;
    GameObject parent;
    void Start()
    {
        parent = transform.parent.gameObject;
        borders = FindObjectOfType<Borders>();
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = borders.wrappedPosition(parent.transform.position);
    }
}
