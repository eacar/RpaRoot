using System;
using System.Net;

namespace Rpa.Attributes
{
    public sealed class StringValue : Attribute
    {
        public StringValue(string value)
        {
            Value = value;
        }
        public string Value { get; }
    }
    public sealed class HttpStatusCodeValue : Attribute
    {
        public HttpStatusCodeValue(HttpStatusCode value)
        {
            Value = value;
        }
        public HttpStatusCode Value { get; }
    }
}