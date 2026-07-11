using Microsoft.AspNetCore.Http;

namespace Clinic.Application.Helper
{
    public static class FileHelper
    {
        public static async Task<string> UploadFile(
            IFormFile file,
            string folderName,
            string[]? allowedExtensions = null,
            long maxFileSize = 5 * 1024 * 1024)
        {
            if (file == null || file.Length == 0)
                return string.Empty;

            if (file.Length > maxFileSize)
                throw new Exception("File size exceeds the allowed limit.");

            var extension = Path.GetExtension(file.FileName).ToLower();

            if (allowedExtensions != null &&
                !allowedExtensions.Contains(extension))
            {
                throw new Exception("Invalid file type.");
            }

            var folderPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "Uploads",
                folderName);

            Directory.CreateDirectory(folderPath);

            var fileName = $"{Guid.NewGuid():N}{extension}";

            var filePath = Path.Combine(folderPath, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);

            await file.CopyToAsync(stream);

            return fileName;
        }

        public static async Task<List<string>> UploadFiles(
            List<IFormFile> files,
            string folderName,
            string[]? allowedExtensions = null,
            long maxFileSize = 5 * 1024 * 1024)
        {
            var uploadedFiles = new List<string>();

            foreach (var file in files)
            {
                var fileName = await UploadFile(
                    file,
                    folderName,
                    allowedExtensions,
                    maxFileSize);

                if (!string.IsNullOrEmpty(fileName))
                    uploadedFiles.Add(fileName);
            }

            return uploadedFiles;
        }
    }
}