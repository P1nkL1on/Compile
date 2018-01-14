declare void @printf(???)


declare void @put(???)


declare void @putstring(???)


declare void @putchar(i8)


define void @sortArray(i32 %arr, i32 %arr_length){
entry:
  %i = i32 0
;For
  br label %Forcond6
Forcond6:
  %cond6 = icmp slt i32 %i, %arr_length

  br i1 %cond6, label %Foraction6, label %Forcont6
Foraction6:
  %max = ?ARRAY ELEMENT?
  %maxindex = %i
  %tmp0 = add i32 1, %i
  %j = %tmp0
;For
  br label %Forcond5
Forcond5:
  %cond5 = icmp slt i32 %j, %arr_length

  br i1 %cond5, label %Foraction5, label %Forcont5
Foraction5:
;If
  %cond4 = icmp sgt i32 ?ARRAY ELEMENT?, %max

  br i1 %cond4, label %Ifthen4, label %Ifcont4
Ifthen4:
  %max = ?ARRAY ELEMENT?
  %maxindex = %j

  br label %Ifcont4
Ifcont4:
   ...
  br label %Forcond5
Forcont5:
  %temp = ?ARRAY ELEMENT?
  ?ARRAY ELEMENT? = ?ARRAY ELEMENT?
  ?ARRAY ELEMENT? = %temp

   ...
  br label %Forcond6
Forcont6:
  ret void
}


define void @traceArray(i32 %arr, i32 %arr_length){
entry:
  %i = i32 0
;For
  br label %Forcond7
Forcond7:
  %cond7 = icmp slt i32 %i, %arr_length

  br i1 %cond7, label %Foraction7, label %Forcont7
Foraction7:
  %tmp1 = add ??? \n,    ...
  %tmp2 = add ??? %tmp1,  : 
  %tmp3 = add ??? %tmp2,    ...
  %tmp5 = call void @printf(??? %tmp3)
%tmp5
   ...
  br label %Forcond7
Forcont7:
  ret void
}


define void @main(i32 %argv, i32 %argc){
entry:
  ?ARRAY ELEMENT? = i32 10
  ?ARRAY ELEMENT? = i32 8
  ?ARRAY ELEMENT? = i32 4
  ?ARRAY ELEMENT? = i32 3
  ?ARRAY ELEMENT? = i32 7
  ?ARRAY ELEMENT? = i32 11
  ?ARRAY ELEMENT? = i32 16
  ?ARRAY ELEMENT? = i32 3
  ?ARRAY ELEMENT? = i32 3
  ?ARRAY ELEMENT? = i32 2
  ?ARRAY ELEMENT? = i32 4

  %tmp6 = call void @printf(??? Original array\n)
%tmp6
  %tmp7 = call void @traceArray(i32 %arr, i32 11)
%tmp7
  %tmp8 = call void @sortArray(i32 %arr, i32 11)
%tmp8
  %tmp9 = call void @printf(??? Sorted array\n)
%tmp9
  %tmp10 = call void @traceArray(i32 %arr, i32 11)
%tmp10
  ret void
}


