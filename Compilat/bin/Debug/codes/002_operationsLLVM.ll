; main int (  )
define i32 @$0main() #0 {
  %_0var1 = alloca i32
  %_1va2 = alloca i32
  store i32 2, i32* %_1va2
  store i32 100, i32* %_0var1
  %$1_0var1 = load i32, i32* %_0var1
  store i32 3, i32* %_1va2
  %tmp1 = add i32 4, 3
  store i32 %tmp1, i32* %_1va2
  %tmp4 = mul i32 %$1_0var1, %$1_0var1
  %tmp3 = add i32 %tmp4, %$1_0var1
  store i32 %tmp3, i32* %_1va2
  %_2summ = alloca i32
  %tmp7 = add i32 20, 10
  store i32 %tmp7, i32* %_2summ
  %_3x = alloca i32
  store i32 1, i32* %_3x
  %$1_3x = load i32, i32* %_3x
  %_4y = alloca i32
  store i32 15, i32* %_4y
  %$1_4y = load i32, i32* %_4y
  %_5xy = alloca i32
  %tmp9 = add i32 %$1_4y, %$1_3x
  store i32 %tmp9, i32* %_5xy
  %_6XY = alloca i32
  %tmp11 = mul i32 %$1_3x, %$1_4y
  store i32 %tmp11, i32* %_6XY
  %_7res = alloca i32
  %tmp13 = sdiv i32 %$1_3x, %$1_4y
  store i32 %tmp13, i32* %_7res
  %_8ref = alloca double
  %tmp17 = fdiv double 12.33, 33.4
  %tmp16 = fmul double 1.0, %tmp17
  %tmp20 = fdiv double 10.0, 20.0
  %tmp15 = fadd double %tmp16, %tmp20
  store double %tmp15, double* %_8ref
  ret i32 1
}


