// Services/ImageUploadService.cs
using Microsoft.AspNetCore.Hosting;  
using Microsoft.AspNetCore.Http;    
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Runrs.Services
{
    public class ImageUploadService : IImageUploadService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
        private const long _maxFileSize = 5 * 1024 * 1024; // 5MB

        public ImageUploadService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<string> UploadImageAsync(IFormFile imageFile, string folderName = "club-images")
        {
            if (imageFile == null || imageFile.Length == 0)
                return null;

            // Validate file extension
            var extension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
            if (!_allowedExtensions.Contains(extension))
                throw new InvalidOperationException("Invalid file type. Allowed: JPG, PNG, GIF");

            // Validate file size
            if (imageFile.Length > _maxFileSize)
                throw new InvalidOperationException("File size exceeds 5MB limit");

            // Create unique filename
            var uniqueFileName = $"{Guid.NewGuid()}{extension}";
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", folderName);

            // Ensure directory exists
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // Save file
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            return $"/uploads/{folderName}/{uniqueFileName}";
        }

        public void DeleteImage(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl)) return;

            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, imageUrl.TrimStart('/'));
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }

    public interface IImageUploadService
    {
        Task<string> UploadImageAsync(IFormFile imageFile, string folderName = "club-images");
        void DeleteImage(string imageUrl);
    }
}