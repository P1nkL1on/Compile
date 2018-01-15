; pow int ( int )
define i32 @$0pow(i32 %_0x) #0 {
  ret i32    ...
}


; main int (  )
define i32 @$1main() #1 {
;While
  br label %Whilecond2
Whilecond2:
  %tmp1 = icmp eq i1 1, 1
  br i1 %tmp1, label %Whileaction2, label %Whilecont2
Whileaction2:
;While
  br label %Whilecond1
Whilecond1:
  %tmp2 = icmp eq i1 1, 1
  br i1 %tmp2, label %Whileaction1, label %Whilecont1
Whileaction1:
  %_0c = alloca i32
  store i32 0, i32* %_0c

  br label %Whilecond1
Whilecont1:
  %_1b = alloca i32
  store i32 1, i32* %_1b

  br label %Whilecond2
Whilecont2:
  %_2s = alloca i32
  store i32 0, i32* %_2s
  %$1_2s = load i32, i32* %_2s
  %_3i = alloca i32
  store i32 0, i32* %_3i
  %$1_3i = load i32, i32* %_3i
;For
  br label %Forcond4
Forcond4:
  %$2_3i = load i32, i32* %_3i
  %tmp3 = icmp slt i32 %$2_3i, 100
  br i1 %tmp3, label %Foraction4, label %Forcont4
Foraction4:
;If
  %tmp4 = icmp slt i32 %$2_3i, 50
  br i1 %tmp4, label %Ifthen3, label %Ifelse3
Ifthen3:
  %tmp6 = mul i32 %$2_3i, 10
  %tmp5 = add i32 %tmp6, %$1_2s
  store i32 %tmp5, i32* %_2s
  %$2_2s = load i32, i32* %_2s

  br label %Ifcont3
Ifelse3:
  %tmp10 = mul i32 %$2_3i, 5
  %tmp9 = sub i32 %$2_2s, %tmp10
  store i32 %tmp9, i32* %_2s
  %$3_2s = load i32, i32* %_2s

  br label %Ifcont3
Ifcont3:
  %tmp13 = add i32 1, %$3_2s
  store i32 %tmp13, i32* %_2s
  %$4_2s = load i32, i32* %_2s

  %tmp15 = add i32 1, %$2_3i
  store i32 %tmp15, i32* %_3i
  %$3_3i = load i32, i32* %_3i
  %tmp17 = sub i32 %$4_2s, 1
  store i32 %tmp17, i32* %_2s
  %$5_2s = load i32, i32* %_2s
  br label %Forcond4
Forcont4:
  %_4a = alloca i32
  store i32 0, i32* %_4a
  ret i32 0
}


