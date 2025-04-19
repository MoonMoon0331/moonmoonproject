
//角色對話節點
VAR NPCNode = 0


//如果再文化後面輸入#U，指的是要更新角色的表情資訊
//角色表情 (D)Default(H)Happy(S)Sad(A)Angry(T)Thinking(K)Shock
//#U#0#D#ID //更新 (ID) (表情) (顯示名字/根據ID/根據玩家名字/none)


->EyeCatchDay1_1

===EyeCatchDay1_1===
 ……李志誠，對嗎？ #U#0#T#白晴方
*點頭 #U#999#D#Player
 嗯，好。行李不多，不錯。#U#0#H#白晴方
 我是白晴方，接下來幾天，我們會有很長的相處時間。#U#0#H#白晴方
先說一下，這裡跟外面不太一樣。不用急著理解，你只要記得自己是來「工作」的就好。#U#0#D#白晴方

->END