using System.Collections.Concurrent;
using docker_lab_backend.Models;

namespace docker_lab_backend.Services;

public interface ITaskService
{
    IEnumerable<TaskItem> GetAllTasks();
    TaskItem? GetTaskById(int id);
    TaskItem CreateTask(TaskItem task);
    bool UpdateTask(int id, TaskItem task);
    bool DeleteTask(int id);
}

public class TaskService : ITaskService
{
    private readonly ConcurrentDictionary<int, TaskItem> _tasks = new();
    private int _nextId = 1;

    public IEnumerable<TaskItem> GetAllTasks()
    {
        return _tasks.Values;
    }

    public TaskItem? GetTaskById(int id)
    {
        _tasks.TryGetValue(id, out var task);
        return task;
    }

    public TaskItem CreateTask(TaskItem task)
    {
        task.Id = Interlocked.Increment(ref _nextId);
        _tasks.TryAdd(task.Id, task);
        return task;
    }

    public bool UpdateTask(int id, TaskItem task)
    {
        if (!_tasks.ContainsKey(id))
            return false;
            
        task.Id = id;
        _tasks[id] = task;
        return true;
    }

    public bool DeleteTask(int id)
    {
        return _tasks.TryRemove(id, out _);
    }
}
