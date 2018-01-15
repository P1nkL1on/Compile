@str0 = private unnamed_addr constant [5 x i8] c"okay\00"
@str1 = private unnamed_addr constant [11 x i8] c"here we go\00"
@str2 = private unnamed_addr constant [1 x i8] c"\00"
; main int (  )
define i32 @main() #0 {
  %_0Xint = alloca i32
  store i32 10, i32* %_0Xint
  %_1Xdouble = alloca double
  store double 15.0, double* %_1Xdouble
  %_2A = alloca i32
  %_3a = alloca i32
  %_4b = alloca i32
  %_5c = alloca i32
  %_6d = alloca i32
  %_7e = alloca i32
  %_8D = alloca double
  store double 22.0, double* %_8D
  %_9F = alloca double
  store double 1.0, double* %_9F
  %_10E = alloca double
  %_11G = alloca double
  %_12H = alloca double
  store double 10.0, double* %_12H
  %_13K = alloca double
  %tmp2 = fadd double 30.2, 20.0
  %tmp1 = fadd double %tmp2, 10.0
  store double %tmp1, double* %_13K
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
  ret i32 0
}


