using ClinicSystem.DAL;
using ClinicSystem.DTOs.DoctorDTOs;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace ClinicSystem.BLL
{
    public class clsDoctor
    {
        public enum enMode {Add = 0, Update = 1}

        public int DoctorId {  get; set; }
        public int PersonId {  get; set; }
        public string ?Specialization {  get; set; }

        enMode Mode = enMode.Add;

        private DoctorAddUpdateDTO DDto
        {
            get
            {
                return new DoctorAddUpdateDTO(this.DoctorId, this.PersonId, this.Specialization);
            }
        }
        public clsDoctor(DoctorAddUpdateDTO DoctorDto, enMode mode = enMode.Add)
        {
            this.DoctorId = DoctorDto.PersonID;
            this.PersonId = DoctorDto.PersonID;
            this.Specialization = DoctorDto.Specialization;
            this.Mode = mode; 
        }

        public bool Add()
        {
            this.PersonId = clsDoctorsData.AddDoctor(DDto);

            return PersonId != -1;
        }

        public bool Update()
        {
            return clsDoctorsData.UpdateDoctor(DDto);
        }

        public static List<DoctorDTO>? GetAll() =>
            clsDoctorsData.GetAllDoctors();

        public static bool DeleteDoctor(int DoctorId) =>
            clsDoctorsData.DeleteDoctor(DoctorId);


        public static DoctorDTO? Find(int Doctorid) =>
            clsDoctorsData.GetDoctorByID(Doctorid);



    }
}
