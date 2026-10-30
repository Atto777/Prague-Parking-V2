
using System.Text.Json;

namespace PragueParking.Filhantering
{
    public class FilHantering
    {
        public static void Spara<T>(T data)
        {
            string json = JsonSerializer.Serialize(
                data,
                new JsonSerializerOptions
                {
                    WriteIndented = true
                });

            File.WriteAllText("parkering.json", json);
        }

        public static T? Ladda<T>()
        {
            if (!File.Exists("parkering.json"))
            {
                return default;
            }

            string json = File.ReadAllText("parkering.json");

            return JsonSerializer.Deserialize<T>(json);
        }
    }
}
