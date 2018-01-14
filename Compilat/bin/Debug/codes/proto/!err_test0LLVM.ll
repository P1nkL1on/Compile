; fac int ( int, int, int )
define i32 @fac(i32 %x, i32 %y, i32 %z) #0 {
  %tmp2 = mul i32 %y, %z
  %tmp1 = add i32 %tmp2, %x
  ret i32 %tmp1
}


; fac double ( int, int, double )
define f64 @fac(i32 %x, i32 %y, f64 %z) #1 {
  %tmp2 = fmul f64    ..., %z
  %tmp1 = fadd f64 %tmp2,    ...
  ret f64 %tmp1
}


; main void (  )
define void @main() #2 {
  %fac1 = alloca f64
  %tmp1 = call f64 @fac(i32 10, i32 10, f64 50.00000)
  store f64 %tmp1, f64* %fac1
  ret void
}


; main int ( string )
define i32 @main(i8* %S) #3 {
  %tmp1 = call i32 @fac(i32 1, i32 2, i32 3)
  %tmp2 = call f64 @fac(i32 1, i32 2, f64 3.00000)
  %DD = alloca f64
  %tmp7 = add i32 3, 7
  %tmp6 = add i32 %tmp7, 6
  %tmp5 = add i32 %tmp6, 5
  %tmp4 = add i32 %tmp5, 2
  %tmp3 = add i32 %tmp4, 1
  %tmp13 = call i32 @fac(i32 1, i32 1, i32 1)
  %tmp14 = call i32 @fac(i32 1, i32 1, i32 2)
  %tmp15 = call f64 @fac(i32 1, i32 2, f64 3.00000)
  %tmp16 = call f64 @fac(i32 %tmp13, i32 %tmp14, f64 %tmp15)
  %tmp17 = call f64 @fac(i32 %tmp3, i32 3, f64 %tmp16)
  store f64 %tmp17, f64* %DD
  ret i32 0
}


