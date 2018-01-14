@str0 = private unnamed_addr constant [1 x i8] c"\00"
; main int (  )
define i32 @main() #0 {
  %_0res = alloca i8*
  store i8* getelementptr ([1 x i8], [1 x i8]* @str0, i64 0, i64 0), i8** %_0res
  %$1_0res = load i8*, i8** %_0res
  %_1i = alloca i32
  store i32 0, i32* %_1i
  %$1_1i = load i32, i32* %_1i
  %_2summ = alloca f64
  store f64 0.00000, f64* %_2summ
  %$1_2summ = load f64, f64* %_2summ
;For
  br label %Forcond4
Forcond4:
  %$2_1i = load i32, i32* %_1i
  %cond4 = icmp slt i32 %$2_1i, 10

  br i1 %cond4, label %Foraction4, label %Forcont4
Foraction4:
  %_3j = alloca i32
  store i32 0, i32* %_3j
  %$1_3j = load i32, i32* %_3j
;For
  br label %Forcond1
Forcond1:
  %$2_3j = load i32, i32* %_3j
  %$3_1i = load i32, i32* %_1i
  %cond1 = icmp slt i32 %$2_3j, %$3_1i

  br i1 %cond1, label %Foraction1, label %Forcont1
Foraction1:
  %tmp1 = fmul f64 %$1_2summ,    ...
  store f64 %tmp1, f64* %_2summ
  %$2_2summ = load f64, f64* %_2summ
  %tmp3 = fadd f64    ..., %$2_2summ
  store f64 %tmp3, f64* %_2summ
  %$3_2summ = load f64, f64* %_2summ
  %_4d = alloca i32
  store i32 10, i32* %_4d

  %tmp5 = add i32 1, %$2_3j
  store i32 %tmp5, i32* %_3j
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
  %cond2 = icmp sgt i32 %$2_5sto, 0

  br i1 %cond2, label %Whileaction2, label %Whilecont2
Whileaction2:
  %tmp7 = sub i32 %$2_5sto, 1
  store i32 %tmp7, i32* %_5sto
  %$3_5sto = load i32, i32* %_5sto
  %_6d = alloca i32
  store i32 20, i32* %_6d

  br label %Whilecond2
Whilecont2:
  %tmp9 = add i8*    ..., %$1_0res
  store i8* %tmp9, i8** %_0res
  %$2_0res = load i8*, i8** %_0res
  %_7j = alloca i32
  store i32 0, i32* %_7j
  %$1_7j = load i32, i32* %_7j
;For
  br label %Forcond3
Forcond3:
  %$2_7j = load i32, i32* %_7j
  %$4_1i = load i32, i32* %_1i
  %cond3 = icmp slt i32 %$2_7j, %$4_1i

  br i1 %cond3, label %Foraction3, label %Forcont3
Foraction3:
  %tmp11 = fdiv f64 %$3_2summ,    ...
  store f64 %tmp11, f64* %_2summ
  %$4_2summ = load f64, f64* %_2summ
  %_8d = alloca i32
  store i32 30, i32* %_8d

  %tmp13 = add i32 1, %$2_7j
  store i32 %tmp13, i32* %_7j
  %$3_7j = load i32, i32* %_7j
  br label %Forcond3
Forcont3:

  %tmp15 = add i32 1, %$4_1i
  store i32 %tmp15, i32* %_1i
  %$5_1i = load i32, i32* %_1i
  br label %Forcond4
Forcont4:
  ret i32 0
}


