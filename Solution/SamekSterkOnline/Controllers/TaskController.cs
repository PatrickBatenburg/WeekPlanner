﻿using LinqToDB;
using SamenSterkOnline.MySQL;
using SamenSterkOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SamenSterkOnline.Controllers
{
    public class TaskController
    {
        private Task task = null;

        /// <summary>
        /// Initializes a new instance of the TaskController class.
        /// </summary>
        public TaskController()
        {
        }

        /// <summary>
        /// View the details about all the tasks of the specified user.
        /// </summary>
        /// <param name="userId">User id of the task.</param>
        /// <returns>A list of tasks filled with values from the database.</returns>
        public List<Task> Details(uint? userId)
        {
            List<Task> tasks = new List<Task>();

            if (userId == null)
            {
                tasks = null;
            }
            else
            {
				try
				{
					using (TaskDatabase db = new TaskDatabase())
					{
						List<Task> query = (from task in db.Tasks
											where task.UserId == userId
											select task).ToList();

						if (query != null)
						{
							foreach (Task _task in query)
							{
								task = new Task()
								{
									Id = _task.Id,
									UserId = _task.UserId,
									Title = _task.Title,
									Date = _task.Date,
									Duration = _task.Duration,
									Label = _task.Label
								};

								tasks.Add(task);
							}
						}
					}
				}
				catch (Exception)
				{
					tasks = null;
				}
			}

            return tasks;
        }

		/// <summary>
		/// Creates a new task.
		/// </summary>
		/// <param name="model">Task details to create.</param>
		/// <returns>0 on failure, 1 on success, 2 on unexpected database error.</returns>
		public int Create(Task model)
        {
            int result = 0;

			try
			{
				using (TaskDatabase db = new TaskDatabase())
				{
					result = db.Insert(model);
				}
			}
			catch (Exception)
			{
				result = 2;
			}

            return result;
        }

		/// <summary>
		/// Edit an existing task.
		/// </summary>
		/// <param name="model">Task details to edit.</param>
		/// <returns>0 on failure, 1 on success, 2 on unexpected database error.</returns>
		public int Edit(Task model)
        {
            int result = 0;

			try
			{
				using (TaskDatabase db = new TaskDatabase())
				{
					result = db.Update(model);
				}
			}
			catch (Exception)
			{
				result = 2;
			}

			return result;
        }

		/// <summary>
		/// Checks if an existing task exceeds another repeating task or task.
		/// </summary>
		/// <param name="model">Repeating task details to check on.</param>
		/// <returns>0 on nothing exceeds, 3 on DateTime exceeds, 2 on unexpected database error.</returns>
		public int Exceeds(Task model)
        {
            int result = 0;
            int[] totalrecords = new int[4];
            RepeatingTaskController taskController = new RepeatingTaskController();
            List<RepeatingTask> repeatingTasks = new List<RepeatingTask>();
            List<Task> tasks = new List<Task>();

			if (model != null)
			{
				repeatingTasks = taskController.Details(model.UserId);
				tasks = Details(model.UserId);
			}

			try
			{
				using (TaskDatabase db = new TaskDatabase())
				{
					List<RepeatingTask> repeatingTaskQuery = (from repeatingTask in repeatingTasks
															  where repeatingTask.Day == model.Date.ToString("dddd")
															  select repeatingTask).ToList();

					List<RepeatingTask> repeatingTaskDate = (from repeatingTask in repeatingTaskQuery
															 where repeatingTask.Time.Add(new TimeSpan(repeatingTask.Duration - 1, 0, 0)) < new TimeSpan(model.Date.Hour, 0, 0)
															 select repeatingTask).ToList();

					totalrecords[0] = repeatingTaskDate.Count;

					repeatingTaskDate = (from repeatingTask in repeatingTaskQuery
										 where repeatingTask.Time > new TimeSpan(model.Date.Hour + model.Duration - 1, 0, 0)
										 select repeatingTask).ToList();

					totalrecords[1] = repeatingTaskDate.Count;

					if (repeatingTaskQuery.Count != totalrecords[0] + totalrecords[1])
					{
						result = 3;
					}
					else
					{
						List<Task> taskQuery = (from task in tasks
												where task.Date.DayOfWeek == model.Date.DayOfWeek && task.Id != model.Id
												select task).ToList();

						List<Task> taskDate = (from task in taskQuery
											   where task.Date.AddHours(task.Duration - 1) < model.Date
											   select task).ToList();

						totalrecords[2] = taskDate.Count;

						taskDate = (from task in taskQuery
									where task.Date > model.Date.AddHours(model.Duration - 1)
									select task).ToList();

						totalrecords[3] = taskDate.Count;

						if (taskQuery.Count != totalrecords[2] + totalrecords[3])
						{
							result = 3;
						}
					}
				}
			}
			catch (Exception)
			{
				result = 2;
			}

            return result;
        }

		/// <summary>
		/// Deletes an existing task.
		/// </summary>
		/// <param name="model">Task details to delete.</param>
		/// <returns>0 on failure, 1 on success, 2 on unexpected database error.</returns>
		public int Delete(Task model)
        {
            int result = 0;

			try
			{
				using (TaskDatabase db = new TaskDatabase())
				{
					result = db.Delete(model);
				}
			}
			catch (Exception)
			{
				result = 2;
			}

            return result;
        }
    }
}