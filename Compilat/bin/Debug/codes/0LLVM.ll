define i32 @where(){
entry:
  %a = i32 10
  ret i32    ...
}


define i32 @main(){
entry:
  %tmp1 = call i32 @where()
  %b = %tmp1
  %tmp1 = add i32 ?ARRAY ELEMENT?, 10
  %c = %tmp1
?ARRAY ELEMENT?
  ?ARRAY ELEMENT? = i32 20
  ret i32 1
}


