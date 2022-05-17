using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public AudioMixer m_Mixer;
    public AudioSource m_BgSound;
    public AudioClip[] m_BgList;

    private static SoundManager m_instance;
    // ΩÃ±€≈Ê
    public static SoundManager Instance
    {
        get
        {
            if (!m_instance)
            {
                m_instance = FindObjectOfType(typeof(SoundManager)) as SoundManager;

                if (m_instance == null)
                    Debug.Log("No Singletone Obj");
            }
            return m_instance;
        }
    }

    private void Awake()
    {
        if (m_instance == null)
        {
            m_instance = this;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else if (m_instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(this);
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        for (int i = 0; i < m_BgList.Length; i++)
        {
            if(arg0.name == m_BgList[i].name)
                BgSoundPlay(m_BgList[i]);
        }
    }

    /*public void BGSoundVolume(float val) // UI ∫º∑˝ º≥¡§√¢
    {
        m_Mixer.SetFloat("BGSoundVolume", Mathf.Log10(val) * 20);
    }*/

    public void SFXPlay(string sfxName, AudioClip clip)
    {
        GameObject go = new GameObject(sfxName + "Sound");
        AudioSource audiosource = go.AddComponent<AudioSource>();
        audiosource.outputAudioMixerGroup = m_Mixer.FindMatchingGroups("SFX")[0];
        audiosource.clip = clip;
        audiosource.Play();

        Destroy(go, clip.length);
    }

    public void BgSoundPlay(AudioClip clip)
    {
        m_BgSound.outputAudioMixerGroup = m_Mixer.FindMatchingGroups("BGSound")[0];
        m_BgSound.clip = clip;
        m_BgSound.loop = true;
        m_BgSound.volume = 0.1f;
        m_BgSound.Play();
    }
}
