using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartSceneManager : MonoBehaviour
{
    [SerializeField]
    private AudioClip m_ButtonSound;

    public GameObject m_PausePanel;
    public GameObject m_ContinuePanel;
    public GameObject[] m_PauseMenus;
    public GameObject[] m_PauseIcon;
    public GameObject[] m_KeyHelpersMenus;
    public GameObject[] m_SettingText;
    public Image[] m_BGM;
    public Image[] m_SFX;

    public bool isPause;

    int m_SettingMenuNumber;
    private Sprite m_SettingGageOn;
    private Sprite m_SettingGageOff;

    private void Awake()
    {
        m_ButtonSound = Resources.Load<AudioClip>("8SoundResources/06_08SFX/volume_sound");

        m_SettingGageOn = Resources.Load<Sprite>("7Textures/UI/Settings/GageOn");
        m_SettingGageOff = Resources.Load<Sprite>("7Textures/UI/Settings/GageOff");
    }

    public void StartStage()
    {
        FadeInOutManager.Instance.InStartFadeAnim("VillageStage", 0);
    }

    public void ContinueButton()
    {
        m_ContinuePanel.SetActive(true);
        isPause = true;
    }

    public void SettingButton()
    {
        m_PauseIcon[0].SetActive(true);
        m_PausePanel.SetActive(true);
        isPause = true;
    }

    public void GameExitButton()
    {
        Debug.Log("게임 종료");
    }

    public void ButtonSound()
    {
        SoundManager.Instance.SFXPlay("Button", m_ButtonSound);
    }

    private void Update()
    {
        SettingMenu();
        SFXSoundCheck();
        BGMSoundCheck();
        //PauseFalse();
    }

    public void PauseFalse()
    {
        if(isPause)
        {
            m_PausePanel.SetActive(false);
            m_ContinuePanel.SetActive(false);
            isPause = false;
        }
    }

    public void PauseChagneMenu(int MenuNumber)
    {
        SoundManager.Instance.SFXPlay("Button", m_ButtonSound);

        for (int i = 0; i < 2; i++)
        {
            if (i == MenuNumber)
            {
                m_PauseMenus[i].SetActive(true);
                m_PauseIcon[i].SetActive(true);
            }
            else
            {
                m_PauseMenus[i].SetActive(false);
                m_PauseIcon[i].SetActive(false);
            }
        }
    }

    void SettingMenu()
    {
        if (Input.GetKeyUp(KeyCode.UpArrow) && m_SettingMenuNumber > -1 && m_SettingMenuNumber < 2)
        {
            m_SettingMenuNumber++;
            SoundManager.Instance.SFXPlay("Button", m_ButtonSound);
            SettingTextCtrl(m_SettingMenuNumber);
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow) && m_SettingMenuNumber > 0 && m_SettingMenuNumber < 3)
        {
            m_SettingMenuNumber--;
            SoundManager.Instance.SFXPlay("Button", m_ButtonSound);
            SettingTextCtrl(m_SettingMenuNumber);
        }
        else if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            if (m_SettingMenuNumber == 1 && SoundManager.Instance.m_SFXGage > -1 && SoundManager.Instance.m_SFXGage < 5)
            {
                SoundManager.Instance.m_SFXGage++;
                SoundManager.Instance.SFXPlay("Button", m_ButtonSound);
                SettingGageCtrl(SoundManager.Instance.m_SFXGage);
            }
            else if (m_SettingMenuNumber == 2 && SoundManager.Instance.m_BGMGage > -1 && SoundManager.Instance.m_BGMGage < 5)
            {
                SoundManager.Instance.m_BGMGage++;
                SoundManager.Instance.SFXPlay("Button", m_ButtonSound);
                SettingGageCtrl(SoundManager.Instance.m_BGMGage);
            }
            else
                return;
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            if (m_SettingMenuNumber == 1 && SoundManager.Instance.m_SFXGage > 0 && SoundManager.Instance.m_SFXGage < 6)
            {
                SoundManager.Instance.m_SFXGage--;
                SoundManager.Instance.SFXPlay("Button", m_ButtonSound);
                SettingGageCtrl(SoundManager.Instance.m_SFXGage);
            }
            else if (m_SettingMenuNumber == 2 && SoundManager.Instance.m_BGMGage > 0 && SoundManager.Instance.m_BGMGage < 6)
            {
                SoundManager.Instance.m_BGMGage--;
                SoundManager.Instance.SFXPlay("Button", m_ButtonSound);
                SettingGageCtrl(SoundManager.Instance.m_BGMGage);
            }
            else
                return;
        }
    }

    void SettingTextCtrl(int Gage)
    {
        for (int i = 0; i < 3; i++)
        {
            if (i == Gage)
            {
                m_SettingText[i].SetActive(true);
            }
            else
            {
                m_SettingText[i].SetActive(false);
            }
        }
    }

    public void KeyHelpersMouse(int MouseNumber)
    {
        for (int i = 0; i < 3; i++)
        {
            if (i == MouseNumber)
                m_KeyHelpersMenus[i].SetActive(true);
            else
                m_KeyHelpersMenus[i].SetActive(false);
        }
    }

    void SettingGageCtrl(int Gage)
    {
        if (!m_SFX[0] && !m_BGM[0])
        {
            for (int i = Gage; i < 6; i++)
            {
                if (m_SettingMenuNumber == 1)
                {
                    if (i == 0)
                        m_SFX[1].sprite = m_SettingGageOff;
                    else if (i == Gage)
                        m_SFX[i].sprite = m_SettingGageOn;
                    else if (i > Gage)
                        m_SFX[i].sprite = m_SettingGageOff;
                }

                else if (m_SettingMenuNumber == 2)
                {
                    if (i == 0)
                        m_BGM[1].sprite = m_SettingGageOff;
                    else if (i == Gage)
                        m_BGM[i].sprite = m_SettingGageOn;
                    else if (i > Gage)
                        m_BGM[i].sprite = m_SettingGageOff;
                }
            }
        }
    }

    void SFXSoundCheck()
    {
        if (SoundManager.Instance.m_SFXGage == 0)
            SoundManager.Instance.SFXSoundVolume(-80);
        else if (SoundManager.Instance.m_SFXGage == 1)
            SoundManager.Instance.SFXSoundVolume(-20);
        else if (SoundManager.Instance.m_SFXGage == 2)
            SoundManager.Instance.SFXSoundVolume(-10);
        else if (SoundManager.Instance.m_SFXGage == 3)
            SoundManager.Instance.SFXSoundVolume(-5);
        else if (SoundManager.Instance.m_SFXGage == 4)
            SoundManager.Instance.SFXSoundVolume(0);
        else if (SoundManager.Instance.m_SFXGage == 5)
            SoundManager.Instance.SFXSoundVolume(10);
    }

    void BGMSoundCheck()
    {
        if (SoundManager.Instance.m_BGMGage == 0)
            SoundManager.Instance.BGSoundVolume(-80);
        else if (SoundManager.Instance.m_BGMGage == 1)
            SoundManager.Instance.BGSoundVolume(-20);
        else if (SoundManager.Instance.m_BGMGage == 2)
            SoundManager.Instance.BGSoundVolume(-10);
        else if (SoundManager.Instance.m_BGMGage == 3)
            SoundManager.Instance.BGSoundVolume(-5);
        else if (SoundManager.Instance.m_BGMGage == 4)
            SoundManager.Instance.BGSoundVolume(0);
        else if (SoundManager.Instance.m_BGMGage == 5)
            SoundManager.Instance.BGSoundVolume(10);
    }
}
