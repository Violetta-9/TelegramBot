using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Telegram.Application.Contracts;

namespace Telegram.Application.Queries
{
     public class GetCity:IRequest<CityInfo>
    {
        public string Name { get; set; }

        public GetCity(string name)
        {
            Name = name;
        }
    }
}
