﻿using Refit;
using Benefit.Service.APIs.Imdb.DTO;

namespace Benefit.Service.APIs.Imdb;

public interface IImdbApiClient
{
    [Get("/en/API/SearchName/{key}/{name}")]
    Task<ImdbPerson> GetPerson(string key, string name);
}