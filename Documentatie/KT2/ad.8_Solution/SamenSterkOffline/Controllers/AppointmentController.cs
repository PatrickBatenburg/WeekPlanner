﻿using SamenSterkOffline.Models;
using SQLite;
using System.Collections.Generic;
using System.Linq;

namespace SamenSterkOffline.Controllers
{
    public class AppointmentController : ControllerBase
    {
        private Appointment appointment;

        /// <summary>
        /// Initializes a new instance of the AppointmentController class.
        /// </summary>
        public AppointmentController()
        {
            appointment = null;
        }

        /// <summary>
        /// View the details about all the appointments of the specified user.
        /// </summary>
        /// <param name="userId">User id of the appointment.</param>
        /// <returns>A list of appointments filled with values from the database.</returns>
        public List<Appointment> Details()
        {
            List<Appointment> appointments = new List<Appointment>();

            using (SQLiteConnection db = new SQLiteConnection(Program.DB_PATH))
            {
                List<Appointment> query = (from appointment in db.Table<Appointment>()
                                           select appointment).ToList();

                if (query != null)
                {
                    foreach (Appointment _appointment in query)
                    {
                        appointment = new Appointment()
                        {
                            Id = _appointment.Id,
                            Name = _appointment.Name,
                            Date = _appointment.Date
                        };

                        appointments.Add(appointment);
                    }
                }
            }

            return appointments;
        }

        /// <summary>
        /// Creates a new appointment.
        /// </summary>
        /// <param name="model">Appointment details to create.</param>
        /// <returns>0 on failure, 1 on success.</returns>
        public int Create(Appointment model)
        {
            int result = 0;

            using (SQLiteConnection db = new SQLiteConnection(Program.DB_PATH))
            {
                result = db.Insert(model);
            }

            return result;
        }

        /// <summary>
        /// Edit an existing appointment.
        /// </summary>
        /// <param name="model">Appointment details to edit.</param>
        /// <returns>0 on failure, 1 on success.</returns>
        public int Edit(Appointment model)
        {
            int result = 0;

            using (SQLiteConnection db = new SQLiteConnection(Program.DB_PATH))
            {
                result = db.Update(model);
            }

            return result;
        }

        /// <summary>
        /// Deletes an existing appointment.
        /// </summary>
        /// <param name="model">Appointment details to delete.</param>
        /// <returns>0 on failure, 1 on success.</returns>
        public int Delete(Appointment model)
        {
            int result = 0;

            using (SQLiteConnection db = new SQLiteConnection(Program.DB_PATH))
            {
                result = db.Delete(model);
            }

            return result;
        }
    }
}