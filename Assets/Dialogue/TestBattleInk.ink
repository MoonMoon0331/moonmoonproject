VAR timeLimit = 0.0
VAR totalTimeLimit = 0.0
VAR chapterName = "Opening1"
VAR itCanChoosing = false



->Day1

===Day1===
->Opening1

===Opening1===
你好?
~totalTimeLimit = 12.0
~timeLimit = 2
~chapterName = "Opening2"
+給老子把錢拿來#NONE#RED#SHAKE
    ->Intimidation

=Intimidation
~itCanChoosing = false
你是誰? 
我是誰不重要。
怪怪的人。
->CallEnd

===Opening2===
~itCanChoosing = true
你好
~chapterName = "CallEnd"
~timeLimit = 10.0
+你好，這裡是警政署連線系統。#NONE#BLACK#SHAKE
    ->policeFraud
+你好，請問您是___小姐嗎?#NONE#BLACK#SHAKE
    ->relativeFraud
->CallEnd

===policeFraud===
我才不相信呢。
~itCanChoosing = false
->CallEnd

===relativeFraud===
欸不是欸，你應該打錯電話了喔。
~itCanChoosing = false
->END

===CallEnd===
~itCanChoosing = false
(嘟...嘟...嘟...)

->END
