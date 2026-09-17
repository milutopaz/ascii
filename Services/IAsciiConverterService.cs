namespace AsciiConverter.Services;

public interface IAsciiConverterService
{
    public string ConvertToASCII(string path, int width, string char_set);

    public Task<string> ConvertAsync(byte[] bytes, int width, string char_set);
}