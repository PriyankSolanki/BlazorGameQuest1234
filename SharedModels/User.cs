namespace SharedModels;

public class User
{
    public int Id { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public bool IsAdmin { get; set; }
    
    public User(){}

    public User(int id, string login, string password, bool isAdmin)
    {
        Id = id;
        Login = login;
        Password = password;
        IsAdmin = isAdmin;
    }
}