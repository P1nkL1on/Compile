@str15 = private unnamed_addr constant [1 x i8] c"\00"
; main int (  )
define i32 @$0main() #0 {
  %_0res = alloca i8*
  store i8* getelementptr ([1 x i8], [1 x i8]* @str15, i64 0, i64 0), i8** %_0res
  %$1_0res = load i8*, i8** %_0res
  %_1i = alloca i32
  store i32 0, i32* %_1i
  %$1_1i = load i32, i32* %_1i
  %_2summ = alloca double
  store double 0.0, double* %_2summ
  %$1_2summ = load double, double* %_2summ
;For
  br label %Forcond4
Forcond4:
  %$2_1i = load i32, i32* %_1i
  %tmp1 = icmp slt i32 %$2_1i, 10
  br i1 %tmp1, label %Foraction4, label %Forcont4
Foraction4:
  %_3j = alloca i32
  store i32 0, i32* %_3j
  %$1_3j = load i32, i32* %_3j
;For
  br label %Forcond1
Forcond1:
  %$2_3j = load i32, i32* %_3j
  %$3_1i = load i32, i32* %_1i
  %tmp2 = icmp slt i32 %$2_3j, %$3_1i
  br i1 %tmp2, label %Foraction1, label %Forcont1
Foraction1:
  %tmp4 = sitofp i32 %$2_3j to double
  %tmp3 = fmul double %$1_2summ, %tmp4
  store double %tmp3, double* %_2summ
  %$2_2summ = load double, double* %_2summ
  %tmp7 = sitofp i32 %$3_1i to double
  %tmp6 = fadd double %tmp7, %$2_2summ
  store double %tmp6, double* %_2summ
  %$3_2summ = load double, double* %_2summ
  %_4d = alloca i32
  store i32 10, i32* %_4d

  %tmp9 = add i32 1, %$2_3j
  store i32 %tmp9, i32* %_3j
  %$3_3j = load i32, i32* %_3j
  br label %Forcond1
Forcont1:
  %_5sto = alloca i32
  store i32 100, i32* %_5sto
  %$1_5sto = load i32, i32* %_5sto
;While
  br label %Whilecond2
Whilecond2:
  %$2_5sto = load i32, i32* %_5sto
  %tmp11 = icmp sgt i32 %$2_5sto, 0
  br i1 %tmp11, label %Whileaction2, label %Whilecont2
Whileaction2:
  %tmp12 = sub i32 %$2_5sto, 1
  store i32 %tmp12, i32* %_5sto
  %$3_5sto = load i32, i32* %_5sto
  %_6d = alloca i32
  store i32 20, i32* %_6d

  br label %Whilecond2
Whilecont2:
  %tmp15 = trunc double %$3_2summ to i8*
  %tmp14 = add i8* %tmp15, %$1_0res
  store i8* %tmp14, i8** %_0res
  %$2_0res = load i8*, i8** %_0res
  %_7j = alloca i32
  store i32 0, i32* %_7j
  %$1_7j = load i32, i32* %_7j
;For
  br label %Forcond3
Forcond3:
  %$2_7j = load i32, i32* %_7j
  %$4_1i = load i32, i32* %_1i
  %tmp17 = icmp slt i32 %$2_7j, %$4_1i
  br i1 %tmp17, label %Foraction3, label %Forcont3
Foraction3:
  %tmp19 = sitofp i32 %$4_1i to double
  %tmp18 = fdiv double %$3_2summ, %tmp19
  store double %tmp18, double* %_2summ
  %$4_2summ = load double, double* %_2summ
  %_8d = alloca i32
  store i32 30, i32* %_8d

  %tmp21 = add i32 1, %$2_7j
  store i32 %tmp21, i32* %_7j
  %$3_7j = load i32, i32* %_7j
  br label %Forcond3
Forcont3:

  %tmp23 = add i32 1, %$4_1i
  store i32 %tmp23, i32* %_1i
  %$5_1i = load i32, i32* %_1i
  br label %Forcond4
Forcont4:
  ret i32 0
}


