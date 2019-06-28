using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Streams.Resources {
	public class ResourceReaderStream : Stream {
		private readonly Stream stream;
		private readonly string key;
		private List<byte> value;
		private readonly Buffer buffer = new Buffer();
		private int pointer;

		public override bool CanRead { get; }
		public override bool CanSeek { get; }
		public override bool CanWrite { get; }
		public override long Length { get; }
		public override long Position { get; set; }

		public ResourceReaderStream(Stream stream, string key) {
			this.stream = stream;
			this.key = key;
			SeekValue();
		}

		public override int Read(byte[] buffer, int offset, int count) {
			count = Math.Min(count, value.Count - pointer);
			for (int i = 0; i < count; i++) {
				buffer[i + offset] = value[i + pointer];
			}

			pointer += count;
			return count;
		}

		private void SeekValue() {
			// while not end of stream read next field pair, compare with key and skip value if it doesn't equal to key
			while (true) {
				var keyBytes = ReadToken();
				if (keyBytes == null) {
					value = new List<byte>();
					break;
				}

				var k = Encoding.ASCII.GetString(keyBytes.ToArray());
				if (k != key) {
					ReadToken();
				}
				else {
					value = ReadToken();
					break;
				}
			}
		}

		private List<byte> ReadToken() {
			var token = new List<byte>();
			var chunk = buffer.Chunk;
			while (true) {
				if (buffer.IsScanned) {
					buffer.Len = stream.Read(chunk, 0, Constants.BufferSize);
				}

				if (buffer.Len == 0) {
					return null;
				}

				for (int i = buffer.Pos; i < buffer.Len; i++) {
					// \0 byte
					if (i > 1 && chunk[i - 1] == 0 && chunk[i] == 0) {
						continue;
					}

					// end of token
					if ((i > 0 && chunk[i - 1] == 0) && chunk[i] == 1) {
						buffer.IsScanned = false;
						buffer.Pos = i + 1;
						token.RemoveAt(token.Count - 1);
						return token;
					}

					token.Add(chunk[i]);
				}

				buffer.IsScanned = true;
				buffer.Pos = 0;
			}
		}

		public override void Flush() {
			// nothing to do
		}

		public override void Write(byte[] buffer, int offset, int count) {
			throw new NotImplementedException();
		}

		public override long Seek(long offset, SeekOrigin origin) {
			throw new NotImplementedException();
		}

		public override void SetLength(long value) {
			throw new NotImplementedException();
		}
	}

	class Buffer {
		public byte[] Chunk { get; } = new byte[Constants.BufferSize];
		public int Len { get; set; }
		public int Pos { get; set; }
		public bool IsScanned { get; set; } = true;
	}
}