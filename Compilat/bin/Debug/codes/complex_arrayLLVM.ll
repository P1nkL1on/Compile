@str8 = private unnamed_addr constant [5 x i8] c"mike\00"
@str9 = private unnamed_addr constant [5 x i8] c"john\00"
@str10 = private unnamed_addr constant [5 x i8] c"mary\00"
@str11 = private unnamed_addr constant [7 x i8] c"ashley\00"
; main int (  )
define i32 @main() #0 {
  %tmp2 = load i8**, i8*** %_0a
  store i8* getelementptr ([5 x i8], [5 x i8]* @str8, i64 0, i64 0), i8** %tmp2, align 0
  %tmp3 = getelementptr i8**, i8*** %_0a, i32 1
  %tmp5 = load i8**, i8*** %tmp3
  store i8* getelementptr ([5 x i8], [5 x i8]* @str9, i64 0, i64 0), i8** %tmp5, align 0
  %tmp6 = getelementptr i8**, i8*** %_0a, i32 2
  %tmp8 = load i8**, i8*** %tmp6
  store i8* getelementptr ([5 x i8], [5 x i8]* @str10, i64 0, i64 0), i8** %tmp8, align 0
  %tmp9 = getelementptr i8**, i8*** %_0a, i32 3
  %tmp11 = load i8**, i8*** %tmp9
  store i8* getelementptr ([7 x i8], [7 x i8]* @str11, i64 0, i64 0), i8** %tmp11, align 0

  ret i32 0
}


