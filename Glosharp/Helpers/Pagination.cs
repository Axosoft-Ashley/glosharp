using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Glosharp.Models.Request;

namespace Glosharp.Helpers
{
    internal static class Pagination
    {
        internal static IDictionary<string, string> Setup(IDictionary<string, string> parameters, ApiOptions options)
        {
            
            if (options.PerPage.HasValue)
            {
                parameters.Add("per_page", options.PerPage.Value.ToString(CultureInfo.InvariantCulture));
            }

            if (options.Page.HasValue)
            {
                parameters.Add("page", options.Page.Value.ToString(CultureInfo.InvariantCulture));
            }

            return parameters;
        }

        internal static bool ShouldContinue(Uri uri, ApiOptions options)
        {
            if (uri == null)
            {
                return false;
            }

            if (uri.Query.Contains("page=") && options.PerPage.HasValue)
            {
                var allValues = ToQueryStringDictionary(uri);

                string pageValue;
                if (allValues.TryGetValue("page", out pageValue))
                {
                    var startPage = options.Page ?? 1;
                    var pageCount = options.PerPage.Value;

                    var endPage = startPage + pageCount;
                    if (pageValue.Equals(endPage.ToString(CultureInfo.InvariantCulture), StringComparison.OrdinalIgnoreCase))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        static Dictionary<string, string> ToQueryStringDictionary(Uri uri)
        {
            return uri.Query.Split('&')
                .Select(keyValue =>
                {
                    var indexOf = keyValue.IndexOf('=');
                    if (indexOf > 0)
                    {
                        var key = keyValue.Substring(0, indexOf);
                        var value = keyValue.Substring(indexOf + 1);
                        return new KeyValuePair<string, string>(key, value);
                    }

                    //just a plain old value, return it
                    return new KeyValuePair<string, string>(keyValue, null);
                })
                .ToDictionary(x => x.Key, x => x.Value);
        }
    }
}