using DataLayer.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class User : IUser
    {
		
			public long Id { get; set; }

			public string Firstname { get; set; }

			public string Lastname { get; set; }

			public string Username { get; set; }

			public string Email { get; set; }

			public string Password { get; set; }

			public string Address { get; set; }

			public string Image { get; set; }

			public DateTime DateOfBirth { get; set; }
		}
	}


