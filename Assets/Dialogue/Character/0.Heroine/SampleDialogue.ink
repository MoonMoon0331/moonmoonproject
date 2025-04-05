//角色進入天數
VAR currentDay = 1
//角色表情 (D)Default(H)Happy(S)Sad(A)Angry(T)Thinking(K)Shock
VAR currentEmotion = "D"
//角色對話節點
VAR NPCNode = 0
//講話角色ID //999是主角
VAR currentNPCID = 0
//目前顯示角色名稱
VAR NPCName = "劉婉蓉"

//如果再文化後面輸入#U，指的是要更新角色的表情資訊

->Start

===Start===
{currentDay == 1:->Day1}
{currentDay == 2:->Day2}

這天不存在
->END

===Day1===
~currentNPCID = 999
~currentEmotion = "H"
你好啊，今天天氣真的很好。 #U
~currentEmotion = "S"
要是可以不用上班就太好了 #U
~currentNPCID = 0
~currentEmotion = "H"
沒事的，我們還是要繼續加油!! #U

->END

===Day2===
已經是第二天了但完全沒有發生任何改變呢...

->END