; main char ( int, char** )
define i8 @main(i32 %_0argc, i8** %_1args) #0 {
  %_0X = alloca i32
  store i32 10, i32* %_0X
  %$1_0X = load i32, i32* %_0X
  %_1res = alloca i8
  store i8 117, i8* %_1res
  %$1_1res = load i8, i8* %_1res
  %_2res_value = alloca i32
  store i32 %$1_0X, i32* %_2res_value
  %$1_2res_value = load i32, i32* %_2res_value
;If
  %tmp1 = icmp sgt i32 %$1_0X, 5
  br i1 %tmp1, label %Ifthen1, label %Ifelse1
Ifthen1:
  store i8 121, i8* %_1res
  %$2_1res = load i8, i8* %_1res
  %tmp2 = mul i32 %$1_2res_value, %$1_0X
  store i32 %tmp2, i32* %_2res_value
  %$2_2res_value = load i32, i32* %_2res_value

  br label %Ifcont1
Ifelse1:
  store i8 110, i8* %_1res
  %$3_1res = load i8, i8* %_1res
  %tmp4 = sdiv i32 %$2_2res_value, %$1_0X
  store i32 %tmp4, i32* %_2res_value
  %$3_2res_value = load i32, i32* %_2res_value

  br label %Ifcont1
Ifcont1:
;If
  %tmp6 = icmp eq i8 %$3_1res, 110
  br i1 %tmp6, label %Ifthen2, label %Ifelse2
Ifthen2:
  store i32 0, i32* %_2res_value
  %$4_2res_value = load i32, i32* %_2res_value
  br label %Ifcont2
Ifelse2:
  %tmp7 = add i32 1, %$4_2res_value
  store i32 %tmp7, i32* %_2res_value
  %$5_2res_value = load i32, i32* %_2res_value
  br label %Ifcont2
Ifcont2:
  %tmp9 = sub i32 %$5_2res_value, 1
  store i32 %tmp9, i32* %_2res_value
  %$6_2res_value = load i32, i32* %_2res_value
;If
  %tmp11 = icmp eq i8 %$3_1res, 121
  br i1 %tmp11, label %Ifthen3, label %Ifcont3
Ifthen3:
  store i8 112, i8* %_1res
  %$4_1res = load i8, i8* %_1res
  br label %Ifcont3
Ifcont3:
  ret i8 %$4_1res
}


