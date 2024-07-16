using Application.DTO;
using Application.Interfaces.Context;
using Domain.Files;
using Endpoint.Messages;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Endpoint.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileUploadController : ControllerBase
    {
        private readonly IBus _bus;
        private readonly IDataBaseContext _context;
        public FileUploadController(IBus bus, IDataBaseContext context)
        {
            _bus = bus;
            _context = context;
        }

        [HttpPost]
        [Route("UploadQueue1")]
        public async Task<IActionResult> UploadFileQueue1(IFormFile file)
            {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");

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
                            return BadRequest("Duplicate code");
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

                foreach (var item in csvData)
                {
                    await _bus.Publish(new CreateItemCommand
                    {
                        Item = item,
                        At = DateTime.Now
                    });
                }
            }
            return Ok("File uploaded and SaveDB In Queue1.");
        }
        [HttpPost]
        [Route("UploadQueue2")]
        public async Task<IActionResult> UploadFileQueue2(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");

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
                            return BadRequest("Duplicate code");
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
                foreach (var item in csvData)
                {
                    await _bus.Publish(new ProcessItemCommand
                    {
                        Item = item,
                        At = DateTime.Now
                    });
                }
            }

            return Ok("File uploaded and processed.");
        }
    }
}
