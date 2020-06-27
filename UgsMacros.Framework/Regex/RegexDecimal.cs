using System;
using System.Text.RegularExpressions;

namespace UgsMacros.Framework.Regex
{
    public class RegexDecimal
    {
        private string _name;

        public RegexDecimal(string name)
        {
            _name = name;
        }

        public string Expression => $"(?<{_name}>[-]*[0-9]*([.][0-9]{{1,3}})*(&[0-9]{{1,3}}/[0-9]{{1,3}})*)"; //$"(?<{_name}>[-]*[0-9]*([.][0-9]{{1,3}})*)";

        public decimal GetValue(Match match)
        {
            var value = match.Groups[_name].Value;

            if (value.Contains("&"))
            {
                var parts = value.Split('&', '/');
                var result = Convert.ToDecimal(parts[0]) + (Convert.ToDecimal(parts[1]) / Convert.ToDecimal(parts[2]));
                return Decimal.Round(result, 3);
            }

            return Convert.ToDecimal(value);
        }

        public decimal GetMillimeters(Match match)
        {
            return GetValue(match) * 25.4m;
        }
    }
}
