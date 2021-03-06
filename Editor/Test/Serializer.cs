//
// Copyright (c) 2017 Geri Borbás http://www.twitter.com/_eppz
//
//  Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//  The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
using UnityEngine;
using UnityEditor;

using System;
using System.IO;
using NUnit.Framework;


namespace EPPZ.Persistence.Editor.Test
{


	using Entities;
	using Extensions;
	using Mode = EPPZ.Persistence.JSONSerializer.Mode;


	/// <summary>
	/// Being an `abstract class` tests will only run in subclasses.
	/// </summary>
	[TestFixture]
	public abstract class Serializer
	{


		/// <summary>
		/// Tests in this fixture covers base functionality of every `Serializer`.
		/// </summary>
		protected EPPZ.Persistence.Serializer serializer;

		// Folders.
		protected string resourcePath;
		protected string resourcesFolderPath;
		protected string testFolderPath;
		protected string tempFolderPath;

		// Model.
		protected Payload alpha, beta, gamma;
		protected Entity first, second, third, fourth;

		protected string first_string;
		protected string second_string;
		protected string third_string;
		protected string fourth_string;

		protected string first_string_zip;
		protected string second_string_zip;
		protected string third_string_zip;
		protected string fourth_string_zip;


		public virtual void Setup()
		{		
			// Models.
			alpha = new Payload(
				1,
				"Alpha",
				0,1,2,3,4,5,6,7,8,9
			);
			beta = new Payload(
				2,
				"Beta",
				0,1,2,3,4,5,6,7,8,9
			);
			gamma = new Payload(
				3,
				"Gamma",
				0,1,2,3,4,5,6,7,8,9
			);

			first = new Entity(
				1,
				"First",
				alpha,
				beta,
				gamma
			);

			second = new Entity(
				2,
				"Second",
				beta,
				gamma
			);

			third = new Entity(
				3,
				"Third",
				gamma
			);

			fourth = new Entity(
				4,
				"Fourth"
			);
		}


	#region Models

		[Test]
		public void PayloadEquals()
		{
			Assert.AreEqual(
				new Payload(1, "Alpha", 0,1,2,3,4,5,6,7,8,9),
				new Payload(1, "Alpha", 0,1,2,3,4,5,6,7,8,9)
			);

			Assert.AreEqual(
				new Payload(1, "Alpha", 0,1,2,3,4,5,6,7,8,9),
				alpha
			);

			Assert.AreEqual(
				alpha,
				alpha
			);
		}

		[Test]
		public void EntityEquals()
		{
			Assert.AreEqual(
				new Entity(3, "Third", gamma),
				new Entity(3, "Third", gamma)
			);

			Assert.AreEqual(
				new Entity(3, "Third", new Payload(3, "Gamma", 0,1,2,3,4,5,6,7,8,9)),
				new Entity(3, "Third", gamma)
			);

			Assert.AreEqual(
				new Entity(3, "Third", gamma),
				third
			);

			Assert.AreEqual(
				new Entity(3, "Third", new Payload(3, "Gamma", 0,1,2,3,4,5,6,7,8,9)),
				third
			);

			Assert.AreEqual(
				third,
				third
			);

			// `null` payloads.
			Assert.AreEqual(
				new Entity(0, "Empty"),
				new Entity(0, "Empty")
			);

			// `null` payloads.
			Assert.AreNotEqual(
				new Entity(3, "Third", new Payload(3, "Gamma", 0,1,2,3,4,5,6,7,8,9)),
				new Entity(0, "Empty")
			);

			// `null` payloads.
			Assert.AreNotEqual(
				new Entity(0, "Empty"),
				new Entity(3, "Third", new Payload(3, "Gamma", 0,1,2,3,4,5,6,7,8,9))
			);
		}

	#endregion


	#region Zip

		[Test]
		public void Zip()
		{
			Assert.AreEqual(
				first_string.Zip(),
				first_string_zip
			);

			Assert.AreEqual(
				second_string.Zip(),
				second_string_zip
			);

			Assert.AreEqual(
				third_string.Zip(),
				third_string_zip
			);

			Assert.AreEqual(
				fourth_string.Zip(),
				fourth_string_zip
			);
		}

