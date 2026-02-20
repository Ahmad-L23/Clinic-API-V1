using ClinicSystem.DAL;
using ClinicSystem.DTOs.PatientDTOs;
using System;
using System.Collections.Generic;

namespace ClinicSystem.BLL
{
    public class clsPatient
    {
        public enum enMode { Add = 0, Update = 1 }

        public enMode Mode = enMode.Add;

        public int? PatientID { get; set; }
        public int PersonID { get; set; }

        // DTO helper
        private PatientAddUpdateDTO PDto => new PatientAddUpdateDTO(PatientID, PersonID);

        // Constructor
        public clsPatient(PatientAddUpdateDTO PDto, enMode mode = enMode.Add)
        {
            this.PersonID = PDto.PersonID;
            this.PatientID = PDto.PatientID;
            this.Mode = mode;
        }



        // Private add method
        private bool _AddPatient()
        {

            PatientID = clsPatientsData.AddPatient(PDto);
            return  (PatientID.Value != -1);
        }

        // Private update method
        private bool _UpdatePatient()
        {
            
            return clsPatientsData.UpdatePatient(PDto);
        }

        // Save method
        public bool Save()
        {
            return Mode switch
            {
                enMode.Add => _AddPatient(),
                enMode.Update => _UpdatePatient(),
                _ => false
            };
        }

        // Static methods
        public static List<PatientDTO> GetAllPatients(int pageNumber = 1, int pageSize = 10) =>
            clsPatientsData.GetAllPatients(pageNumber, pageSize);

        public static bool DeletePatient(int id) =>
            clsPatientsData.DeletePatient(id);

        public static clsPatient? Find(int patientId)
        {
            PatientAddUpdateDTO? patient = clsPatientsData.GetPatientByID(patientId);
            if (patient != null)
                return new clsPatient(patient, enMode.Update);

            return null;
        }
    }
}