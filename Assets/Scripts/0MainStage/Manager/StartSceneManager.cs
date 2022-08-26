using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneManager : MonoBehaviour
{
    public void StartStage()
    {
        SceneManager.LoadScene(0);
    }
}
