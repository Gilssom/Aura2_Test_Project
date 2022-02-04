using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DoTweenTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            transform.Translate(Vector3.forward * 100 * Time.deltaTime);
        }
    }
}
