; Declared int ( int, int, double )
define i32 @$9Declared(i32 %_0a1, i32 %_1a2, double %_2d1) #9 {
  %tmp2 = mul i32 %_1a2, %_0a1
  %tmp1 = add i32 %tmp2, %_0a1
  ret i32 %tmp1
}


; DeclaredOnly void ( double* )
declare void @$1DeclaredOnly(double*) #1


; foo int ( int, char )
define i32 @$2foo(i32 %_0A, i8 %_1C) #2 {
  %tmp1 = add i32 %_0A, %_0A
  ret i32 %tmp1
}


; Mul int ( int, int )
define i32 @$3Mul(i32 %_0X, i32 %_1Y) #3 {
  %_0res = alloca i32
  %tmp1 = mul i32 %_0X, %_1Y
  store i32 %tmp1, i32* %_0res
  %$1_0res = load i32, i32* %_0res
  ret i32 %$1_0res
}


; GiveMeTen double (  )
define double @$4GiveMeTen() #4 {
  ret double 10.0
}


; DoNothing void (  )
define void @$5DoNothing() #5 {
  ret void
}


; OnlyArgs void ( char, int, char, double )
define void @$6OnlyArgs(i8 %_0c, i32 %_1b, i8 %_2D, double %_3E) #6 {
  ret void
}


; OnlyArgs void ( char, int, int, int )
define void @$7OnlyArgs(i8 %_0c, i32 %_1b, i32 %_2D, i32 %_3E) #7 {
  ret void
}


; main int ( int, char** )
define i32 @$8main(i32 %_0argc, i8** %_1args) #8 {
  %tmp1 = call i32 @$2foo(i32 1, i8 50)
  %_0foores = alloca i32
  %tmp2 = call i32 @$2foo(i32 3, i8 52)
  store i32 %tmp2, i32* %_0foores
  %tmp3 = call i32 @$3Mul(i32 3, i32 6)
  store i32 %tmp3, i32* %_0foores
  %_1ten = alloca double
  %tmp4 = call double @$4GiveMeTen()
  store double %tmp4, double* %_1ten
  tail call void @$5DoNothing()
  tail call void @$7OnlyArgs(i8 99, i32 10, i32 1, i32 2)
  %tmp5 = add i32 2, 1
  %tmp7 = fmul double 1.2, 20.0
  tail call void @$6OnlyArgs(i8 102, i32 %tmp5, i8 102, double %tmp7)
  tail call void @$6OnlyArgs(i8 99, i32 10, i8 107, double 23.3)
  ret i32 0
}


