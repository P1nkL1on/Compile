@str3 = private unnamed_addr constant [24 x i8] c"123    332   as 'hui'  \00"
; main int (  )
define i32 @main() #0 {
;While
Whileaction1:
  %tmp1 = add i32 2, 1
  br label %Whilecond1
Whilecond1:
  %cond1 = icmp 1
  br i1 %cond1, label %Whileaction1, label %Whilecont1
  br label %Whilecond1
Whilecont1:
;While
Whileaction2:
  %tmp3 = add i32 2, 3
  br label %Whilecond2
Whilecond2:
  %cond2 = icmp    ...
  br i1 %cond2, label %Whileaction2, label %Whilecont2
  br label %Whilecond2
Whilecont2:
;While
Whileaction3:
  %tmp5 = add i32 2, 5
  br label %Whilecond3
Whilecond3:
  %cond3 = icmp    ...
  br i1 %cond3, label %Whileaction3, label %Whilecont3
  br label %Whilecond3
Whilecont3:
  %ten = alloca i8
  store i8 92, i8* %ten
;If
  %cond6 = icmp sgt i32 1, 2

  br i1 %cond6, label %Ifthen6, label %Ifelse6
Ifthen6:
  %tmp7 = add i32 1, 2
  br label %Ifcont6
Ifelse6:
  %tmp9 = add i32 1, 3
  %tmp11 = add i32 6, 5
;If
  %cond5 = icmp    ...
  br i1 %cond5, label %Ifthen5, label %Ifelse5
Ifthen5:
  br label %Ifcont5
Ifelse5:
;If
  %cond4 = icmp    ...
  br i1 %cond4, label %Ifthen4, label %Ifcont4
Ifthen4:
  br label %Ifcont4
Ifcont4:

  br label %Ifcont5
Ifcont5:

  br label %Ifcont6
Ifcont6:
;For
  br label %Forcond7
Forcond7:
  %cond7 = icmp 1
  br i1 %cond7, label %Foraction7, label %Forcont7
Foraction7:
  br label %Forcond7
Forcont7:
;If
  %cond11 = icmp eq i32 0, 0

  br i1 %cond11, label %Ifthen11, label %Ifelse11
Ifthen11:
  br label %Ifcont11
Ifelse11:
;If
  %cond8 = icmp sgt i32 2, 1

  br i1 %cond8, label %Ifthen8, label %Ifcont8
Ifthen8:
44
  br label %Ifcont8
Ifcont8:
;If
  %cond9 = icmp sgt i32 3, 2

  br i1 %cond9, label %Ifthen9, label %Ifcont9
Ifthen9:
  br label %Ifcont9
Ifcont9:
;If
  %cond10 = icmp sgt i32 5, 6

  br i1 %cond10, label %Ifthen10, label %Ifcont10
Ifthen10:
  br label %Ifcont10
Ifcont10:

  br label %Ifcont11
Ifcont11:
  %b = alloca i32
  store i32 10, i32* %b
;While
  br label %Whilecond13
Whilecond13:
  %cond13 = icmp    ...
  br i1 %cond13, label %Whileaction13, label %Whilecont13
Whileaction13:
  %s = alloca i8*
  store i8* getelementptr ([24 x i8], [24 x i8]* @str3, i64 0, i64 0), i8** %s
;While
Whileaction12:
  %tmp13 = add i32 3, 1
  br label %Whilecond12
Whilecond12:
  %cond12 = icmp    ...
  br i1 %cond12, label %Whileaction12, label %Whilecont12
  br label %Whilecond12
Whilecont12:

  br label %Whilecond13
Whilecont13:
  ret i32 1
}


; a int (  )
define i32 @a() #1 {
  ret i32 0
}


