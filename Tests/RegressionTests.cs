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

        [Theory]
        [InlineData("1.0.24.0")]
        [InlineData("4.0.0.0")]
        [InlineData("2.0.3500.0")]
        public void ReadNotEffectedByResheaderVersions(string version){
            string resx = Tests.ResxContants.DefaultResxWithVersion(new System.Version(version));   
            var resReader = ResXResourceReader.FromFileContents(resx);
            resReader.GetEnumerator();
        }
    }

}