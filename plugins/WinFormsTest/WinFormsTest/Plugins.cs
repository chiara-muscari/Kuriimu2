﻿using Kontract.Attributes;
using Kontract.Interfaces.Common;
using Kontract.Interfaces.Text;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsTest
{
    [Export(typeof(ITextAdapter))]
    [Export(typeof(ILoadFiles))]
    [Export(typeof(IIdentifyFiles))]
    [PluginExtensionInfo("*.text")]
    [PluginInfo("Test-Text-Id")]
    public class TestTextPlugin : ITextAdapter, ILoadFiles, IIdentifyFiles, IRequestFiles
    {
        public IEnumerable<TextEntry> Entries => _texts.Select(x => new TextEntry { OriginalText = x });

        private IEnumerable<string> _texts;

        public event EventHandler<RequestFileEventArgs> RequestFile;

        public string NameFilter => throw new NotImplementedException();

        public int NameMaxLength => throw new NotImplementedException();

        public string LineEndings { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void Dispose()
        {
            ;
        }

        public bool Identify(string filename)
        {
            using (var br = new BinaryReader(File.OpenRead(filename)))
            {
                return br.ReadUInt32() == 0x16161616;
            }
        }

        private List<StreamInfo> _files;

        public void Load(StreamInfo file)
        {
            // Maybe open more files if needed by the format
            var args = new RequestFileEventArgs { FileName = Path.Combine(Path.GetDirectoryName(file.FileName), Path.GetFileNameWithoutExtension(file.FileName) + ".text2") };
            RequestFile(this, args);

            _files = new List<StreamInfo> { file, args.SelectedStreamInfo };

            // Here a format class can get initialized and all opened files passed in
            var buffer = new byte[5];
            _files[1].FileData.Read(buffer, 0, 5);
            _texts = new List<string> { "Text1", "Text2", "Text3", Encoding.ASCII.GetString(buffer) };
        }
    }
}
