@_0a = global i8 97, align 1
@_1CN = global i32 122, align 4
@_2LIFE = global i32 42, align 4
; main int (  )
define i32 @main() #0 {
  %$1_1CN = load i32, i32* @_1CN
  %_0x = alloca i32
  store i32 10, i32* %_0x
  %$1_0x = load i32, i32* %_0x
  %_1y = alloca i32
  store i32 20, i32* %_1y
  %$1_1y = load i32, i32* %_1y
  store i32 %$1_1y, i32* %_0x
  %$2_0x = load i32, i32* %_0x
  %tmp1 = add i32 %$1_1CN, %$2_0x
  store i32 %tmp1, i32* %_0x
  %$3_0x = load i32, i32* %_0x
  %tmp3 = add i32 1, %$3_0x
  store i32 %tmp3, i32* %_0x
  %$4_0x = load i32, i32* %_0x
  ret i32 0
}


