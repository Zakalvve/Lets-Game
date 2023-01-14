using System;
using System.ComponentModel.DataAnnotations;

namespace LetsGame.Data.Data_Annotations
{
    public class FutureDateAttribute : ValidationAttribute
    {
        public FutureDateAttribute() { }

        public override bool IsValid(object? value) {
            if (value == null) return false;
            DateTime input = (DateTime)value;
            if (input > DateTime.Now) {
                return true;
            }
            return false;
        }
    }
}
