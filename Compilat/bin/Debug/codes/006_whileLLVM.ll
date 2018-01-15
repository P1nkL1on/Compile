@str12 = private unnamed_addr constant [16 x i8] c"hi mark! x %i \0A\00"
; puts int ( char* )
declare i32 @puts(i8*) #0


; putchar int ( char )
declare i32 @putchar(i8) #1


; printf int ( char* )
declare i32 @printf(i8*, ...) #2


; DoSomthing void ( char* )
declare void @DoSomthing(i8*) #3


; DoSomthing void ( double )
declare void @$0DoSomthing(double) #4


; main int ( int, char** )
define i32 @main(i32 %_0argc, i8** %_1args) #5 {
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
  %tmp3 = icmp slt i32 %$2_0i, 100
  %tmp1 = and i1 %tmp2, %tmp3
  br i1 %tmp1, label %Whileaction1, label %Whilecont1
Whileaction1:
  %tmp5 = call i32 (i8*, ...) @printf(i8* getelementptr ([16 x i8], [16 x i8]* @str12, i64 0, i64 0), i32 %$2_0i)
  %tmp6 = sub i32 %$2_0i, 3
  store i32 %tmp6, i32* %_0i
  %$3_0i = load i32, i32* %_0i

  br label %Whilecond1
Whilecont1:
  ret i32 13
}


