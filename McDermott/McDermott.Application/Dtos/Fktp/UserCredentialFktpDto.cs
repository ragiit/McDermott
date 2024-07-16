using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Dtos.Fktp
{
    public record UserCredentialFktpDto : IMapFrom<UserCredentialFktp>
    {
        public string Username { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
    }

}
