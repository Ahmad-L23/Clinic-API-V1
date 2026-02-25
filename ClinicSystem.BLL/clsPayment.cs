using ClinicSystem.DAL;
using ClinicSystem.DTOs.PaymentDTOs;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace ClinicSystem.BLL
{
    public class clsPayment
    {
        public enum enMode { Add=0, Update=1 }
        public enMode mode = enMode.Add; 
        public int? PaymentId{ get; set; }
        public DateTime PaymentDate {  get; set; }
        public string PaymentMethod {  get; set; }
        public double AmountPaid {  get; set; }
        public string? AdditionalNotes {  get; set; }

        private PaymentAddUpdateDTO Pdto { get { return new PaymentAddUpdateDTO(this.PaymentId, this.PaymentDate, this.PaymentMethod, this.AmountPaid, this.AdditionalNotes); } }

        public clsPayment(PaymentAddUpdateDTO Pdto, enMode mode = enMode.Add)
        {
            this.PaymentId = Pdto.PaymentID;
            this.PaymentDate = Pdto.PaymentDate;
            this.PaymentMethod = Pdto.PaymentMethod;
            this.AmountPaid = Pdto.AmountPaid;
            this.AdditionalNotes = Pdto.AdditionalNotes;
        }

        private bool _Add()
        {
            this.PaymentId = clsPaymentsData.AddPayment(Pdto);

            return PaymentId != -1;
        }


        private bool _Update()
        {
            return clsPaymentsData.UpdatePayment(Pdto);
        }
        public bool Save()
        {
            switch(mode)
            {
                case enMode.Add:
                    return _Add();

                case enMode.Update:
                    return _Update();
            }

            return false;
        }




       public static List<PaymentDTO> GetAll()
        => clsPaymentsData.GetAllPayments();

        
        public static bool DeletePayment(int PaymentId) =>
            clsPaymentsData.DeletePayment(PaymentId);



        public static PaymentDTO? Find(int PaymentId)
            => clsPaymentsData.GetPaymentByID(PaymentId);
    }


}
