﻿using LinqToDB;
using SamenSterkOnline.MySQL;
using SamenSterkOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SamenSterkOnline.Controllers
{
    public class AppointmentController
    {
        private Appointment appointment = null;

        /// <summary>
        /// Initializes a new instance of the AppointmentController class.
        /// </summary>
        public AppointmentController()
        {
        }

        /// <summary>
        /// View the details about all the appointments of the specified user.
        /// </summary>
        /// <param name="userId">User id of the appointment.</param>
        /// <returns>A list of appointments filled with values from the database.</returns>
        public List<Appointment> Details(uint? userId)
        {
            List<Appointment> appointments = new List<Appointment>();

            if (userId == null)
            {
                appointments = null;
            }
            else
            {
				try
				{
					using (TaskDatabase db = new TaskDatabase())
					{
						List<Appointment> query = (from appointment in db.Appointments
												   where appointment.UserId == userId
												   select appointment).ToList();

						if (query != null)
						{
							foreach (Appointment _appointment in query)
							{
								appointment = new Appointment()
								{
									Id = _appointment.Id,
									UserId = _appointment.UserId,
									Name = _appointment.Name,
									Date = _appointment.Date
								};

								appointments.Add(appointment);
							}
						}
					}
				}
				catch (Exception)
				{
					appointments = null;
				}
            }

            return appointments;
        }

		/// <summary>
		/// Creates a new appointment.
		/// </summary>
		/// <param name="model">Appointment details to create.</param>
		/// <returns>0 on failure, 1 on success, 2 on unexpected database error.</returns>
		public int Create(Appointment model)
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
		/// Edit an existing appointment.
		/// </summary>
		/// <param name="model">Appointment details to edit.</param>
		/// <returns>0 on failure, 1 on success, 2 on unexpected database error.</returns>
		public int Edit(Appointment model)
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
		/// Deletes an existing appointment.
		/// </summary>
		/// <param name="model">Appointment details to delete.</param>
		/// <returns>0 on failure, 1 on success, 2 on unexpected database error.</returns>
		public int Delete(Appointment model)
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