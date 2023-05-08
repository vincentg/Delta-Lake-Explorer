namespace Delta_Lake_Explorer.Contracts.Services;

public interface IActivationService
{
    Task ActivateAsync(object activationArgs);
}
