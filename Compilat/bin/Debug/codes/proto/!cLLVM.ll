@str0 = private unnamed_addr constant [5 x i8] c"1  4\00"
@str1 = private unnamed_addr constant [8 x i8] c"su   ck\00"
; main int (  )
define i32 @main() #0 {
  %o = alloca i32
  %a = alloca i32
  %b = alloca i32
  %a1 = alloca i32
  %b2 = alloca i32
  %c = alloca f64***
  %d = alloca f64
  store f64    ..., f64* %d
  %e = alloca f64
  store f64    ..., f64* %e
  %f = alloca f64***
  %g = alloca f64
  %x = alloca i32
  store i32 10, i32* %x
  %y = alloca i32
  store i32 100, i32* %y
  %S1 = alloca i8*
  store i8* getelementptr ([5 x i8], [5 x i8]* @str0, i64 0, i64 0), i8** %S1
  %S2 = alloca i8*
  %tmp1 = add i8* %S1, %S1
  store i8* %tmp1, i8** %S2
  %S3 = alloca i8*
  %tmp4 = add i8* getelementptr ([8 x i8], [8 x i8]* @str1, i64 0, i64 0), %S1
  %tmp3 = add i8* %tmp4, %S2
  store i8* %tmp3, i8** %S3
  %tmp12 = load f64**, f64*** %f, align 4
  %tmp9 = getelementptr f64*, f64** %tmp12, i32 4
  %tmp13 = load f64*, f64** %tmp9, align 4
  %tmp7 = getelementptr f64, f64* %tmp13, i32 1
  store f64    ..., f64* %tmp7, align 8
%c
  ret i32 0
}


