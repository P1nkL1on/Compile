@str0 = private unnamed_addr constant [5 x i8] c"okay\00"
@str1 = private unnamed_addr constant [11 x i8] c"here we go\00"
@str2 = private unnamed_addr constant [1 x i8] c"\00"
; main int (  )
define i32 @main() #0 {
  %Xint = alloca i32
  store i32 10, i32* %Xint
  %Xdouble = alloca f64
  store f64 15.00000, f64* %Xdouble
  %A = alloca i32
  %a = alloca i32
  %b = alloca i32
  %c = alloca i32
  %d = alloca i32
  %e = alloca i32
  %D = alloca f64
  store f64 22.00000, f64* %D
  %F = alloca f64
  store f64 1.00000, f64* %F
  %E = alloca f64
  %G = alloca f64
  %H = alloca f64
  store f64    ..., f64* %H
  %K = alloca f64
  %tmp2 = fadd f64 30,2000000, 20.00000
  %tmp1 = fadd f64 %tmp2, 10.00000
  store f64 %tmp1, f64* %K
  %_c = alloca i8
  store i8 97, i8* %_c
  %_c1 = alloca i8
  store i8 98, i8* %_c1
  %_c2 = alloca i8
  store i8 99, i8* %_c2
  %S = alloca i8*
  store i8* getelementptr ([5 x i8], [5 x i8]* @str0, i64 0, i64 0), i8** %S
  %S2 = alloca i8*
  store i8* getelementptr ([11 x i8], [11 x i8]* @str1, i64 0, i64 0), i8** %S2
  %S3 = alloca i8*
  store i8* getelementptr ([1 x i8], [1 x i8]* @str2, i64 0, i64 0), i8** %S3
  %TR = alloca i1
  store i1 1, i1* %TR
  %FL = alloca i1
  store i1 0, i1* %FL
}


