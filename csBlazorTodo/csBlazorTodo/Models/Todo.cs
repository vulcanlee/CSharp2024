﻿namespace csBlazorTodo.Models;

public class TodoModel
{
    public string Content { get; set; }
    public Guid Id { get; set; }
    public bool IsCompleted { get; set; }
    public bool IsEditing { get; set; }
}
