using System.Data.Common;
using System.Reflection;
using System.Text;
using MySql.Data.MySqlClient;

namespace Hazbin.Core.Features;

public class DataStorage<T> {
    private readonly MySqlConnection _connection;

    public DataStorage(string connectionString) {
        this._connection = new MySqlConnection(connectionString);
        this._connection.Open();
    }

    public DataStorage(string host, string dbName, string username, string pwd)
        : this($"Server={host};Database={dbName};Uid={username};Pwd={pwd}") { }

    public async Task InsertAsync(T data) {
        string tableName = typeof(T).Name.ToLower();
        List<PropertyInfo> props = typeof(T)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.CanRead)
            .ToList();

        await this.EnsureTableExistsAsync(tableName, props);

        string columns = string.Join(", ", props.Select(p => $"`{p.Name}`"));
        string paramNames = string.Join(", ", props.Select(p => $"@{p.Name}"));

        string query = $"INSERT INTO `{tableName}` ({columns}) VALUES ({paramNames})";

        MySqlCommand cmd = new(query, this._connection);
        foreach (PropertyInfo prop in props) {
            object value = prop.GetValue(data) ?? DBNull.Value;
            cmd.Parameters.AddWithValue($"@{prop.Name}", value);
        }

        await cmd.ExecuteNonQueryAsync();
    }

    public async Task<bool> UpdateAsync(T data) {
        string tableName = typeof(T).Name.ToLower();

        List<PropertyInfo> props = typeof(T)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.CanRead)
            .ToList();

        PropertyInfo? idProp = props.FirstOrDefault(p => p.Name.Equals("Id", StringComparison.OrdinalIgnoreCase));
        if (idProp == null)
            throw new InvalidOperationException($"Type {typeof(T).Name} is not contains field 'Id'.");

        object? idValue = idProp.GetValue(data);
        if (idValue == null)
            throw new InvalidOperationException("Field 'Id' cannot be null for update.");

        StringBuilder sb = new();
        sb.Append($"UPDATE `{tableName}` SET ");

        List<string> setStatements = new();
        foreach (PropertyInfo prop in props) {
            if (prop.Name.Equals("Id", StringComparison.OrdinalIgnoreCase))
                continue;

            setStatements.Add($"`{prop.Name}` = @{prop.Name}");
        }

        sb.Append(string.Join(", ", setStatements));
        sb.Append(" WHERE `Id` = @Id");

        string query = sb.ToString();

        MySqlCommand cmd = new(query, this._connection);

        foreach (PropertyInfo prop in props) {
            if (prop.Name.Equals("Id", StringComparison.OrdinalIgnoreCase))
                continue;

            object value = prop.GetValue(data) ?? DBNull.Value;
            cmd.Parameters.AddWithValue($"@{prop.Name}", value);
        }

        cmd.Parameters.AddWithValue("@Id", idValue);

        int affected = await cmd.ExecuteNonQueryAsync();
        return affected > 0;
    }

    private async Task EnsureTableExistsAsync(string tableName, List<PropertyInfo> props) {
        string checkQuery = $"SHOW TABLES LIKE '{tableName}'";
        MySqlCommand checkCmd = new(checkQuery, this._connection);
        object? result = await checkCmd.ExecuteScalarAsync();

        if (result != null)
            return;

        StringBuilder sb = new();
        sb.AppendLine($"CREATE TABLE `{tableName}` (");

        foreach (PropertyInfo prop in props) {
            string sqlType = GetSqlType(prop.PropertyType);
            bool isId = prop.Name.Equals("Id", StringComparison.OrdinalIgnoreCase);

            if (isId)
                sb.AppendLine($"  `{prop.Name}` {sqlType} PRIMARY KEY,");
            else
                sb.AppendLine($"  `{prop.Name}` {sqlType},");
        }

        if (sb.Length >= 3)
            sb.Length -= 3;

        sb.AppendLine(");");

        MySqlCommand createCmd = new(sb.ToString(), this._connection);
        await createCmd.ExecuteNonQueryAsync();
    }

    public async Task<T?> Get(Func<T, bool> func) {
        string tableName = typeof(T).Name.ToLower();
        string query = $"SELECT * FROM `{tableName}`";

        MySqlCommand cmd = new(query, this._connection);
        using (DbDataReader reader = await cmd.ExecuteReaderAsync()) {
            List<PropertyInfo> props = typeof(T)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanWrite)
                .ToList();

            while (await reader.ReadAsync()) {
                T instance = Activator.CreateInstance<T>();
                foreach (PropertyInfo prop in props) {
                    if (!reader.HasRows || reader[prop.Name] == DBNull.Value)
                        continue;

                    object value = reader[prop.Name];
                    prop.SetValue(instance, Convert.ChangeType(value, prop.PropertyType));
                }

                if (func(instance)) {
                    return instance;
                }
            }
        }

        return default;
    }

    private static string GetSqlType(Type type) {
        if (type == typeof(int) || type == typeof(uint)) return "INT";
        if (type == typeof(long) || type == typeof(ulong)) return "BIGINT";
        if (type == typeof(string)) return "TEXT";
        if (type == typeof(bool)) return "TINYINT(1)";
        if (type == typeof(DateTime)) return "DATETIME";
        if (type == typeof(float) || type == typeof(double) || type == typeof(decimal)) return "DOUBLE";

        throw new NotSupportedException($"Type {type.Name} is not supported.");
    }
}