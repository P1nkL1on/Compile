@str0 = private unnamed_addr constant [5 x i8] c"okay\00"
@str1 = private unnamed_addr constant [11 x i8] c"here we go\00"
@str2 = private unnamed_addr constant [1 x i8] c"\00"
; main int (  )
define i32 @main() #0 {
  %_0Xint = alloca i32
  store i32 10, i32* %_0Xint
  %_1Xdouble = alloca f64
  store f64 15.00000, f64* %_1Xdouble
  %_2A = alloca i32
  %_3a = alloca i32
  %_4b = alloca i32
  %_5c = alloca i32
  %_6d = alloca i32
  %_7e = alloca i32
  %_8D = alloca f64
  store f64 22.00000, f64* %_8D
  %_9F = alloca f64
  store f64 1.00000, f64* %_9F
  %_10E = alloca f64
  %_11G = alloca f64
  %_12H = alloca f64
  store f64    ..., f64* %_12H
  %_13K = alloca f64
  %tmp2 = fadd f64 30,2000000, 20.00000
  %tmp1 = fadd f64 %tmp2, 10.00000
  store f64 %tmp1, f64* %_13K
  %_14_c = alloca i8
  store i8 97, i8* %_14_c
  %_15_c1 = alloca i8
  store i8 98, i8* %_15_c1
  %_16_c2 = alloca i8
  store i8 99, i8* %_16_c2
  %_17S = alloca i8*
  store i8* getelementptr ([5 x i8], [5 x i8]* @str0, i64 0, i64 0), i8** %_17S
  %_18S2 = alloca i8*
  store i8* getelementptr ([11 x i8], [11 x i8]* @str1, i64 0, i64 0), i8** %_18S2
  %_19S3 = alloca i8*
  store i8* getelementptr ([1 x i8], [1 x i8]* @str2, i64 0, i64 0), i8** %_19S3
  %_20TR = alloca i1
  store i1 1, i1* %_20TR
  %_21FL = alloca i1
  store i1 0, i1* %_21FL
}


