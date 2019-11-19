using Godot;
using System;
using System.Collections.Generic;

public class MessageLabel : Label
{
    private float Duration = 2.0f;
    private Queue<string> messageQueue = new Queue<string>();
    private Timer timer;

    public override void _Ready()
    {
        this.timer = new Timer();
        this.timer.SetWaitTime(this.Duration);
        this.timer.Connect("timeout", this, "_on_Timer_timeout");
        AddChild(this.timer);
    }

    public void AddMessage(string msg)
    {
        if (this.timer.IsStopped())
        {
            this.Text = msg;
            this.timer.Start();
        }
        else
        {
            this.messageQueue.Enqueue(msg);
        }
    }

    private void _on_Timer_timeout()
    {
        GD.Print("No more this");
        this.Text = "";

        if (this.messageQueue.Count == 0)
        {
            this.timer.Stop();
        }
        else
        {
            this.Text = this.messageQueue.Dequeue();
        }

    }

}
