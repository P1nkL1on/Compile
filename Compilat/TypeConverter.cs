using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilat
{
    public class TypeConverter
    {
        // nev conversion system
        // int double string char bool other
        static bool[,] canConvert = new bool[,] { {true, true, true, false, true, false},
                                                  {false, true, true, false, true, false},
                                                  {false, false, true, false, false, false},
                                                  {true, false, true, true, true, false},
                                                  {true, true, false, false, true, false},
                                                  {false, false, false, false, false, false}};

        static int fromTypeToInt(ValueType vt)
        {
            if (vt.pointerLevel > 0)
                return -1;
            if (vt.rootType == VT.Cint) return 0;
            if (vt.rootType == VT.Cdouble) return 1;
            if (vt.rootType == VT.Cstring) return 2;
            if (vt.rootType == VT.Cchar) return 3;
            if (vt.rootType == VT.Cboolean) return 4;
            return -1;
        }

        static ValueType fromIntToType(int ind)
        {
            switch (ind)
            {
                case 0: return new ValueType(VT.Cint);
                case 1: return new ValueType(VT.Cdouble);
                case 2: return new ValueType(VT.Cstring);
                case 3: return new ValueType(VT.Cchar);
                case 4: return new ValueType(VT.Cboolean);
                default:
                    return new ValueType(VT.Cunknown);
            }
        }

        public static bool canConvertFromTypeToType(ValueType from, ValueType to)
        {
            if (from == to)
                return true;    // no need to convert
            int fromNumber = fromTypeToInt(from),
                toNumber = fromTypeToInt(to);
            if (fromNumber < 0 || toNumber < 0)
                return false;   // no place in convert table

            try
            {
                return canConvert[fromNumber, toNumber];    // return table value
            }
            catch (Exception e) { return false; }   // i doknow wat can go wrong
        }

        public static IOperation applyConvert(IOperation from, ValueType convertType)
        {
            return new Conv(from, convertType);
        }

        public static ValueType TryConvert(TypeConvertion needType, ref IOperation[] args)
        {
            int vars = needType.to.Length;
            for (int i = 0; i < vars; i++)
            {
                // check i
                List<ValueType> convertList = new List<ValueType>();
                bool canBeConverted = true;

                if (args.Length != needType.from[i].Count) continue; // invalid parameter count

                for (int j = 0; j < needType.from[i].Count; j++)
                {
                    if (canConvertFromTypeToType(args[j].returnTypes(), needType.from[i][j]))
                        convertList.Add(needType.from[i][j]);
                    else
                    { canBeConverted = false; break; }
                }
                if (!canBeConverted)
                    continue;
                else
                {
                    // variant #i can be performed using some (or noone) convertions

                    // completely convertable
                    // from list apply
                    for (int j = 0; j < convertList.Count; j++)
                        if (args[j].returnTypes() != convertList[j])
                            args[j] = applyConvert(args[j], convertList[j]);
                    // after this
                    return needType.to[i];
                }
            }

            // can not be converted
            throw new Exception("Can not convert!" + ArgsToString(args) + " to " + ArgsToString(needType.from[0].ToArray()));
            //return new ValueType(VT.Cunknown);
        }

        public static ValueType TryConvertAsum(ref IOperation[] args)
        {
            if (args.Length != 2)
                throw new Exception("Used not form assum!");
            if (args[0].returnTypes() == args[1].returnTypes())
                return args[0].returnTypes();
            if (canConvertFromTypeToType(args[1].returnTypes(), args[0].returnTypes()))
            {
                args[1] = applyConvert(args[1], args[0].returnTypes()); // int = bool ---> int = (int) bool; --/-> (bool)int = bool;
                return args[0].returnTypes();
            }

            throw new Exception("Can not convert assume type!" + ArgsToString(args));
        }

        public static ValueType TryConvertEqual(ref IOperation[] args)
        {
            if (args.Length != 2)
                throw new Exception("Used not for equality!");
            if (args[0].returnTypes() == args[1].returnTypes())
                return args[0].returnTypes();

            return TryConvert(new TypeConvertion("BBBIIBDDBCCBSSB", 2),ref args);
        }

        public static ValueType TryConvertSumm(TypeConvertion needType, ref IOperation[] args)
        {
            //bool inverted = false;
            if (args.Length != 2)
                throw new Exception("Used not for summ/diff!");
            if (args[0].returnTypes() == args[1].returnTypes() && args[0].returnTypes().pointerLevel > 0)
                return args[0].returnTypes();
            // can return **int + **int = **int;
            for (int i = 0; i < 2; i++)
            {
                // test this again
                if (args[0].returnTypes().pointerLevel == 0 && args[1].returnTypes().pointerLevel > 0)
                    // int + ***X == ***X; but we should conver this fucker to INT
                    if (args[0].returnTypes().rootType == VT.Cint || args[0].returnTypes().rootType == VT.Cchar || args[0].returnTypes().rootType == VT.Cboolean)
                    {
                        
                        args[0] = applyConvert(args[0], new ValueType(VT.Cint));
                        if (i == 1) 
                            args = new IOperation[]{args[1], args[0]};

                        return args[(i == 0)? 1 : 0].returnTypes();
                    }

                if (i == 0)
                { IOperation temp = args[0]; args[0] = args[1]; args[1] = temp; }   // swap them and check again
            }

            return TryConvert(needType, ref args);
            //throw new Exception("Can not convert summ!");
        }

        static string ArgsToString(IOperation[] args)
        {
            string res = " ( ";
            for (int i = 0; i < args.Length; i++, res += (args.Length > i) ? ", " : "")
                res += args[i].returnTypes().ToString();
            return res + " )";
        }

        static string ArgsToString(ValueType[] args)
        {
            string res = " ( ";
            for (int i = 0; i < args.Length; i++, res += (args.Length > i) ? ", " : "")
                res += args[i].ToString();
            return res + " )";
        }
    }
}
