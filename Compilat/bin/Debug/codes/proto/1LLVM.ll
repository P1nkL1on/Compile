; func1 int (  )
define i32 @func1() #0 {
  ret i32 0
}


; func2 int (  )
define i32 @func2() #1 {
  %x = alloca f64
  store f64    ..., f64* %x
  ret i32 0
}


