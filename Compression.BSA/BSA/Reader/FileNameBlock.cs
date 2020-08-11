﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Wabbajack.Common;

namespace Compression.BSA
{
    internal class FileNameBlock
    {
        public readonly Lazy<ReadOnlyMemorySlice<byte>[]> Names;

        public FileNameBlock(BSAReader bsa, long position)
        {
            Names = new Lazy<ReadOnlyMemorySlice<byte>[]>(
                mode: System.Threading.LazyThreadSafetyMode.ExecutionAndPublication,
                valueFactory: () =>
                {
                    using var stream = bsa.GetStream();
                    stream.BaseStream.Position = position;
                    ReadOnlyMemorySlice<byte> data = stream.ReadBytes(checked((int)bsa._totalFileNameLength));
                    ReadOnlyMemorySlice<byte>[] names = new ReadOnlyMemorySlice<byte>[bsa._fileCount];
                    for (int i = 0; i < bsa._fileCount; i++)
                    {
                        var index = data.Span.IndexOf(default(byte));
                        if (index == -1)
                        {
                            throw new InvalidDataException("Did not end all of its strings in null bytes");
                        }
                        names[i] = data.Slice(0, index + 1);
                        var str = names[i].ReadStringTerm(bsa.HeaderType);
                        data = data.Slice(index + 1);
                    }
                    if (data.Length > 0)
                    {
                        throw new InvalidDataException("File name block did not parse all of its data");
                    }
                    return names;
                });
        }
    }
}
