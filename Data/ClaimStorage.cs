using System.Collections.Generic;
using ClaimSystemMVC.Models;

namespace ClaimSystemMVC.Data
{
    public static class ClaimStorage
    {
        public static List<ClaimModel> Claims { get; set; } = new List<ClaimModel>();
        public static List<Lecturer> Lecturers { get; set; } = new List<Lecturer>(); // This should be here
    }
}
