@str0 = private unnamed_addr constant [5 x i8] c"aaaa\00"
; puts int ( char* )
declare i32 @puts(i8*) #0


; putchar int ( char )
declare i32 @putchar(i8) #1


; printf int ( char* )
declare i32 @printf(i8*, ...) #2


; main int ( int, char** )
define i32 @main(i32 %argc, i8** %args) #3 {
  %i = alloca i32
  store i32 10, i32* %i
  %$1i = load i32, i32* %i
  %apaca = alloca i1
  store i1 1, i1* %apaca
;While
  br label %Whileaction1
Whileaction1:
  %tmp1 = call i32 @puts(i8* getelementptr ([5 x i8], [5 x i8]* @str0, i64 0, i64 0))
  %tmp2 = sub i32 %$1i, 3
  store i32 %tmp2, i32* %i
  %$2i = load i32, i32* %i
  br label %Whilecond1
Whilecond1:
  %cond1 = icmp sgt i32 %$2i, 10

  br i1 %cond1, label %Whileaction1, label %Whilecont1
  br label %Whilecond1
Whilecont1:
  ret i32 13
}


