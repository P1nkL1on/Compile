@str3 = private unnamed_addr constant [9 x i8] c"hi mark!\00"
; puts int ( char* )
declare i32 @puts(i8*) #0


; putchar int ( char )
declare i32 @putchar(i8) #1


; printf int ( char* )
declare i32 @printf(i8*, ...) #2


; main int ( int, char** )
define i32 @main(i32 %_0argc, i8** %_1args) #3 {
  %_0i = alloca i32
  store i32 0, i32* %_0i
  %$1_0i = load i32, i32* %_0i
;For
  br label %Forcond1
Forcond1:
  %$2_0i = load i32, i32* %_0i
  %tmp2 = icmp slt i32 %$2_0i, 10
  %tmp1 = icmp eq i1 %tmp2, 1
  %cond1 = icmp %tmp1
  br i1 %cond1, label %Foraction1, label %Forcont1
Foraction1:
  %tmp3 = call i32 @puts(i8* getelementptr ([9 x i8], [9 x i8]* @str3, i64 0, i64 0))
  %tmp4 = add i32 1, %$2_0i
  store i32 %tmp4, i32* %_0i
  %$3_0i = load i32, i32* %_0i
  br label %Forcond1
Forcont1:
  ret i32 13
}


