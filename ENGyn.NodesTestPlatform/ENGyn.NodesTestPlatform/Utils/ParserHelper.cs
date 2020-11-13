using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Reflection;

namespace ENGyn.NodesTestPlatform.Utils
{
    public static class ParserHelper
    {
        /// <summary>
        /// Converts a JToken parameter to any other primitive type given a Type parameterType
        /// </summary>
        /// <param name="parameter">Item to be parsed (Jtoken)</param>
        /// <param name="parameterType">Parameter type to be converted</param>
        /// <returns>object with the JToken value converted</returns>
        /// <exception cref="InvalidCastException">Thrown when a invalid conversion is performed</exception>
        public static object ParseParameter(JToken parameter, Type parameterType)
        {
            var requiredTypeCode = Type.GetTypeCode(parameterType);
            object result = null;

            try
            {
                switch (requiredTypeCode)
                {
                    case TypeCode.Object:
                        result = parameter.Value<string>();
                        break;

                    case TypeCode.String:
                        result = parameter.Value<string>();
                        break;

                    case TypeCode.Char:
                        result = parameter.Value<char>();
                        break;

                    case TypeCode.Boolean:
                        result = parameter.Value<bool>();
                        break;

                    case TypeCode.DateTime:
                        result = parameter.Value<DateTime>();
                        break;

                    case TypeCode.Int16:
                        result = parameter.Value<short>();
                        break;

                    case TypeCode.Int32:
                        result = parameter.Value<int>();
                        break;

                    case TypeCode.Int64:
                        result = parameter.Value<long>();
                        break;

                    case TypeCode.Single:
                        result = parameter.Value<float>();
                        break;

                    case TypeCode.Double:
                        result = parameter.Value<double>();
                        break;

                    case TypeCode.Byte:
                        result = parameter.Value<byte>();
                        break;

                    default:
                        result = parameter.Value<object>();
                        break;
                }

                return result;

            }
            catch (Exception ex)
            {
                throw new InvalidCastException("A error occured during parameter conversion process.", ex);                
            }

        }

        /// <summary>
        /// Converts a newtonsoft's JArray of parameters to an IList that contains all the parameters parsed to native types 
        /// based on what currenly have on the method signature
        /// </summary>
        /// <param name="jsonParameters">JArray of parameters</param>
        /// <param name="parameters">Parameter info array that contains the types of the method signature</param>
        /// <returns>A IList that contains all the parsed parameters</returns>
        public static IList ConvertParametersToSignatureTypes(JArray jsonParameters, ParameterInfo[] parameters = null)
        {
            IList convertedParameters = new ArrayList();

            // When an array is given we don't have access to parameter types so by default everything is converted to object
            if (parameters.Equals(null))
            {
                foreach (var item in jsonParameters)
                {
                    var parsedParameter = ParseParameter(item, typeof(object));
                    convertedParameters.Add(parsedParameter);
                }
            }
            else
            {
                for (int i = 0; i < parameters.Length; i++)
                {
                    // This condition is true when an array is given as argument
                    if (jsonParameters[i] is JArray)
                    {
                        var parsedParameter = ConvertParametersToSignatureTypes((JArray)jsonParameters[i]);
                        convertedParameters.Add(parsedParameter);
                    } 
                    else
                    {
                        var parsedParameter = ParseParameter(jsonParameters[i], parameters[i].ParameterType);
                        convertedParameters.Add(parsedParameter);
                    }
                }
            }

            return convertedParameters;
        }
    }
}