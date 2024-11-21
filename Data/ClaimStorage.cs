using System.Collections.Generic;
using ClaimSystemMVC.Models;

namespace ClaimSystemMVC.Data
{
    public static class ClaimStorage
    {
        // In-memory storage for claims
        public static List<ClaimModel> Claims { get; set; } = new List<ClaimModel>();
    }
}
