; pow int ( int )
define i32 @pow(i32 %x) #0 {
  ret i32    ...
}


; main int (  )
define i32 @main() #1 {
;While
  br label %Whilecond31
Whilecond31:
  %cond31 = icmp 1
  br i1 %cond31, label %Whileaction31, label %Whilecont31
Whileaction31:
;While
  br label %Whilecond30
Whilecond30:
  %cond30 = icmp 1
  br i1 %cond30, label %Whileaction30, label %Whilecont30
Whileaction30:
  %c = alloca i32
  store i32 0, i32* %c

  br label %Whilecond30
Whilecont30:
  %b = alloca i32
  store i32 1, i32* %b

  br label %Whilecond31
Whilecont31:
  %s = alloca i32
  store i32 0, i32* %s
  %i = alloca i32
  store i32 0, i32* %i
;For
  br label %Forcond33
Forcond33:
  %cond33 = icmp slt i32 %i, 100

  br i1 %cond33, label %Foraction33, label %Forcont33
Foraction33:
;If
  %cond32 = icmp slt i32 %i, 50

  br i1 %cond32, label %Ifthen32, label %Ifelse32
Ifthen32:
  %tmp2 = mul i32 %i, 10
  %tmp1 = add i32 %tmp2, %s
  store i32 %tmp1, i32* %s

  br label %Ifcont32
Ifelse32:
  %tmp6 = mul i32 %i, 5
  %tmp5 = sub i32 %s, %tmp6
  store i32 %tmp5, i32* %s

  br label %Ifcont32
Ifcont32:
  %tmp9 = add i32 1, %s
  store i32 %tmp9, i32* %s

  %tmp11 = add i32 1, %i
  store i32 %tmp11, i32* %i
  %tmp13 = sub i32 %s, 1
  store i32 %tmp13, i32* %s
  br label %Forcond33
Forcont33:
  %a = alloca i32
  store i32 0, i32* %a
  ret i32 0
}


