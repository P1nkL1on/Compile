define i32 @Foo(i32 %X, i32 %Y){
entry:
  %tmp0 = add i32 %Y, %X
  ret i32 %tmp0
}


define void @main(){
entry:
  %D = i32 10
  %Y = i32 20
  %tmp1 = add i32 30, 30
  %DD = i32 0
  %tmp2 = add i32 %DD, 3
  %tmp3 = add i32 %tmp2, 2
  %tmp4 = add i32 %tmp3, 2
  %tmp6 = call i32 @Foo(i32 1, i32 %tmp4)
  %X = %tmp6
  ret void
}


