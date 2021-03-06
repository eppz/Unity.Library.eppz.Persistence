//
// Copyright (c) 2017 Geri Borbás http://www.twitter.com/_eppz
//
//  Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//  The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
using UnityEngine;

using System;
using System.IO;
using System.IO.Compression;
using System.Text;


namespace EPPZ.Persistence.Extensions
{


	public static class Bytes_Extensions
	{


		/// <summary>
		/// Convert bytes to UTF8 string.
		/// </summary>
		public static string String(this byte[] this_)
		{ return Encoding.UTF8.GetString(this_); }

		/// <summary>
		/// Convert bytes to Base64 string.
		/// </summary>
		public static string Base64String(this byte[] this_)
		{ return Convert.ToBase64String(this_); }

		/// <summary>
		/// Compress (Gzip) byte array (using `System.IO.Compression.GZipStream`).
		/// </summary>
		public static byte[] Compress(this byte[] this_)
		{
			if (this_.Length == 0) return this_; // Only if any

			using (MemoryStream inputStream = new MemoryStream(this_))
				using (MemoryStream outputStream = new MemoryStream())
				{
					using (GZipStream zipStream = new GZipStream(outputStream, CompressionMode.Compress))
					{ inputStream._CopyTo(zipStream); }
					return outputStream.ToArray();
				}
		}

		/// <summary>
		/// Decompress (Gzipped) byte array (using `System.IO.Compression.GZipStream`).
		/// </summary>
		public static byte[] Decompress(this byte[] this_)
		{
			if (this_.Length == 0) return this_; // Only if any

			using (MemoryStream inputStream = new MemoryStream(this_))
    			using (MemoryStream outputStream = new MemoryStream())
				{
					using (GZipStream zipStream = new GZipStream(inputStream, CompressionMode.Decompress))
					{ zipStream._CopyTo(outputStream); }
					return outputStream.ToArray();
				}
		}
	}
}