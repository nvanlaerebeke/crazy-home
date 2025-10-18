using System;
using LanguageExt.Common;
using Microsoft.Extensions.DependencyInjection;
using PlugwiseControl.Message.Responses;

namespace PlugwiseControl.Actions;

internal sealed class PlugwiseActions {
    private readonly IServiceProvider _services;

    public PlugwiseActions(IServiceProvider services) {
        _services = services;
    }

    public Result<SwitchOnResponse> On(string mac) {
        return _services.GetRequiredService<On>().Execute(mac);
    }

    public Result<SwitchOffResponse> Off(string mac) {
        return _services.GetRequiredService<Off>().Execute(mac);
    }
}
