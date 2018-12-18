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
        }
    }

    public class FileInfo
    {
        public string FileName { get; set; }
    }
}
