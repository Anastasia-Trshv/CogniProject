namespace Cogni.Contracts.Responses;

public record TagCategoryResponse(
    int Id,
    string Name,
    List<TagResponse> Tags);

public record HobbyResponse(
    int Id,
    string Name,
    List<TagCategoryResponse> Categories);
