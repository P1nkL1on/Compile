; main int (  )
define i32 @main() #0 {
  %var1 = alloca i32
  %va2 = alloca i32
  store i32 2, i32* %va2
  store i32 100, i32* %~var1
  %~var1 = load i32, i32* %var1
  store i32 3, i32* %va2
  %tmp1 = add i32 4, 3
  store i32 %tmp1, i32* %va2
  %tmp4 = mul i32 %~var1, %~var1
  %tmp3 = add i32 %tmp4, %~var1
  store i32 %tmp3, i32* %va2
  %summ = alloca i32
  %tmp7 = add i32 20, 10
  store i32 %tmp7, i32* %summ
  %x = alloca i32
  store i32 1, i32* %x
  %y = alloca i32
  store i32 15, i32* %y
  %xy = alloca i32
  %tmp9 = add i32 %y, %x
  store i32 %tmp9, i32* %xy
  %XY = alloca i32
  %tmp11 = mul i32 %x, %y
  store i32 %tmp11, i32* %XY
  %res = alloca i32
  %tmp13 = sdiv i32 %x, %y
  store i32 %tmp13, i32* %res
  %ref = alloca f64
  %tmp17 = fdiv f64 12,33000000, 33,4000000
  %tmp16 = fmul f64 1.00000, %tmp17
  %tmp20 = fdiv f64 10.00000, 20.00000
  %tmp15 = fadd f64 %tmp16, %tmp20
  store f64 %tmp15, f64* %ref
}


