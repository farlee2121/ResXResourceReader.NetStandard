using Xunit;


namespace System.Resources.NetStandard.Tests
{

    public class RegressionTests
    {
        [Fact]
        public void MissingManifestResource(){
            // NOTE: the cause of this issue was an incorrect namespace in SR.designer.cs
            //       Appears to only be triggered when it tries to load an error message string from resource file
            Assert.Throws<InvalidOperationException>(() =>{
                using(var writeStream = new System.IO.MemoryStream()){
                    ResXResourceWriter writer = new ResXResourceWriter(writeStream);
                    writer.Generate();
                    writer.Generate();
                    // writer.AddResource("this is name", new byte[0]);
                }
            });
        }
    }

}