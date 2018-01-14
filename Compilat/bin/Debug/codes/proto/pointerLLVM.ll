; D int ( int*, int* )
define i32 @D(i32* %a, i32* %b) #0 {
  store i32 10, i32* %a, align 4
  store i32 5, i32* %b, align 4
  %tmp5 = load i32, i32* %b, align 4
  %tmp6 = load i32, i32* %a, align 4
  %tmp3 = add i32 %tmp5, %tmp6
  ret i32 %tmp3
}


; F int ( int )
define i32 @F(i32 %a) #1 {
  %tmp1 = mul i32 %a, %a
  ret i32 %tmp1
}


; main int (  )
define i32 @main() #2 {
  store i32 5, i32* %a, align 4
  %tmp2 = getelementptr i32, i32* %a, i32 1
  store i32 6, i32* %tmp2, align 4
  %tmp5 = getelementptr i32, i32* %a, i32 2
  store i32 7, i32* %tmp5, align 4

  %originalA = alloca i32
  store i32 100, i32* %originalA
  %originalB = alloca i32
  store i32 120, i32* %originalB
  %Ap = alloca i32*
  store i32* %originalA, i32** %Ap
  %Bp = alloca i32*
  store i32* %originalB, i32** %Bp
  %reS = alloca i32
  %tmp8 = call i32 @D(i32* %Ap, i32* %Bp)
  store i32 %tmp8, i32* %reS
  %res = alloca i32
  %tmp9 = call i32 @F(i32 %originalA)
  store i32 %tmp9, i32* %res
  ret i32 0
}


