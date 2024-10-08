﻿using GameZone.Models;
using GameZone.ViewModels;

namespace GameZone.Services
{
    public interface IGamesService
    {
        IEnumerable<Game> GetAll();

        Game? GetById(int id);
        Task Create(CreateGameFromViewModel gameViewModel);
        Task<Game?>  Update(EditGameFormViewModel editGameFormViewModel);

        bool Delete(int id);


    }
}
