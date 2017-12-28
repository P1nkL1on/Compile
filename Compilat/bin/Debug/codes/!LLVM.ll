define i32 @main(){
entry:
  %tmp0 = add i8 %C, 10
  %tmp1 = add i8 %C, %glob
  %D = %tmp1
  %tmp2 = add i32 1,    ...
  ret i32 %tmp2
}


