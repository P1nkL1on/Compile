; main int (  )
define i32 @main() #0 {
  %_0k = alloca i32
  store i32 100, i32* %_0k
  %_1j = alloca i32
  store i32 0, i32* %_1j
  %$1_1j = load i32, i32* %_1j
;For
  br label %Forcond1
Forcond1:
  %$2_1j = load i32, i32* %_1j
  %tmp2 = icmp slt i32 %$2_1j, 10
  %tmp1 = icmp eq i1 %tmp2, 1
  %cond1 = icmp %tmp1
  br i1 %cond1, label %Foraction1, label %Forcont1
Foraction1:
  %tmp3 = sub i32 %$2_1j, 1
  store i32 %tmp3, i32* %_1j
  %$3_1j = load i32, i32* %_1j
  br label %Forcond1
Forcont1:
  %_2s = alloca i32
  store i32 0, i32* %_2s
  %$1_2s = load i32, i32* %_2s
;While
  br label %Whilecond2
Whilecond2:
  %tmp6 = trunc i32 1 to i1
  %tmp5 = icmp eq i1 %tmp6, 1
  %cond2 = icmp %tmp5
  br i1 %cond2, label %Whileaction2, label %Whilecont2
Whileaction2:
  br label %Whilecond2
Whilecont2:
;While
  br label %Whileaction4
Whileaction4:
  %_3i = alloca i32
  store i32 0, i32* %_3i
  %$1_3i = load i32, i32* %_3i
;For
  br label %Forcond3
Forcond3:
  %$2_3i = load i32, i32* %_3i
  %tmp8 = icmp slt i32 %$2_3i, 10
  %tmp7 = icmp eq i1 %tmp8, 1
  %cond3 = icmp %tmp7
  br i1 %cond3, label %Foraction3, label %Forcont3
Foraction3:
  %tmp9 = mul i32 %$1_2s, 2
  store i32 %tmp9, i32* %_2s
  %$2_2s = load i32, i32* %_2s

  %tmp11 = add i32 1, %$2_3i
  store i32 %tmp11, i32* %_3i
  %$3_3i = load i32, i32* %_3i
  %tmp13 = add i32 1, %$2_2s
  store i32 %tmp13, i32* %_2s
  %$3_2s = load i32, i32* %_2s
  br label %Forcond3
Forcont3:
  br label %Whilecond4
Whilecond4:
  %tmp15 = icmp eq i1    ..., 1
  %cond4 = icmp %tmp15
  br i1 %cond4, label %Whileaction4, label %Whilecont4
  br label %Whilecond4
Whilecont4:
  ret i32 0
}


