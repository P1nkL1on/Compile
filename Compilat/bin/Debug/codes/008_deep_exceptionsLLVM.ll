; main int (  )
define i32 @main() #0 {
;For
  br label %Forcond1
Forcond1:
  %tmp1 = icmp eq i1 1, 1
  %cond1 = icmp %tmp1
  br i1 %cond1, label %Foraction1, label %Forcont1
Foraction1:
  br label %Forcond1
Forcont1:
  %_0i = alloca i32
  store i32 0, i32* %_0i
  %$1_0i = load i32, i32* %_0i
;For
  br label %Forcond2
Forcond2:
  %$2_0i = load i32, i32* %_0i
  %tmp3 = icmp slt i32 %$2_0i, 10
  %tmp2 = icmp eq i1 %tmp3, 1
  %cond2 = icmp %tmp2
  br i1 %cond2, label %Foraction2, label %Forcont2
Foraction2:
  %tmp4 = add i32 1, %$2_0i
  store i32 %tmp4, i32* %_0i
  %$3_0i = load i32, i32* %_0i
  br label %Forcond2
Forcont2:
  %_1x = alloca i32
  store i32 10, i32* %_1x

  %_2x = alloca i32
  store i32 11, i32* %_2x



  ret i32 0
}


