define i32 @ret(f64 %x){
entry:
  %a = i32 10
  ret i32    ...
}


define i32 @ret(f64 %xx1, f64 %xx2, f64 %xx3, ??? %S){
entry:
  %a = i32 100
  ret i32    ...
}


define i32 @main(i32 %a, i32 %b){
entry:
  %d =    ...
  %tmp1 = call i32 @ret(f64    ..., f64    ..., f64 3.0000, ???    ...)
  %tmp2 = call i32 @ret(f64    ..., f64    ..., f64    ..., ??? lol)
  %tmp2 = sub i32    ..., %tmp2
  %tmp4 = call i32 @ret(f64    ...)
  %tmp4 = add i32 %tmp2, %tmp4
  %tmp5 = add i32 %tmp1, %tmp4
  ret i32 %tmp5
}


