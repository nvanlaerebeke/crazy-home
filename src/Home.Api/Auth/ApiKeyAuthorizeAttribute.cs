using Microsoft.AspNetCore.Mvc;

namespace Home.Api.Auth;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class ApiKeyAuthorizeAttribute : TypeFilterAttribute {
    public ApiKeyAuthorizeAttribute() : base(typeof(ApiKeyAuthorizeFilter)) {
    }
}
