namespace Core;

public class InviteLinksUsers : BaseEntity
{
    public User User { get; set; }
    
    public InviteLink InviteLink { get; set; }
}
