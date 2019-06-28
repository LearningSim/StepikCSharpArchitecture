using System;
using System.Collections.Generic;
using System.IO;

namespace Streams.Compression
{
	internal class CustomCompressionStream
	{
		private readonly MemoryStream stream;
		private byte[] readBuffer = new byte[0];
		private readonly Buffer destination = new Buffer();

		public CustomCompressionStream(MemoryStream baseStream, bool readOnly)
		{
			stream = baseStream;
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

			stream.Write(compressed.ToArray(), offset, compressed.Count);
		}

		public int Read(byte[] buffer, int offset, int count)
		{
			destination.Reset(buffer, offset, count);

			// read remains from buffer
			destination.CopyFrom(readBuffer);

			while (true)
			{
				var streamDepleted = stream.Position >= stream.Length;
				if (streamDepleted)
				{
					return destination.Offset;
				}

				// read the next portion 
				// which fit within out buffer free space
				var bytes = ReadSequence();
				int len = destination.CopyFrom(bytes);

				// save remains to buffer
				if (len < bytes.Length)
				{
					readBuffer = GetSubarray(bytes, len);
					break;
				}
			}

			return buffer.Length;
		}

		private byte CountAdjancentDuplicates(byte[] data, int offset, int max)
		{
			byte bt = data[offset];
			byte byteCount = 1;
			offset++;
			while (offset < max && data[offset] == bt)
			{
				offset++;
				byteCount++;
			}

			return byteCount;
		}

		private byte[] ReadSequence()
		{
			byte bt = (byte) stream.ReadByte();
			byte len = (byte) stream.ReadByte();
			byte[] bytes = new byte[len];
			for (int i = 0; i < len; i++)
			{
				bytes[i] = bt;
			}

			return bytes;
		}

		private byte[] GetSubarray(byte[] array, int startIndex)
		{
			var sub = new byte[array.Length - startIndex];
			Array.Copy(array, startIndex, sub, 0, array.Length - startIndex);
			return sub;
		}
	}

	internal class Buffer
	{
		public byte[] Data { get; private set; }
		public int Offset { get; private set; }
		public int Count { get; private set; }
		public int Max { get; private set; }
		public int AvailableSpace => Max - Offset;

		public void Reset(byte[] data, int offset, int count)
		{
			Count = count;
			Offset = offset;
			Data = data;
			Max = offset + count;
		}

		public int CopyFrom(byte[] source)
		{
			int len = Math.Min(source.Length, AvailableSpace);
			Array.Copy(source, 0, Data, Offset, len);
			Offset += len;
			return len;
		}
	}
}