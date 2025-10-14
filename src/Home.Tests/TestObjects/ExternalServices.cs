using Moq;
using PlugwiseControl;

namespace Home.Tests.TestObjects;

internal sealed class ExternalServices {
    public Mock<IRequestManager> RequestManager => new();
}

