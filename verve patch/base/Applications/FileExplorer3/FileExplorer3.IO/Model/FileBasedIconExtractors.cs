using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Tools;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using FileExplorer.WPF.Models;
using QuickZip.Converters;
using QuickZip.UserControls.Logic.Tools.IconExtractor;
using ExifLib;
using System.Windows.Media.Imaging;
using FileExplorer.WPF.Utils;
using FileExplorer.WPF.Defines;
using FileExplorer.IO.Defines;
using FileExplorer.WPF.Utils;
using FileExplorer.IO;

namespace FileExplorer.Models
{
    public class GetFromSystemImageList //: IModelIconExtractor<IEntryModel>
    {
        private static IconExtractor _iconExtractor = new ExIconExtractor();
        public static IModelIconExtractor<IEntryModel> Instance = Create();
        private static string[] excludedExtensions = ".lnk,.exe".Split(',');
        public static IModelIconExtractor<IEntryModel> Create()
        {
            Func<IEntryModel, string> keyFunc = (m) =>
            {
                if (m.IsDirectory)
                    //if (model.FullPath.StartsWith("::"))
                    return "GetFromSystemImageList - " + m.FullPath;
                //else return "GetFromSystemImageList - Directory";
                else
                {
                    string extension = m.GetExtension().ToLower();

                    if (String.IsNullOrEmpty(extension))
                    {
                        //Without extension.
                        if (m.FullPath.StartsWith("::"))
                            return "GetFromSystemImageList - " + m.FullPath;
                        else return "GetFromSystemImageList - File";
                    }
                    else
                    {
                        if (excludedExtensions.Contains(extension))
                            return "GetFromSystemImageList - " + m.FullPath;
                        return "GetFromSystemImageList - " + extension;
                    }
                }
            };
            Func<IEntryModel, byte[]> getIconFunc = em =>
            {
                 if (em != null && !String.IsNullOrEmpty(em.FullPath))
                     using (FileSystemInfoEx fsi = FileSystemInfoEx.FromString(em.FullPath))
                     {
                         Bitmap bitmap = null;
                         if (fsi != null && fsi.Exists)
                             return fsi.RequestPIDL(pidl =>
                             {
                                 if (pidl != null)
                                 {
                                     bitmap = _iconExtractor.GetBitmap(QuickZip.Converters.IconSize.extraLarge,
                                         pidl.Ptr, em.IsDirectory, false);

                                     if (bitmap != null)
                                         return
                                             bitmap.ToByteArray();
                                 }
                                 return new byte[] {};
                             });
                     }
                return null;
            };

            return ModelIconExtractor<IEntryModel>.FromFuncCachable(
            keyFunc,
            (em) => getIconFunc(em)
            );
        }
    }

    public class DrawOverlayTextExtractor //: IModelIconExtractor<IEntryModel>
    {
        private static async Task<byte[]> getIcon(IModelIconExtractor<IEntryModel> baseExtractor, IEntryModel em,
             Func<IEntryModel, string> text2DrawFunc, System.Drawing.Color color)
        {
            if (em != null && !String.IsNullOrEmpty(em.FullPath))
            {
                byte[] baseBytes = await baseExtractor.GetIconBytesForModelAsync(em, CancellationToken.None);
                Bitmap baseBitmap =
                    new Bitmap(new MemoryStream(baseBytes));

                using (Graphics g = Graphics.FromImage(baseBitmap))
                {
                    g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                    string text = text2DrawFunc(em);
                    Font font = new Font("Comic Sans MS", Math.Max(baseBitmap.Width / 5, 1),
                        System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic);
                    float height = g.MeasureString(text, font).Height;
                    float rightOffset = baseBitmap.Width / 5;

                    //if (size == IconSize.small)
                    //{
                    //    font = new Font("Arial", 5, System.Drawing.FontStyle.Bold);
                    //    height = g.MeasureString(ext, font).Height;
                    //    rightOffset = 0;
                    //}


                    g.DrawString(text, font,
                                new System.Drawing.SolidBrush(color),                                
                                new RectangleF(0, baseBitmap.Height - height, baseBitmap.Width - rightOffset, height),
                                new StringFormat(StringFormatFlags.DirectionRightToLeft));
                    return baseBitmap.ToByteArray();
                }
            }
            return new byte[] { };
        }


        public static IModelIconExtractor<IEntryModel> Create(IModelIconExtractor<IEntryModel> baseExtractor, 
            Func<IEntryModel, string> keyFunc,
            Func<IEntryModel, string> text2DrawFunc, System.Drawing.Color color)
        {          
            return ModelIconExtractor<IEntryModel>.FromTaskFuncCachable(
          keyFunc,
          em => getIcon(baseExtractor, em, text2DrawFunc, color)
          );
        }

        //private Func<IEntryModel, string> _text2DrawFunc;

