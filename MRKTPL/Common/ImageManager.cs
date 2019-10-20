using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Net;

namespace MRKTPL.Common
{
    public class ImageManager
    {
        public string ValidateImage(IFormFile postedFile, int maxPictureSize)
        {
            string errorMessage = "";
            try
            {
                if (postedFile.FileName != "")
                {
                    if (Convert.ToInt32(postedFile.Length / 1024) > maxPictureSize)
                    {
                        errorMessage += "The size of the picture is too large, kindly upload a smaller picture. Size should be less than 2Mb.";
                    }

                    if (!postedFile.ContentType.ToString().Contains("image"))
                    {
                        errorMessage += "Please select an image file to upload. Only PNG, GIF and JPEG file formats are accepted.";
                    }

                }
                else
                {
                    errorMessage = "Please select a file to upload";
                }
            }
            catch (Exception ex)
            {
                errorMessage = "Unknown error has occurred.";
                throw ex;
            }

            return errorMessage;
        }

        public string CreateImageFileName(IFormFile postedFile)
        {
            string fileName = "";

            string postedFileExtension = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.') + 1);
            try
            {
                fileName = Guid.NewGuid().ToString();
                fileName = fileName + "." + postedFileExtension;

            }
            catch (Exception ex)
            {
                fileName = "";
                throw ex;
            }

            return fileName;

        }

       
        public bool SavePicture(string postedFile, string imageName, string imagePath)
        {
            string filePath = Path.Combine(imagePath + imageName);
          //  File.Move(imageName,Path.Combine(imagePath + imageName));
            
            return true;
        }

        
        public string CreateThumbNailHeight(int width, int height, string imageName, string imagePath, string thumbMarker)
        {
            string thumbPath = "";

            try
            {
                System.Drawing.Image selectedImage = System.Drawing.Image.FromFile(Path.Combine(imagePath + imageName));      
                //System.Web.HttpContext.Current.Server.MapPath -->Replace by Path.Combine
                string[] imageNameArray = imageName.Split('.');

                int imageWidth = selectedImage.Width;
                int imageHeight = selectedImage.Height;

                if (imageWidth > 20)
                {
                    Decimal sizeRatio = ((Decimal)imageHeight / imageWidth);
                    int thumbHeight = Decimal.ToInt32(sizeRatio * width);

                    //set particular height
                    if (thumbHeight > height)
                    {
                        thumbHeight = height;
                        //  width = ((height * imageWidth) / imageHeight);
                    }
                    //

                    Bitmap bmpImage = new Bitmap(width, thumbHeight);

                    System.Drawing.Graphics graphicObject = System.Drawing.Graphics.FromImage(bmpImage);
                    graphicObject.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    graphicObject.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    graphicObject.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                    System.Drawing.Rectangle rectDestination = new System.Drawing.Rectangle(0, 0, width, thumbHeight);
                    graphicObject.DrawImage(selectedImage, rectDestination, 0, 0, imageWidth, imageHeight, GraphicsUnit.Pixel);
                    bmpImage.Save(Path.Combine(imagePath + imageNameArray[0] + thumbMarker + "." + imageNameArray[1]), System.Drawing.Imaging.ImageFormat.Jpeg);
                    thumbPath = imagePath + imageNameArray[0] + thumbMarker + "." + imageNameArray[1];
                    bmpImage.Dispose();
                    selectedImage.Dispose();
                }
                else
                {
                    // string[] imageNameArray = imageName.Split('.');
                    string thumbNailImage = Path.Combine(imagePath + imageNameArray[0] + thumbMarker + "." + imageNameArray[1]);
                    selectedImage.Save(thumbNailImage);
                    thumbPath = imagePath + imageNameArray[0] + thumbMarker + "." + imageNameArray[1];
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return thumbPath;
        }   

        private Image RezizeImage(Image img, int maxWidth, int maxHeight)
        {
            if (img.Height < maxHeight && img.Width < maxWidth) return img;
            using (img)
            {
                Double xRatio = (double)img.Width / maxWidth;
                Double yRatio = (double)img.Height / maxHeight;
                Double ratio = Math.Max(xRatio, yRatio);
                int nnx = (int)Math.Floor(img.Width / ratio);
                int nny = (int)Math.Floor(img.Height / ratio);
                Bitmap cpy = new Bitmap(nnx, nny, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                using (Graphics gr = Graphics.FromImage(cpy))
                {
                    gr.Clear(Color.Transparent);

                    // This is said to give best quality when resizing images
                    gr.InterpolationMode = InterpolationMode.HighQualityBicubic;

                    gr.DrawImage(img,
                        new Rectangle(0, 0, nnx, nny),
                        new Rectangle(0, 0, img.Width, img.Height),
                        GraphicsUnit.Pixel);
                }
                return cpy;
            }

        }

        public string CreateThumbNail(int width, int height, string imageName, string imagePath, string thumbMarker)
        {
            string thumbPath = "";

            try
            {
                System.Drawing.Image selectedImage = System.Drawing.Image.FromFile(Path.Combine(imagePath + imageName));
                int imageWidth = selectedImage.Width;
                int imageHeight = selectedImage.Height;

                Decimal sizeRatio = ((Decimal)imageHeight / imageWidth);
                if (imageWidth < width)
                {
                    width = imageWidth;
                }
                int thumbHeight = Decimal.ToInt32(sizeRatio * width);



                System.Drawing.Image square = imageCrop(selectedImage, 50, 50, AnchorPosition.Top);

                string postedFileExtension = "." + imageName.Substring(imageName.LastIndexOf('.') + 1);

                Bitmap bmpImage = new Bitmap(width, thumbHeight);

                string[] imageNameArray = imageName.Split('.');

                System.Drawing.Graphics graphicObject = System.Drawing.Graphics.FromImage(bmpImage);
                graphicObject.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                graphicObject.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                graphicObject.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                System.Drawing.Rectangle rectDestination = new System.Drawing.Rectangle(0, 0, width, thumbHeight);
                graphicObject.DrawImage(selectedImage, rectDestination, 0, 0, imageWidth, imageHeight, GraphicsUnit.Pixel);
                bmpImage.Save(Path.Combine(imagePath + imageNameArray[0] + thumbMarker + postedFileExtension));
                thumbPath = imagePath + imageNameArray[0] + thumbMarker + postedFileExtension;
                bmpImage.Dispose();
                selectedImage.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return thumbPath;
        }

        public enum Dimensions
        {
            Width,
            Height
        }

        public enum AnchorPosition
        {
            Top,
            Center,
            Bottom,
            Left,
            Right
        }

        protected Image imageCrop(Image image, int width, int height, AnchorPosition anchor)
        {
            int sourceWidth = image.Width;
            int sourceHeight = image.Height;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = (Convert.ToSingle(width) / Convert.ToSingle(sourceWidth));
            nPercentH = (Convert.ToSingle(height) / Convert.ToSingle(sourceHeight));

            if (nPercentH < nPercentW)
            {
                nPercent = nPercentW;
                switch (anchor)
                {
                    case AnchorPosition.Top:
                        destY = 0;
                        break;
                    case AnchorPosition.Bottom:
                        destY = Convert.ToInt32(height - (sourceHeight * nPercent));
                        break;
                    default:
                        destY = Convert.ToInt32((height - (sourceHeight * nPercent)) / 2);
                        break;
                }
            }
            else
            {
                nPercent = nPercentH;
                switch (anchor)
                {
                    case AnchorPosition.Left:
                        destX = 0;
                        break;
                    case AnchorPosition.Right:
                        destX = Convert.ToInt32((width - (sourceWidth * nPercent)));
                        break;
                    default:
                        destX = Convert.ToInt32(((width - (sourceWidth * nPercent)) / 2));
                        break;
                }
            }

            int destWidth = Convert.ToInt32((sourceWidth * nPercent));
            int destHeight = Convert.ToInt32((sourceHeight * nPercent));
            Bitmap bmPhoto = new Bitmap(width, height);
            bmPhoto.SetResolution(image.HorizontalResolution, image.VerticalResolution);
            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
            grPhoto.DrawImage(image, new Rectangle(destX, destY, destWidth, destHeight), new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight), GraphicsUnit.Pixel);
            grPhoto.Dispose();

            return bmPhoto;
        }

           
        #region S3 Server Image Management

        
        public ImageFormat ReturnImageFormat(string extension)
        {
            ImageFormat imgFormat;
            extension = extension.TrimStart('.');
            switch (extension.ToLower())
            {
                case "bmp":
                    imgFormat = System.Drawing.Imaging.ImageFormat.Bmp;
                    break;

                case "gif":
                    imgFormat = System.Drawing.Imaging.ImageFormat.Gif;
                    break;

                case "ico":
                    imgFormat = System.Drawing.Imaging.ImageFormat.Icon;
                    break;

                case "jpeg":
                    imgFormat = System.Drawing.Imaging.ImageFormat.Jpeg;
                    break;

                case "jpg":
                    imgFormat = System.Drawing.Imaging.ImageFormat.Jpeg;
                    break;

                case "png":
                    imgFormat = System.Drawing.Imaging.ImageFormat.Png;
                    break;

                case "tiff":
                    imgFormat = System.Drawing.Imaging.ImageFormat.Tiff;
                    break;

                default:
                    imgFormat = System.Drawing.Imaging.ImageFormat.Jpeg;
                    break;
            }
            return imgFormat;
        }

        public byte[] imageToByteArray(System.Drawing.Image imageIn, string extension)
        {
            MemoryStream ms = new MemoryStream();
            extension = extension.TrimStart('.');
            switch (extension.ToLower())
            {
                case "bmp":
                    imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                    break;

                case "gif":
                    imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                    break;

                case "ico":
                    imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Icon);
                    break;

                case "jpeg":
                    imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    break;

                case "jpg":
                    imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    break;

                case "png":
                    imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    break;

                case "tiff":
                    imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Tiff);
                    break;

                default:
                    imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    break;

            }

            byte[] bytImage = ms.GetBuffer();
            return bytImage;
        }

        public string CreateThumbNailToS3(int width, int height, string imageName, Image selectedImage, string bucketName, string targetFolderName, string tragetFolderPath, string thumbMarker)
        {
            try
            {
                int imageWidth = selectedImage.Width;
                int imageHeight = selectedImage.Height;

                Decimal sizeRatio = ((Decimal)imageHeight / imageWidth);
                if (imageWidth < width)
                {
                    width = imageWidth;
                }
                int thumbHeight = Decimal.ToInt32(sizeRatio * width);

                Bitmap bmpImage = new Bitmap(width, thumbHeight);
                string[] imageNameArray = imageName.Split('.');
                System.Drawing.Graphics graphicObject = System.Drawing.Graphics.FromImage(bmpImage);
                graphicObject.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                graphicObject.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                graphicObject.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                System.Drawing.Rectangle rectDestination = new System.Drawing.Rectangle(0, 0, width, thumbHeight);
                graphicObject.DrawImage(selectedImage, rectDestination, 0, 0, imageWidth, imageHeight, GraphicsUnit.Pixel);
                imageName = imageNameArray[0] + thumbMarker + "." + imageNameArray[1];
                string filePath = Path.Combine("~/TempImageUpload/" + imageName);
                
                bmpImage.Save(filePath, ReturnImageFormat(imageNameArray[1]));
                if (File.Exists(filePath))
                {
                    bool result = SavePicture(filePath, imageName,  targetFolderName);  //SavePictureToS3UsingFilePath
                    if (result)
                    {
                        tragetFolderPath = tragetFolderPath + imageName;
                        File.Delete(filePath);
                    }
                }
                //MemoryStream imgStream = new MemoryStream(imageToByteArray(bmpImage, imageNameArray[1]));
                //bool result = SavePictureToS3(imgStream, imageName, bucketName, targetFolderName);
                //if (result)
                //{
                //    tragetFolderPath = tragetFolderPath + imageName;
                //}
                graphicObject.Dispose();
                bmpImage.Dispose();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return tragetFolderPath;
        }

        public string CreateBannerImageThumbnail(System.Drawing.Image imageToTransfer, string ImageName, string Path)
        {
            ImageManager Imgmng = new ImageManager();

            int SmallBannerWidth = 0;
            int SmallBannerHeight = 0;
            string SmallBannerImageName = "";
            //Common Bucket
            string path = $"{Directory.GetCurrentDirectory()}{@"\wwwroot\BannerImagesFolder"}";
            //string bucketName = Path.CopyTo(IHostingEnvironment.WebRootPath, "ErrorLog");// ConfigurationManager.AppSettings["BucketName"].ToString();
            string targetFolderName = "";
            string targetFolderPath = "";

            System.Drawing.Image selectedImageToTransfer = imageToTransfer;


            //  Images Thumbnail Images Path & Folder 
            switch (Path)
            {
                case "BannerImagesFolder":
                    targetFolderName = path;
                    targetFolderPath = path;
                    SmallBannerImageName = Imgmng.CreateThumbNailToS3(SmallBannerWidth, SmallBannerHeight, ImageName, selectedImageToTransfer, "banners", targetFolderName, targetFolderPath, "_176x123");
                    break;
                //case "staticbanners":
                //case "themes":
                //    targetFolderName = ConfigurationManager.AppSettings["S3ThemeFolder"].ToString();
                //    targetFolderPath = ConfigurationManager.AppSettings["S3ThemePath"].ToString();
                //    break;
                //case "brands":
                //    targetFolderName = ConfigurationManager.AppSettings["S3BrandFolder"].ToString();
                //    targetFolderPath = ConfigurationManager.AppSettings["S3BrandPath"].ToString();
                //    SmallBannerWidth = 176;
                //    SmallBannerHeight = 123;
                //    SmallBannerImageName = Imgmng.CreateThumbNailToS3(SmallBannerWidth, SmallBannerHeight, ImageName, selectedImageToTransfer, bucketName, targetFolderName, targetFolderPath, "_176x123");
                //    break;
                //case "StoreImage":
                //    targetFolderName = ConfigurationManager.AppSettings["S3StoreImagesFolder"].ToString();
                //    targetFolderPath = ConfigurationManager.AppSettings["S3StoreImagesPath"].ToString();
                //    SmallBannerWidth = 176;
                //    SmallBannerHeight = 123;
                //    SmallBannerImageName = Imgmng.CreateThumbNailToS3(SmallBannerWidth, SmallBannerHeight, ImageName, selectedImageToTransfer, bucketName, targetFolderName, targetFolderPath, "_176x123");
                //    break;
                //case "CategoryBucket":
                //    targetFolderName = ConfigurationManager.AppSettings["S3ThemeFolder"].ToString();
                //    targetFolderPath = ConfigurationManager.AppSettings["S3ThemePath"].ToString();
                //    SmallBannerWidth = 176;
                //    SmallBannerHeight = 123;
                //    SmallBannerImageName = Imgmng.CreateThumbNailToS3(SmallBannerWidth, SmallBannerHeight, ImageName, selectedImageToTransfer, bucketName, targetFolderName, targetFolderPath, "_176x123");
                //    break;

                //case "CategoryIcon":
                //    targetFolderName = ConfigurationManager.AppSettings["S3ThemeFolder"].ToString();
                //    targetFolderPath = ConfigurationManager.AppSettings["S3ThemePath"].ToString();
                //    SmallBannerWidth = 56;
                //    SmallBannerHeight = 41;
                //    SmallBannerImageName = Imgmng.CreateThumbNailToS3(SmallBannerWidth, SmallBannerHeight, ImageName, selectedImageToTransfer, bucketName, targetFolderName, targetFolderPath, "_56x41");
                //    break;

                //case "GiftImages":
                //    targetFolderName = ConfigurationManager.AppSettings["S3ProductFolder"].ToString();
                //    targetFolderPath = ConfigurationManager.AppSettings["S3ProductFolder"].ToString();
                //    SmallBannerWidth = 132;
                //    SmallBannerHeight = 132;
                //    SmallBannerImageName = Imgmng.CreateSquareThumbNailToS3(SmallBannerWidth, SmallBannerHeight, ImageName, selectedImageToTransfer, bucketName, targetFolderName, targetFolderPath, "_132x132");
                //    break;

                //case "advertiseImages":
                //    targetFolderName = ConfigurationManager.AppSettings["S3ThemeFolder"].ToString();
                //    targetFolderPath = ConfigurationManager.AppSettings["S3ThemePath"].ToString();
                //    SmallBannerWidth = 176;
                //    SmallBannerHeight = 123;
                //    SmallBannerImageName = Imgmng.CreateThumbNailToS3(SmallBannerWidth, SmallBannerHeight, ImageName, selectedImageToTransfer, bucketName, targetFolderName, targetFolderPath, "_176x123");
                //    break;
            }

            // Preview thumb
            SmallBannerWidth = 60;
            SmallBannerHeight = 40;
            SmallBannerImageName = Imgmng.CreateThumbNailToS3(SmallBannerWidth, SmallBannerHeight, ImageName, selectedImageToTransfer, "", targetFolderName, targetFolderPath, "_preview");
            return "Uploaded";


        }

        public string CreateThumbNailToS3Height(int width, int height, string imageName, Image selectedImage, string bucketName, string targetFolderName, string tragetFolderPath, string thumbMarker)
        {
            try
            {
                int imageWidth = selectedImage.Width;
                int imageHeight = selectedImage.Height;

                Decimal sizeRatio = ((Decimal)imageWidth / imageHeight);
                if (imageHeight < height)
                {
                    height = imageHeight;
                }
                int thumbWidth = Decimal.ToInt32(sizeRatio * height);

                Bitmap bmpImage = new Bitmap(thumbWidth, height);
                string[] imageNameArray = imageName.Split('.');
                System.Drawing.Graphics graphicObject = System.Drawing.Graphics.FromImage(bmpImage);
                graphicObject.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                graphicObject.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                graphicObject.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                System.Drawing.Rectangle rectDestination = new System.Drawing.Rectangle(0, 0, thumbWidth, height);
                graphicObject.DrawImage(selectedImage, rectDestination, 0, 0, imageWidth, imageHeight, GraphicsUnit.Pixel);
                imageName = imageNameArray[0] + thumbMarker + "." + imageNameArray[1];
                string filePath = Path.Combine("~/TempImageUpload/" + imageName);
                //System.Web.HttpContext.Current.Server.MapPath("~/TempImageUpload/" + imageName);
                bmpImage.Save(filePath, ReturnImageFormat(imageNameArray[1]));
                if (File.Exists(filePath))
                {
                    bool result = SavePicture(filePath, imageName, targetFolderName);
                    if (result)
                    {
                        tragetFolderPath = tragetFolderPath + imageName;
                        File.Delete(filePath);
                    }
                }
                //MemoryStream imgStream = new MemoryStream(imageToByteArray(bmpImage, imageNameArray[1]));
                //bool result = SavePictureToS3(imgStream, imageName, bucketName, targetFolderName);
                //if (result)
                //{
                //    tragetFolderPath = tragetFolderPath + imageName;
                //}
                graphicObject.Dispose();
                bmpImage.Dispose();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return tragetFolderPath;
        }


        // it will check for width and height to create thumbnail.
        public string CreatePhotoreelThumbNailToS3(int width, int height, string imageName, Image selectedImage, string bucketName, string targetFolderName, string tragetFolderPath, string thumbMarker)
        {
            try
            {
                int imageWidth = selectedImage.Width;
                int imageHeight = selectedImage.Height;

                Decimal sizeRatio = ((Decimal)imageHeight / imageWidth);
                int thumbHeight = Decimal.ToInt32(sizeRatio * width);
                if (imageWidth < width)
                {
                    width = imageWidth;
                }
                //set particular height
                if (thumbHeight > height)
                {
                    thumbHeight = height;
                    width = ((height * imageWidth) / imageHeight);
                }


                Bitmap bmpImage = new Bitmap(width, thumbHeight);
                string[] imageNameArray = imageName.Split('.');
                System.Drawing.Graphics graphicObject = System.Drawing.Graphics.FromImage(bmpImage);
                graphicObject.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                graphicObject.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                graphicObject.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                System.Drawing.Rectangle rectDestination = new System.Drawing.Rectangle(0, 0, width, thumbHeight);
                graphicObject.DrawImage(selectedImage, rectDestination, 0, 0, imageWidth, imageHeight, GraphicsUnit.Pixel);
                imageName = imageNameArray[0] + thumbMarker + "." + imageNameArray[1];
                string filePath = Path.Combine("~/TempImageUpload/" + imageName);
                bmpImage.Save(filePath, ReturnImageFormat(imageNameArray[1]));
                if (File.Exists(filePath))
                {
                    bool result = SavePicture(filePath, imageName,  targetFolderName);
                    if (result)
                    {
                        tragetFolderPath = tragetFolderPath + imageName;
                        File.Delete(filePath);
                    }
                }
                //MemoryStream imgStream = new MemoryStream(imageToByteArray(bmpImage, imageNameArray[1]));
                //bool result = SavePictureToS3(imgStream, imageName, bucketName, targetFolderName);
                //if (result)
                //{
                //    tragetFolderPath = tragetFolderPath + imageName;
                //}
                graphicObject.Dispose();
                bmpImage.Dispose();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return tragetFolderPath;
        }

        public string CreateSquareThumbNailToS3(int width, int height, string imageName, Image selectedImage, string bucketName, string targetFolderName, string tragetFolderPath, string thumbMarker)
        {
            try
            {
                System.Drawing.Image square = imageCrop(selectedImage, width, height, AnchorPosition.Top);
                string[] imageNameArray = imageName.Split('.');
                System.Drawing.Graphics graphicObject = System.Drawing.Graphics.FromImage(square);
                graphicObject.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                graphicObject.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                graphicObject.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                imageName = imageNameArray[0] + thumbMarker + "." + imageNameArray[1];
                string filePath = Path.Combine("~/TempImageUpload/" + imageName);
                square.Save(filePath, ReturnImageFormat(imageNameArray[1]));
                if (File.Exists(filePath))
                {
                    bool result = SavePicture(filePath, imageName,  targetFolderName);
                    if (result)
                    {
                        tragetFolderPath = tragetFolderPath + imageName;
                        File.Delete(filePath);
                    }
                }
                //Stream imgStream = new MemoryStream(imageToByteArray(square, imageNameArray[1]));
                //bool result = SavePicture(imgStream, imageName,  targetFolderName);
                //if (result)
                //{
                //    tragetFolderPath = tragetFolderPath + imageName;
                //}
                //graphicObject.Dispose();
                //square.Dispose();

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return tragetFolderPath;
        }

        public string CreateStatusThumbNailToS3(int width, int height, string imageName, Image selectedImage, string bucketName, string targetFolderName, string tragetFolderPath, string thumbMarker)
        {
            try
            {
                int imageWidth = selectedImage.Width;
                int imageHeight = selectedImage.Height;

                if (imageWidth > imageHeight && imageWidth > width)
                {
                    Decimal sizeRatio = ((Decimal)imageHeight / imageWidth);

                    int thumbHeight = Decimal.ToInt32(sizeRatio * width);

                    if (thumbHeight > height)
                    {
                        thumbHeight = height;
                        width = Decimal.ToInt32(sizeRatio * height);
                    }

                    Bitmap bmpImage = new Bitmap(width, thumbHeight);
                    string[] imageNameArray = imageName.Split('.');
                    System.Drawing.Graphics graphicObject = System.Drawing.Graphics.FromImage(bmpImage);
                    graphicObject.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    graphicObject.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    graphicObject.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                    System.Drawing.Rectangle rectDestination = new System.Drawing.Rectangle(0, 0, width, thumbHeight);
                    graphicObject.DrawImage(selectedImage, rectDestination, 0, 0, imageWidth, imageHeight, GraphicsUnit.Pixel);

                    imageName = imageNameArray[0] + thumbMarker + "." + imageNameArray[1];
                    string filePath = Path.Combine("TempImageUpload/" + imageName);
                    //System.Web.HttpContext.Current.Server.MapPath("TempImageUpload/" + imageName);
                    bmpImage.Save(filePath, ReturnImageFormat(imageNameArray[1]));

                    if (File.Exists(filePath))
                    {
                        bool result = SavePicture(filePath, imageName,  targetFolderName);
                        if (result)
                        {
                            tragetFolderPath = tragetFolderPath + imageName;
                            File.Delete(filePath);
                        }
                    }

                    //Stream imgStream = new MemoryStream(imageToByteArray(bmpImage, imageNameArray[1]));
                    //bool result = SavePictureToS3(imgStream, imageName, bucketName, targetFolderName);
                    //if (result)
                    //{
                    //    tragetFolderPath = tragetFolderPath + imageName;
                    //}

                    graphicObject.Dispose();
                    bmpImage.Dispose();

                }
                else if (imageHeight > imageWidth && imageHeight > height)
                {

                    Decimal sizeRatio = ((Decimal)imageWidth / imageHeight);

                    int thumbWidth = Decimal.ToInt32(sizeRatio * height);

                    if (thumbWidth > width)
                    {
                        thumbWidth = width;
                        height = Decimal.ToInt32(sizeRatio * width);
                    }

                    Bitmap bmpImage = new Bitmap(thumbWidth, height);

                    string[] imageNameArray = imageName.Split('.');

                    System.Drawing.Graphics graphicObject = System.Drawing.Graphics.FromImage(bmpImage);
                    graphicObject.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    graphicObject.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    graphicObject.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                    System.Drawing.Rectangle rectDestination = new System.Drawing.Rectangle(0, 0, thumbWidth, height);
                    graphicObject.DrawImage(selectedImage, rectDestination, 0, 0, imageWidth, imageHeight, GraphicsUnit.Pixel);

                    imageName = imageNameArray[0] + thumbMarker + "." + imageNameArray[1];
                    string filePath = Path.Combine("TempImageUpload/" + imageName);
                    bmpImage.Save(filePath, ReturnImageFormat(imageNameArray[1]));

                    if (File.Exists(filePath))
                    {
                        bool result = SavePicture(filePath, imageName, targetFolderName);
                        if (result)
                        {
                            tragetFolderPath = tragetFolderPath + imageName;
                            File.Delete(filePath);
                        }
                    }    
                    graphicObject.Dispose();
                    bmpImage.Dispose();

                }
                else
                {
                    Decimal sizeRatio = 0;
                    if (imageWidth < width)
                    {
                        width = imageWidth;
                        sizeRatio = ((Decimal)imageHeight / imageWidth);
                        height = Decimal.ToInt32(sizeRatio * width);
                    }
                    else if (imageHeight < height)
                    {
                        height = imageHeight;
                        sizeRatio = ((Decimal)imageWidth / imageHeight);
                        width = Decimal.ToInt32(sizeRatio * height);
                    }
                    else
                    {
                        if (width > height)
                        {
                            width = height;
                        }
                        else
                        {
                            height = width;
                        }
                    }

                    Bitmap bmpImage = new Bitmap(width, height);

                    string[] imageNameArray = imageName.Split('.');

                    System.Drawing.Graphics graphicObject = System.Drawing.Graphics.FromImage(bmpImage);
                    graphicObject.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    graphicObject.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    graphicObject.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                    System.Drawing.Rectangle rectDestination = new System.Drawing.Rectangle(0, 0, width, height);
                    graphicObject.DrawImage(selectedImage, rectDestination, 0, 0, imageWidth, imageHeight, GraphicsUnit.Pixel);

                    imageName = imageNameArray[0] + thumbMarker + "." + imageNameArray[1];
                    string filePath = Path.Combine("TempImageUpload/" + imageName);
                    bmpImage.Save(filePath, ReturnImageFormat(imageNameArray[1]));

                    if (File.Exists(filePath))
                    {
                        bool result = SavePicture(filePath, imageName,  targetFolderName);    //bucketName,
                        if (result)
                        {
                            tragetFolderPath = tragetFolderPath + imageName;
                            File.Delete(filePath);
                        }
                    }   
                    graphicObject.Dispose();
                    bmpImage.Dispose();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return tragetFolderPath;
        }

        #endregion

        public string SaveUrlImage(string PostedUrl, string ImagePath)
        {
            string MyImage = "";
            string profileImageExt = PostedUrl.Substring(PostedUrl.LastIndexOf(".") + 1);
            string profileImage = Guid.NewGuid().ToString() + "." + profileImageExt;
            byte[] bytes = GetBytesFromUrl(PostedUrl);
            bool Result = WriteBytesToFile(Path.Combine("~/" + ImagePath + profileImage), bytes);
            if (Result == true)
            {
                MyImage = profileImage;
            }
            return MyImage;
        }
        public System.Drawing.Image DownloadImageFromUrl(string imageUrl)
        {
            System.Drawing.Image image = null;

            try
            {
                System.Net.HttpWebRequest webRequest = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(imageUrl);
                webRequest.AllowWriteStreamBuffering = true;
                webRequest.Timeout = 30000;

                System.Net.WebResponse webResponse = webRequest.GetResponse();

                System.IO.Stream stream = webResponse.GetResponseStream();

                image = System.Drawing.Image.FromStream(stream);

                webResponse.Close();
            }
            catch (Exception ex)
            {
                return null;
            }

            return image;
        }
        static public byte[] GetBytesFromUrl(string url)
        {
            byte[] b;
            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(url);
            WebResponse myResp = myReq.GetResponse();

            Stream stream = myResp.GetResponseStream();
            //int i;
            using (BinaryReader br = new BinaryReader(stream))
            {
                //i = (int)(stream.Length);
                b = br.ReadBytes(500000);
                br.Close();
            }
            myResp.Close();
            return b;
        }
               
        static public bool WriteBytesToFile(string fileName, byte[] content)
        {
            bool result = false;
            FileStream fs = new FileStream(fileName, FileMode.Create);
            BinaryWriter w = new BinaryWriter(fs);
            try
            {
                w.Write(content);
                result = true;
            }
            finally
            {
                fs.Close();
                w.Close();
            }
            return result;
        }  
        public void CreateThumbNailToTakeWidthHeight(int width, int height, Image selectedImage, out int imgWidth, out int imgHeight)
        {
            int thumbHeight = 0;
            try
            {
                if (selectedImage != null)
                {
                    int imageWidth = selectedImage.Width;
                    int imageHeight = selectedImage.Height;

                    Decimal sizeRatio = ((Decimal)imageHeight / imageWidth);
                    if (imageWidth < width)
                    {
                        width = imageWidth;
                    }
                    thumbHeight = Decimal.ToInt32(sizeRatio * width);
                }
                else
                {
                    thumbHeight = height;
                    imgWidth = width;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            imgHeight = thumbHeight;
            imgWidth = width;
        }
        
    }
}
