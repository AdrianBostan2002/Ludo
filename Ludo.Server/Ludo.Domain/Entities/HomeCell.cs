﻿using Ludo.Domain.Enums;
using Ludo.Domain.Interfaces;

namespace Ludo.Domain.Entities
{
    public class HomeCell : ICell
    {
        public List<Piece> Pieces { get; set; }

        public ColorType Color { get; set; }
    }
}