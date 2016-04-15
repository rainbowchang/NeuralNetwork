using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeuralNetwork
{
    class Job
    {
        List<Task> tasks;
        Form1 f;
        public Job(Form1 f)
        {
            tasks = new List<Task>();
            this.f = f;
        }

        public Task addTask(int totalTaskSteps)
        {
            Task task = new Task(totalTaskSteps);
            tasks.Add(task);
            task.OnProcessEvent += new Task.ProcessEventHandler(HandlerEvent);
            return task;
        }

        protected void HandlerEvent(object sender, ProcessEventArgs e){
            f.action(getPercentage());
        }

        public float getPercentage()
        {
            if (tasks.Count == 0)
                return 0.0f;
            float c = 0.0f;
            foreach (Task task in tasks)
            {
                c += task.percent;
            }
            return c / tasks.Count;
        }
    }

    class Task
    {
        int total, current;

        public Task(int Total)
        {
            total = Total;
            current = 0;
        }

        public void process()
        {
            current++;
            if (current > total)
                throw new Exception("Process degree can not over 100%");
            OnRaiseProcessEvent(new ProcessEventArgs(percent));
        }

        public float percent
        {
            get
            {
                return current * 1.0f / total;
            }
        }

        public delegate void ProcessEventHandler(object sender, ProcessEventArgs e);

        public event ProcessEventHandler OnProcessEvent;

        protected virtual void OnRaiseProcessEvent(ProcessEventArgs e)
        {
            ProcessEventHandler handler = OnProcessEvent;

            if (handler != null)
            {
                handler(this, e);
            }
        }
    }

    public class ProcessEventArgs : EventArgs
    {
        private float percentage = 0.0f;
        public ProcessEventArgs(float percentage)
        {
            this.percentage = percentage;
        }
        public float Percentage
        {
            get
            {
                return percentage;
            }
        }
    }
}
