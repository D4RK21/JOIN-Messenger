using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using Newtonsoft.Json;

namespace Core
{
    public class Room : BaseEntity
    {
        [NotMapped]
        public static Expression<Func<Room, Room>> Selector { get; } = q => new Room()
        {
            Participants = q.Participants,
            Photo = q.Photo,
            BaseRole = q.BaseRole,
            PhotoSource = q.PhotoSource,
            RoomDescription = q.RoomDescription,
            RoomName = q.RoomName,
            RoomRoles = q.RoomRoles,
            TextChannels = q.TextChannels,
            Id = q.Id
        };
        public string RoomName { get; set; }

        public string RoomDescription { get; set; }

        public IList<ParticipantInfo> Participants { get; set; }

        public IList<Role> RoomRoles { get; set; }

        public IList<TextChannel> TextChannels { get; set; }

        public Role BaseRole { get; set; }

        [JsonIgnore]
        [NotMapped]
        public ReadOnlyCollection<byte> Photo { get; set; }

        public string PhotoSource
        {
            get
            {
                if (this.Photo != null)
                {
                    return Convert.ToBase64String(this.Photo.ToArray());
                }

                return string.Empty;
            }
            set { this.Photo = Array.AsReadOnly(Convert.FromBase64String(value)); }
        }
    }
}
