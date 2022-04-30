using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    // ��ȭ �����͸� ������ Dictionary
    Dictionary<int, string[]> m_talkData;

    [SerializeField]
    bool LastTalk;

    void Awake()
    {
        DontDestroyOnLoad(this);
        m_talkData = new Dictionary<int, string[]>(); // �ʱ�ȭ
        GenerateData();
    }

    void GenerateData()
    {
        // Add �� ��ȭ ���� �߰� , ��ȭ �ϳ����� ���� ������ ��������Ƿ� �迭�� ���
        // Winter Stage Object
        m_talkData.Add(1000, new string[] { "..." , "�һ��̴�." });
        m_talkData.Add(2000, new string[] { "�� ���°ž�." });
        m_talkData.Add(3000, new string[] { "..." , "�̱ÿ��� �һ��� �ִ�." });
        m_talkData.Add(4000, new string[] { "�󸮱� ��ų�� ȹ���Ͽ����ϴ�." });

        // Quest Talk
        // Winter Stage
        m_talkData.Add(10 + 1000, new string[] { "�������." , "���� �Ϸ� ���̳���." });

        m_talkData.Add(20 + 2000, new string[] { "�ʴ� ������?!", "... ������ݾ�!!", "�ָ� ������!!" });
        m_talkData.Add(21 + 2000, new string[] { "�ܸ� ���� �ϴ� ���� �����." });

        // Winter _ Maze Stage
        m_talkData.Add(40 + 3000, new string[] { "...", "���� �𸣰� ���� �����´�.", "���� �Ҹ��� �鸮�µ�..." });
        m_talkData.Add(41 + 4000, new string[] {"??? : ���ƾ�!! �̰� ����!!", "�ʹ� �����ݾ�...", "�ϴ� ���� ���� �����״ϱ�..", "�˾Ƽ� �����!!",
        "...", "�׷�.. �ѹ� ������.."});
    }

    // ������ ��ȭ ������ ��ȯ�ϴ� �Լ� ����
    public string GetTalk(int id, int talkIndex)
    {
        // ContainsKey() :: Dictionary �� �����Ͱ� �ִ��� Ȯ���� ����
        if(!m_talkData.ContainsKey(id))
        {
            // ����Ʈ �� ó�� ��縶�� ������
            // �⺻ ��縦 ������ ���� �ȴ�.
            if (!m_talkData.ContainsKey(id - id % 10))
            {
                // ��ȯ ���� �ִ� ����Լ��� return ���� �� ���־�� �Ѵ�.
                return GetTalk(id - id % 100, talkIndex); // Get First Talk
            }
            // �ش� ����Ʈ ���� ���� ��簡 ������
            // ����Ʈ �� ó�� ��縦 �����´�
            else
            {
                return GetTalk(id - id % 10, talkIndex); // Get First Quest Talk
            }
        }

        // ��ȭ�� ���� ������ ���Ͽ� ���� Ȯ��\
        if(talkIndex == m_talkData[id].Length)
        {
            LastTalk = true;
            return null;
        }
        else
        {
            // id �� ��ȭ Get -> talkIndex �� ��ȭ�� �� ������ Get
            LastTalk = false;
            return m_talkData[id][talkIndex];
        }
    }
}
