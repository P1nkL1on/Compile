define f64 @main(i32 %arg, i8** %atgv){
  %d = f64 10.00000
  %dd = f64* f64 %d

  %tmp2 = getelementptr f64, f64* %dd, i32 3
  store f64 560.00000, f64* tmp2, align 8
  %ds = %dd
  %X = i32 10
  %B = i32 20
  ret f64 0.00000
}


