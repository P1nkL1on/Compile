; Declared int ( int, int, double )
define i32 @Declared(i32 %a1, i32 %a2, f64 %d1) #0 {
  %tmp2 = mul i32 %a2, %a1
  %tmp1 = add i32 %tmp2, %a1
  ret i32 %tmp1
}


; DeclaredOnly void ( double* )
declare void @DeclaredOnly(f64*) #1


; foo int ( int, char )
define i32 @foo(i32 %A, i8 %C) #2 {
  %tmp1 = add i32 %A, %A
  ret i32 %tmp1
}


; Mul int ( int, int )
define i32 @Mul(i32 %X, i32 %Y) #3 {
  %res = alloca i32
  %tmp1 = mul i32 %X, %Y
  store i32 %tmp1, i32* %res
  %$1res = load i32, i32* %res
  ret i32 %$1res
}


; GiveMeTen double (  )
define f64 @GiveMeTen() #4 {
  ret f64 10.00000
}


; DoNothing void (  )
define void @DoNothing() #5 {
  ret void
}


; OnlyArgs void ( char, int, double, double )
define void @OnlyArgs(i8 %c, i32 %b, f64 %D, f64 %E) #6 {
  ret void
}


; OnlyArgs void ( char, int, int, int )
define void @OnlyArgs(i8 %c, i32 %b, i32 %D, i32 %E) #7 {
  ret void
}


; main int ( int, char** )
define i32 @main(i32 %argc, i8** %args) #8 {
  %tmp1 = call i32 @foo(i32 1, i8 50)
  %foores = alloca i32
  %tmp2 = call i32 @foo(i32 3, i8 52)
  store i32 %tmp2, i32* %foores
  %tmp3 = call i32 @Mul(i32 3, i32 6)
  store i32 %tmp3, i32* %foores
  %ten = alloca f64
  %tmp4 = call f64 @GiveMeTen()
  store f64 %tmp4, f64* %ten
  tail call void @DoNothing()
  %tmp5 = add i32 2, 1
  %tmp7 = fsub f64 4,6000000, 233,333000000
  %tmp9 = fmul f64 1,2000000, 20.00000
  tail call void @OnlyArgs(i8 102, i32 %tmp5, f64 %tmp7, f64 %tmp9)
  tail call void @OnlyArgs(i8 99, i32 10, f64 15.00000, f64 23,3000000)
  ret i32 0
}


