using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DallETest
{
    public interface IOpenAIProxy
    {
        //👇 Send the Prompt Text with and return a list of image URLs
        Task<GenerateImageResponse> GenerateImages(
            GenerateImageRequest prompt,
            CancellationToken cancellation = default);

        //👇 Download the Image as byte array
        Task<byte[]> DownloadImage(string url);
    }
}
