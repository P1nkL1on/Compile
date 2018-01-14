; main char ( int, char** )
define i8 @main(i32 %argc, i8** %args) #0 {
  %X = alloca i32
  store i32 10, i32* %X
  %1X = load i32, i32* %X
  %res = alloca i8
  store i8 117, i8* %res
  %1res = load i8, i8* %res
  %res_value = alloca i32
  store i32 %1X, i32* %res_value
  %1res_value = load i32, i32* %res_value
;If
  %cond1 = icmp sgt i32 %1X, 5

  br i1 %cond1, label %Ifthen1, label %Ifelse1
Ifthen1:
  store i8 121, i8* %res
  %2res = load i8, i8* %res
  %tmp1 = mul i32 %1res_value, %1X
  store i32 %tmp1, i32* %res_value
  %2res_value = load i32, i32* %res_value

  br label %Ifcont1
Ifelse1:
  store i8 110, i8* %res
  %3res = load i8, i8* %res
  %tmp3 = sdiv i32 %2res_value, %1X
  store i32 %tmp3, i32* %res_value
  %3res_value = load i32, i32* %res_value

  br label %Ifcont1
Ifcont1:
;If
  %cond2 = icmp eq i8 %3res, 110

  br i1 %cond2, label %Ifthen2, label %Ifelse2
Ifthen2:
  store i32 0, i32* %res_value
  %4res_value = load i32, i32* %res_value
  br label %Ifcont2
Ifelse2:
  %tmp5 = add i32 1, %4res_value
  store i32 %tmp5, i32* %res_value
  %5res_value = load i32, i32* %res_value
  br label %Ifcont2
Ifcont2:
  %tmp7 = sub i32 %5res_value, 1
  store i32 %tmp7, i32* %res_value
  %6res_value = load i32, i32* %res_value
;If
  %cond3 = icmp eq i8 %3res, 121

  br i1 %cond3, label %Ifthen3, label %Ifcont3
Ifthen3:
  store i8 112, i8* %res
  %4res = load i8, i8* %res
  br label %Ifcont3
Ifcont3:
  ret i8 %4res
}


