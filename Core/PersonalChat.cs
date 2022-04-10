using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;

namespace Core
{
    public class PersonalChat : BaseEntity
    {
        [NotMapped]
        public static Expression<Func<PersonalChat, PersonalChat>> Selector { get; } = q => new PersonalChat()
        {
            Participants = q.Participants,
            ChatName = q.ChatName,
            Id = q.Id
        };
        public string ChatName { get; set; }

        public IList<UsersPersonalChats> Participants { get; set; }
    }
}
