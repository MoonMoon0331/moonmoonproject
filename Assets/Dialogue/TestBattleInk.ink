VAR timeLimit = 0.0

->Day1

===Day1===
->Opening1

===Opening1===
~timeLimit = 1
你好?
+給老子把錢拿來
    ->Intimidation

->Opening2

=Intimidation
你是誰? 
我是誰不重要
怪咖
->CallEnd

===Opening2===
~timeLimit = 5.0

你好?
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
