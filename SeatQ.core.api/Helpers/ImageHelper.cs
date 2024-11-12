using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;

namespace SeatQ.core.api.Helpers
{
    public class ImageHelper
    {
        public static bool FileIsWebFriendlyImage(Stream stream)
        {
            try
            {
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

        public static bool isValidImageSize(Stream stream, int width, int height)
        {
            try
            {
                using (Image img = Image.FromStream(stream))
                {
                    stream.Position = 0;
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

        public static bool isSquareImage(Stream stream)
        {
            try
            {
                using (Image img = Image.FromStream(stream))
                {
                    stream.Position = 0;
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

        public static bool ValidateImage(Stream stream, Size expectedMaxSize, Size expectedMinSize)
        {
            try
            {
                using (Image img = Image.FromStream(stream))
                {
                    stream.Position = 0;
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
            }
        }

        public static string SaveImage(Stream stream, string fileName)
        {
            string uploadPath = "~/Content/Uploads";
            using (var file = new FileStream(Path.Combine(HttpContext.Current.Server.MapPath(uploadPath), fileName), FileMode.Create))
            {
                stream.CopyTo(file);
                file.Flush();
            }
            stream.Position = 0;

            return uploadPath + "/" + fileName;
        }

        public static string MoveToBusiness(string fromPath)
        {
            try
            {
                if (fromPath.StartsWith("~"))
                    fromPath = HttpContext.Current.Server.MapPath(fromPath);
                var localPath = "~" + new Uri(fromPath).LocalPath;
                var fileName = Path.GetFileName(fromPath);

                string businessPath = "~/Content/Uploads/Business";
                string destPath = Path.Combine(HttpContext.Current.Server.MapPath(businessPath), fileName);

                if (!File.Exists(destPath))
                {
                    File.Copy(HttpContext.Current.Server.MapPath(localPath), destPath);
                    TryDeleteFile(fromPath);
                }

                return businessPath + "/" + fileName;
            }
            catch { return fromPath; }
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

        public static bool DeleteImageWithRelativePath(string path)
        {
            try
            {
                bool retVal = false;
                string filePath = HttpContext.Current.Server.MapPath(path);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                return retVal;
            }
            catch
            {
                return false;
            }
        }

        public static string MapPathReverse(string absolutePath)
        {
            return "~" + new Uri(absolutePath, UriKind.RelativeOrAbsolute).LocalPath;
        }

        public static string ContentAbsUrl(string relativeContentPath)
        {
            Uri contextUri = HttpContext.Current.Request.Url;

            var baseUri = string.Format("{0}://{1}{2}", contextUri.Scheme,
               contextUri.Host, contextUri.Port == 80 ? string.Empty : ":" + contextUri.Port);
            if (relativeContentPath == null)
            {
                return baseUri;
            }
            return string.Format("{0}{1}", baseUri, VirtualPathUtility.ToAbsolute(relativeContentPath));
        }
    }
}