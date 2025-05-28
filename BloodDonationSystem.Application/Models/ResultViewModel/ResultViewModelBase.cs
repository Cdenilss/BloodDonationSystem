namespace BloodDonationSystem.Application.Models.ResultViewModel;

public abstract class ResultViewModelBase
{
    public bool IsSuccess { get; }
    public string[] Errors { get; }
    public DateTime Timestamp { get; } = DateTime.UtcNow;

    protected ResultViewModelBase(bool isSuccess, string[] errors)
    {
        IsSuccess = isSuccess;
        Errors = errors ?? Array.Empty<string>();
    }
}