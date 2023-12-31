﻿using Ludo.Domain.Interfaces;
using Ludo.MediatRPattern.Interfaces;

namespace Ludo.Business.UseCases.Game.RollDiceUseCase
{
    public class RollDiceRequest : IRequest<(List<IPlayer>, int, string, bool)>
    {
        public string ConnectionId { get; set; }
        public int GameId { get; set; }
    }
}