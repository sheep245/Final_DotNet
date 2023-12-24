namespace Point_Of_Sales.Helpers
{
    public class FileProcess
    {
        public static async void FileUpload(IFormFile file, string filePath)
        {
            if (file != null && file.Length > 0)
            {
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }
        }
    }
}
