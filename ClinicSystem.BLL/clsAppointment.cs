using ClinicSystem.DAL;
using ClinicSystem.DTOs.AppointmentDTOs;
using System;
using System.Collections.Generic;

namespace ClinicSystem.BLL
{
    public class clsAppointment
    {
        public enum enMode { Add = 0, Update = 1 }

        public enMode Mode = enMode.Add;

        public int? AppointmentID { get; set; }
        public int PatientID { get; set; }
        public int DoctorID { get; set; }
        public DateTime AppointmentDateTime { get; set; }
        public byte AppointmentStatus { get; set; }
        public int? MedicalRecordID { get; set; }
        public int? PaymentID { get; set; }

        // DTO Helper
        private AppointmentAddUpdateDTO ADto =>
            new AppointmentAddUpdateDTO(
                AppointmentID,
                PatientID,
                DoctorID,
                AppointmentDateTime,
                AppointmentStatus,
                MedicalRecordID,
                PaymentID
            );

        // Constructor
        public clsAppointment(AppointmentAddUpdateDTO dto, enMode mode = enMode.Add)
        {
            AppointmentID = dto.AppointmentID;
            PatientID = dto.PatientID;
            DoctorID = dto.DoctorID;
            AppointmentDateTime = dto.AppointmentDateTime;
            AppointmentStatus = dto.AppointmentStatus;
            MedicalRecordID = dto.MedicalRecordID;
            PaymentID = dto.PaymentID;
            Mode = mode;
        }

        /* =========================================================
           PRIVATE METHODS
        ========================================================= */

        private bool _AddAppointment()
        {
            AppointmentID = clsAppointmentsData.AddAppointment(ADto);
            return AppointmentID.HasValue && AppointmentID.Value > 0;
        }

        private bool _UpdateAppointment()
        {
            return clsAppointmentsData.UpdateAppointment(ADto);
        }

        /* =========================================================
           SAVE
        ========================================================= */

        public bool Save()
        {
            return Mode switch
            {
                enMode.Add => _AddAppointment(),
                enMode.Update => _UpdateAppointment(),
                _ => false
            };
        }

        /* =========================================================
           STATIC METHODS
        ========================================================= */

        public static (List<AppointmentDTO> Data, int TotalRecords, int TotalPages)
            GetAllAppointments(int pageNumber = 1, int pageSize = 10) =>
            clsAppointmentsData.GetAllAppointments(pageNumber, pageSize);


        public static List<AppointmentDTO> GetAppointmentsByPatientID(int patientId) =>
            clsAppointmentsData.GetAppointmentsByPatientID(patientId);


        public static List<AppointmentDTO> GetAppointmentsByDoctorID(int doctorId) =>
            clsAppointmentsData.GetAppointmentsByDoctorID(doctorId);


        public static bool DeleteAppointment(int appointmentId) =>
            clsAppointmentsData.DeleteAppointment(appointmentId);


        public static bool AppointmentExists(int doctorId, DateTime dateTime) =>
            clsAppointmentsData.AppointmentExists(doctorId, dateTime);


        public static clsAppointment? Find(int appointmentId)
        {
            AppointmentDTO? dto = clsAppointmentsData.GetAppointmentByID(appointmentId);

            if (dto == null)
                return null;

            AppointmentAddUpdateDTO addUpdateDto =
                new AppointmentAddUpdateDTO(
                    dto.AppointmentID,
                    dto.PatientID,
                    dto.DoctorID,
                    dto.AppointmentDateTime,
                    dto.AppointmentStatus,
                    dto.MedicalRecordID,
                    dto.PaymentID
                );

            return new clsAppointment(addUpdateDto, enMode.Update);
        }
    }
}