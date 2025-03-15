VAR timeLimit = 0.0
VAR chapterName = "Opening1"
VAR itCanChoosing = false

->Day1

===Day1===
->Opening1

===Opening1===
你好?

~timeLimit = 2
~chapterName = "Opening2"
+給老子把錢拿來
    ->Intimidation

=Intimidation
你是誰? 
我是誰不重要
怪咖
->CallEnd

===Opening2===
~itCanChoosing = true
你好
~chapterName = "CallEnd"
~timeLimit = 10.0
+你好，這裡是警政署連線系統
    ->policeFraud
+你好，請問您是___小姐嗎?
    ->relativeFraud
    
->CallEnd

===policeFraud===
->END

===relativeFraud===
->END

===CallEnd===
(嘟...嘟...嘟...)

->END
