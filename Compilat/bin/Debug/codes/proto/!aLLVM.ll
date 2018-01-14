define i32 @func(i32 %x){
entry:
  %tmp0 = add i32 %x, %a
  ret i32 %tmp0
}


define i32 @main(i32 %arg, ??? %argc){
entry:
  %tmp2 = call i32 @func(i32 %b)
  ret i32 %tmp2
}


define void @func1(i32 %v1, i32 %v2){
entry:
  %v1 = %v2
  ret void
}


define i32 @main2(i32 %arg, ??? %argc, f64 %d1, f64 %d2, ??? %S, ??? %SS){
entry:
  %flag = i1 True
  %tmp3 = call void @func1(i32 1, i32 1)
%tmp3
%x
%y
%z
%x1
%x2
%x3
%x4
;If
  %tmp3 = and i1 %flag,    ...
  %cond3 = icmp %tmp3
  br i1 %cond3, label %Ifthen3, label %Ifcont3
Ifthen3:
;While
  br label %Whilecond2
Whilecond2:
  %tmp4 = or i1    ...,    ...
  %cond2 = icmp eq i1    ..., %tmp4

  br i1 %cond2, label %Whileaction2, label %Whilecont2
Whileaction2:
  %i = i32 0
;For
  br label %Forcond1
Forcond1:
  %cond1 = icmp slt i32 %i, 10

  br i1 %cond1, label %Foraction1, label %Forcont1
Foraction1:
  %tmp5 = add i32 1, %i
  %i = %tmp5
  br label %Forcond1
Forcont1:

  br label %Whilecond2
Whilecont2:

  br label %Ifcont3
Ifcont3:
  ret i32 0
}


