using System.Text.Json;
using System.Text.Json.Serialization;
using Cogni.Database.Context;
using Microsoft.EntityFrameworkCore;

namespace Cogni.Services;

public sealed class SakinorvaService
{
    private static readonly string[] Functions = ["Te", "Ti", "Fe", "Fi", "Se", "Si", "Ne", "Ni"];
    private static readonly double[] GrantWeights = [4.0, 3.0, 0.5, -2.0];
    private readonly CogniDbContext _context;

    public SakinorvaService(CogniDbContext context) => _context = context;

    public async Task<JsonDocument> GetCatalogAsync(CancellationToken cancellationToken = default)
    {
        var catalog = await _context.SakinorvaCatalogs.AsNoTracking()
            .OrderBy(item => item.Id).FirstOrDefaultAsync(cancellationToken);
        if (catalog is null)
            throw new InvalidOperationException("Каталог Sakinorva отсутствует в БД. Запустите DevelopmentDatabaseSeeder.");
        return JsonDocument.Parse(catalog.JsonContent);
    }

    public async Task<object> CalculateAsync(Dictionary<string, int> answers, CancellationToken cancellationToken = default)
    {
        using var catalog = await GetCatalogAsync(cancellationToken);
        var questions = catalog.RootElement.GetProperty("questions").EnumerateArray().ToArray();
        var expectedIds = questions.Select(q => q.GetProperty("id").GetString()!).ToHashSet(StringComparer.Ordinal);

        var missing = expectedIds.Except(answers.Keys, StringComparer.Ordinal).OrderBy(x => x).ToArray();
        var unknown = answers.Keys.Except(expectedIds, StringComparer.Ordinal).OrderBy(x => x).ToArray();
        if (missing.Length > 0 || unknown.Length > 0)
            throw new ArgumentException($"Нужно передать ответы на все 48 вопросов. Пропущены: [{string.Join(", ", missing)}]. Неизвестные: [{string.Join(", ", unknown)}].");
        var invalid = answers.Where(a => a.Value < 1 || a.Value > 5).Select(a => a.Key).OrderBy(x => x).ToArray();
        if (invalid.Length > 0)
            throw new ArgumentException($"Ответы должны быть целыми числами от 1 до 5. Некорректные: [{string.Join(", ", invalid)}].");

        var means = new Dictionary<string, double>();
        var strengths = new Dictionary<string, double>();
        foreach (var function in Functions)
        {
            var ids = questions.Where(q => q.GetProperty("function").GetString() == function)
                .Select(q => q.GetProperty("id").GetString()!);
            var values = ids.Select(id => answers[id]).ToArray();
            var mean = values.Average();
            means[function] = Math.Round(mean, 4);
            strengths[function] = (mean - 1.0) / 4.0;
        }

        var stacks = catalog.RootElement.GetProperty("stacks");
        var scores = new Dictionary<string, double>();
        foreach (var type in stacks.EnumerateObject())
        {
            var stack = type.Value.EnumerateArray().Select(x => x.GetString()!).ToArray();
            var score = stack.Select((function, index) => GrantWeights[index] * strengths[function]).Sum();
            scores[type.Name] = score;
        }

        var maxPositive = GrantWeights.Where(w => w > 0).Sum(); // 7.5
        var minNegative = GrantWeights.Where(w => w < 0).Sum(); // -2
        var ranked = scores.OrderByDescending(x => x.Value).Select((x, index) => new
        {
            rank = index + 1,
            type = x.Key,
            stack = stacks.GetProperty(x.Key).EnumerateArray().Select(v => v.GetString()).ToArray(),
            score = Math.Round(x.Value, 4),
            matchPercent = Math.Round(Math.Clamp((x.Value - minNegative) / (maxPositive - minNegative) * 100.0, 0, 100), 1)
        }).ToArray();

        return new
        {
            algorithm = "Sakinorva / Grant-Brownsword",
            note = "Проценты отдельных типов независимы и не суммируются до 100%. Это эвристическая реализация весов из mbti4.php, а не официальный закрытый алгоритм Sakinorva.",
            result = ranked[0].type,
            top3 = ranked.Take(3),
            allTypes = ranked,
            functionMeans = means,
            functionStrengthPercent = strengths.ToDictionary(x => x.Key, x => Math.Round(x.Value * 100, 1))
        };
    }
}
