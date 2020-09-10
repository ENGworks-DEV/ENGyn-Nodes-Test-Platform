using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENGyn.NodesTestPlatform.Utils
{
    public static class ParserHelper
    {
        /// <summary>
        /// Converts a srting to any data type of the dest parameter
        /// </summary>
        /// <typeparam name="T">Generic Datatype</typeparam>
        /// <param name="src">source string</param>
        /// <param name="dest">destiny param where datatype will be taken. It can be any value.</param>
        /// <returns></returns>
        public static T ConvertTo<T>(this string src, T dest)
        {
            if (src is T variable) return variable;

            try
            {
                //Handling Nullable types i.e, int?, double?, bool? .. etc
                if (Nullable.GetUnderlyingType(typeof(T)) != null)
                {
                    return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFrom(src);
                }

                return (T)Convert.ChangeType(src, typeof(T));
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        /// <summary>
        /// Converts a string input to any other primitive type
        /// </summary>
        /// <param name="requiredType">Destinaion Type</param>
        /// <param name="inputvalue">String input value</param>
        /// <returns>String value parsed to destiny type</returns>
        public static object ParseType(Type requiredType, string inputvalue)
        {
            var requiredTypeCode = Type.GetTypeCode(requiredType);
            object result = null;
            bool parseStatus = true;

            switch(requiredTypeCode)
            {
                case TypeCode.String:
                    result = inputvalue;
                    break;
                        
                case TypeCode.Char:
                    char charResult;
                    parseStatus = char.TryParse(inputvalue, out charResult);
                    result = charResult;
                    break;

                case TypeCode.Boolean:
                    bool boolResult;
                    parseStatus = bool.TryParse(inputvalue, out boolResult);
                    result = boolResult;
                    break;

                case TypeCode.DateTime:
                    DateTime dateResult;
                    parseStatus = DateTime.TryParse(inputvalue, out dateResult);
                    result = dateResult;
                    break;

                case TypeCode.Int16:
                    short integer16;
                    parseStatus = short.TryParse(inputvalue, out integer16);
                    result = integer16;
                    break;

                case TypeCode.Int32:
                    int integer32;
                    parseStatus = int.TryParse(inputvalue, out integer32);
                    result = integer32;
                    break;

                case TypeCode.Int64:
                    long integer64;
                    parseStatus = long.TryParse(inputvalue, out integer64);
                    result = integer64;
                    break;

                case TypeCode.Single:
                    float floatResult;
                    parseStatus = float.TryParse(inputvalue, out floatResult);
                    result = floatResult;
                    break;

                case TypeCode.Double:
                    double doubleResult;
                    parseStatus = double.TryParse(inputvalue, out doubleResult);
                    result = doubleResult;
                    break;

                case TypeCode.Byte:
                    byte byteResult;
                    parseStatus = byte.TryParse(inputvalue, out byteResult);
                    result = byteResult;
                    break;

                default:
                    parseStatus = false;
                    break;
            }

            if (!parseStatus)
            {
                string errorMessage = string.Format("Argument type {0} cannot be parsed to {1}", inputvalue.GetType().Name, requiredType.Name);
                throw new InvalidCastException(errorMessage);
            }

            return result;
        }
    }
}
