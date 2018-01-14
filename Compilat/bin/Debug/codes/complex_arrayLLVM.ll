@str0 = private unnamed_addr constant [5 x i8] c"mike\00"
@str1 = private unnamed_addr constant [5 x i8] c"john\00"
@str2 = private unnamed_addr constant [5 x i8] c"mary\00"
@str3 = private unnamed_addr constant [7 x i8] c"ashley\00"
; main int (  )
define i32 @main() #0 {
  store i8* getelementptr ([5 x i8], [5 x i8]* @str0, i64 0, i64 0), i8** %_0a, align 0
  %tmp2 = getelementptr i8*, i8** %_0a, i32 1
  store i8* getelementptr ([5 x i8], [5 x i8]* @str1, i64 0, i64 0), i8** %tmp2, align 0
  %tmp5 = getelementptr i8*, i8** %_0a, i32 2
  store i8* getelementptr ([5 x i8], [5 x i8]* @str2, i64 0, i64 0), i8** %tmp5, align 0
  %tmp8 = getelementptr i8*, i8** %_0a, i32 3
  store i8* getelementptr ([7 x i8], [7 x i8]* @str3, i64 0, i64 0), i8** %tmp8, align 0

  ret i32 0
}