		[Test]
		public void Unzip()
		{
			Assert.AreEqual(
				first_string_zip.Unzip(),
				first_string
			);

			Assert.AreEqual(
				second_string_zip.Unzip(),
				second_string
			);

			Assert.AreEqual(
				third_string_zip.Unzip(),
				third_string
			);

			Assert.AreEqual(
				fourth_string_zip.Unzip(),
				fourth_string
			);
		}

	#endregion


	#region String (Zip)

		[Test]
		public void StringToObject_Unzip()
		{
			Assert.AreEqual(
				serializer.StringToObject<Entity>(first_string_zip.Unzip()),
				first
			);

			Assert.AreEqual(
				serializer.StringToObject<Entity>(second_string_zip.Unzip()),
				second
			);

			Assert.AreEqual(
				serializer.StringToObject<Entity>(third_string_zip.Unzip()),
				third
			);

			Assert.AreEqual(
				serializer.StringToObject<Entity>(fourth_string_zip.Unzip()),
				fourth
			);

			// Error.
			Assert.IsNull(
				serializer.StringToObject<Entity>("<ERROR>".Unzip())
			);
		}

		[Test]
		public void ObjectToString_Zip()
		{
			Assert.AreEqual(
				serializer.ObjectToString(first).Zip(),
				first_string_zip
			);

			Assert.AreEqual(
				serializer.ObjectToString(second).Zip(),
				second_string_zip
			);
			
			Assert.AreEqual(
				serializer.ObjectToString(third).Zip(),
				third_string_zip
			);

			Assert.AreEqual(
				serializer.ObjectToString(fourth).Zip(),
				fourth_string_zip
			);
		}

	#endregion


	#region String

		[Test]
		public void StringToObject()
		{
			Assert.AreEqual(
				serializer.StringToObject<Entity>(first_string),
				first
			);

			Assert.AreEqual(
				serializer.StringToObject<Entity>(second_string),
				second
			);

			Assert.AreEqual(
				serializer.StringToObject<Entity>(third_string),
				third
			);

			Assert.AreEqual(
				serializer.StringToObject<Entity>(fourth_string),
				fourth
			);

			// Error.
			Assert.IsNull(
				serializer.StringToObject<Entity>("<ERROR>")
			);
		}

		[Test]
		public void DeserializeToObject()
		{
			serializer.SetDefaultSerializer();

			Assert.AreEqual(
				first_string.DeserializeToObject<Entity>(),
				first
			);

			Assert.AreEqual(
				second_string.DeserializeToObject<Entity>(),
				second
			);

			Assert.AreEqual(
				third_string.DeserializeToObject<Entity>(),
				third
			);

			Assert.AreEqual(
				fourth_string.DeserializeToObject<Entity>(),
				fourth
			);

			// Error.
			Assert.IsNull(
				"<ERROR>".DeserializeToObject<Entity>()
			);	
		}

		[Test]
		public void ObjectToString()
		{
			Assert.AreEqual(
				serializer.ObjectToString(first),
				first_string
			);

			Assert.AreEqual(
				serializer.ObjectToString(second),
				second_string
			);
			Assert.AreEqual(
				serializer.ObjectToString(third),
				third_string
			);

			Assert.AreEqual(
				serializer.ObjectToString(fourth),
				fourth_string
			);
		}

		[Test]
		public void SerializeToString()
		{
			serializer.SetDefaultSerializer();

			Assert.AreEqual(
				first.SerializeToString(),
				first_string
			);

			Assert.AreEqual(
				second.SerializeToString(),
				second_string
			);
			Assert.AreEqual(
				third.SerializeToString(),
				third_string
			);

			Assert.AreEqual(
				fourth.SerializeToString(),
				fourth_string
			);
		}

	#endregion


	#region File

