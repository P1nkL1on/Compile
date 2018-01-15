; func1 int (  )
define i32 @$0func1() #0 {
  ret i32 0
}


; func2 int (  )
define i32 @$1func2() #1 {
  %_0x = alloca double
  %tmp3 = call i32 @$0func1()
  %tmp4 = call i32 @$0func1()
  %tmp2 = mul i32 %tmp3, %tmp4
  %tmp1 = sitofp i32 %tmp2 to double
  store double %tmp1, double* %_0x
  ret i32 0
}


