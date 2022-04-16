namespace Core;

public class UserToken : BaseEntity
{
    public string UserName { get; set; }
        
    public string Email { get; set; }
        
    public byte[] PasswordHash { get; set; }
    
    public byte[] PasswordSalt { get; set; }
}