using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.DataBase.Auth
{
    public class RegDto : IDTO
    {
		public string Firstname { get; set; }

		public string Lastname { get; set; }

		public string Username { get; set; }

		public string Email { get; set; }

		public string Password { get; set; }

		public string Address { get; set; }

		public string Role { get; set; }

		public IFormFile ProfileImage { get; set; }

		public DateTime DateOfBirth { get; set; }
	}
}
