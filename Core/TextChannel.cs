namespace Core
{
    public class TextChannel : BaseEntity
    {
        public string ChannelName { get; set; }
        
        public string ChannelDescription { get; set; }

        public bool IsAdminChannel { get; set; }
    }
}
