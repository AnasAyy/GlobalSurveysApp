namespace GlobalSurveysApp.Data.Repo
{
    public interface IImageConvertRepo
    {
        public  Task<byte[]> ConvertToByte(IFormFile img);
    }

    public class ImageConvertRepo : IImageConvertRepo
    {

        public async Task<byte[]> ConvertToByte(IFormFile img)
        {
            using var memoryStream = new MemoryStream();
            await img.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
    }
}
