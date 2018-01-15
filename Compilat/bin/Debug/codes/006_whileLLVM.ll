@str4 = private unnamed_addr constant [16 x i8] c"hi mark! x %i \0A\00"
; puts int ( char* )
declare i32 @puts(i8*) #0


; putchar int ( char )
declare i32 @putchar(i8) #1


; printf int ( char* )
declare i32 @printf(i8*, ...) #2


; main int ( int, char** )
define i32 @main(i32 %_0argc, i8** %_1args) #3 {
  %_0i = alloca i32
  store i32 10, i32* %_0i
  %$1_0i = load i32, i32* %_0i
  %_1apaca = alloca i1
  store i1 1, i1* %_1apaca
;While
  br label %Whilecond1
Whilecond1:
  %$2_0i = load i32, i32* %_0i
  %tmp2 = icmp sgt i32 %$2_0i, 0
  %tmp1 = icmp eq i1 %tmp2, 1
  %cond1 = icmp %tmp1
  br i1 %cond1, label %Whileaction1, label %Whilecont1
Whileaction1:
  %tmp3 = call i32 (i8*, ...) @printf(i8* getelementptr ([16 x i8], [16 x i8]* @str4, i64 0, i64 0), i32 %$2_0i)
  %tmp4 = sub i32 %$2_0i, 3
  store i32 %tmp4, i32* %_0i
  %$3_0i = load i32, i32* %_0i

  br label %Whilecond1
Whilecont1:
  ret i32 13
}


