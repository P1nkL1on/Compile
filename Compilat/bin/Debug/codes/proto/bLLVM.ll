; main int ( int*, int** )
define i32 @main(i32* %arg, i32** %argv) #0 {
  %a = alloca i32
  store i32 10, i32* %a
  %pa = alloca i32*
  store i32* %a, i32** %pa
  %ppa = alloca i32**
  store i32** %pa, i32*** %ppa
  ret i32 0
}


; d1 int ( string*, string** )
define i32 @d1(i8** %s, i8*** %ss) #1 {
  %tmp1 = add i32 0, 0
  ret i32 %tmp1
}


; d2 double ( double )
define f64 @d2(f64 %sss) #2 {
  ret f64 10.00000
}


; d3 char ( char*****, char )
define i8 @d3(i8***** %s, i8 %ss) #3 {
  ret i8 97
}


