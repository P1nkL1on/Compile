; main int ( int, char** )
define i32 @main(i32 %_0argc, i8** %_1args) #0 {
  %_0X = alloca i32
  store i32 10, i32* %_0X
  %$1_0X = load i32, i32* %_0X
  %_1Xp = alloca i32*
  store i32* %$1_0X, i32** %_1Xp
  %_2xpp = alloca i32**
  store i32** %_1Xp, i32*** %_2xpp
  %_3xppp = alloca i32***
  store i32*** %_2xpp, i32**** %_3xppp
  %_4xpppp = alloca i32****
  store i32**** %_3xppp, i32***** %_4xpppp
  %tmp1 = add i32 11, %$1_0X
  store i32 %tmp1, i32* %_0X
  %$2_0X = load i32, i32* %_0X
%tmp4 = load i32****, i32**** %_4xpppp, align 4
%tmp5 = load i32***, i32*** %tmp4, align 4
%tmp6 = load i32**, i32** %tmp5, align 4
  %tmp7 = load i32*, i32** %tmp6
  store i32 14, i32* %tmp7, align 4
  ret i32 1
}


