using AllGreen.Lib.Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace AllGreen.Lib.DomainModel.Json
{
    public abstract class JsonCapableElement : IJson
    {
        public string Json => GetJson(JsonOptions.Default);

        public abstract string GetJson(JsonOptions options);

        public string GetJsonObjectItem(string name, string value) => string.Format("{0}:{1}", GetJsonScalar(name), value);
        public string GetJsonObjectItemOrNull(string name, string value) => value == null ? null : GetJsonObjectItem(name, value);

        public string GetJsonScalarNull() => "null";
        public string GetJsonScalar(string value) => value == null ? GetJsonScalarNull() : string.Format("\"{0}\"", value.Replace("\"", "\\\""));
        public string GetJsonScalar(bool value) => value ? "true" : "false";
        public string GetJsonScalar(int value) => string.Format(CultureInfo.InvariantCulture, "{0}", value);
        public string GetJsonScalar(double value) => string.Format(CultureInfo.InvariantCulture, "{0}", value);
        public string GetJsonScalar(TimeSpan value) => string.Format(CultureInfo.InvariantCulture, "{0}", value.TotalSeconds);
        public string GetJsonScalar(DateTime value) => string.Format(CultureInfo.InvariantCulture, "\"{0:yyyy-MM-dd HH:mm:ss}\"", value);

        public string GetJsonScalarOrNull(string value) => value != null ? GetJsonScalar(value) : null;
        public string GetJsonScalarOrNull(bool? value) => value.HasValue ? GetJsonScalar(value.Value) : null;
        public string GetJsonScalarOrNull(int? value) => value.HasValue ? GetJsonScalar(value.Value) : null;
        public string GetJsonScalarOrNull(double? value) => value.HasValue ? GetJsonScalar(value.Value) : null;
        public string GetJsonScalarOrNull(TimeSpan? value) => value.HasValue ? GetJsonScalar(value.Value) : null;
        public string GetJsonScalarOrNull(DateTime? value) => value.HasValue ? GetJsonScalar(value.Value) : null;

        public string GetJsonObject(IEnumerable<string> items) => items == null ? null : string.Format("{0}{2}{1}", "{", "}", string.Join(",", items.Where(item => item != null).ToArray()));
        public string GetJsonArray(IEnumerable<string> items) => items == null ? null : string.Format("{0}{2}{1}", "[", "]", string.Join(",", items.Where(item => item != null).ToArray()));
    }
}
