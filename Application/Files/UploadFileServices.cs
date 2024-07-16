using Application.DTO;
using Application.Interfaces.Context;
using Domain.Files;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Application.Files
{
    public class UploadFileServices : IUploadFileServices
    {
        private readonly IDataBaseContext _context;

        public UploadFileServices(IDataBaseContext context)
        {
            _context = context;
        }

        public async Task<BaseDto> DeleteAllDataAsync()
        {
            _context.csvFiles.RemoveRange(_context.csvFiles);
            _context.SaveChanges();
            return new BaseDto(true, new List<string> { "Delete data successed" });

        }

        public async Task<List<CsvFilesDTO>> FetchAllAsync()
        {
            var data = await _context.csvFiles.Select(x => new CsvFilesDTO { Id = x.Id, Code = x.Code, Name = x.Name, Value = x.Value }).ToListAsync();
            return data;
        }

        public async Task<BaseDto<CsvFilesDTO>> FetchbycodeAsync(string code)
        {
            var data = await _context.csvFiles.Select(x => new CsvFilesDTO { Id = x.Id, Code = x.Code, Name = x.Name, Value = x.Value }).FirstOrDefaultAsync(d => d.Code == code);
            if (data == null)
            {

                return new BaseDto<CsvFilesDTO>(true, new List<string> { "file not found " }, null);
            }
            else
            {
                return new BaseDto<CsvFilesDTO>(true, new List<string> { "file not found " }, data);
            }
        }

        public async Task<BaseDto> UploadFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return new BaseDto(true, new List<string> { "No file uploaded." });

            }

            using (var stream = new StreamReader(file.OpenReadStream()))
            {
                var headers = await stream.ReadLineAsync();
                var csvData = new List<CsvFiles>();
                var files = _context.csvFiles.ToList();
                while (!stream.EndOfStream)
                {
                    var line = (await stream.ReadLineAsync())?.Split(',');
                    if (line != null)
                    {
                        var code = line[0];
                        var isExist = files.Where(d => d.Code == code).FirstOrDefault();
                        if (isExist != null)
                        {
                            return new BaseDto(true, new List<string> { $"Duplicate code found: {code}" });
                        }

                        csvData.Add(new CsvFiles
                        {
                            Code = code,
                            Name = line[1],
                            Value = line[2]
                        });
                    }
                }
                await _context.csvFiles.AddRangeAsync(csvData);
                _context.SaveChanges();
                return new BaseDto(true, new List<string> { "file uploade successed" });

            }
        }


    }
}