using Ludo.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ludo.Business.UseCases.PostTestUseCase
{
    public class PostTestRequest: IRequest<PostTestResponse>
    {
        public string Person { get; set; }
    }
}