		[Test]
		public void FileToObject()
		{
			Assert.AreEqual(
				serializer.FileToObject<Entity>(testFolderPath + "first"),
				first
			);

			Assert.AreEqual(
				serializer.FileToObject<Entity>(testFolderPath + "second"),
				second
			);

			Assert.AreEqual(
				serializer.FileToObject<Entity>(testFolderPath + "third"),
				third
			);

			Assert.AreEqual(
				serializer.FileToObject<Entity>(testFolderPath + "fourth"),
				fourth
			);

			// Error.
			Assert.IsNull(
				serializer.FileToObject<Entity>("<ERROR>")
			);
		}

		[Test]
		public void ObjectToFile()
		{		
			// Using primary file extension.
			serializer.ObjectToFile(first, tempFolderPath + "first_test");
			FileAssert.AreEqual(
				testFolderPath + "first".WithFileExtension(serializer),			
				tempFolderPath + "first_test".WithFileExtension(serializer)
			);

			serializer.ObjectToFile(second, tempFolderPath + "second_test");
			FileAssert.AreEqual(
				testFolderPath + "second".WithFileExtension(serializer),
				tempFolderPath + "second_test".WithFileExtension(serializer)
			);

			// Using seconday file extension (in `testFolderPath` folder).
			serializer.ObjectToFile(third, tempFolderPath + "third_test");
			FileAssert.AreEqual(
				(testFolderPath + "third.data").WithExistingFileExtension(serializer),
				tempFolderPath + "third_test".WithFileExtension(serializer)
			);

			serializer.ObjectToFile(fourth, tempFolderPath + "fourth_test");
			FileAssert.AreEqual(
				(testFolderPath + "fourth.data").WithExistingFileExtension(serializer),
				tempFolderPath + "fourth_test".WithFileExtension(serializer)
			);
		}

		[Test]
		public void SerializeToFileAt()
		{			
			first.SerializeToFileAt(tempFolderPath + "first_test");
			FileAssert.AreEqual(
				testFolderPath + "first".WithFileExtension(serializer),			
				tempFolderPath + "first_test".WithFileExtension(serializer)
			);

			second.SerializeToFileAt(tempFolderPath + "second_test");
			FileAssert.AreEqual(
				testFolderPath + "second".WithFileExtension(serializer),			
				tempFolderPath + "second_test".WithFileExtension(serializer)
			);

			third.SerializeToFileAt(tempFolderPath + "third_test");
			FileAssert.AreEqual(
				(testFolderPath + "third").WithExistingFileExtension(serializer),			
				tempFolderPath + "third_test".WithFileExtension(serializer)
			);

			fourth.SerializeToFileAt(tempFolderPath + "fourth_test");
			FileAssert.AreEqual(
				(testFolderPath + "fourth").WithExistingFileExtension(serializer),			
				tempFolderPath + "fourth_test".WithFileExtension(serializer)
			);
		}

		[Test]
		public void FileToObject_Extensions()
		{
			// Extension gets added silently (See `Serializer.GetFilePathWithFileExtension()`).
			Assert.AreEqual(
				serializer.FileToObject<Entity>(testFolderPath + "first"),
				first
			);

			// Any other extension gets replaced silently.
			Assert.AreEqual(
				serializer.FileToObject<Entity>(testFolderPath + "second.json"),
				second
			);

			Assert.AreEqual(
				serializer.FileToObject<Entity>(testFolderPath + "third.zip"),
				third
			);

			Assert.AreEqual(
				serializer.FileToObject<Entity>(testFolderPath + "fourth.bytes"),
				fourth
			);
		}

