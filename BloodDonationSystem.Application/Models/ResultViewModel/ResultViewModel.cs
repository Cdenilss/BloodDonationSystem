namespace BloodDonationSystem.Application.Models.ResultViewModel;

public class ResultViewModel : ResultViewModelBase
{
    private ResultViewModel(bool isSuccess, string[] errors) 
        : base(isSuccess, errors) { }

    // Factory Methods
    public static ResultViewModel Success() 
        => new ResultViewModel(true, Array.Empty<string>());

    public static ResultViewModel Error(params string[] errors) 
        => new ResultViewModel(false, errors);
}    

public class ResultViewModel<T> : ResultViewModelBase
    {
        public T? Data { get; }

        // Construtor para sucesso com dados
        public ResultViewModel(T data, string[]? errors = null) 
            : base(true, errors)
        {
            Data = data;
        }

        // Construtor para erros (sem dados)
        public ResultViewModel(bool isSuccess, string[] errors) 
            : base(isSuccess, errors)
        {
            Data = default;
        }

        // Factory Methods
        public static ResultViewModel<T> Success(T data) 
            => new ResultViewModel<T>(data);

        public static ResultViewModel<T> Error(params string[] errors) 
            => new ResultViewModel<T>(false, errors);
    }
