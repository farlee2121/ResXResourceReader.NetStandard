// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Drawing;
using System.Reflection;
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
                TypeName = "System.Resources.ResXFileRef, System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089",
                ValueData = "TestResources/Files/Test.xml;System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089;utf-8",
            };
            var dataNode = new ResXDataNode(nodeInfo, null);
            var typeResolver = new AssemblyNamesTypeResolutionService(Array.Empty<AssemblyName>());
            Assert.Equal("Test", dataNode.Name);
            Assert.Equal(@"<?xml version=""1.0"" encoding=""utf-8"" ?>
<Root>
    <Element>Text</Element>
</Root>", dataNode.GetValue(typeResolver));
        }
    }
}
