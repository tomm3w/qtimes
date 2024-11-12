using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using Core.Models;
using Core.Exceptions;

namespace Core.Helpers
{
    public class ImageHelper
    {
        public static bool isSquareImage(HttpPostedFileBase file)
        {
            try
            {
                using (Image img = Image.FromStream(file.InputStream))
                {
                    if (img.Height == img.Width)
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool isValidImageSize(HttpPostedFileBase file, int width, int height)
        {
            try
            {
                using (Image img = Image.FromStream(file.InputStream))
                {
                    if (img.Width == width && img.Height == height)
                    {
                        return true;
                    }
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public static bool ValidateImage(HttpPostedFileBase file, Size expectedMaxSize, Size expectedMinSize)
        {


            try
            {
                using (Image img = Image.FromStream(file.InputStream))
                {
                    if (img.Height > expectedMaxSize.Height || img.Width > expectedMaxSize.Width)
                        return false;
                    else if (img.Height < expectedMinSize.Height || img.Width < expectedMinSize.Width)
                        return false;

                    return true;
                }
            }
            catch (Exception ex)
            {

                return false;
                //throw ex;
            }
        }

        public static string ValidateImageAndSaveWithBase64(string fileBase64, Size expectedMaxSize, Size expectedMinSize, string fileName)
        {
            string retVal = null; ;
            if (string.IsNullOrEmpty(fileBase64))
            {
                return null;
            }

            else
            {
                try
                {
                    byte[] filebytes = Convert.FromBase64String(fileBase64);
                    using (MemoryStream stream = new MemoryStream(filebytes))
                    {
                        if (FileIsWebFriendlyImage(stream))
                        {
                            using (Image img = Image.FromStream(stream))
                            {
                                if (img.Height > expectedMaxSize.Height || img.Width > expectedMaxSize.Width)
                                    throw new CoreException
                                    {
                                        ErrorCode = ErrorCode.ErrorCodeInvalidPhotoSize,
                                        ErrorMessage = String.Format("Image must between {0}X{1} to {2}X{3}.",
                                            ConfigurationHelper.Instance.UserProfileMinResolution.Width,
                                            ConfigurationHelper.Instance.UserProfileMinResolution.Height,
                                            ConfigurationHelper.Instance.UserProfileMaxResolution.Width,
                                            ConfigurationHelper.Instance.UserProfileMaxResolution.Height)
                                    };
                                else if (img.Height < expectedMinSize.Height || img.Width < expectedMinSize.Width)
                                    throw new CoreException
                                    {
                                        ErrorCode = ErrorCode.ErrorCodeInvalidPhotoSize,
                                        ErrorMessage = String.Format("Image must between {0}X{1} to {2}X{3}.",
                                            ConfigurationHelper.Instance.UserProfileMinResolution.Width,
                                            ConfigurationHelper.Instance.UserProfileMinResolution.Height,
                                            ConfigurationHelper.Instance.UserProfileMaxResolution.Width,
                                            ConfigurationHelper.Instance.UserProfileMaxResolution.Height)
                                    };
                                else
                                {
                                    string uploadPath = "~/Content/Uploads";
                                    retVal = uploadPath + "/" + fileName;
                                }

                            }
                        }
                        else
                        {
                            throw new CoreException { ErrorCode = ErrorCode.ErrorCodeInvalidPhotoFormat, ErrorMessage = "Image should be formated as jpeg,jpng,png or gif" };
                        }
                        stream.Close();
                        if (!string.IsNullOrEmpty(retVal))
                        {
                            string savePath = HttpContext.Current.Server.MapPath(retVal);
                            FileStream fs = new FileStream(HttpContext.Current.Server.MapPath(retVal),
                                                   FileMode.OpenOrCreate,
                                                   FileAccess.Write,
                                                   FileShare.None);
                            fs.Write(filebytes, 0, filebytes.Length);
                            fs.Close();

                        }
                    }


                }
                catch (CoreException ex)
                {
                    throw ex;
                }
                catch (Exception ex)
                {

                    throw new CoreException { ErrorCode = ErrorCode.UnKnownError, ErrorMessage = ex.Message };
                    //throw ex;
                }
            }

            return retVal;
        }

        public static bool FileIsWebFriendlyImage(Stream stream)
        {
            try
            {
                //Read an image from the stream...
                var i = Image.FromStream(stream);

                //Move the pointer back to the beginning of the stream
                stream.Seek(0, SeekOrigin.Begin);

                if (ImageFormat.Jpeg.Equals(i.RawFormat))
                    return true;
                return ImageFormat.Png.Equals(i.RawFormat) || ImageFormat.Gif.Equals(i.RawFormat);
            }
            catch
            {
                return false;
            }
        }

        public static string SaveImage(HttpPostedFileBase file)
        {
            string fileName = Guid.NewGuid().ToString("N") + Path.GetExtension(file.FileName);
            string uploadPath = "~/Content/Uploads";
            string path = uploadPath + fileName;// Path.Combine(HttpContext.Current.Server.MapPath(uploadPath), fileName);

            file.SaveAs(Path.Combine(HttpContext.Current.Server.MapPath(uploadPath), fileName));
            //return path;
            return uploadPath + "/" + fileName;// Path.Combine(uploadPath, fileName);
        }

        public static string SaveImage(HttpPostedFileBase file, string fileName)
        {
            //string fileName = Guid.NewGuid().ToString("N") + Path.GetExtension(file.FileName);
            string uploadPath = "~/Content/Uploads";
            string path = uploadPath + fileName;// Path.Combine(HttpContext.Current.Server.MapPath(uploadPath), fileName);

            file.SaveAs(Path.Combine(HttpContext.Current.Server.MapPath(uploadPath), fileName));
            //return path;
            return uploadPath + "/" + fileName;// Path.Combine(uploadPath, fileName);
        }

        public static string SaveImage(HttpPostedFileBase file, string uploadPath, string fileName)
        {
            //string fileName = Guid.NewGuid().ToString("N") + Path.GetExtension(file.FileName);
            //string uploadPath = "~/Content/Uploads";
            string path = uploadPath + fileName;// Path.Combine(HttpContext.Current.Server.MapPath(uploadPath), fileName);

            file.SaveAs(Path.Combine(HttpContext.Current.Server.MapPath(uploadPath), fileName));
            //return path;
            return uploadPath + "/" + fileName;// Path.Combine(uploadPath, fileName);
        }

        public static string SaveImage(string file, string savefilename)
        {


            if (!string.IsNullOrEmpty(file))
            {
                byte[] filebytes = Convert.FromBase64String(file);
                string uploadPath = "~/Content/Uploads";
                string path = uploadPath + savefilename;// Path.Combine(HttpContext.Current.Server.MapPath(uploadPath), fileName);

                FileStream fs = new FileStream(path,
                                               FileMode.CreateNew,
                                               FileAccess.Write,
                                               FileShare.None);
                fs.Write(filebytes, 0, filebytes.Length);
                fs.Close();
                return path;
            }
            return null;


        }

        public static void TryDeleteFile(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
            catch { }
        }

        public static void MoveFile(string sourcefilePath, string destinationfilePath)
        {
            try
            {
                if (File.Exists(sourcefilePath))
                {
                    File.Move(sourcefilePath, destinationfilePath);
                }
            }
            catch { }
        }

        public static bool DeleteImageWithRelativePath(string path)
        {
            bool retVal = false;
            string filePath = HttpContext.Current.Server.MapPath(path);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            return retVal;
        }

        #region UserImage
        //public static string GetBASE64UserImage(User user)
        //{
        //    string retVal = string.Empty;
        //    try
        //    {
        //        string userPhotoPath = System.Web.Hosting.HostingEnvironment.MapPath(user.Photo);
        //        if (File.Exists(userPhotoPath))
        //        {
        //            using (Image userPhoto = Image.FromFile(userPhotoPath))
        //            {

        //                string thumbnailPhoto = Path.Combine(Path.GetDirectoryName(userPhotoPath), Path.GetFileNameWithoutExtension(userPhotoPath) + "thumb" + Path.GetExtension(userPhotoPath));// System.Web.Hosting.HostingEnvironment.MapPath(user.Photo + "thumb");
        //                if (!File.Exists(thumbnailPhoto))
        //                {
        //                    Image img = ScaleImage(userPhoto, ConfigurationHelper.Instance.UserProfileThumbSize.Width, ConfigurationHelper.Instance.UserProfileThumbSize.Height, thumbnailPhoto);
        //                    img.Save(thumbnailPhoto, ImageFormat.Jpeg);


        //                }
        //                using (FileStream fs = new FileStream(thumbnailPhoto, FileMode.Open, FileAccess.Read))
        //                {
        //                    BinaryReader br = new BinaryReader(fs);
        //                    byte[] imageBytes = br.ReadBytes((int)fs.Length);
        //                    retVal = Convert.ToBase64String(imageBytes);
        //                    br.Close();

        //                }
        //            }
        //        }
        //    }
        //    catch (ArgumentNullException ex)
        //    {
        //        throw new MeetException {ErrorCode = .ErrorCodePhotoDoesnotExists, ErrorMessage = "Photo doesnot exist", ErrorType = ErrorType.data };
        //    }
        //    return retVal;
        //}
        #endregion

        #region "Resize image"
        public static Image ScaleImage(Image image, int maxWidth, int maxHeight)
        {
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            //var newImage = new Bitmap(newWidth, newHeight);
            var newImage = ImageUtilities.ResizeImage(image, newWidth, newHeight);
            //Graphics.FromImage(newImage).DrawImage(image, 0, 0, newWidth, newHeight);
            return newImage;
        }

        public static bool ScaleImage(string sourcePath, int maxWidth, int maxHeight, string destPath)
        {
            try
            {
                Image img = ScaleImage(Image.FromFile(sourcePath), maxWidth, maxHeight);
                img.Save(destPath);
                img.Dispose();
            }
            catch (Exception e)
            {
                //Logger.Log(e);
                return false;
            }
            return true;
        }

        public static Image ScaleImage(Image image, int maxWidth, int maxHeight, string destPath)
        {
            Image img = null;
            try
            {
                img = ScaleImage(image, maxWidth, maxHeight);
                img.Save(destPath);

            }
            catch (Exception e)
            {
                throw e;
                //return null;
            }
            return img;
        }

        public static bool ScaleImageWithHeight(string sourcePath, int maxHeight, string destPath)
        {
            return ScaleImage(sourcePath, int.MaxValue, maxHeight, destPath);
        }

        public static bool ScaleImage(string sourcePath, int maxWidth, string destPath)
        {
            return ScaleImage(sourcePath, maxWidth, int.MaxValue, destPath);
        }
        #endregion
    }

    #region Resize
    public static class ImageUtilities
    {
        /// <summary>
        /// A quick lookup for getting image encoders
        /// </summary>
        private static Dictionary<string, ImageCodecInfo> encoders = null;

        /// <summary>
        /// A quick lookup for getting image encoders
        /// </summary>
        public static Dictionary<string, ImageCodecInfo> Encoders
        {
            //get accessor that creates the dictionary on demand
            get
            {
                //if the quick lookup isn't initialised, initialise it
                if (encoders == null)
                {
                    encoders = new Dictionary<string, ImageCodecInfo>();
                }

                //if there are no codecs, try loading them
                if (encoders.Count == 0)
                {
                    //get all the codecs
                    foreach (ImageCodecInfo codec in ImageCodecInfo.GetImageEncoders())
                    {
                        //add each codec to the quick lookup
                        encoders.Add(codec.MimeType.ToLower(), codec);
                    }
                }

                //return the lookup
                return encoders;
            }
        }

        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public static System.Drawing.Bitmap ResizeImage(System.Drawing.Image image, int width, int height)
        {
            //a holder for the result
            Bitmap result = new Bitmap(width, height);
            // set the resolutions the same to avoid cropping due to resolution differences
            result.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            //use a graphics object to draw the resized image into the bitmap
            using (Graphics graphics = Graphics.FromImage(result))
            {
                //set the resize quality modes to high quality
                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                //draw the image into the target bitmap
                graphics.DrawImage(image, 0, 0, result.Width, result.Height);
            }

            //return the resulting bitmap
            return result;
        }

        /// <summary> 
        /// Saves an image as a jpeg image, with the given quality 
        /// </summary> 
        /// <param name="path">Path to which the image would be saved.</param> 
        /// <param name="quality">An integer from 0 to 100, with 100 being the 
        /// highest quality</param> 
        /// <exception cref="ArgumentOutOfRangeException">
        /// An invalid value was entered for image quality.
        /// </exception>
        public static void SaveJpeg(string path, Image image, int quality)
        {
            //ensure the quality is within the correct range
            if ((quality < 0) || (quality > 100))
            {
                //create the error message
                string error = string.Format("Jpeg image quality must be between 0 and 100, with 100 being the highest quality.  A value of {0} was specified.", quality);
                //throw a helpful exception
                throw new ArgumentOutOfRangeException(error);
            }

            //create an encoder parameter for the image quality
            EncoderParameter qualityParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            //get the jpeg codec
            ImageCodecInfo jpegCodec = GetEncoderInfo("image/jpeg");

            //create a collection of all parameters that we will pass to the encoder
            EncoderParameters encoderParams = new EncoderParameters(1);
            //set the quality parameter for the codec
            encoderParams.Param[0] = qualityParam;
            //save the image using the codec and the parameters
            image.Save(path, jpegCodec, encoderParams);
        }

        /// <summary> 
        /// Returns the image codec with the given mime type 
        /// </summary> 
        public static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            //do a case insensitive search for the mime type
            string lookupKey = mimeType.ToLower();

            //the codec to return, default to null
            ImageCodecInfo foundCodec = null;

            //if we have the encoder, get it to return
            if (Encoders.ContainsKey(lookupKey))
            {
                //pull the codec from the lookup
                foundCodec = Encoders[lookupKey];
            }

            return foundCodec;
        }
    }
    #endregion


}