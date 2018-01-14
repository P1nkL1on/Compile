; main int (  )
define i32 @main() #0 {
  %i = alloca i32
  store i32 0, i32* %i
;While
  br label %Whilecond29
Whilecond29:
  %cond29 = icmp 1
  br i1 %cond29, label %Whileaction29, label %Whilecont29
Whileaction29:
  %tmp1 = add i32 1, %i
  store i32 %tmp1, i32* %i
;If
  %cond28 = icmp sgt i32 %i, 100

  br i1 %cond28, label %Ifthen28, label %Ifcont28
Ifthen28:
   ...
  br label %Ifcont28
Ifcont28:

  br label %Whilecond29
Whilecont29:
  ret i32 1
}


