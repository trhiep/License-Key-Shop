using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;

namespace License_Key_Shop_Web.Utils
{
    public class CloudinaryUploader
    {
        // Thông tin tài khoản Cloudinary của bạn
        private const string CloudName = "duzrv35z5";
        private const string ApiKey = "167631264475554";
        private const string ApiSecret = "_jXx1bhbSmznVkr50_BbnkmxZXw";

        public static string ProcessUpload(IFormFile imageFile)
        {
            try
            {
                // Khởi tạo Cloudinary
                Account account = new Account(CloudName, ApiKey, ApiSecret);
                Cloudinary cloudinary = new Cloudinary(account);

                // Tạo một unique filename cho ảnh
                string uniqueFilename = Guid.NewGuid().ToString();

                // Đường dẫn lưu trữ ảnh trên Cloudinary
                string cloudinaryPath = $"Uploader/{uniqueFilename}";

                // Tạo upload parameters
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(imageFile.FileName, imageFile.OpenReadStream()),
                    PublicId = cloudinaryPath,
                    Overwrite = true,
                };

                // Upload ảnh lên Cloudinary
                var uploadResult = cloudinary.Upload(uploadParams);

                // Xử lý kết quả upload (uploadResult) theo nhu cầu của bạn
                // Ví dụ: Lấy URL của ảnh sau khi upload
                string imageUrl = uploadResult.Uri.ToString();

                // Đoạn mã xử lý sau khi upload thành công
                Console.WriteLine("Upload success!");

                return imageUrl;
            }
            catch (Exception ex)
            {
                return "Error";
            }
        }
    }
}
