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
using System.Runtime.Serialization.Formatters.Binary;


namespace EPPZ.Persistence
{


	using Extensions;


    public class BinarySerializer : Serializer
    {
        

	#region Overrides

		public override string[] FileExtensions
		{ get { return new string[]{ "bytes", "txt", "data" }; } }

	#endregion


	#region String

		public override string SerializeObjectToString(object _object)
		{
 			Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes"); // Fix iOS AOT issue
			BinaryFormatter formatter = new BinaryFormatter();
			using (MemoryStream stream = new MemoryStream())
    		{
        		formatter.Serialize(stream, _object);
        		return stream.ToArray().Base64String();
    		}
		}

		public override T DeserializeString<T>(string _string)
		{ 
			T _object = default(T);
			try
			{
				Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes"); // Fix iOS AOT issue
				using (MemoryStream stream = new MemoryStream(_string.Base64Bytes()))
   				{
        			BinaryFormatter formatter = new BinaryFormatter();
        			_object = (T)formatter.Deserialize(stream);
    			}
			}
			catch (Exception exception)
			{ Debug.LogWarning("Serializer exception: "+exception); }

			return _object;
		}

	#endregion


	#region File

		public override void SerializeObjectToFile(object _object, string filePath)
		{
            Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes"); // Fix iOS AOT issue
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream file = File.Create(filePath.WithFileExtension(this));
			formatter.Serialize(file, _object);
			file.Close();
        }

		public override T DeserializeFile<T>(string filePath)
		{
            if (IsFileExistWithFileExtensions(filePath) == false) return default(T); // Only if saved any

			T _object = default(T);
			try
			{
				Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes"); // Fix iOS AOT issue
				BinaryFormatter formatter = new BinaryFormatter();
				FileStream file = File.Open(GetExistingFilePathWithFileExtensions(filePath), FileMode.Open);
				_object = (T)formatter.Deserialize(file);
				file.Close();
			}
			catch (Exception exception)
			{ Debug.LogWarning("Serializer exception: "+exception); }

			return _object;
        }

	#endregion


	#region Resource

		public override T DeserializeResource<T>(string resourcePath)
		{
            T _object = default(T);
			try
			{
				Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes"); // Fix iOS AOT issue
				TextAsset textAsset = Resources.Load(resourcePath) as TextAsset;
				if (textAsset == null)
				{ return _object; }
				Stream stream = new MemoryStream(textAsset.bytes);
				BinaryFormatter formatter = new BinaryFormatter();
				_object = (T)formatter.Deserialize(stream);
			}
			catch (Exception exception)
			{ Debug.LogWarning("Serializer exception: "+exception); }

			return _object;
        }

	#endregion
	

    }   
}