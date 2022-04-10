namespace Core
{
    public class ParticipantInfo : BaseEntity

    {
        public User User { get; set; }

        public Role Role { get; set; }

        public bool Notifications { get; set; }
    }
}
