VAR stillTime = true
VAR timeLimit = 0
->Day1

===Day1===
->Opening1

===Opening1===
~timeLimit = 5
~stillTime = true
你好?
*給老子把錢拿來
    ->checkTime
    
=checkTime
{stillTime:
        -> Intimidation
- else:
        -> Opening2
}

=Intimidation
你是誰? 
我是誰不重要
怪咖
->CallEnd

===Opening2===
~timeLimit = 5
~stillTime = true
你好?
*你好，這裡是警政署連線系統
    ->checkTime1
*你好，請問您是___小姐嗎?
    ->checkTime2
    
=checkTime1
{stillTime:
        -> policeFraud
- else:
        -> CallEnd
}

=checkTime2
{stillTime:
        -> relativeFraud
- else:
        -> CallEnd
}

===policeFraud===
->END

===relativeFraud===
->END

===CallEnd===
(嘟...嘟...嘟...)

->END
