using Microsoft.AspNetCore.Mvc;
using setupWebAPI.Models;
using System.Text.Json;

namespace setupWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private static TaskService _taskService = new TaskService();
        private static List<TaskItem> tasks = _taskService.LoadTasks();

        private readonly string firebaseUrl =
            "https://bscsse-tn03-webtask-api-default-rtdb.asia-southeast1.firebasedatabase.app/tasks";

        [HttpGet]
        public IActionResult GetTasks()
        {
            return Ok(tasks);
        }

        [HttpPost]
        public async Task<IActionResult> PostTask([FromBody] TaskItem task)
        {
            if (task == null) return BadRequest("Task is null");

            task.Id = tasks.Count > 0 ? tasks.Max(t => t.Id) + 1 : 1;
            tasks.Add(task);
            _taskService.SaveTasks(tasks);

            // Try saving to Firebase (don’t fail if error)
            try
            {
                using var http = new HttpClient();
                var json = JsonSerializer.Serialize(task);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                var response = await http.PutAsync($"{firebaseUrl}/{task.Id}.json", content);
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"⚠️ Firebase upload failed: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Firebase upload error: {ex.Message}");
            }

            return Ok(task);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = tasks.FirstOrDefault(t => t.Id == id);
            if (task == null) return NotFound("Task not found");

            tasks.Remove(task);
            _taskService.SaveTasks(tasks);

            // Try deleting from Firebase
            try
            {
                using var http = new HttpClient();
                var response = await http.DeleteAsync($"{firebaseUrl}/{id}.json");
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"⚠️ Firebase delete failed: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Firebase delete error: {ex.Message}");
            }

            return Ok(new { message = "Task deleted", id });
        }
    }
}