		[Test]
		public void ObjectToFile_ManageFileExtensions()
		{
			// Extension gets added silently (See `Serializer.GetExistingFilePathWithFileExtensions()`).
			serializer.ObjectToFile(first, tempFolderPath+"first_test");
			FileAssert.AreEqual(
				testFolderPath + "first_test."+serializer.PrimaryFileExtension,
				tempFolderPath + "first_test."+serializer.PrimaryFileExtension
			);

			// `WithFileExtension` creates a new path using primary extension (even if the file in non-existent).
			serializer.ObjectToFile(second, tempFolderPath+"second_test.json"); // `json` get replaced
			FileAssert.AreEqual(
				testFolderPath + "second_test".WithFileExtension(serializer),
				tempFolderPath + "second_test".WithFileExtension(serializer)
			);

			// `WithExistingFileExtension` only returns a path is there is an existing file there (it iterates over every defined file extension).
			serializer.ObjectToFile(third, tempFolderPath+"third_test.zip"); // `zip` get replaced
			FileAssert.AreEqual(
				(testFolderPath + "third_test").WithExistingFileExtension(serializer),
				tempFolderPath + "third_test".WithFileExtension(serializer)
			);

			serializer.ObjectToFile(fourth, tempFolderPath + "fourth_test.bytes"); // `bytes` get replaced
			FileAssert.AreEqual(
				(testFolderPath + "fourth_test").WithExistingFileExtension(serializer),
				tempFolderPath + "fourth_test".WithFileExtension(serializer)
			);
		}

	#endregion


	#region Resource

		[Test]
		public void ResourceToObject()
		{
			Assert.AreEqual(
				first,
				serializer.ResourceToObject<Entity>(resourcePath + "first")
			);
			
			Assert.AreEqual(
				second,
				serializer.ResourceToObject<Entity>(resourcePath + "second")
			);
			
			Assert.AreEqual(
				third,
				serializer.ResourceToObject<Entity>(resourcePath + "third")
			);
			
			Assert.AreEqual(
				fourth,
				serializer.ResourceToObject<Entity>(resourcePath + "fourth")
			);
		}

		[Test]
		public void ObjectToResource()
		{			
			// Write to `Assets/Resources`, import, then read back as resource (using `UnityEngine.Resources`).
			serializer.ObjectToFile(first, resourcesFolderPath + "first_test");
			AssetDatabase.ImportAsset("Assets/Resources/" + resourcePath + "first_test".WithFileExtension(serializer));
			Assert.AreEqual(
				File.ReadAllBytes(testFolderPath + "first_test".WithFileExtension(serializer)),
				(Resources.Load(resourcePath + "first_test") as TextAsset).bytes
			);

			serializer.ObjectToFile(second, resourcesFolderPath + "second_test");
			AssetDatabase.ImportAsset("Assets/Resources/" + resourcePath + "second_test".WithFileExtension(serializer));
			Assert.AreEqual(
				File.ReadAllBytes(testFolderPath + "second_test".WithFileExtension(serializer)),
				(Resources.Load(resourcePath + "second_test") as TextAsset).bytes
			);

			// Using `WithExistingFileExtension` to take care of secondary extensions.
			serializer.ObjectToFile(third, resourcesFolderPath + "third_test");
			AssetDatabase.ImportAsset("Assets/Resources/" + resourcePath + "third_test".WithFileExtension(serializer));
			Assert.AreEqual(
				File.ReadAllBytes((testFolderPath + "third_test").WithExistingFileExtension(serializer)),
				(Resources.Load(resourcePath + "third_test") as TextAsset).bytes
			);

			serializer.ObjectToFile(fourth, resourcesFolderPath + "fourth_test");
			AssetDatabase.ImportAsset("Assets/Resources/" + resourcePath + "fourth_test".WithFileExtension(serializer));
			Assert.AreEqual(
				File.ReadAllBytes((testFolderPath + "fourth_test").WithExistingFileExtension(serializer)),
				(Resources.Load(resourcePath + "fourth_test") as TextAsset).bytes
			);
		}

	#endregion


	#region File or Resource

		[Test]
		public void FileOrResourceToObject()
		{
			// Load file if available.
			Assert.AreEqual(
				serializer.FileOrResourceToObject<Entity>(
					testFolderPath+"first",
					"<ERROR>"
				),
				first
			);

			// Load resource if no file available.
			Assert.AreEqual(
				serializer.FileOrResourceToObject<Entity>(
					"<ERROR>",
					resourcePath + "first"
				),
				first
			);

			// Error.
			Assert.IsNull(
				serializer.FileOrResourceToObject<Entity>(
					"<ERROR>",
					"<ERROR>"
				)
			);
		}

	#endregion	


	}
}