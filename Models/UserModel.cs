using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClaimSystemMVC.Models
{
    public class UserModel
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
    }
    public static class UserStorage
    {
        public static List<UserModel> Users { get; set; } = new List<UserModel>();
    }


}