@str4 = private unnamed_addr constant [5 x i8] c"mike\00"
@str5 = private unnamed_addr constant [5 x i8] c"john\00"
@str6 = private unnamed_addr constant [5 x i8] c"mary\00"
@str7 = private unnamed_addr constant [7 x i8] c"ashley\00"
; main int (  )
define i32 @main() #0 {
  store i8* getelementptr ([5 x i8], [5 x i8]* @str4, i64 0, i64 0), i8** %a, align 0
  %tmp2 = getelementptr i8*, i8** %a, i32 1
  store i8* getelementptr ([5 x i8], [5 x i8]* @str5, i64 0, i64 0), i8** %tmp2, align 0
  %tmp5 = getelementptr i8*, i8** %a, i32 2
  store i8* getelementptr ([5 x i8], [5 x i8]* @str6, i64 0, i64 0), i8** %tmp5, align 0
  %tmp8 = getelementptr i8*, i8** %a, i32 3
  store i8* getelementptr ([7 x i8], [7 x i8]* @str7, i64 0, i64 0), i8** %tmp8, align 0

  ret i32 0
}


