using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Herbapedia.Util
{
    public class Cadenas
    {
        public static string Capitalizar(ref string toCapitalize)
        {
            char[] array = toCapitalize.ToArray();
            array[0] = toCapitalize.ToArray().First().ToString().ToUpper().ToCharArray()[0];
            toCapitalize = string.Empty;
            foreach (char ch in array)
            {
                toCapitalize += ch;
            }
            return toCapitalize;
        }

    }
}
