using Microsoft.AspNetCore.Mvc;

namespace Home.Api.Auth;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public sealed class ApiKeyAuthorizeAttribute : TypeFilterAttribute {
    public ApiKeyAuthorizeAttribute() : base(typeof(ApiKeyAuthorizeFilter)) {
    }
}
