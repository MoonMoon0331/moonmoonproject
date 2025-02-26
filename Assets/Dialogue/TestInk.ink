->Scene1
VAR Favorability = 0


===Scene1===
問大家一個問題 #小美

1+2等於多少 #

* 等於2
    回答錯誤 
    ~Favorability = Favorability-5
    ->Wrong
* 等於3
    回答正確 
    ~Favorability = Favorability + 5
    ->Right
    
=Right
你很聰明
->Combine

=Wrong
可惜了
->Combine

= Combine
{Right:之前回答正確了}
{Favorability>0:->End1|->End2}

=End1
測試非常成功
->END

=End2
測試非常成功，但是你有點笨我不喜歡
->END

->END