using System.Collections;
using System.Collections.Generic;

public class QuestData
{
    public string QuestName;
    public int[] NpcId;

    // ����ü ������ ���� �Ű����� �����ڸ� �ۼ�
    public QuestData(string name, int[] npc)
    {
        QuestName = name;
        NpcId = npc;
    }
}
