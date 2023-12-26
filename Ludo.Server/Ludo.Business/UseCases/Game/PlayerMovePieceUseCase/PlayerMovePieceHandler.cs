using FluentValidation;
using Ludo.Domain.DTOs;
using Ludo.MediatRPattern.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ludo.Business.UseCases.Game.PlayerMovePieceUseCase
{
    public class PlayerMovePieceHandler : IRequestHandler<PlayerMovePieceRequest, List<PieceDto>>
    {


        public Task<List<PieceDto>> Handle(PlayerMovePieceRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
