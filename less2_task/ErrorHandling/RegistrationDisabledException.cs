namespace less2_task.ErrorHandling;

public class RegistrationDisabledException : Exception
{
    public RegistrationDisabledException() : base("Registration is currently disabled.")
    {
    }
}
