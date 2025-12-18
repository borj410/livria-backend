namespace LivriaBackend.IAM.Domain.Model.Aggregates;
using LivriaBackend.IAM.Domain.Model.ValueObjects;

/// <summary>
/// Clase que controla el acceso a una cuenta de usuario
/// Contiene los campos asociados al proceso de login y verificación de identidad
/// </summary>

public class Identity
{
    public int Id { get; private set; } 
    
    /// <summary>
    /// La Id del usuario asociado a la Identity
    /// </summary>
    public int UserId { get; set; }
    
    /// <summary>
    /// El username único asociado a la Identity
    /// </summary>
    public string UserName { get; set; }
    
    /// <summary>
    /// La contraseña asociada a la Identity
    /// </summary>
    public PasswordHash HashedPassword { get; set; }
    
    /// <summary>
    /// Constructor sin parámetros
    /// </summary>
    
    protected Identity() { }

    /// <summary>
    /// Constructor con parámetros para Identity
    /// </summary>
    /// <param name="userid">Id de usuario asociado</param>
    /// <param name="username">Username de usuario asociado</param>
    /// <param name="hashedPassword">Contraseña encriptada de usuario asociado</param>
    public Identity(int userid, string username, string hashedPassword)
    {
        if (string.IsNullOrWhiteSpace(username) || username.Length < 3 || username.Length > 50)
        {
            throw new ArgumentException("Username must be between 3 and 50 characters.");
        }
        
        if (string.IsNullOrWhiteSpace(hashedPassword) || hashedPassword.Length < 8 || hashedPassword.Length > 100)
        {
            throw new ArgumentException("Password must be between 8 and 100 characters.");
        }
        
        UserId = userid;
        UserName = username;
        HashedPassword = new PasswordHash(hashedPassword);
    }

    /// <summary>
    /// Método protegido para actualizar las propiedades de Identity
    /// </summary>
    /// <param name="username"></param>
    /// <param name="hashedPassword"></param>
    protected void UpdateIdentity(string username, string hashedPassword)
    {
        UserName = username;
        HashedPassword = new PasswordHash(hashedPassword);
    }
    
    public bool VerifyPassword(string plainTextPassword)
    {
        if (HashedPassword == null) return false;
        return HashedPassword.Matches(plainTextPassword);
    }
    
    public void UpdateUsername(string newUsername)
    {
        if (string.IsNullOrWhiteSpace(newUsername) || newUsername.Length < 3 || newUsername.Length > 50)
        {
            throw new ArgumentException("Username must be between 3 and 50 characters.");
        }
        UserName = newUsername;
    }
    
    public void UpdatePassword(string currentPassword, string newPassword)
    {
        if (string.IsNullOrWhiteSpace(newPassword) || newPassword.Length < 8 || newPassword.Length > 100)
        {
            throw new ArgumentException("Password must be between 8 and 100 characters.");
        }

        if (!VerifyPassword(currentPassword))
            throw new ArgumentException("Current password is not correct.");
        
        HashedPassword = new PasswordHash(newPassword);
    }

}