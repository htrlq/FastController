# FastController
## `.net framework 4.0` runtime `light weight web api`, `configuration` http informat host and port, `simple` builder web api， struct adjacence `mvc 6.0`
### tag
>> 0.0.1 code is instance new

# Simple
## Server
>
Program.cs
>>
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new FastControllerFactory();
            factory.Register<UserController>();
            factory.Use("127.0.0.1", 8008);

            Console.ReadLine();
        }
    }
> UserController.cs
>>
    [Route("/User")]
    public class UserController: FastControllerBase
    {
        [Route("Image")]
        public void Image(HttpPostParam param)
        {
            var value = param.GetValue<FileInfo>();

            param.Json(value);
        }
    }

    public class FileInfo
    {
        public string FileName { get; set; }
    }
    
## Client
> Program.cs
>> 
    class Program
    {
        static void Main(string[] args)
        {
            Start();
        }

        static async void Start()
        {
            var maxCount = 100000000;
            var maxThread = 100;
            var tasks = new Task[maxThread];
            var url = "http://127.0.0.1:8008/User/Image";
            var json = "{ \"FileName\":\"D:/150106109346115258.jpg\" }";
            var encoding = Encoding.UTF8;
            var context = new StringContent(json, encoding, "application/json");

            for (int i = 0; i < maxThread; i++)
            {
                var task = Task.Run(async () =>
                {
                    using (var httpClient = new HttpClient())
                    {
                        while (maxCount > 0)
                        {
                            var result = await httpClient.PostAsync(url, context);
                            Console.WriteLine($"Current Count:{maxCount} result:{await result.Content.ReadAsStringAsync()}");
                            Interlocked.Decrement(ref maxCount);

                            await Task.Delay(100);
                        }
                    }
                });
                task.Wait(1000);

                tasks[i] = task;
            }

            Task.WaitAll(tasks);
            Console.WriteLine("End");
            Console.ReadLine();
        }
    }
