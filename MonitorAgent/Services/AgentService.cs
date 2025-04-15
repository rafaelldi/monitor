using Grpc.Core;
using MonitorAgent.ProcessList;

namespace MonitorAgent.Services;

internal sealed class AgentService(
    ProcessHandler processHandler
) : Agent.AgentBase
{
    public override Task<ProcessListResponse> GetProcessList(ProcessListRequest request, ServerCallContext context)
    {
        var processes = processHandler.GetProcessList();
        var response = new ProcessListResponse();
        response.Processes.AddRange(processes);
        return Task.FromResult(response);
    }

    public override Task<ProcessDetailsResponse> GetProcessDetails(ProcessDetailsRequest request, ServerCallContext context)
    {
        var processDetails = processHandler.GetProcessDetails(request.ProcessId);
        var response = new ProcessDetailsResponse
        {
            Details = processDetails
        };
        return Task.FromResult(response);
    }

    public override Task<ProcessEnvironmentResponse> GetProcessEnvironment(ProcessEnvironmentRequest request, ServerCallContext context)
    {
        var processEnvironment = processHandler.GetProcessEnvironment(request.ProcessId);
        var response = new ProcessEnvironmentResponse();
        response.Environment.AddRange(processEnvironment);
        return Task.FromResult(response);
    }
}