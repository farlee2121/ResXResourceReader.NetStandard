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
using System.IO;
using System.Linq;
using System.Text;
using System.ComponentModel.Design;

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

            var tempFilePath = Path.GetTempFileName();
            using (ResXResourceWriter resx = new ResXResourceWriter(tempFilePath))
            {
                resx.AddResource(dataNode);
            }
            File.Delete(tempFilePath);
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

            StringBuilder resxOutput = new StringBuilder();
            using (ResXResourceWriter writer = new ResXResourceWriter(new StringWriter(resxOutput)))
            {
                writer.AddResource(dataNode);
            }
            using (ResXResourceReader reader = new ResXResourceReader(new StringReader(resxOutput.ToString())))
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

        private List<ResXDataNode> ReaderToNodes(ResXResourceReader reader)
        {
            List<ResXDataNode> nodes = new List<ResXDataNode>();

            IDictionaryEnumerator dict = reader.GetEnumerator();
            while (dict.MoveNext())
            {
                nodes.Add((ResXDataNode)dict.Value);
            }

            return nodes;
        }

        [Fact]
        public void ResxDataNode_ResXFileRefsWrittenBackWithSameAssemblyInfo()
        {
            // This test ensures compatibility with tooling like Visual Studio's visual ResX editor
            
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            
            string originalResx = Example.ResxWithFileRef;
            StringBuilder writerOutput = new StringBuilder();

            using (var reader = new ResXResourceReader(new StringReader(originalResx)))
            {
                reader.UseResXDataNodes = true;
                var dataNodes = ReaderToNodes(reader);

                using (ResXResourceWriter writer = new ResXResourceWriter(new StringWriter(writerOutput)))
                {
                    dataNodes.ForEach(writer.AddResource);
                    writer.Generate();
                }
            }

            Assert.Equal(originalResx, writerOutput.ToString().Replace("\r\n", "\n"));
        }

        [Fact]
        public void ResxDataNode_CustomTypeConvertersDontOverwriteDefaultConverters()
        {
            string expectedIntTypeName = "some-text";
            string customTypeConverter(Type type)
            {
                if (type == typeof(int)) return expectedIntTypeName;
                else return null;
            }

            var intNode = new ResXDataNode("int-node", 1, customTypeConverter);

            var fileRefNode = new ResXDataNode("file-node", new ResXFileRef("i-am-file.txt", typeof(string).AssemblyQualifiedName), customTypeConverter);


            var expected =
                (
                    ("int-node", expectedIntTypeName),
                    ("file-node", NetStandard.ResXConstants.ResxFileRefTypeInfo)
                );
            var actual =
                (
                    ("int-node", intNode.GetDataNodeInfo().TypeName),
                    ("file-node", fileRefNode.GetDataNodeInfo().TypeName)
                );

            Assert.Equal(expected, actual);
        }
    }
}
