using System.Threading.Tasks;

namespace Financial.Api.Infrastructure;

public interface IDatabaseInitializer
{
    Task InitializeAsync();
}