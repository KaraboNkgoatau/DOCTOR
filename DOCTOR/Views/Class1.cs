using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DOCTOR.ViewModel
{
    public class Class1
    {
        public class Prescription
        {
            public string Prescription_Date { get; set; }
            public string PrescriptionLines { get; set; }
            //public string lastName { get; set; }
        }
        public class RootObject
        {
            public List<Prescription> Lectures { get; set; }
        }
    }
}