﻿using Ludo.Domain.Interfaces;

namespace Ludo.Domain.Entities
{
    public class Game : IGame
    {
        public int Id { get; set; }
        public List<IPlayer>? Players { get; set; }
        public Board? Board { get; set; }
    }
}