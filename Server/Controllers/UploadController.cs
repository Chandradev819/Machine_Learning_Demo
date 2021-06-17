using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using ML_Demo3.Shared;
using ML_Demo3_Server;
using System;
using System.Threading.Tasks;

namespace ML_Demo3.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly IWebHostEnvironment env;

        public UploadController(IWebHostEnvironment env)
        {
            this.env = env;
        }

        [HttpPost]
        public async Task<ImageFile> Post([FromBody] ImageFile[] files)
        {
            ImageFile obj = new ImageFile();
            foreach (var file in files)
            {
                var buf = Convert.FromBase64String(file.base64data);
                obj.filePath = (env.ContentRootPath + "\\" + "Images" + "\\" + Guid.NewGuid().ToString("N") + "-" + file.fileName);
                await System.IO.File.WriteAllBytesAsync(obj.filePath, buf);
                obj.result = ImageDefectionFromML(obj.filePath);
            }
            return obj;
        }

        public string ImageDefectionFromML(string imageUrl)
        {
            var sampleData = new MLModel.ModelInput()
            {
                ImageSource = imageUrl,
            };

            //Load model and predict output
            var result = MLModel.Predict(sampleData);

            return result.Prediction.ToString();
        }
    }
}
