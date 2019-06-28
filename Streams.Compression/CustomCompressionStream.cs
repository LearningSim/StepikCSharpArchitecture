using System;
using System.Collections.Generic;
using System.IO;

namespace Streams.Compression
{
	internal class CustomCompressionStream
	{
		private readonly MemoryStream baseStream;
		private bool readOnly;
		private byte[] readBuffer = new byte[0];

		public CustomCompressionStream(MemoryStream baseStream, bool readOnly)
		{
			this.baseStream = baseStream;
			this.readOnly = readOnly;
		}

		public void Write(byte[] data, int offset, int count)
		{
			var compressed = new List<byte>();
			for (int i = offset; i < offset + count; i++)
			{
				byte newByte = data[i];
				byte adjancentDuplicates = CountAdjancentDuplicates(data, i, offset + count);

				compressed.Add(newByte);
				compressed.Add(adjancentDuplicates);

				i += adjancentDuplicates - 1;
			}

			baseStream.Write(compressed.ToArray(), offset, compressed.Count);
		}

		public int Read(byte[] buffer, int offset, int count)
		{
			int max = offset + count;

			readBuffer.CopyTo(buffer, 0);
			offset += readBuffer.Length;

			while (true)
			{
				if (baseStream.Position >= baseStream.Length)
				{
					return offset;
				}

				var bytes = ReadSequence();
				int len = Math.Min(bytes.Length, max - offset);
				Array.Copy(bytes, 0, buffer, offset, len);
				offset += len;

				if (len < bytes.Length)
				{
					readBuffer = new byte[bytes.Length - len];
					Array.Copy(bytes, len, readBuffer, 0, bytes.Length - len);
					break;
				}
			}

			return buffer.Length;
		}

		private byte CountAdjancentDuplicates(byte[] data, int offset, int max)
		{
			byte newByte = data[offset];
			byte byteCount = 1;
			offset++;
			while (offset < max && data[offset] == newByte)
			{
				offset++;
				byteCount++;
			}

			return byteCount;
		}

		private byte[] ReadSequence()
		{
			byte bt = (byte) baseStream.ReadByte();
			byte count = (byte) baseStream.ReadByte();
			byte[] bytes = new byte[count];
			for (int i = 0; i < count; i++)
			{
				bytes[i] = bt;
			}

			return bytes;
		}
	}
}