using Godot;
using System;

public static class GlobalHelper
{
    public static void Log(object message)
    {
        GD.Print($"[{DateTime.Now:HH:mm:ss.fff}] {message}");
    }
}