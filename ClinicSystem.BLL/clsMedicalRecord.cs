using ClinicSystem.DAL;
using ClinicSystem.DTOs.MedicalRecordDTOs;
using System;
using System.Collections.Generic;

namespace ClinicSystem.BLL
{
    public class clsMedicalRecord
    {
        public enum enMode { Add = 0, Update = 1 }

        public enMode Mode = enMode.Add;

        public int? MedicalRecordID { get; set; }
        public string? VisitDescription { get; set; }
        public string? Diagnosis { get; set; }
        public string? AdditionalNotes { get; set; }

        // DTO Helper
        private MedicalRecordAddUpdateDTO MDto =>
            new MedicalRecordAddUpdateDTO(
                MedicalRecordID,
                VisitDescription,
                Diagnosis,
                AdditionalNotes
            );

        // Constructor
        public clsMedicalRecord(MedicalRecordAddUpdateDTO dto, enMode mode = enMode.Add)
        {
            this.MedicalRecordID = dto.MedicalRecordID;
            this.VisitDescription = dto.VisitDescription;
            this.Diagnosis = dto.Diagnosis;
            this.AdditionalNotes = dto.AdditionalNotes;
            this.Mode = mode;
        }

        // Private Add
        private bool _AddMedicalRecord()
        {
            MedicalRecordID = clsMedicalRecordsData.AddMedicalRecord(MDto);
            return (MedicalRecordID.Value != -1);
        }

        // Private Update
        private bool _UpdateMedicalRecord()
        {
            return clsMedicalRecordsData.UpdateMedicalRecord(MDto);
        }

        // Save
        public bool Save()
        {
            return Mode switch
            {
                enMode.Add => _AddMedicalRecord(),
                enMode.Update => _UpdateMedicalRecord(),
                _ => false
            };
        }

        // Static Methods

        public static List<MedicalRecordDTO> GetAllMedicalRecords(int pageNumber = 1, int pageSize = 10) =>
            clsMedicalRecordsData.GetAllMedicalRecords(pageNumber, pageSize);

        public static bool DeleteMedicalRecord(int id) =>
            clsMedicalRecordsData.DeleteMedicalRecord(id);

        public static clsMedicalRecord? Find(int medicalRecordId)
        {
            MedicalRecordDTO? dto = clsMedicalRecordsData.GetMedicalRecordByID(medicalRecordId);

            if (dto != null)
            {
                var addUpdateDto = new MedicalRecordAddUpdateDTO(
                    dto.MedicalRecordID,
                    dto.VisitDescription,
                    dto.Diagnosis,
                    dto.AdditionalNotes
                );

                return new clsMedicalRecord(addUpdateDto, enMode.Update);
            }

            return null;
        }
    }
}