@str4 = private unnamed_addr constant [14 x i8] c" %i x %i   %i\00"
@str5 = private unnamed_addr constant [4 x i8] c"  \0A\00"
; puts int ( char* )
declare i32 @puts(i8*) #0


; putchar int ( char )
declare i32 @putchar(i8) #1


; printf int ( char* )
declare i32 @printf(i8*, ...) #2


; main int ( int, char** )
define i32 @main(i32 %argc, i8** %args) #3 {
  %LENG = alloca i32
  store i32 10, i32* %LENG
  %$1LENG = load i32, i32* %LENG
  %i = alloca i32
  store i32 0, i32* %i
  %$1i = load i32, i32* %i
;For
  br label %Forcond6
Forcond6:
  %$2i = load i32, i32* %i
  %$2LENG = load i32, i32* %LENG
  %cond6 = icmp slt i32 %$2i, %$2LENG

  br i1 %cond6, label %Foraction6, label %Forcont6
Foraction6:
  %j = alloca i32
  store i32 0, i32* %j
  %$1j = load i32, i32* %j
;For
  br label %Forcond5
Forcond5:
  %$2j = load i32, i32* %j
  %$3LENG = load i32, i32* %LENG
  %cond5 = icmp slt i32 %$2j, %$3LENG

  br i1 %cond5, label %Foraction5, label %Forcont5
Foraction5:
  %tmp1 = mul i32 %$2i, %$2j
  %tmp3 = call i32 (i8*, ...) @printf(i8* getelementptr ([14 x i8], [14 x i8]* @str4, i64 0, i64 0), i32 %$2i, i32 %$2j, i32 %tmp1)
  %tmp4 = add i32 1, %$2j
  store i32 %tmp4, i32* %j
  %$3j = load i32, i32* %j
  %tmp6 = call i32 @puts(i8* getelementptr ([4 x i8], [4 x i8]* @str5, i64 0, i64 0))
  br label %Forcond5
Forcont5:
  %tmp7 = add i32 1, %$2i
  store i32 %tmp7, i32* %i
  %$3i = load i32, i32* %i
  br label %Forcond6
Forcont6:
  ret i32 1
}


