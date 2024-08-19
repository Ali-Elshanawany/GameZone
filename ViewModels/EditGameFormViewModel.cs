using GameZone.Attributes;
using GameZone.Settings;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace GameZone.ViewModels
{
    public class EditGameFormViewModel : GameFormViewModel
    {
        public string? CurrentCover {get;set;}
        public int Id { get; set; }

        [AllowedExtensionAttributes(FileSettings.allowedExtentions), MaxSize(FileSettings.maxFileSizeInBytes)]
        public IFormFile? Cover { get; set; } = default!;

    }
}
