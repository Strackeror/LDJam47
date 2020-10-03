using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Borders : MonoBehaviour
{
    public GameObject U, D, L, R;
    public float shrink_ratio = 0f;
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
        shrink_ratio += Time.deltaTime * 0.125f;
        U.transform.position = initU + new Vector3(0, -2) * (shrink_ratio);
        D.transform.position = initD + new Vector3(0, 2) * (shrink_ratio);
        L.transform.position = initL + new Vector3(2, 0) * (shrink_ratio);
        R.transform.position = initR + new Vector3(-2, 0) * (shrink_ratio);
    }
}
