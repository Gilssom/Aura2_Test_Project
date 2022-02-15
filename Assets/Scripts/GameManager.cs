using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Image _FadeBG;

    void Start()
    {
        _FadeBG = _FadeBG.GetComponent<Image>();
    }
    
    void Update()
    {
        Color color = _FadeBG.color;

        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Check");
            color.a = 100;
            StartCoroutine(Fadein());
        }
    }

    IEnumerator Fadein()
    {
        Color color = _FadeBG.color;

        while (color.a <= 255)
        {
            color.a += Time.deltaTime * 0.1f;
        }

        yield return null;
    }
}
