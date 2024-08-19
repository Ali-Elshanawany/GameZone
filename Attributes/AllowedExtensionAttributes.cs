using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace GameZone.Attributes
{
    public class AllowedExtensionAttributes :ValidationAttribute
    {
        private readonly string _allow;

        public AllowedExtensionAttributes(string allowedExtension)
        {
            _allow = allowedExtension;
        }
        protected override ValidationResult? IsValid
            (object? value, ValidationContext validationContext)
        {
           var file=value as IFormFile;
            if (file != null) 
            {
                var extension=Path.GetExtension(file.FileName);

                var isAllowed = _allow.Split(",").Contains(extension,StringComparer.OrdinalIgnoreCase);

                if (!isAllowed)
                {
                    return new ValidationResult($"Only {_allow} are Allowed.");

                }


            }

            return ValidationResult.Success;
        }

    }
}
