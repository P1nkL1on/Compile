; main int (  )
define i32 @main() #0 {
  %_0no = alloca i1
  store i1 0, i1* %_0no
  %$1_0no = load i1, i1* %_0no
  %_1yes = alloca i1
  store i1 1, i1* %_1yes
  %$1_1yes = load i1, i1* %_1yes
  %_2res = alloca double
  store double 1.0, double* %_2res
  %$1_2res = load double, double* %_2res
;If
  %tmp3 = icmp eq i32 0, 0
  %tmp5 = icmp eq i32 1, 1
  %tmp6 = icmp eq i32 2, 2
  %tmp4 = or i1 %tmp5, %tmp6
  %tmp2 = or i1 %tmp3, %tmp4
  %cond1 = icmp eq i1 %tmp2, 1

  br i1 %cond1, label %Ifthen1, label %Ifcont1
Ifthen1:
  %tmp10 = sitofp i32 2 to double
  %tmp9 = fmul double %$1_2res, %tmp10
  store double %tmp9, double* %_2res
  %$2_2res = load double, double* %_2res
  br label %Ifcont1
Ifcont1:
  %tmp14 = zext i1 %$1_1yes to i32
  %tmp13 = mul i32 %tmp14, 12
  %tmp18 = zext i1 %$1_0no to i32
  %tmp17 = mul i32 %tmp18, 17
  %tmp16 = sub i32 30, %tmp17
  %tmp12 = add i32 %tmp13, %tmp16
  ret i32 %tmp12
}


