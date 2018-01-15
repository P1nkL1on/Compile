; mult int ( int, int* )
define i32 @$0mult(i32 %_0a, i32* %_1b) #0 {
  %tmp3 = load i32*, i32* %_1b, align 4
  %tmp1 = mul i32 %_0a, %tmp3
  ret i32 %tmp1
}


; main int (  )
define i32 @$1main() #1 {
  %_0t = alloca i1
  store i1 1, i1* %_0t
  %tmp1 = trunc i32 100 to i1
  store i1 %tmp1, i1* %_0t
  ret i32 0
}


