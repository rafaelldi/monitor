using Microsoft.Diagnostics.NETCore.Client;
using Microsoft.Diagnostics.Runtime;
using MonitorAgent;

namespace Monitor.MemoryDumps;

internal static class MemoryDumpManager
{
    internal static async Task<string> CollectMemoryDump(int pid, MemoryDumpType dumpType, CancellationToken token)
    {
        var memoryDumpId = Path.GetRandomFileName();
        var dumpFilePath = GetDumpFilePath(memoryDumpId);
        var type = Map(dumpType);
        var client = new DiagnosticsClient(pid);
        await client.WriteDumpAsync(type, dumpFilePath, false, token);

        return memoryDumpId;
    }

    private static DumpType Map(MemoryDumpType dumpType) => dumpType switch
    {
        MemoryDumpType.Normal => DumpType.Normal,
        MemoryDumpType.WithHeap => DumpType.WithHeap,
        MemoryDumpType.Triage => DumpType.Triage,
        MemoryDumpType.Full => DumpType.Full,
        _ => throw new ArgumentOutOfRangeException(nameof(dumpType), dumpType, null)
    };

    internal static void DeleteMemoryDump(string memoryDumpId)
    {
        var dumpFilePath = GetDumpFilePath(memoryDumpId);
        if (File.Exists(dumpFilePath))
        {
            File.Delete(dumpFilePath);
        }
    }

    internal static void GetClrStack(string memoryDumpId)
    {
        var dumpFilePath = GetDumpFilePath(memoryDumpId);
        var dataTarget = DataTarget.LoadDump(dumpFilePath, new CacheOptions());
        var clrInfo = dataTarget.ClrVersions[0];
        var runtime = clrInfo.CreateRuntime();
        var threads = runtime.Threads
            .Select(it => (it, it.EnumerateStackTrace()))
            .ToList();
        foreach (var thread in threads)
        {
        }
    }

    private static string GetDumpFilePath(string memoryDumpId)
    {
        var dumpFilename = $"{memoryDumpId}.dmp";
        return Path.Combine(Path.GetTempPath(), dumpFilename);
    }
}