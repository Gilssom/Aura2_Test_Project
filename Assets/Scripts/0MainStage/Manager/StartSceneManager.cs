using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneManager : MonoBehaviour
{
    [SerializeField]
    private AudioClip m_ButtonSound;

    private void Awake()
    {
        m_ButtonSound = Resources.Load<AudioClip>("8SoundResources/06_08SFX/volume_sound");
    }

    public void StartStage()
    {
        //SceneManager.LoadScene(0);
        FadeInOutManager.Instance.InStartFadeAnim("VillageStage", 0);
    }
}
