using System.ComponentModel.DataAnnotations;

namespace Cogni.Contracts.Requests;

public sealed record CalculateSakinorvaRequest(
    [Required] Dictionary<string, int> Answers
);
