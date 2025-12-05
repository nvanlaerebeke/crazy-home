using System.CommandLine;

namespace Home.CommandLine;

internal interface IRootCommandEntryPoint {
    Command GetCommand();
}
