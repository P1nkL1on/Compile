@str9 = private unnamed_addr constant [14 x i8] c" %i x %i   %i\00"
@str10 = private unnamed_addr constant [4 x i8] c"  \0A\00"
; puts int ( char* )
declare i32 @$0puts(i8*) #0


; putchar int ( char )
declare i32 @$1putchar(i8) #1


; printf int ( char* )
declare i32 @$2printf(i8*, ...) #2


; main int ( int, char** )
define i32 @$3main(i32 %_0argc, i8** %_1args) #3 {
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
  %tmp1 = icmp slt i32 %$2_1i, %$2_0LENG
  br i1 %tmp1, label %Foraction2, label %Forcont2
Foraction2:
  %_2j = alloca i32
  store i32 0, i32* %_2j
  %$1_2j = load i32, i32* %_2j
;For
  br label %Forcond1
Forcond1:
  %$2_2j = load i32, i32* %_2j
  %$3_0LENG = load i32, i32* %_0LENG
  %tmp2 = icmp slt i32 %$2_2j, %$3_0LENG
  br i1 %tmp2, label %Foraction1, label %Forcont1
Foraction1:
  %tmp3 = mul i32 %$2_1i, %$2_2j
  %tmp5 = call i32 (i8*, ...) @$2printf(i8* getelementptr ([14 x i8], [14 x i8]* @str9, i64 0, i64 0), i32 %$2_1i, i32 %$2_2j, i32 %tmp3)
  %tmp6 = add i32 1, %$2_2j
  store i32 %tmp6, i32* %_2j
  %$3_2j = load i32, i32* %_2j
  %tmp8 = call i32 @$0puts(i8* getelementptr ([4 x i8], [4 x i8]* @str10, i64 0, i64 0))
  br label %Forcond1
Forcont1:
  %tmp9 = add i32 1, %$2_1i
  store i32 %tmp9, i32* %_1i
  %$3_1i = load i32, i32* %_1i
  br label %Forcond2
Forcont2:
  ret i32 1
}


