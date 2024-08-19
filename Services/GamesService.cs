using GameZone.Data;
using GameZone.Models;
using GameZone.Settings;
using GameZone.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace GameZone.Services
{
    public class GamesService : IGamesService  
    {

        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment ;
        private readonly string _imgPath;

        public GamesService(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _imgPath = $"{_webHostEnvironment.WebRootPath}{FileSettings.imgPath}";
        }

        // Save Cover in Server
        public async Task Create(CreateGameFromViewModel gameViewModel)
        {
            var coveName = await SaveCover(gameViewModel.Cover);

            Game game = new()
            {
                Name = gameViewModel.Name,
                Description = gameViewModel.Description,
                CategoryId = gameViewModel.CategoryId,
                Cover = coveName,
                Device = gameViewModel.SelectedDevices.Select(d => new GameDevice { DeviceId = d }).ToList()
            };

            _context.Add(game);
            _context.SaveChanges();
        }


        public IEnumerable<Game> GetAll()
        {
            var games = _context.Games.Include(g=>g.Category)
                                      .Include(g=>g.Device)
                                      .ThenInclude(d=>d.Device)
                                      .AsNoTracking()
                                      .ToList(); 
            return games;
        }

        public Game? GetById(int id)
        {
           return _context.Games.Include(g => g.Category)
                                         .Include(g => g.Device)
                                         .ThenInclude(d => d.Device)
                                         .AsNoTracking()
                                         .SingleOrDefault(g=>g.Id ==id);
        }

        public async Task<Game?> Update(EditGameFormViewModel editGameFormViewModel)
        {
            var game = _context.Games
                .Include(g=>g.Device)
                .SingleOrDefault(g=>g.Id ==editGameFormViewModel.Id);
            if (game == null)
                return null;

            var hasNewCover= editGameFormViewModel.Cover != null;
            var oldCover=game.Cover;
            game.Name= editGameFormViewModel.Name;
            game.Description= editGameFormViewModel.Description;
            game.CategoryId = editGameFormViewModel.CategoryId;
            game.Device=editGameFormViewModel.SelectedDevices.Select(d=>new GameDevice { DeviceId=d}).ToList();

            if (hasNewCover)
            {
                game.Cover= await SaveCover(editGameFormViewModel.Cover!);
            }

            var effectedRows = _context.SaveChanges();
            if (effectedRows > 0)
            {
                if (hasNewCover)
                {
                    var cover = Path.Combine(_imgPath,oldCover);
                    File.Delete(cover);
                }
                return game;
            }
            else
            {
                var cover = Path.Combine(_imgPath, game.Cover);
                File.Delete(cover);
                return null;
            }

        }
        public bool Delete(int id)
        {
           var isDeleted=false;
            
            var game= _context.Games.Find(id);

            if (game is null)
            {
                return isDeleted;
            }
            _context.Games.Remove(game);

            var effectedRows= _context.SaveChanges();
            if (effectedRows > 0)
            {
                isDeleted = true;
                var cover=Path.Combine(_imgPath,game.Cover);
                File.Delete(cover);
            }

            return isDeleted;
        }



        private async Task<string> SaveCover(IFormFile cover)
        {
            var coveName = $"{Guid.NewGuid()}{Path.GetExtension(cover.FileName)}";

            var path = Path.Combine(_imgPath, coveName);

            using var stream = File.Create(path);

            await cover.CopyToAsync(stream);

            return coveName;
        }



    }
    
}
