using System;

namespace FastController
{
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Method)]
    public class RouteAttribute:Attribute
    {
        public string Url { get; }

        public RouteAttribute(string url)
        {
            Url = url;
        }
    }
}
