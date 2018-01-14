; main int (  )
define i32 @main() #0 {
  %_0summ = alloca i32
  store i32 0, i32* %_0summ
  %$1_0summ = load i32, i32* %_0summ
  %_1a = alloca i32
  store i32 10, i32* %_1a
  %$1_1a = load i32, i32* %_1a
  %_2b = alloca i32
  store i32 10, i32* %_2b
  %$1_2b = load i32, i32* %_2b
  %_3c = alloca i32
  store i32 10, i32* %_3c
  %$1_3c = load i32, i32* %_3c
  %_4DDDDD = alloca i32
  store i32 1000, i32* %_4DDDDD
  %tmp2 = add i32 %$1_3c, %$1_2b
  %tmp1 = add i32 %tmp2, %$1_1a
  store i32 %tmp1, i32* %_0summ
  %$2_0summ = load i32, i32* %_0summ

  %_5summ2 = alloca i32
  store i32 0, i32* %_5summ2
  %$1_5summ2 = load i32, i32* %_5summ2
  %_6a = alloca i32
  store i32 20, i32* %_6a
  %$1_6a = load i32, i32* %_6a
  %_7b = alloca i32
  store i32 20, i32* %_7b
  %$1_7b = load i32, i32* %_7b
  %_8c = alloca i32
  store i32 30, i32* %_8c
  %$1_8c = load i32, i32* %_8c
  %tmp6 = add i32 %$1_8c, %$1_7b
  %tmp5 = add i32 %tmp6, %$1_6a
  store i32 %tmp5, i32* %_5summ2
  %$2_5summ2 = load i32, i32* %_5summ2

  %_9diff = alloca i32
  %tmp9 = sub i32 %$2_0summ, %$2_5summ2
  store i32 %tmp9, i32* %_9diff
  ret i32 0
}


