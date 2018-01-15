; main int ( int, char** )
define i32 @main(i32 %_0argc, i8** %_1args) #0 {
  %_0X = alloca i32
  store i32 10, i32* %_0X
  %_1Xp = alloca i32*
  store i32* %_0X, i32** %_1Xp
  %_2xpp = alloca i32**
  store i32** %_1Xp, i32*** %_2xpp
  %_3xppp = alloca i32***
  store i32*** %_2xpp, i32**** %_3xppp
  %_4xpppp = alloca i32****
  store i32**** %_3xppp, i32***** %_4xpppp
%tmp2 = load i32***, i32**** %_4xpppp, align 4
%tmp3 = load i32**, i32*** %tmp2, align 4
%tmp4 = load i32*, i32** %tmp3, align 4
  %tmp5 = load i32*, i32** %tmp4
  store i32 14, i32* %tmp5, align 4
  ret i32 1
}


