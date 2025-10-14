using System.Text.Json;
using NUnit.Framework;

namespace Home.Tests.TestObjects;

public static class HttpMessageExtensionMethods {
    private static readonly JsonSerializerOptions SerializerOptions = new() { PropertyNameCaseInsensitive = true };

    public static async Task<T?> ToObject<T>(this HttpResponseMessage response) {
        var content = await response.GetContent();
        await TestContext.Out.WriteLineAsync("Received response:");
        await TestContext.Out.WriteLineAsync(content);
        return JsonSerializer.Deserialize<T>(content, SerializerOptions);
    }

    private static async Task<string> GetContent(this HttpResponseMessage response) {
        return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
    }
}
