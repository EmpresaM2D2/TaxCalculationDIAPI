using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppTaxCalculationSAPDIAPI
{
    public  static class Extentions
    {
        public static string ConvertObjectToJson(this object obj)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
                
                serializer.WriteObject(memoryStream, obj);
                return FormatJson( System.Text.Encoding.UTF8.GetString(memoryStream.ToArray()));
            }
        }

        public static string FormatJson(string json)
        {
            int indentation = 0;
            int inc = 4; // Spaces for indentation
            StringBuilder output = new StringBuilder();
            char[] chars = json.ToCharArray();

            foreach (char c in chars)
            {
                if (c == '{' || c == '[')
                {
                    indentation += inc;
                    output.Append(c);
                    output.Append("\n" + new string(' ', indentation));
                }
                else if (c == '}' || c == ']')
                {
                    indentation -= inc;
                    output.Append("\n" + new string(' ', indentation));
                    output.Append(c);
                }
                else if (c == ',')
                {
                    output.Append(c);
                    output.Append("\n" + new string(' ', indentation));
                }
                else
                {
                    output.Append(c);
                }
            }

            return output.ToString();
        }
    }
}
