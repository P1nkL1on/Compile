; main int (  )
define i32 @main() #0 {
  %summ = alloca i32
  store i32 0, i32* %summ
  %a = alloca i32
  store i32 10, i32* %a
  %b = alloca i32
  store i32 10, i32* %b
  %c = alloca i32
  store i32 10, i32* %c
  %DDDDD = alloca i32
  store i32 1000, i32* %DDDDD
  %tmp2 = add i32 %c, %b
  %tmp1 = add i32 %tmp2, %a
  store i32 %tmp1, i32* %summ

  %summ2 = alloca i32
  store i32 0, i32* %summ2
  %a = alloca i32
  store i32 20, i32* %a
  %b = alloca i32
  store i32 20, i32* %b
  %c = alloca i32
  store i32 30, i32* %c
  %tmp6 = add i32 %c, %b
  %tmp5 = add i32 %tmp6, %a
  store i32 %tmp5, i32* %summ2

  %diff = alloca i32
  %tmp9 = sub i32 %summ, %summ2
  store i32 %tmp9, i32* %diff
  ret i32 0
}


