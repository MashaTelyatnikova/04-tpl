using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherForecast
{
    public static class TasksExtentions
    {

        public static Task WhenAnyEceptEceptions(this Task[] tasks)
        {
            while (tasks.Count() != 0)
            {
                var completedTask = (Task<double>)Task.WhenAny(tasks).Result;
                if (completedTask.Status == TaskStatus.Faulted)
                {
                    tasks = tasks.Where(t => !t.Equals(completedTask)).ToArray();
                }
                else
                {
                    return completedTask;
                }
            }

            return null;
        }
    }
}
