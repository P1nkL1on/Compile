; foo double* ( string, int )
define f64* @foo(i8* %S, i32 %x) #0 {
  %res = alloca f64
  store f64    ..., f64* %res
  %pres = alloca f64*
  store f64* %res, f64** %pres
  ret f64* %pres
}


; main int ( int, int )
define i32 @main(i32 %a1, i32 %a2) #1 {
  %s = alloca i8*
  %s1 = alloca i8*
  %s2 = alloca i8*
  ret i32 0
}


