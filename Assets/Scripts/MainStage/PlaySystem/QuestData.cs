using System.Collections;
using System.Collections.Generic;

public class QuestData
{
    public string m_QuestName;
    public int[] m_NpcId;

    public QuestData(string name, int[] npc)
    {
        m_QuestName = name;
        m_NpcId = npc;
    }
}
