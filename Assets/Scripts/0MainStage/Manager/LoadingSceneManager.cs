using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingSceneManager : MonoBehaviour
{
    static string nextScene;

    [SerializeField]
    Image progressBar;

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    void Start()
    {
        StartCoroutine(LoadSceneProcess());
    }

    IEnumerator LoadSceneProcess()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene); // �񵿱� ��� > �ٸ��۾� ����
        op.allowSceneActivation = false; // ���� �ѱ��� �ȳѱ���

        float timer = -5f;
        while (!op.isDone)
        {
            yield return null;

            if (op.progress < 0.3f)
            {
                progressBar.fillAmount = op.progress;
            }
            else
            {
                timer += Time.unscaledDeltaTime;
                progressBar.fillAmount = Mathf.Lerp(0.3f, 1f, timer);
                if (progressBar.fillAmount >= 1f)
                {
                    FadeInOutManager.Instance.OutStartFadeAnim();
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
}
