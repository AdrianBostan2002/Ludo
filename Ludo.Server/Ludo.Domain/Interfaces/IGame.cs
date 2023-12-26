﻿using Ludo.Domain.Entities;

namespace Ludo.Domain.Interfaces
{
    public interface IGame
    {
        int Id { get; set; } 
        Board? Board { get; set; }
        List<IPlayer>? Players { get; set; }
        Queue<string> RollDiceOrder {  get; set; }    
    }
}