using Dapper;

using Domain;

using Infrastructure.Persistence;

namespace Infrastructure.Repositories;

public class TemplateRepository(DbConnectionFactory dbConnectionFactory)
{

 

    public async Task<EmailTemplate> GetTemplateByName(string name)
    {
        using var connection = dbConnectionFactory.CreateConnection();
        await connection.OpenAsync();

        var sql = "SELECT * FROM email_templates WHERE name = @Name";
        return await connection.QueryFirstAsync<EmailTemplate>(sql, new { Name = name });
    }
}