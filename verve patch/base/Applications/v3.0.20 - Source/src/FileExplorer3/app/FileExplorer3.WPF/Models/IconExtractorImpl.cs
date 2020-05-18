using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using FileExplorer.WPF.Utils;
using FileExplorer.WPF.Utils;
using FileExplorer.Models;

namespace FileExplorer.WPF.Models
{
    public static class EntryModelIconExtractors
    {
        public static IModelIconExtractor<IEntryModel> ProvideValue(byte[] value)
        {
            return new ProvideValueIconExtractor(value);
        }
    }


    public class ProvideValueIconExtractor : IModelIconExtractor<IEntryModel>
    {
        private static byte[] Value { get; set; }

        public ProvideValueIconExtractor(byte[] value)
        {
            Value = value;
        }

        public Task<byte[]> GetIconBytesForModelAsync(IEntryModel model, CancellationToken ct)
        {
            return Task<ImageSource>.FromResult(Value);
        }
    }

  
    public class GetUriIcon : IModelIconExtractor<IEntryModel>
    {
        private Func<IEntryModel, Uri> _uriFunc;
        private System.Threading.CancellationToken CancellationToken;
        private Func<HttpClient> _clientFunc;
        public GetUriIcon(Func<IEntryModel, Uri> uriFunc, Func<HttpClient> clientFunc = null)
        {
            _uriFunc = uriFunc;
            _clientFunc = clientFunc ?? (() => new HttpClient());
        }


        public async Task<byte[]> GetIconBytesForModelAsync(IEntryModel model, CancellationToken ct)
        {
            var response = await _clientFunc().GetAsync(_uriFunc(model).AbsoluteUri, ct);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var output = await response.Content.ReadAsByteArrayAsync();

                BitmapImage retIcon = new BitmapImage();
                return new MemoryStream(output).ToArray();
            }
            return await new GetDefaultIcon().GetIconBytesForModelAsync(model, ct);
        }
    }
}
