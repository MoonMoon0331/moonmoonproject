//角色進入天數
VAR currentDay = 1
//左邊角色表情 (D)Default(H)Happy(S)Sad(A)Angry(T)Thinking(K)Shock
VAR leftEmotion = "D" 
//右邊角色表情 (D)Default(H)Happy(S)Sad(A)Angry(T)Thinking(K)Shock
VAR rightEmotion = "D"
//角色對話節點
VAR NPCNode = 0
//講話角色 (L)左邊->我方角色(R)右邊->對方角色
VAR currentNPC = "L"
//目前顯示角色名稱
VAR NPCName = "劉婉蓉"

->Start

===Start===
{ currentDay == 1:
    ->Day1
- else:
    這天不存在
    ->END
}
->Day1

===Day1===
你好啊，今天天氣真的很好

->END