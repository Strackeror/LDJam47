using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Borders : MonoBehaviour
{

    public GameObject U, D, L, R;
    public GameObject circle;
    public float shrinkRatio = 1f;
    public float shrinkTime = 20f;
    public float startDiameter = 4f;

    Vector3 initU, initD, initL, initR;

    // Start is called before the first frame update
    void Start()
    {
        initU = U.transform.position;
        initD = D.transform.position;
        initL = L.transform.position;
        initR = R.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        shrinkRatio -= Time.deltaTime / shrinkTime;
        if (shrinkRatio < 0.25f)
        {
            shrinkRatio = 0.25f;
        }
        circle.transform.localScale = Vector2.one * shrinkRatio;
    }

    public bool shouldWrap(Vector3 pos)
    {
        var diameter = startDiameter * shrinkRatio;
        return pos.magnitude > diameter / 2;
    }

    public Vector3 wrappedPosition(Vector3 pos)
    {
        var diameter = startDiameter * shrinkRatio;
        return pos.normalized * -1 * (diameter - pos.magnitude);
    }

    public float Diameter()
    {
        return 4 * shrinkRatio;
    }
}
