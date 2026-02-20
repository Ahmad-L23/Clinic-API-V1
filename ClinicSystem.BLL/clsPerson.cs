using ClinicSystem.DAL;
using ClinicSystem.DTOs.PersonDTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace ClinicSystem.BLL
{
    public  class clsPerson
    {

        public enum enMode { Add=0, Update=1};
        
        public enMode Mode = enMode.Add;
        public int PersonID {  get; set; }
        public string Name { get; set; }
        public DateTime? DateOfBirth {  get; set; }
        public bool? Gender {  get; set; }
        public string PhoneNumber {  get; set; } 
        public string? Email {  get; set; }
        public string? Address {  get; set; }


        private PersonDTO PDto
        {
            get
            {
                // Create and return a new PersonDTO
                return new PersonDTO(
                    PersonID: PersonID,
                    Name:Name,
                    DateOfBirth:DateOfBirth,
                    Gender: Gender,
                    PhoneNumber: PhoneNumber,
                    Email: Email,
                    Address: Address
                );
            }
        }

        public clsPerson(PersonDTO PersonDto, enMode Mode = enMode.Add)
        {
            this.PersonID = PersonDto.PersonID;
            this.Name = PersonDto.Name;
            this.DateOfBirth = PersonDto.DateOfBirth;
            this.Gender = PersonDto.Gender;
            this.PhoneNumber = PersonDto.PhoneNumber;
            this.Email = PersonDto.Email;
            this.Address = PersonDto.Address;
            this.Mode = Mode;
        }


        private bool _AddPerson()
        {
            PersonID = clsPersonData.AddPerson(PDto);

            return (PersonID != -1) ;
        }

        private bool _UpdatePrson()
        {
            return clsPersonData.UpdatePerson(PDto);
        }


        public bool Save()
        {
            switch(Mode)
            {
                case enMode.Add:
                    return _AddPerson();

                case enMode.Update:
                    return _UpdatePrson();
            }
            return false;
        }

        public static List<PersonDTO>GetAllPersons() =>
             clsPersonData.GetAllPersons();
        
        public static bool DeletePerson(int id) =>
             clsPersonData.DeletePerson(id);

        public static clsPerson? Find(int id)
        {
            PersonDTO? Person = clsPersonData.GetPersonByID(id);
           
            if(Person != null)
            {
                return new clsPerson(Person, enMode.Update);
            }

            return null;
        }
    }
}
