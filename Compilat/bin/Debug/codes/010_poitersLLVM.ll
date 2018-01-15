; main int (  )
define i32 @main() #0 {
  %_0x = alloca i32
  store i32 10, i32* %_0x
  %_1px = alloca i32*
  store i32* %_0x, i32** %_1px
  %_2ppx = alloca i32**
  store i32** %_1px, i32*** %_2ppx
  store i32 1, i32* %_0x
  %tmp1 = getelementptr i32*, i32** %_1px, i32 14
  %tmp3 = load i32*, i32** %tmp1
  store i32 2, i32* %tmp3, align 4
  %tmp4 = getelementptr i32*, i32** %_1px, i32 1
  %tmp6 = load i32, i32* %tmp4, align 4
  ret i32 %tmp6
}


