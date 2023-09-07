using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.DataBase.User
{
    public class UserDto : IDTO
    {
		public string Firstname { get; set; }

		public string Lastname { get; set; }

		public string Username { get; set; }

		public string Address { get; set; }

		public DateTime Birthdate { get; set; }

		public string Email { get; set; }
	}
}
