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
  %cond1 = icmp slt i32 %$2_1j, 10

  br i1 %cond1, label %Foraction1, label %Forcont1
Foraction1:
  %tmp1 = sub i32 %$2_1j, 1
  store i32 %tmp1, i32* %_1j
  %$3_1j = load i32, i32* %_1j
  br label %Forcond1
Forcont1:
  %_2s = alloca i32
  store i32 0, i32* %_2s
  %$1_2s = load i32, i32* %_2s
;While
  br label %Whilecond2
Whilecond2:
  %cond2 = icmp    ...
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
  %cond3 = icmp slt i32 %$2_3i, 10

  br i1 %cond3, label %Foraction3, label %Forcont3
Foraction3:
  %tmp3 = mul i32 %$1_2s, 2
  store i32 %tmp3, i32* %_2s
  %$2_2s = load i32, i32* %_2s

  %tmp5 = add i32 1, %$2_3i
  store i32 %tmp5, i32* %_3i
  %$3_3i = load i32, i32* %_3i
  %tmp7 = add i32 1, %$2_2s
  store i32 %tmp7, i32* %_2s
  %$3_2s = load i32, i32* %_2s
  br label %Forcond3
Forcont3:
  br label %Whilecond4
Whilecond4:
  %cond4 = icmp    ...
  br i1 %cond4, label %Whileaction4, label %Whilecont4
  br label %Whilecond4
Whilecont4:
  ret i32 0
}


