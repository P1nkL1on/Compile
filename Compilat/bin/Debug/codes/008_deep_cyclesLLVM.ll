@str6 = private unnamed_addr constant [14 x i8] c" %i x %i   %i\00"
@str7 = private unnamed_addr constant [4 x i8] c"  \0A\00"
; puts int ( char* )
declare i32 @puts(i8*) #0


; putchar int ( char )
declare i32 @putchar(i8) #1


; printf int ( char* )
declare i32 @printf(i8*, ...) #2


; main int ( int, char** )
define i32 @main(i32 %_0argc, i8** %_1args) #3 {
  %_0LENG = alloca i32
  store i32 10, i32* %_0LENG
  %$1_0LENG = load i32, i32* %_0LENG
  %_1i = alloca i32
  store i32 0, i32* %_1i
  %$1_1i = load i32, i32* %_1i
;For
  br label %Forcond2
Forcond2:
  %$2_1i = load i32, i32* %_1i
  %$2_0LENG = load i32, i32* %_0LENG
  %tmp2 = icmp slt i32 %$2_1i, %$2_0LENG
  %tmp1 = icmp eq i1 %tmp2, 1
  %cond2 = icmp %tmp1
  br i1 %cond2, label %Foraction2, label %Forcont2
Foraction2:
  %_2j = alloca i32
  store i32 0, i32* %_2j
  %$1_2j = load i32, i32* %_2j
;For
  br label %Forcond1
Forcond1:
  %$2_2j = load i32, i32* %_2j
  %$3_0LENG = load i32, i32* %_0LENG
  %tmp4 = icmp slt i32 %$2_2j, %$3_0LENG
  %tmp3 = icmp eq i1 %tmp4, 1
  %cond1 = icmp %tmp3
  br i1 %cond1, label %Foraction1, label %Forcont1
Foraction1:
  %tmp5 = mul i32 %$2_1i, %$2_2j
  %tmp7 = call i32 (i8*, ...) @printf(i8* getelementptr ([14 x i8], [14 x i8]* @str6, i64 0, i64 0), i32 %$2_1i, i32 %$2_2j, i32 %tmp5)
  %tmp8 = add i32 1, %$2_2j
  store i32 %tmp8, i32* %_2j
  %$3_2j = load i32, i32* %_2j
  %tmp10 = call i32 @puts(i8* getelementptr ([4 x i8], [4 x i8]* @str7, i64 0, i64 0))
  br label %Forcond1
Forcont1:
  %tmp11 = add i32 1, %$2_1i
  store i32 %tmp11, i32* %_1i
  %$3_1i = load i32, i32* %_1i
  br label %Forcond2
Forcont2:
  ret i32 1
}


