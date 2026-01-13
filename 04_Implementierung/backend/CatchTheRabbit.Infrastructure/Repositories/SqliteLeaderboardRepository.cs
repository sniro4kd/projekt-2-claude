using System.Data;
using CatchTheRabbit.Core.Interfaces;
using CatchTheRabbit.Core.Models;
using Dapper;
using Microsoft.Data.Sqlite;

namespace CatchTheRabbit.Infrastructure.Repositories;

/// <summary>
/// SQLite implementation of the leaderboard repository.
/// </summary>
public class SqliteLeaderboardRepository : ILeaderboardRepository
{
    private readonly string _connectionString;

    public SqliteLeaderboardRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IEnumerable<LeaderboardEntry>> GetTopEntriesAsync(PlayerRole role, int count)
    {
        const string sql = @"
            SELECT Id, Nickname, Role, ThinkingTimeMs, CreatedAt
            FROM LeaderboardEntries
            WHERE Role = @Role
            ORDER BY ThinkingTimeMs ASC
            LIMIT @Count";

        using var connection = GetConnection();
        var entries = await connection.QueryAsync<LeaderboardEntryDto>(sql, new
        {
            Role = role.ToString().ToLowerInvariant(),
            Count = count
        });

        return entries.Select(MapToEntry);
    }

    public async Task<int> AddEntryAsync(LeaderboardEntry entry)
    {
        const string sql = @"
            INSERT INTO LeaderboardEntries (Nickname, Role, ThinkingTimeMs, CreatedAt)
            VALUES (@Nickname, @Role, @ThinkingTimeMs, @CreatedAt);
            SELECT last_insert_rowid();";

        using var connection = GetConnection();
        return await connection.ExecuteScalarAsync<int>(sql, new
        {
            entry.Nickname,
            Role = entry.Role.ToString().ToLowerInvariant(),
            entry.ThinkingTimeMs,
            CreatedAt = entry.CreatedAt.ToString("o")
        });
    }

    public async Task<int> CountBetterEntriesAsync(PlayerRole role, long thinkingTimeMs)
    {
        const string sql = @"
            SELECT COUNT(*)
            FROM LeaderboardEntries
            WHERE Role = @Role AND ThinkingTimeMs < @ThinkingTimeMs";

        using var connection = GetConnection();
        return await connection.ExecuteScalarAsync<int>(sql, new
        {
            Role = role.ToString().ToLowerInvariant(),
            ThinkingTimeMs = thinkingTimeMs
        });
    }

    public async Task InitializeDatabaseAsync()
    {
        const string sql = @"
            CREATE TABLE IF NOT EXISTS LeaderboardEntries (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Nickname TEXT NOT NULL,
                Role TEXT NOT NULL CHECK(Role IN ('rabbit', 'children')),
                ThinkingTimeMs INTEGER NOT NULL,
                CreatedAt TEXT NOT NULL
            );

            CREATE INDEX IF NOT EXISTS IX_Leaderboard_Role_Time
            ON LeaderboardEntries(Role, ThinkingTimeMs ASC);";

        using var connection = GetConnection();
        await connection.ExecuteAsync(sql);
    }

    private IDbConnection GetConnection()
    {
        var connection = new SqliteConnection(_connectionString);
        connection.Open();
        return connection;
    }

    private static LeaderboardEntry MapToEntry(LeaderboardEntryDto dto)
    {
        return new LeaderboardEntry
        {
            Id = dto.Id,
            Nickname = dto.Nickname,
            Role = Enum.Parse<PlayerRole>(dto.Role, ignoreCase: true),
            ThinkingTimeMs = dto.ThinkingTimeMs,
            CreatedAt = DateTime.Parse(dto.CreatedAt)
        };
    }

    // DTO for database mapping
    private class LeaderboardEntryDto
    {
        public int Id { get; set; }
        public string Nickname { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public long ThinkingTimeMs { get; set; }
        public string CreatedAt { get; set; } = string.Empty;
    }
}
