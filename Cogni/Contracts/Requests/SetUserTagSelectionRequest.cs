namespace Cogni.Contracts.Requests;

public record SetUserTagSelectionRequest(
    List<int> CategoryIds,
    List<int> TagIds);
