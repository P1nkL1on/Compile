@str3 = private unnamed_addr constant [9 x i8] c"hi mark!\00"
; puts int ( char* )
declare i32 @puts(i8*) #0


; putchar int ( char )
declare i32 @putchar(i8) #1


; printf int ( char* )
declare i32 @printf(i8*, ...) #2


; main int ( int, char** )
define i32 @main(i32 %argc, i8** %args) #3 {
  %i = alloca i32
  store i32 0, i32* %i
  %$1i = load i32, i32* %i
;For
  br label %Forcond4
Forcond4:
  %$2i = load i32, i32* %i
  %cond4 = icmp slt i32 %$2i, 10

  br i1 %cond4, label %Foraction4, label %Forcont4
Foraction4:
  %tmp1 = call i32 @puts(i8* getelementptr ([9 x i8], [9 x i8]* @str3, i64 0, i64 0))
%tmp1
  %tmp2 = add i32 1, %$2i
  store i32 %tmp2, i32* %i
  %$3i = load i32, i32* %i
  br label %Forcond4
Forcont4:
  ret i32 13
}


