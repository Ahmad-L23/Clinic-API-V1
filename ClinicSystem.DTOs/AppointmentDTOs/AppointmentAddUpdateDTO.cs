using System;

namespace ClinicSystem.DTOs.AppointmentDTOs
{
    public class AppointmentAddUpdateDTO
    {
        public int? AppointmentID { get; set; }  // Nullable for Add
        public int PatientID { get; set; }
        public int DoctorID { get; set; }
        public DateTime AppointmentDateTime { get; set; }
        public byte AppointmentStatus { get; set; }
        public int? MedicalRecordID { get; set; }
        public int? PaymentID { get; set; }

        public AppointmentAddUpdateDTO(
            int? appointmentID,
            int patientID,
            int doctorID,
            DateTime appointmentDateTime,
            byte appointmentStatus,
            int? medicalRecordID,
            int? paymentID)
        {
            AppointmentID = appointmentID;
            PatientID = patientID;
            DoctorID = doctorID;
            AppointmentDateTime = appointmentDateTime;
            AppointmentStatus = appointmentStatus;
            MedicalRecordID = medicalRecordID;
            PaymentID = paymentID;
        }
    }
}