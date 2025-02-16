using Microsoft.AspNetCore.Mvc;
using RegisterAPII.Models;

namespace RegisterAPII.Models
{
    public class UserModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string Issue { get; set; } // المشكلة اللي بيواجهها المستخدم
    }
}