using Application.DTO;
using Domain.Files;
using Microsoft.AspNetCore.Http;
using static Application.Files.UploadFileServices;

namespace Application.Files
{
    public interface IUploadFileServices
    {
        Task<BaseDto> UploadFileAsync(IFormFile file);
        Task<List<CsvFilesDTO>> FetchAllAsync();
        Task<BaseDto<CsvFilesDTO>> FetchbycodeAsync(string code);
        Task<BaseDto> DeleteAllDataAsync();
    }
}
