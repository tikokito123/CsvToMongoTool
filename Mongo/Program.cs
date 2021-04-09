using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Net;
using System.IO.Compression;

namespace Mongo
{
    class Program
    {
        static string path = null;
        static List<Employee> employees = new List<Employee>();
        static string programDirectory = AppDomain.CurrentDomain.BaseDirectory;
        static async Task Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("please enter some argumets first!");
                return;
            }

            foreach (var arg in args)
            {
                FileInfo directoryFile = new FileInfo(arg);
                if (IsURL(arg))
                    await DownloadFile(programDirectory, arg);
                
                if (directoryFile.Name.EndsWith(".gz"))
                    Decompress(directoryFile);
                
                await ReadFiles(arg);
            }
            Console.ReadLine();
        }

        private static async Task ReadFiles(string arg)
        {
            using (StreamReader sr = new StreamReader(path != null ? path : arg))
            {
                int counter = 0;
                string currentLine;
                while ((currentLine = await sr.ReadLineAsync()) != null)
                {
                    bool isFirstLine = true;
                    
                    if (isFirstLine)
                    {
                        isFirstLine = false;
                        continue;
                    }
                    
                    counter++;
                    employees.Add(CSVToEmployee(currentLine.Split(',')));
                    if (counter % 500 == 0)
                    {
                        await SendingRecords(employees).ConfigureAwait(false);
                        Console.WriteLine($"Sent ${employees.Count} employees to mongo");
                        employees.Clear();
                    }
                }
            }
        }

        private static async Task DownloadFile(string programDirectory, string file) => await new WebClient().DownloadFileTaskAsync(file, programDirectory);

        private static bool IsURL(string file)
        {
            Uri uriResult;
            bool isURL = Uri.TryCreate(file, UriKind.Absolute, out uriResult)
            && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            return isURL;
        }

        private static void Decompress(FileInfo directoryFile)
        {
            using (FileStream fs = directoryFile.OpenRead())
            {
                string currentFileName = directoryFile.FullName;
                string newFileName = currentFileName.Remove(currentFileName.Length - directoryFile.Extension.Length);

                using (FileStream decompressedFileStream = File.Create(newFileName))
                {
                    using (GZipStream decompressionStream = new GZipStream(fs, CompressionMode.Decompress))
                    {
                        decompressionStream.CopyTo(decompressedFileStream);
                        Console.WriteLine($"Decompressed: {decompressedFileStream.Name}");
                        path = decompressedFileStream.Name;
                    }
                }
            }
        }

        private static async Task SendingRecords(List<Employee> employees) => await MongoConnection.dbCollector.InsertManyAsync(employees);

        static Employee CSVToEmployee(string[] csv)
        {
            Employee employee = new Employee();
            employee.first_name = csv[0];
            employee.last_name = csv[1];
            employee.age = int.Parse(csv[2]);
            employee.job = csv[3];
            employee.joined = Convert.ToDateTime(csv[4]);
            return employee;
        }
    }
}
