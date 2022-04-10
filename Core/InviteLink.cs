using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;

namespace Core
{
    public class InviteLink : BaseEntity
    {
        [NotMapped]
        public static Expression<Func<InviteLink, InviteLink>> Selector { get; } = q => new InviteLink()
        {
            Room = q.Room,
            Url = q.Url,
            User = q.User,
            ExpirationTime = q.ExpirationTime,
            IsUsed = q.IsUsed,
            Id = q.Id
        };


        public string Url { get; set; }
        
        public string ExpirationTime { get; set; }

        public List<InviteLinksUsers> User { get; set; }

        public Room Room { get; set; }
        
        public bool IsUsed { get; set; }
    }
}
