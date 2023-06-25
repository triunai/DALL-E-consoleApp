using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DallETest
{
    public record class GenerateImageRequest(
     string Prompt,
     int N,
     string Size);

    public record class GenerateImageResponse(
        long Created,
        GeneratedImageData[] Data);

    public record class GeneratedImageData(string Url);
}
