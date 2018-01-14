; main int (  )
define i32 @main() #0 {
  %k = alloca i32
  store i32 100, i32* %k
  %j = alloca i32
  store i32 0, i32* %j
;For
  br label %Forcond18
Forcond18:
  %cond18 = icmp slt i32 %j, 10

  br i1 %cond18, label %Foraction18, label %Forcont18
Foraction18:
  %tmp1 = sub i32 %j, 1
  store i32 %tmp1, i32* %j
  br label %Forcond18
Forcont18:
  %s = alloca i32
  store i32 0, i32* %s
;While
  br label %Whilecond19
Whilecond19:
  %cond19 = icmp    ...
  br i1 %cond19, label %Whileaction19, label %Whilecont19
Whileaction19:
  br label %Whilecond19
Whilecont19:
;While
Whileaction21:
  %i = alloca i32
  store i32 0, i32* %i
;For
  br label %Forcond20
Forcond20:
  %cond20 = icmp slt i32 %i, 10

  br i1 %cond20, label %Foraction20, label %Forcont20
Foraction20:
  %tmp3 = mul i32 %s, 2
  store i32 %tmp3, i32* %s

  %tmp5 = add i32 1, %i
  store i32 %tmp5, i32* %i
  %tmp7 = add i32 1, %s
  store i32 %tmp7, i32* %s
  br label %Forcond20
Forcont20:
  br label %Whilecond21
Whilecond21:
  %cond21 = icmp    ...
  br i1 %cond21, label %Whileaction21, label %Whilecont21
  br label %Whilecond21
Whilecont21:
  ret i32 0
}


