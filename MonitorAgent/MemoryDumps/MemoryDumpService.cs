using Grpc.Core;

namespace MonitorAgent.MemoryDumps;

internal sealed class MemoryDumpService : MonitorAgent.MemoryDumpService.MemoryDumpServiceBase
{
    public override async Task<MemoryDumpResponse> CollectMemoryDump(MemoryDumpRequest request,
        ServerCallContext context)
    {
        var dumpId = await MemoryDumpManager.CollectThreadDump(request.ProcessId, request.DumpType,
            context.CancellationToken);

        return new MemoryDumpResponse
        {
            MemoryDumpId = dumpId
        };
    }
}