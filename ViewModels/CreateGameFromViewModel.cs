using GameZone.Attributes;
using GameZone.Settings;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace GameZone.ViewModels
{
    public class CreateGameFromViewModel:GameFormViewModel
    {

      

        [AllowedExtensionAttributes(FileSettings.allowedExtentions),MaxSize(FileSettings.maxFileSizeInBytes)]
        public IFormFile Cover { get; set; } = default!;

    }
}
