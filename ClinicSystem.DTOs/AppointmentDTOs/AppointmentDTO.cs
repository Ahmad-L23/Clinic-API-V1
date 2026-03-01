using System;

namespace ClinicSystem.DTOs.AppointmentDTOs
{
    public class AppointmentDTO
    {
        public int AppointmentID { get; }
        public int PatientID { get; }
        public int DoctorID { get; }
        public DateTime AppointmentDateTime { get; }
        public byte AppointmentStatus { get; }

        public int? MedicalRecordID { get; }
        public int? PaymentID { get; }

        // Medical Record Info
        public string? VisitDescription { get; }
        public string? Diagnosis { get; }
        public string? AdditionalNotes { get; }

        // Person / Doctor Info (from View)
        public string? PatientName { get; }
        public string? Specialization { get; }

        public AppointmentDTO(
            int appointmentID,
            int patientID,
            int doctorID,
            DateTime appointmentDateTime,
            byte appointmentStatus,
            int? medicalRecordID,
            int? paymentID,
            string? visitDescription,
            string? diagnosis,
            string? additionalNotes,
            string? patientName,
            string? specialization)
        {
            AppointmentID = appointmentID;
            PatientID = patientID;
            DoctorID = doctorID;
            AppointmentDateTime = appointmentDateTime;
            AppointmentStatus = appointmentStatus;
            MedicalRecordID = medicalRecordID;
            PaymentID = paymentID;
            VisitDescription = visitDescription;
            Diagnosis = diagnosis;
            AdditionalNotes = additionalNotes;
            PatientName = patientName;
            Specialization = specialization;
        }
    }
}