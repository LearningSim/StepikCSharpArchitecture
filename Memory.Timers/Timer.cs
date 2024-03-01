using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Memory.Timers;

public class Timer : IDisposable
{
    private readonly Stopwatch watch = new();
    private readonly int level;
    private readonly StringWriter writer;
    private readonly List<Timer> children = new();
    private bool isFinished;
    private long time;
    private readonly string name;

    private Timer(int level, StringWriter writer, string name)
    {
        this.level = level;
        this.writer = writer;
        this.name = name;
        watch.Start();
    }
    
    public static Timer Start(StringWriter writer, string name = "*") => new(0, writer, name);

    public Timer StartChildTimer(string name = "*")
    {
        var watch = new Timer(level + 1, writer, name);
        children.Add(watch);
        return watch;
    }

    private void MakeReport()
    {
        writer.Write(FormatReportLine(name, level, time));
        long childrenTime = 0;
        foreach (var child in children)
        {
            child.MakeReport();
            childrenTime += child.time;
        }

        if (children.Any())
        {
            writer.Write(FormatReportLine("Rest", level + 1, time - childrenTime));
        }
    }

    private string FormatReportLine(string timerName, int level, long value)
    {
        var intro = new string(' ', level * 4) + timerName;
        return $"{intro,-20}: {value}\n";
    }

    public void Dispose()
    {
        if (isFinished) return;
        isFinished = true;

        time = watch.ElapsedMilliseconds;
        if (level == 0)
        {
            MakeReport();
        }
    }
}