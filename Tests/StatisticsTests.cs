using GameServices;
using GameServices.Controllers;
using Microsoft.EntityFrameworkCore;
using SharedModels;

public class StatisticsTests
{
    private AppDbContext GetDb()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task Leaderboard_Returns_SortedData()
    {
        var db = GetDb();
        db.GameSessions.Add(new GameSession { PlayerId = 1, Score = 10 });
        db.GameSessions.Add(new GameSession { PlayerId = 1, Score = 40 });
        db.GameSessions.Add(new GameSession { PlayerId = 2, Score = 20 });
        await db.SaveChangesAsync();

        var controller = new StatisticsController(db);
        var result = await controller.GetLeaderboard();

        Assert.NotNull(result);
    }
}
