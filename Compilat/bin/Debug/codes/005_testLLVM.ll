; main int (  )
define i32 @main() #0 {
  %D = alloca f64
  store f64 10.00000, f64* %D
  %$1D = load f64, f64* %D
  %X = alloca f64
  %tmp1 = fsub f64 %$1D, %$1D
  store f64 %tmp1, f64* %X
  store f64 12.00000, f64* %D
  %$2D = load f64, f64* %D
  %X2 = alloca f64
  %tmp3 = fadd f64 %$2D, %$2D
  store f64 %tmp3, f64* %X2
  %F = alloca f64
  %tmp6 = fmul f64 %$2D, %$2D
  %tmp5 = fmul f64 %$2D, %tmp6
  store f64 %tmp5, f64* %F
  %$1F = load f64, f64* %F
  %tmp9 = fadd f64 1.00000, %$2D
  store f64 %tmp9, f64* %D
  %$3D = load f64, f64* %D
  %tmp13 = fadd f64 %$3D, %$3D
  %tmp12 = fadd f64 %tmp13, %$3D
  %tmp11 = fadd f64 %tmp12, %$1F
  store f64 %tmp11, f64* %F
  %$2F = load f64, f64* %F
  ret i32 1
}


