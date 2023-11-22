// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Collections;
using System.Drawing;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Resources.Tests;
using Xunit;

namespace System.Resources.NetStandard.Tests
{
    public class ResxDataNodeTests : IClassFixture<ThreadExceptionFixture>
    {
        [Fact]
        public void ResxDataNode_ResXFileRefConstructor()
        {
            var nodeName = "Node";
            var fileRef = new ResXFileRef(string.Empty, string.Empty);
            var dataNode = new ResXDataNode(nodeName, fileRef);

            Assert.Equal(nodeName, dataNode.Name);
            Assert.Same(fileRef, dataNode.FileRef);
        }

        [Fact]
        public void ResxDataNode_CreateForResXFileRef()
        {
            // Simulate an XML file stored in resources, which uses a ResXFileRef. The actual resx XML looks like:
            /*
               <assembly alias="System.Windows.Forms" name="System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" />
               <data name="Test" type="System.Resources.ResXFileRef, System.Windows.Forms">
                    <value>Test.xml;System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089;utf-8</value>
                </data>
            */

            var nodeInfo = new DataNodeInfo
            {
                Name = "Test",
                ReaderPosition = new Point(1, 2),
                TypeName = "System.Resources.ResXFileRef, System.Windows.Forms",
                ValueData = "TestResources/Files/FileRef.xml;System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089;utf-8",
            };
            var dataNode = new ResXDataNode(nodeInfo, null);
            var typeResolver = new AssemblyNamesTypeResolutionService(Array.Empty<AssemblyName>());
            Assert.Equal("Test", dataNode.Name);
            Assert.Equal(Example.FileRef, dataNode.GetValue(typeResolver));
            using (ResXResourceWriter resx = new ResXResourceWriter(@"C:\users\farle\Downloads\nya-test.resx"))
            {
                resx.AddResource(dataNode);
            }
        }

        [Fact]
        public void ResxDataNode_ResXFileRef_RoundTrip()
        {
            var referencedFileContent = Example.FileRef;
            var nodeInfo = new DataNodeInfo
            {
                Name = "Test",
                ReaderPosition = new Point(1, 2),
                TypeName = "System.Resources.ResXFileRef, System.Windows.Forms",
                ValueData = "TestResources/Files/FileRef.xml;System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089;utf-8",
            };
            var dataNode = new ResXDataNode(nodeInfo, null);
            var typeResolver = new AssemblyNamesTypeResolutionService(Array.Empty<AssemblyName>());
            
            string resxPath = @".\nya-test.resx";
            using (ResXResourceWriter writer = new ResXResourceWriter(resxPath))
            {
                writer.AddResource(dataNode);
            }
            using(ResXResourceReader reader = new ResXResourceReader(resxPath))
            {
                var dictionary = new Dictionary<object, object>();
                IDictionaryEnumerator dictionaryEnumerator = reader.GetEnumerator();
                while (dictionaryEnumerator.MoveNext())
                {
                    dictionary.Add(dictionaryEnumerator.Key, dictionaryEnumerator.Value);
                }

                Assert.Equal(referencedFileContent, dictionary.GetValueOrDefault(nodeInfo.Name));
            }
        }
    }
}
