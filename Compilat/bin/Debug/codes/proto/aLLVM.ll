@str2 = private unnamed_addr constant [4 x i8] c"lol\00"
; ret int* ( double* )
define i32* @ret(f64* %x) #0 {
  %a = alloca i32
  store i32 10, i32* %a
  ret i32* %a
}


; ret int* ( double*, double, double, string )
define i32* @ret(f64* %xx1, f64 %xx2, f64 %xx3, i8* %S) #1 {
  %a = alloca i32
  store i32 100, i32* %a
  ret i32* %a
}


; main int* ( int*, int** )
define i32* @main(i32* %a, i32** %b) #2 {
  %d = alloca f64
  store f64    ..., f64* %d
  %tmp2 = call i32* @ret(f64* %d, f64    ..., f64 3.00000, i8*    ...)
  %tmp5 = call i32* @ret(f64* %d, f64    ..., f64    ..., i8* getelementptr ([4 x i8], [4 x i8]* @str2, i64 0, i64 0))
  %tmp4 = sub i32*    ..., %tmp5
  %tmp7 = call i32* @ret(f64* %d)
  %tmp3 = add i32* %tmp4, %tmp7
  %tmp1 = add i32* %tmp2, %tmp3
  ret i32* %tmp1
}


