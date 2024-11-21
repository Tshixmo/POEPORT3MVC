using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClaimSystemMVC.Models;

namespace ClaimSystemMVC.Data
{
    public class ClaimStorage
    {
        // Temporary in-memory list to store claims
        public static List<ClaimModel> Claims = new List<ClaimModel>();
    }
}