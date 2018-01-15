@str8 = private unnamed_addr constant [5 x i8] c"aaaa\00"
; puts int ( char* )
declare i32 @$0puts(i8*) #0


; putchar int ( char )
declare i32 @$1putchar(i8) #1


; printf int ( char* )
declare i32 @$2printf(i8*, ...) #2


; main int ( int, char** )
define i32 @$3main(i32 %_0argc, i8** %_1args) #3 {
  %_0i = alloca i32
  store i32 10, i32* %_0i
  %$1_0i = load i32, i32* %_0i
  %_1apaca = alloca i1
  store i1 1, i1* %_1apaca
;While
  br label %Whileaction1
Whileaction1:
  %tmp1 = call i32 @$0puts(i8* getelementptr ([5 x i8], [5 x i8]* @str8, i64 0, i64 0))
  %tmp2 = sub i32 %$1_0i, 3
  store i32 %tmp2, i32* %_0i
  %$2_0i = load i32, i32* %_0i
  br label %Whilecond1
Whilecond1:
  %tmp4 = icmp sgt i32 %$2_0i, 10
  br i1 %tmp4, label %Whileaction1, label %Whilecont1
  br label %Whilecond1
Whilecont1:
  ret i32 13
}