        //public DrawOverlayTextExtractor(IModelIconExtractor<IEntryModel> baseExtractor, Func<IEntryModel, string> text2DrawFunc)
        //{
        //    _text2DrawFunc = text2DrawFunc;
        //}

        //public Task<byte[]> GetIconBytesForModelAsync(IEntryModel model, CancellationToken ct)
        //{
        //    return Task<byte[]>.Run(() =>
        //    {
        //        if (model != null && !String.IsNullOrEmpty(model.FullPath))
        //        {
        //            using (Bitmap bitmap =
        //                ImageExtractor.ExtractImage(model.FullPath, new Size(120, 90), true))
        //            {
        //                if (bitmap != null)
        //                    return bitmap.ToByteArray();
        //            }
        //        }
        //        return null;
        //    });

        //}

    }

    public class GetFromSystemImageListUsingExtension
    {
        private static IconExtractor _iconExtractor = new IconExtractor();
        static GetFromSystemImageListUsingExtension dummy = new GetFromSystemImageListUsingExtension();
        public static IModelIconExtractor<IEntryModel> Instance = Create();
        public static IModelIconExtractor<IEntryModel> Create(Func<IEntryModel, string> fnameFunc = null)
        {
            fnameFunc = fnameFunc == null ? e => e.Label : fnameFunc;
            Func<IEntryModel, string> keyFunc = (m) =>
                {
                    if (m.IsDirectory)
                        return "GetFromSystemImageListUsingExtension - Directory";
                    else
                    {
                        string fname = fnameFunc(m);
                        string extension = m.Profile.Path.GetExtension(fname).ToLower();

                        if (String.IsNullOrEmpty(extension))
                        {
                            //Without extension.
                            return "GetFromSystemImageListUsingExtension - File";
                        }
                        else
                        {
                            return "GetFromSystemImageListUsingExtension - " + extension;
                        }
                    }
                };
            Func<IEntryModel, byte[]> getIconFunc = em =>
                {
                    if (em.IsDirectory)
                    {
                        return ResourceUtils.GetResourceAsByteArray(dummy, "/Themes/Resources/FolderIcon.png");
                    }

                    if (em != null)
                    {
                        string fname = fnameFunc(em);
                        using (Bitmap bitmap =
                            _iconExtractor.GetBitmap(IconSize.large, fname, em.IsDirectory, false))
                            if (bitmap != null)
                                return bitmap.ToByteArray();

                    }
                    return null;

                };

            return ModelIconExtractor<IEntryModel>.FromFuncCachable(
            keyFunc,
            (em) => getIconFunc(em)
            );
        }
    }

    public class GetImageFromImageExtractor : IModelIconExtractor<IEntryModel>
    {
        public static GetImageFromImageExtractor Instance = new GetImageFromImageExtractor();

        public Task<byte[]> GetIconBytesForModelAsync(IEntryModel model, CancellationToken ct)
        {
            return Task<byte[]>.Run(() =>
                {
                    if (model != null && !String.IsNullOrEmpty(model.FullPath))
                    {
                        using (Bitmap bitmap =
                            ImageExtractor.ExtractImage(model.FullPath, new Size(120, 90), true))
                        {
                            if (bitmap != null)
                                return bitmap.ToByteArray();
                        }
                    }
                    return null;
                });

        }
    }

    public class GetFromIconExtractIcon : IModelIconExtractor<IEntryModel>
    {
        public static GetFromIconExtractIcon Instance = new GetFromIconExtractIcon();

        public Task<byte[]> GetIconBytesForModelAsync(IEntryModel model, CancellationToken ct)
        {
            if (model == null || model.IsDirectory)
                return Task.FromResult<byte[]>(null);

            using (var icon = System.Drawing.Icon.ExtractAssociatedIcon(model.FullPath))
            using (var bitmap = icon.ToBitmap())
                return Task.FromResult<byte[]>(
                    bitmap.ToByteArray());
        }
    }


    public class GetExifThumbnail : IModelIconExtractor<IEntryModel>
    {
        public async Task<byte[]> GetIconBytesForModelAsync(IEntryModel model, CancellationToken ct)
        {
            var diskModel = model as DiskEntryModelBase;
            if (diskModel != null && diskModel.IsFileWithExtension(FileExtensions.ExifExtensions))
            {
                using (var stream =
                    await diskModel.DiskProfile.DiskIO.OpenStreamAsync(diskModel,
                        FileExplorer.Defines.FileAccess.Read, ct))
                {
                    using (ExifReader reader = new ExifReader(stream))
                    {
                        var thumbnailBytes = reader.GetJpegThumbnailBytes();
                        if (thumbnailBytes != null && thumbnailBytes.Length > 0)
                            return thumbnailBytes;
                    }
                }

            }

            return null;

        }
    }
}
