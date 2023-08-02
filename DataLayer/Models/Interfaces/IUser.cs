using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models.Interfaces
{
    public interface IUser
    {
		long Id { get; set; }

		string Firstname { get; set; }

		string Lastname { get; set; }

		string Username { get; set; }

		string Email { get; set; }

		string Password { get; set; }

		string Address { get; set; }

		string Image { get; set; }

		public DateTime DateOfBirth { get; set; }
	}
}
