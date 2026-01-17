using System.Text.Json;
using CoreServer.Domain.Employees;

namespace CoreServer.Application.EmployeeDirectory
{
    public static class EmployeeParser
    {
        public static List<Employee> FromCsv(string csv)
        {
            var result = new List<Employee>();
            if (string.IsNullOrWhiteSpace(csv))
            {
                return result;
            }

            var lines = csv.Split(
                '\n',
                StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            foreach (var line in lines)
            {
                if (line.StartsWith("name,", StringComparison.OrdinalIgnoreCase))
                    continue;

                var p = line.Split(',', StringSplitOptions.TrimEntries);
                if (p.Length < 4)
                {
                    return new List<Employee>();
                }

                if (!TryParseDate(p[3], out var joined))
                {
                    return new List<Employee>();
                }

                result.Add(new Employee(p[0], p[1], p[2], joined));
            }

            return result;
        }

        public static List<Employee> FromJson(string json)
        {
            var result = new List<Employee>();
            if (string.IsNullOrWhiteSpace(json))
            {
                return result;
            }
                
            List<JsonEmployee>? items;
            try
            {
                items = JsonSerializer.Deserialize<List<JsonEmployee>>(
                    json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch
            {
                return result;
            }

            if (items == null || items.Count == 0)
            {
                return result;
            }

            foreach (var i in items)
            {
                if (!TryParseDate(i.Joined, out var joined))
                {
                    result.Clear();
                    return result;
                }

                result.Add(new Employee(i.Name, i.Email, i.Tel, joined));
            }

            return result;
        }

        private static bool TryParseDate(string? s, out DateOnly joined)
        {
            joined = default;

            if (string.IsNullOrWhiteSpace(s))
                return false;

            s = s.Trim();
            if (s.Contains('.'))
                s = s.Replace('.', '-');

            return DateOnly.TryParse(s, out joined);
        }

        private record JsonEmployee(string Name, string Email, string Tel, string? Joined);
    }
}