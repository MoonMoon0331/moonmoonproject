
//角色對話節點
VAR NPCNode = 0


//如果再文化後面輸入#U，指的是要更新角色的表情資訊
//角色表情 (D)Default(H)Happy(S)Sad(A)Angry(T)Thinking(K)Shock
//#U#0#D#ID //更新 (ID) (表情) (顯示名字/根據ID/根據玩家名字/none)


->Start

===Start===

這天不存在
->END

===Day1===
你好啊，今天天氣真的很好。 #U#999#H#Player
要是可以不用上班就太好了 #U#999#S#Player
沒事的，我們還是要繼續加油!! #U#0#H#ID

->END

===Day2===
已經是第二天了但完全沒有發生任何改變呢... #U#0#D#ID
诶對了，你昨天的事情處理了嗎? #U#0#K#ID
    
*還沒欸! #U#999#K#Player
    怎麼會還沒!!!!趕快來做! #U#0#A#ID
    ->END

*放心吧已經做了! #U#999#H#Player

    那就好了，今天又是美好的一天 :) #U#0#H#ID
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