using Dapper;

using MailService.Domain;

using MailService.Infrastructure.Persistence;

namespace MailService.Infrastructure.Repositories;

public class TemplateRepository(DbConnectionFactory dbConnectionFactory)
{

 

    public async Task<EmailTemplate> GetTemplateByName(string name)
    {
        using var connection = dbConnectionFactory.CreateConnection();
        await connection.OpenAsync();

        var sql = "SELECT * FROM email_templates WHERE type = @Type";
        return await connection.QueryFirstAsync<EmailTemplate>(sql, new { Type = name });
    }
}