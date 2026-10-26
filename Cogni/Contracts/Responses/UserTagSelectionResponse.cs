namespace Cogni.Contracts.Responses;

public record UserTagSelectionResponse(
    List<int> CategoryIds,
    List<int> TagIds);
