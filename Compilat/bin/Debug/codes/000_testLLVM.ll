; mult int ( int, int* )
define i32 @mult(i32 %_0a, i32* %_1b) #0 {
  %tmp3 = load i32, i32* %_1b, align 4
  %tmp1 = mul i32 %_0a, %tmp3
  ret i32 %tmp1
}


; main int (  )
define i32 @main() #1 {
  ret i32 0
}


