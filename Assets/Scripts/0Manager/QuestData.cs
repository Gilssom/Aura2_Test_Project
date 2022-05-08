using System.Collections;
using System.Collections.Generic;

public class QuestData
{
    public string QuestName;
    public int[] NpcId;

    // 구조체 생성을 위해 매개변수 생성자를 작성
    public QuestData(string name, int[] npc)
    {
        QuestName = name;
        NpcId = npc;
    }
}
