using GameZone.Data;
using GameZone.Services;
using GameZone.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GameZone.Controllers
{
    public class GamesController : Controller
    { 
        private readonly ICategoriesService _categoriesServices;
        private readonly IDeveciesService _deveciesServices;
        private readonly IGamesService _gamesService;

        public GamesController(ICategoriesService categoriesServices,IDeveciesService deveciesServices,IGamesService gamesServices)
        { 
            _categoriesServices = categoriesServices;
            _deveciesServices = deveciesServices;
            _gamesService = gamesServices;
        }

        public IActionResult Index()
        {
            var games = _gamesService.GetAll();
            return View(games);
        }

        public IActionResult Details(int id)
        {
            var game=_gamesService.GetById(id);
            if(game==null)
                return NotFound();
            return View(game);
        }


        [HttpGet]
        public IActionResult Create()
        {
            CreateGameFromViewModel viewModel = new()
            {
                Categories = _categoriesServices.GetSelectedList(),
                Devices = _deveciesServices.GetDevices()
            };
            return View(viewModel);
        }


        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task <IActionResult> Create(CreateGameFromViewModel model)
        {
            if (!ModelState.IsValid) 
            {
                model.Categories =_categoriesServices.GetSelectedList();

                model.Devices =_deveciesServices.GetDevices(); 

                return View(model);
            }
            // Save Game in Db
            // Save Cover in Server

           await  _gamesService.Create(model);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var game = _gamesService.GetById(id);
            if (game == null)
                return NotFound();
            EditGameFormViewModel viewModel = new()
            {
                Id = id,
                Name = game.Name,
                Description = game.Description,
                CategoryId = game.CategoryId,
                SelectedDevices =game.Device.Select(d=>d.DeviceId).ToList(),
                Categories = _categoriesServices.GetSelectedList(),
                Devices= _deveciesServices.GetDevices(),
                CurrentCover = game.Cover,
            };
            return View(viewModel);
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Edit(EditGameFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = _categoriesServices.GetSelectedList();

                model.Devices = _deveciesServices.GetDevices();

                return View(model);
            }

             var game=await _gamesService.Update(model);

            if (game is null)
                return BadRequest();

            return RedirectToAction(nameof(Index));
        }


        [HttpDelete]
        public IActionResult Delete(int id)
        {
           
            var isDeleted =_gamesService.Delete(id);

            return isDeleted ? Ok() : BadRequest();
        }


    }
}
