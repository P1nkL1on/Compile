@str2 = pritvate unnamed_addr constant [13 x i8] c"Hello world!\00"
; puts int ( char* )
declare i32 @puts(i8*) #0


; putchar int ( char )
declare i32 @putchar(i8) #1


; main void ( int, char** )
define void @main(i32 %arg, i8** %atgv) #2 {
  %tmp1 = call i32 @puts(i8* getelementptr ([13 x i8], [13 x i8]* @str2, i64 0, i64 0))
  ret void
}


