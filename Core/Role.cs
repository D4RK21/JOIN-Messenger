using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Core
{
    public class Role : BaseEntity
    {
        public Role()
        {
            RoleName = "New Role";
            Permissions = new Dictionary<string, bool>();
            CanPin = false;
            CanInvite = false;
            CanDeleteOthersMessages = false;
            CanModerateParticipants = false;
            CanManageRoles = false;
            CanManageChannels = false;
            CanManageRoom = false;
            CanUseAdminChannels = false;
            CanViewAuditLog = false;
        }
        
        public Role(string name = "New Role")
        {
            RoleName = name;
            Permissions = new Dictionary<string, bool>();
            CanPin = false;
            CanInvite = false;
            CanDeleteOthersMessages = false;
            CanModerateParticipants = false;
            CanManageRoles = false;
            CanManageChannels = false;
            CanManageRoom = false;
            CanUseAdminChannels = false;
            CanViewAuditLog = false;
        }

        [JsonIgnore]
        [NotMapped]
        public IDictionary<string, bool> Permissions { get; set; }

        public string RoleName { get; set; }

        public bool CanPin
        {
            get => Permissions["CanPin"];
            set => Permissions["CanPin"] = value;
        }

        public bool CanInvite
        {
            get => Permissions["CanInvite"];
            set => Permissions["CanInvite"] = value;
        }


        public bool CanDeleteOthersMessages
        {
            get => Permissions["CanDeleteOthersMessages"];
            set => Permissions["CanDeleteOthersMessages"] = value;
        }

        public bool CanModerateParticipants
        {
            get => Permissions["CanModerateParticipants"];
            set => Permissions["CanModerateParticipants"] = value;
        }

        public bool CanManageRoles
        {
            get => Permissions["CanManageRoles"];
            set => Permissions["CanManageRoles"] = value;
        }

        public bool CanManageChannels
        {
            get => Permissions["CanManageChannels"];
            set => Permissions["CanManageChannels"] = value;
        }

        public bool CanManageRoom
        {
            get => Permissions["CanManageRoom"];
            set => Permissions["CanManageRoom"] = value;
        }


        public bool CanUseAdminChannels
        {
            get => Permissions["CanUseAdminChannels"];
            set => Permissions["CanUseAdminChannels"] = value;
        }

        public bool CanViewAuditLog
        {
            get => Permissions["CanViewAuditLog"];
            set => Permissions["CanViewAuditLog"] = value;
        }
    }
}
