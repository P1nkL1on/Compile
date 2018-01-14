@str9 = private unnamed_addr constant [1 x i8] c"\00"
; main int (  )
define i32 @main() #0 {
  %res = alloca i8*
  store i8* getelementptr ([1 x i8], [1 x i8]* @str9, i64 0, i64 0), i8** %res
  %i = alloca i32
  store i32 0, i32* %i
  %summ = alloca f64
  store f64 0.00000, f64* %summ
;For
  br label %Forcond25
Forcond25:
  %cond25 = icmp slt i32 %i, 10

  br i1 %cond25, label %Foraction25, label %Forcont25
Foraction25:
  %j = alloca i32
  store i32 0, i32* %j
;For
  br label %Forcond22
Forcond22:
  %cond22 = icmp slt i32 %j, %i

  br i1 %cond22, label %Foraction22, label %Forcont22
Foraction22:
  %tmp1 = fmul f64 %summ,    ...
  store f64 %tmp1, f64* %summ
  %tmp3 = fadd f64    ..., %summ
  store f64 %tmp3, f64* %summ
  %d = alloca i32
  store i32 10, i32* %d

  %tmp5 = add i32 1, %j
  store i32 %tmp5, i32* %j
  br label %Forcond22
Forcont22:
  %sto = alloca i32
  store i32 100, i32* %sto
;While
  br label %Whilecond23
Whilecond23:
  %cond23 = icmp sgt i32 %sto, 0

  br i1 %cond23, label %Whileaction23, label %Whilecont23
Whileaction23:
  %tmp7 = sub i32 %sto, 1
  store i32 %tmp7, i32* %sto
  %d = alloca i32
  store i32 20, i32* %d

  br label %Whilecond23
Whilecont23:
  %tmp9 = add i8*    ..., %res
  store i8* %tmp9, i8** %res
  %j = alloca i32
  store i32 0, i32* %j
;For
  br label %Forcond24
Forcond24:
  %cond24 = icmp slt i32 %j, %i

  br i1 %cond24, label %Foraction24, label %Forcont24
Foraction24:
  %tmp11 = fdiv f64 %summ,    ...
  store f64 %tmp11, f64* %summ
  %d = alloca i32
  store i32 30, i32* %d

  %tmp13 = add i32 1, %j
  store i32 %tmp13, i32* %j
  br label %Forcond24
Forcont24:

  %tmp15 = add i32 1, %i
  store i32 %tmp15, i32* %i
  br label %Forcond25
Forcont25:
  ret i32 0
}


