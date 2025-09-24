using Application.Common;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.RabbitMq
{
    public interface IRabbitAudioService
    {
        Task<Result<string>> SendAudioAsync(IFormFile audioFile);
    }
}