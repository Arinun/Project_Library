using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Project_Library.Areas.Identity.Data;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    public string FisrtName { get; set; }
    public string LastName { get; set; }
    public int IDCard { get; set; }
    public string Address { get; set; }
    public DateTime BirthDay { get; set; }
    public string Phone { get; set; }
}

