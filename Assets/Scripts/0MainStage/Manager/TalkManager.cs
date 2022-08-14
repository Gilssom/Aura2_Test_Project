using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    Dictionary<int, string[]> talkData;

    private static TalkManager m_instance;
    // �̱���
    public static TalkManager Instance
    {
        get
        {
            if (!m_instance)
            {
                m_instance = FindObjectOfType(typeof(TalkManager)) as TalkManager;

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
        }
        else if (m_instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(this);

        talkData = new Dictionary<int, string[]>();
        GenerateData();
    }

    void GenerateData()
    {
        talkData.Add(1000, new string[] { "���ݵ� ����� ���� ��������," + "\n" + "�� �ձ� �ѹ��̸� �� �������ٳ�!",
                                            "��� ��ȥ��" + "\n" + "���� ���ؿԳ�?",
                                            "����� ������."});
        talkData.Add(2000, new string[] { "���� �������� �ʾҴ�?",
                                            "�ʰ� �� ����� ����.",
                                            "�� ���� ����" + "\n" + "���� ���� �ܴ�."});
        talkData.Add(3000, new string[] { "����� ����?",
                                            "ǫ ������.",
                                            "���� �� �����Ű���?"});
        talkData.Add(4000, new string[] { "�����Ͻʽÿ�.",
                                            "�����Ͻʽÿ�.", 
                                            "�����Ͻʽÿ�."});

        talkData.Add(10 + 1000, new string[] { "���⿡ �´ٰ� ������ ���´� �����" + "\n" + "�ڳװ� �´°�?",
                                            "����.. �ƹ��� ����������"  + "\n" + "�̷� �����̰� ���ٴ� �����̱���.",
                                            "... �׷� �׷��� ��Ȳ�� ��Ȳ������"  + "\n" + "��¿ ���� ������..",
                                            "�� �ż����� �Ź��� ��ġ�� �޾� Ư���� ������� �� �� �ֳ�."  + "\n" + "�������� ���� �ο��� �� �� ������..",
                                            "�ƹ�ư ��ȥ���� �踦 ���� �� ������,"  + "\n" + "��ȥ�� ����� ��Ҵٸ� ã�ƿ��ְ�."});

        talkData.Add(10 + 2000, new string[] { "�̷��� � ���ھְ� �� ���� ������.",
                                            "���� �� �ƴϾ�. �� ���� �� ����� �� �� ������.",
                                            "���� �԰� �ִ� �ʵ� ���� ������.",
                                            "��ȥ�� �ʰ��� ������ ����� ���� �ȴܴ�?"  + "\n" + "��ȥ�� ����� ��Ҵٸ� ã�ƿ���."});

        talkData.Add(10 + 3000, new string[] { "���������...",
                                            "�̰��� �ƽô� ���� �ż����� �Ź��� �ɷ��� ���� �̵��� ��,"  + "\n" + "�����͸� ����ϴ� �����ε� ������ ���� ����������.",
                                            "�׵��� ���� ����� �־�����.",
                                            "����ó�� � ���� ���� �帮������� �ʾ�����,"  + "\n" + "��Ȳ�� ��Ȳ������...",
                                            "��·�� �� ��Ź�帳�ϴ�."  + "\n" + "������ �̷��� ���� ���� ���� �븩�̴ϱ��.",
                                            "���� ������ �� ������ ������."});

        talkData.Add(10 + 4000, new string[] { "�ȳ��ϼ���.",
                                            "�������� �ΰ����� ������ ����... �̰� ������ ������ ������,"  + "\n" + "�����͵��� ����ġ�� ��Ȳ��...",
                                            "�� �ɷ����� �������� ���� �����͵��� óġ�ϸ�," + "\n" + "�׷� �ϵ� �������ٴ� ����� �˰� �Ǿ���.",
                                            "�׷��� ���� �������� �� ���� ������..."  + "\n" + "���̵� ���ƿ����� ���߽��ϴ�.",
                                            "�׷���... ������ ���� �ʴ´ٸ�"  + "\n" + "�ǵ��� �� ���� ���� ���������� �մϴ�.",
                                            "��� �̷����� ����� �ܿ�ܿ� ���ݾ� ���� ��...",
                                            "�׷��� ���� �� �̷��� �ִ��� �����Ŵٸ� �� ���� ������," + "\n" + "���ſ� �������� �� ���� �ܿ� �ϴ� �����Դϴ�.",
                                            "���� ������׿�." + "\n" + "�׷��� ������ �����̴ٸ� �����Ͻñ� �ٶ��ϴ�."});
    }

    public string GetTalk(int id, int talkIndex)
    {
        if (talkIndex == talkData[id].Length)
            return null;
        else
            return talkData[id][talkIndex];
    }
}
