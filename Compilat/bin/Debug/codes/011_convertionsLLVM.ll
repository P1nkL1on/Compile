; main int (  )
define i32 @$0main() #0 {
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
  %tmp2 = icmp eq i32 0, 0
  %tmp4 = icmp eq i32 1, 1
  %tmp5 = icmp eq i32 2, 2
  %tmp3 = or i1 %tmp4, %tmp5
  %tmp1 = or i1 %tmp2, %tmp3
  br i1 %tmp1, label %Ifthen1, label %Ifcont1
Ifthen1:
  %tmp9 = sitofp i32 2 to double
  %tmp8 = fmul double %$1_2res, %tmp9
  store double %tmp8, double* %_2res
  %$2_2res = load double, double* %_2res
  br label %Ifcont1
Ifcont1:
  %tmp13 = zext i1 %$1_1yes to i32
  %tmp12 = mul i32 %tmp13, 12
  %tmp17 = zext i1 %$1_0no to i32
  %tmp16 = mul i32 %tmp17, 17
  %tmp15 = sub i32 30, %tmp16
  %tmp11 = add i32 %tmp12, %tmp15
  ret i32 %tmp11
}


