﻿namespace Ludo.Domain.Interfaces
{
    public interface IMediator
    {
        Task <TResponse> Send<TResponse>(IRequest<TResponse> entity);
    }
}