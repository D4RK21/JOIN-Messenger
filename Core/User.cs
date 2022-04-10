using System;
using System.Collections.Generic;
using System.Drawing;

namespace Core
{
    public class User : BaseEntity
    {
        public string UserName { get; set; }
        
        public string Email { get; set; }
        
        public string Password { get; set; }
        
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        
        public bool IsVerified { get; set; }
        
        public DateTime LastAuth { get; set; }
    }
}
