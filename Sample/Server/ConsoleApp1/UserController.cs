using FastController;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace ConsoleApp1
{
    [Route("/User")]
    public class UserController: FastControllerBase
    {
        [Route("Image")]
        public void Image(HttpPostParam param)
        {
            var value = param.GetValue<FileInfo>();

            param.Json(value);
            //lock(string.Intern(value.FileName))
            //{
            //    using (var fileStream = new FileStream(value.FileName, FileMode.Open))
            //    {
            //        var length = fileStream.Length;
            //        var bytes = new byte[length];

            //        fileStream.Read(bytes, 0, bytes.Length);

            //        param.File("image/png", bytes);
            //    }
            //}
        }
    }

    public class FileInfo
    {
        public string FileName { get; set; }
    }
}
