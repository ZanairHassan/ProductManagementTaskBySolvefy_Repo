using DAL.DBContext;
using DAL.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Utilities
{
    public class LoggingUtility
    {
        public static void LogTxt(string message,IConfiguration _configuration)
        {
            string path = _configuration["LogFolderPath"];
            using(StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine($"Date:  {DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss")}||||| {message}");
            }
        }
        public static async void ExcLog(string message,IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var log = new ExceptionLog
                {
                    LogText = message,
                    TimeStamp= DateTime.Now,
                };
                await context.ExceptionLogs.AddAsync(log);
                await context.SaveChangesAsync();
            }
        }
    }
}
