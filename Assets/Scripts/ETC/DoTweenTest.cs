using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DoTweenTest : MonoBehaviour
{
    private Light _Light;

    public float Intensity;
    public float Time;

    // Start is called before the first frame update
    void Start()
    {
        _Light = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            _Light.DOIntensity(Intensity, Time);
        }
    }
}
