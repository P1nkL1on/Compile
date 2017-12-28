define i32 @fac(i32 %x, i32 %y, i32 %z){
entry:
  %tmp0 = mul i32 %y, %z
  %tmp1 = add i32 %tmp0, %x
  ret i32 %tmp1
}


define f64 @fac(i32 %x, i32 %y, f64 %z){
entry:
  %tmp2 = fmul f64    ..., %z
  %tmp3 = fadd f64 %tmp2,    ...
  ret f64 %tmp3
}


define void @main(){
entry:
  %tmp5 = call f64 @fac(i32 10, i32 10, f64 50.000)
  %fac1 = %tmp5
  ret void
}


define i32 @main(??? %S){
entry:
  %tmp6 = call i32 @fac(i32 1, i32 2, i32 3)
%tmp6
  %tmp7 = call f64 @fac(i32 1, i32 2, f64 3.0000)
%tmp7
  %tmp7 = add i32 3, 7
  %tmp8 = add i32 %tmp7, 6
  %tmp9 = add i32 %tmp8, 5
  %tmp10 = add i32 %tmp9, 2
  %tmp11 = add i32 %tmp10, 1
  %tmp13 = call i32 @fac(i32 1, i32 1, i32 1)
  %tmp14 = call i32 @fac(i32 1, i32 1, i32 2)
  %tmp15 = call f64 @fac(i32 1, i32 2, f64 3.0000)
  %tmp16 = call f64 @fac(i32 %tmp13, i32 %tmp14, f64 %tmp15)
  %tmp17 = call f64 @fac(i32 %tmp11, i32 3, f64 %tmp16)
  %DD = %tmp17
  ret i32 0
}


