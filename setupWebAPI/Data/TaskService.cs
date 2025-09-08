using setupWebAPI.Models;
using System.Text.Json;

public class TaskService
{
    private readonly string _filePath = "tasks.json";

    public List<TaskItem> LoadTasks()
    {
        if (!File.Exists(_filePath))
            return new List<TaskItem>();

        using (var stream = new FileStream(_filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        {
            try
            {
                return JsonSerializer.Deserialize<List<TaskItem>>(stream) ?? new List<TaskItem>();
            }
            catch
            {
                return new List<TaskItem>(); // return empty if corrupted
            }
        }
    }

    public void SaveTasks(List<TaskItem> tasks)
    {
        var json = JsonSerializer.Serialize(tasks, new JsonSerializerOptions { WriteIndented = true });

        // overwrite safely
        using (var stream = new FileStream(_filePath, FileMode.Create, FileAccess.Write, FileShare.None))
        using (var writer = new StreamWriter(stream))
        {
            writer.Write(json);
        }
    }
}
