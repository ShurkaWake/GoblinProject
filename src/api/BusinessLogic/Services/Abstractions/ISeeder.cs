using FluentResults;

namespace BusinessLogic.Services.Abstractions
{
    public interface ISeeder
    {
        Task<Result> SeedAsync();
    }
}
