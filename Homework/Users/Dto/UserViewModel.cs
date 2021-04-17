using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Homework.Users.Dto
{
    public class UserViewModel
    {
        public Guid Id { get; }

        public string Login { get; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public DateTime BirthDate { get; set; }

        public Gender Gender { get; set; }

        public string Interest { get; set; }

        public string City { get; set; }

        public UserViewModel() { }

        public UserViewModel(User user)
        {
            Id = user.Id;
            Login = user.Login;
            Name = user.Name;
            Surname = user.Surname;
            Gender = user.Gender;
            Interest = user.Interest;
            City = user.City;
            BirthDate = user.BirthDate;
        }

        public string GenderString
        {
            get
            {
                switch (Gender)
                {
                    case Gender.Male:
                        return "Мужской";
                    case Gender.Female:
                        return "Женский";

                    default:
                        return string.Empty;
                }
            }
        }

        public string BirthDateString => BirthDate == default ? string.Empty : BirthDate.ToShortDateString();
    }
}
