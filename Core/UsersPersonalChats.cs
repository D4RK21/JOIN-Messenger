namespace Core;

public class UsersPersonalChats : BaseEntity
{
    public User User { get; set; }
    
    public PersonalChat PersonalChat { get; set; }
}
