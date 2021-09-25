using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeBasicSettings.Middleware
{
    /// <summary>
    /// This attribute is used to decorate Function with authorize attribute.
    /// It contains one or more roles.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class FunctionAuthorizeAttribute : Attribute
    {
        public FunctionAuthorizeAttribute(params string[] roles)
        {
            Roles = roles;
        }

        public IEnumerable<string> Roles { get; }
    }
}
