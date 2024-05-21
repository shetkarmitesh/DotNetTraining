using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Task
    {
        public string Title { get; set; }
        public string Description { get; set; }


        public Task(string title, string description)
        {
            Title = title;
            Description = description;
        }
    }
    public class program
    {
        static List<Task> tasks = new List<Task>();

        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("\n** Task List Application **");
                Console.WriteLine("1. Create Task");
                Console.WriteLine("2. Read Tasks");
                Console.WriteLine("3. Update Task");
                Console.WriteLine("4. Delete Task");
                Console.WriteLine("5. Exit");

                Console.WriteLine("Enter your choice:");
                int choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        CreateTask();
                        break;
                    case 2:
                        ReadTasks();
                        break;
                    case 3:
                        UpdateTask();
                        break;
                    case 4:
                        DeleteTask();
                        break;
                    case 5:
                        Console.WriteLine("Exiting application...");
                        return;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
        }

        static void CreateTask()
        {
            Console.WriteLine("Enter task title:");
            string title = Console.ReadLine();

            Console.WriteLine("Enter task description :");
            string description = Console.ReadLine();

            tasks.Add(new Task(title, description));
            Console.WriteLine("Task added successfully!");
        }

        static void ReadTasks()
        {
            if (tasks.Count == 0)
            {
                Console.WriteLine("There are no tasks in the list.");
                return;
            }

            Console.WriteLine("** Task List **");
            int index = 1;

            Console.WriteLine("{0,-10} {1,-20} {2,-40}", "Index", "Title", "Description");
            Console.WriteLine("-------  --------------------  ---------------------------------------");
             
            foreach (Task task in tasks)
            {
                //Console.WriteLine($"{index}. {task.Title} ({task.Description})");
                Console.WriteLine($"{index,-10} {task.Title,-20} {task.Description,-40}", index, task.Title, task.Description);
                index++;
            }

            /*for (int i = 0; i < tasks.Count; i++)
            {
                Console.WriteLine($"{i+1}. {tasks[i].Title} ({tasks[i].Description})");

            }*/
        }

        static void UpdateTask()
        {
            if (tasks.Count == 0)
            {
                Console.WriteLine("There are no tasks to update.");
                return;
            }

            ReadTasks();

            Console.WriteLine("Enter the number of the task to update:");
            int taskIndex = int.Parse(Console.ReadLine()) - 1;

            if (taskIndex < 0 || taskIndex >= tasks.Count)
            {
                Console.WriteLine("Invalid task number.");
                return;
            }
            Console.WriteLine("Enter what to change \n1. Title \n2. Description");
            int choice = int.Parse(Console.ReadLine());
            if (choice == 1)
            {
                Console.WriteLine("Update title :");
                string newTitle = Console.ReadLine();
                if (!string.IsNullOrEmpty(newTitle))
                {
                    tasks[taskIndex].Title = newTitle;
                }

            }
            else
            {
                Console.WriteLine("Update description :");
                string newDescription = Console.ReadLine();
                if (!string.IsNullOrEmpty(newDescription))
                {
                    tasks[taskIndex].Description = newDescription;
                }

            }


            Console.WriteLine("Task updated successfully!");
        }

        static void DeleteTask()
        {
            if (tasks.Count == 0)
            {
                Console.WriteLine("There are no tasks to delete.");
                return;
            }

            ReadTasks();

            Console.WriteLine("Enter the number of the task to delete:");
            int taskIndex = int.Parse(Console.ReadLine()) - 1;

            if (taskIndex < 0 || taskIndex >= tasks.Count)
            {
                Console.WriteLine("Invalid task number.");
                return;
            }

            tasks.RemoveAt(taskIndex);
            Console.WriteLine("Task deleted successfully!");
        }
    }
}


