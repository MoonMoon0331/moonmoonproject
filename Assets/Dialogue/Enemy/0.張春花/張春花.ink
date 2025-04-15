VAR timeLimit = 0.0
VAR totalTimeLimit = 0.0
VAR chooseNodeName = "Opening1"
VAR isInChoosing = false
VAR NPCNode = 0

//如果再文化後面輸入#U，指的是要更新角色的表情資訊
//角色表情 (D)Default(H)Happy(S)Sad(A)Angry(T)Thinking(K)Shock(N)隱藏
//#U#0#D#ID#0#D#ID#RED#SHAKE
//0.更新(U)僅更新表情(UE) 
//1.(左邊玩家ID)(這裡是NPCDATABASE)
//2.(左邊玩家表情)
//3.(右邊玩家ID)(這裡是EnemyDATABASE)
//4.(右邊玩家表情)
//5.(顯示名字/根據ID/根據玩家名字/none)
//6.文字顏色(RED紅色)
//7.文字效果(K/SHAKE震動一下)(D/Default正常)


//故事節點
VAR storyNode = 0

//測試用
->Day1

===Day1===
->Day1_1

===Day1_1===
這裡是我們的「辦公室」，每台電腦後面都是一口井。你今天也會學著往井裡丟聲音。#U#0#H#0#N#白晴方
你接下來會學到怎麼跟「客戶」講話，但記住，不要把自己當人。#U#0#D#0#N#白晴方
……這就是工作的地方？#U#999#D#0#N#Player
沒錯!待會撥通後你會聽見一位老奶奶的聲音。你的目標是讓她「主動說出帳號」，明白嗎？#U#0#H#0#N#白晴方
好的，我會盡力的。#U#999#D#0#N#Player
另外，如果你腦中的選項沒有出現喜歡的回答時，可以稍微多想一下喔~#U#0#H#0#N#白晴方
原來如此，我理解了。#U#999#T#0#N#Player
但如果想太久的話，也有可能被對方掛斷喔!那我們開始吧(撥通)#U#0#H#0#N#白晴方

喂？我這裡是……喔對，我剛剛好像有申請補助，你們是社會局的嗎？#U#999#D#0#D#ID
    ->Choose1_1

===Choose1_1===
~totalTimeLimit = 10.0
~timeLimit = 2
~chooseNodeName = "Choose1_2"
~isInChoosing = true
+阿姨，小心，這通是一個詐騙......#U#999#A#0#D#Player#BLACK#D
    ->DialogueTerminated
    
->Choose1_2_1

=DialogueTerminated
~isInChoosing = false
(掛斷)你在搞甚麼東西!!! #U#0#A#0#N#白晴方
......#U#999#D#0#N#Player
你千萬要記得，講話前要多經思考，我們再重來一次。#U#0#A#0#N#白晴方
好的(重新撥通)#U#999#D#0#N#Player
~totalTimeLimit = 8.0
->Choose1_2_2

===Choose1_2_1=== //不更新對話文字
~isInChoosing = true
喂？我這裡是……喔對，我剛剛好像有申請補助，你們是社會局的嗎？#UE#999#D#0#D#ID
~chooseNodeName = "CallEnd"
~timeLimit = 8.0
+啊阿姨~您撥錯啦~但我可以幫你處理唷！#U#999#H#0#D#Player#BLACK#N
    ->Day1_2
+是的阿姨，我來幫您做身份確認喔～#U#999#H#0#D#Player#BLACK#N
    ->Day1_3
    
->CallEnd

===Choose1_2_2=== //仍然更新對話文字
~isInChoosing = true
喂？我這裡是……喔對，我剛剛好像有申請補助，你們是社會局的嗎？#U#999#D#0#D#ID
~chooseNodeName = "CallEnd"
~timeLimit = 8.0
+啊阿姨~您撥錯啦~但我可以幫你處理唷！#U#999#H#0#D#Player#BLACK#N
    ->Day1_2
+是的阿姨，我來幫您做身份確認喔～#U#999#H#0#D#Player#BLACK#N
    ->Day1_3
    
->CallEnd

===Day1_2===
~isInChoosing = false
喔?真是太好了，那我需要做甚麼呢?#U#999#D#0#T#張春花
其實只需要將您的身分證號碼拿給我就好了喔，我們也是政府機關，會幫你將他送給社會局的部門!#U#999#H#0#T#Player
太好了，我可能需要找一下我的身分證號碼喔等我一下~#U#999#H#0#H#張春花
找到了，我的號碼是A123456789~#U#999#D#0#H#張春花
非常感謝您，我會再幫你轉達給他們的。掰掰(掛斷)#U#999#H#0#H#Player
(嘟...嘟...嘟...)#U#999#H#0#N#Player
你表現的很好!!!相信接下來你一定也可以拿到非常多號碼的!那讓我們進入到下一個環節吧! #U#0#H#0#N#白晴方
->END

===Day1_3===
~isInChoosing = false
喔?真是太好了，那我需要做甚麼呢?#U#999#D#0#T#張春花
其實只需要將您的身分證號碼拿給我就好了喔~#U#999#H#0#T#Player
太好了，我可能需要找一下我的身分證號碼喔等我一下~#U#999#H#0#H#張春花
找到了，我的號碼是A123456789~#U#999#D#0#H#張春花
非常感謝您，我會再幫你轉達給他們的。掰掰(掛斷)#U#999#H#0#H#Player
(嘟...嘟...嘟...)#U#999#H#0#N#Player
你表現的很好!!!相信接下來你一定也可以拿到非常多號碼的!那讓我們進入到下一個環節吧! #U#0#H#0#N#白晴方
->END

===CallEnd===
~isInChoosing = false
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
