; where int* (  )
define i32* @where() #0 {
  %a = alloca i32
  store i32 10, i32* %a
  ret i32* %a
}


; main int (  )
define i32 @main() #1 {
  %b = alloca i32*
  %tmp1 = call i32* @where()
  store i32* %tmp1, i32** %b
  %c = alloca i32
  %tmp4 = load i32, i32* %b, align 4
  %tmp2 = add i32 %tmp4, 10
  store i32 %tmp2, i32* %c
%tmp5 = call i32* @where()
  %tmp7 = load i32, i32* %tmp5, align 4
%tmp7
%tmp7 = call i32* @where()
  store i32 20, i32* %tmp7, align 4
  ret i32 1
}


