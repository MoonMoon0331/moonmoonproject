VAR timeLimit = 0.0
VAR totalTimeLimit = 0.0
VAR chapterName = "Opening1"
VAR itCanChoosing = false
VAR NPCNode = 0

//如果再文化後面輸入#U，指的是要更新角色的表情資訊
//角色表情 (D)Default(H)Happy(S)Sad(A)Angry(T)Thinking(K)Shock
//#U#0#D#ID#0#D#ID#RED#SHAKE
//0.更新 
//1.(左邊玩家ID)
//2.(左邊玩家表情)
//3.(右邊玩家ID)
//4.(右邊玩家表情)
//5.(顯示名字/根據ID/根據玩家名字/none)
//6.文字顏色(RED紅色)
//7.文字效果(K/SHAKE震動一下)(D/Default正常)


//故事節點
VAR storyNode = 0

//測試用
->Day1

===Day1===
->Opening1

===Opening1===
你好?#U#999#D#0#D#ID
~totalTimeLimit = 12.0
~timeLimit = 2
~chapterName = "Opening2"
+給老子把錢拿來#U#999#A#0#D#Player#RED#K
    ->Intimidation

=Intimidation
~itCanChoosing = false
你是誰? #U#999#D#0#K#ID
我是誰不重要。#U#999#A#0#K#Player
怪怪的人。#U#999#A#0#S#ID
->CallEnd

===Opening2===
~itCanChoosing = true
你好#U#999#D#0#D#ID
~chapterName = "CallEnd"
~timeLimit = 10.0
+你好，這裡是警政署連線系統。#U#999#T#0#D#Player#BLACK#N
    ->policeFraud
+你好，請問您是___小姐嗎?#U#999#T#0#D#Player#BLACK#N
    ->relativeFraud
->CallEnd

===policeFraud===
我才不相信呢。#U#999#D#0#A#ID
~itCanChoosing = false
->CallEnd

===relativeFraud===
欸不是欸，你應該打錯電話了喔。#U#999#D#0#T#ID
~itCanChoosing = false
->END

===CallEnd===
~itCanChoosing = false
(嘟...嘟...嘟...)
    ->END



===Day2===
    ->END
===Day3===
    ->END
===Day4===
    ->END
===Day5===
    ->END
===Day6===
    ->END
===Day7===
    ->END

->END
