namespace AsciiConverter.Services;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;
using System.Text;

public class AsciiConverterService : IAsciiConverterService
{
    public async Task<string> ConvertAsync(byte[] bytes, int width, string char_set)
    {
        using Image<Rgba32> image = Image.Load<Rgba32>(bytes);

        const double char_scale = 0.55; // ratio of height to width for a terminal character
        int height = Math.Max(1, (int)(image.Height * ((double)width / image.Width) * char_scale));
        image.Mutate(x => x.Resize(width, height));

        var sb = new System.Text.StringBuilder();
        image.ProcessPixelRows(accessor =>
        {
            for(int y = 0; y < accessor.Height; y++)
            {
                Span<Rgba32> row = accessor.GetRowSpan(y);

                for(int x = 0; x < row.Length; x++)
                {
                    ref Rgba32 pixel = ref row[x];
                    double luminance = (0.299 * pixel.R + 0.587 * pixel.G + 0.114 * pixel.B) / 255.0;

                    int lum_index = (int)Math.Round(luminance * char_set.Length - 1);
                    sb.Append(char_set[Math.Clamp(lum_index, 0, char_set.Length - 1)]);
                }
                sb.Append('\n');
            }
        });
        return sb.ToString();
    }

    public string ConvertToASCII(string path, int width, string char_set)
    {
        using Image<Rgba32> image = Image.Load<Rgba32>(path);

        const double char_scale = 0.55; // ratio of height to width for a terminal character
        int height = Math.Max(1, (int)(image.Height * ((double)width / image.Width) * char_scale));
        image.Mutate(x => x.Resize(width, height));

        var sb = new System.Text.StringBuilder();
        image.ProcessPixelRows(accessor =>
        {
            for(int y = 0; y < accessor.Height; y++)
            {
                Span<Rgba32> row = accessor.GetRowSpan(y);

                for(int x = 0; x < row.Length; x++)
                {
                    ref Rgba32 pixel = ref row[x];
                    double luminance = (0.299 * pixel.R + 0.587 * pixel.G + 0.114 * pixel.B) / 255.0;

                    int lum_index = (int)Math.Round(luminance * char_set.Length - 1);
                    sb.Append(char_set[Math.Clamp(lum_index, 0, char_set.Length - 1)]);
                }
                sb.Append('\n');
            }
        });
        return sb.ToString();
    }
}