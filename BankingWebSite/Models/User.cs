using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingWebSite.Models
{
	public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [DataType(DataType.Date)]
        public DateTime Birthday { get; set; }
        public string Address { get; set; }

        public UserAccount UserAccount { get; set; }

        public User(UserViewModel userViewModel)
        {
            Id = userViewModel.Id;
            FirstName = userViewModel.FirstName;
            LastName = userViewModel.LastName;
            Birthday = userViewModel.Birthday;
            Address = userViewModel.Address;
        }
    }

    public class UserViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [DataType(DataType.Date)]
        public DateTime Birthday { get; set; }
        public string Address { get; set; }

        public UserViewModel(User user)
        {
            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Birthday = user.Birthday;
            Address = user.Address;
        }
    }
}
