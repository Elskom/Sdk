namespace XmlAbstraction.Test
{
    using System;
    using System.IO;
    using System.Text;
    using Xunit;

    public class XmlObjectUnitTest
    {
        private const string testXml = @"<?xml version=""1.0"" encoding=""utf-8"" ?><test></test>";

        private static void NoThrows(Action expression)
        {
            try
            {
                expression();
            }
            catch (Exception)
            {
                throw new Exception("Expression threw an exception.");
            }
        }

        [Fact]
        public void TestClassReopenFile()
        {
            var xmlObj = new XmlObject(testXml);
            Assert.Throws<InvalidOperationException>(() => xmlObj.ReopenFile());
            var fstrm = File.Create(
                $"{Environment.CurrentDirectory}{Path.DirectorySeparatorChar}test.xml");
            fstrm.Write(Encoding.UTF8.GetBytes(testXml), 0, testXml.Length);
            fstrm.Dispose();
            xmlObj = new XmlObject(
                $"{Environment.CurrentDirectory}{Path.DirectorySeparatorChar}test.xml",
                testXml);
            NoThrows(() => xmlObj.ReopenFile());
            File.Delete(
                $"{Environment.CurrentDirectory}{Path.DirectorySeparatorChar}test.xml");
        }

        [Fact]
        public void TestClassEdits()
        {
            var testXmlNoRoot = @"<test>
</test>";
            var xmlObj = new XmlObject(testXml);

            // test to make sure that InvalidOperationException is thrown.
            Assert.Throws<InvalidOperationException>(() => xmlObj.AddAttribute("test4", "test", "test"));
            Assert.Throws<InvalidOperationException>(() => xmlObj.Write("test", "test"));
            Assert.Throws<InvalidOperationException>(() => xmlObj.Write("test2", "test", "test"));
            Assert.Throws<InvalidOperationException>(() => xmlObj.Write("test3", "test31", new string[] { "test1", "test2", "test3" }));
            xmlObj.TryRead("test");
            xmlObj.TryRead("test2", "test");
            xmlObj.TryRead("test3", "test31", null);
            Assert.Throws<InvalidOperationException>(() => xmlObj.Delete("test"));
            Assert.Throws<InvalidOperationException>(() => xmlObj.Delete("test2", "test"));
            Assert.Throws<InvalidOperationException>(() => xmlObj.ReopenFile());
            xmlObj = new XmlObject(testXmlNoRoot);

            // reopen data from a file.
            var fstrm = File.Create(
                $"{Environment.CurrentDirectory}{Path.DirectorySeparatorChar}test.xml");
            fstrm.Write(Encoding.UTF8.GetBytes(testXml), 0, testXml.Length);
            fstrm.Dispose();
            xmlObj = new XmlObject(
                $"{Environment.CurrentDirectory}{Path.DirectorySeparatorChar}test.xml",
                testXml);
            NoThrows(() => xmlObj.AddAttribute("test4", "test", "test"));
            NoThrows(() => xmlObj.Write("test", "test"));
            NoThrows(() => xmlObj.Write("test2", "test", "test"));
            NoThrows(() => xmlObj.Write("test3", "test", new string[] { "test1", "test2", "test3" }));
            xmlObj.TryRead("test");
            xmlObj.TryRead("test2", "test");
            xmlObj.TryRead("test3", "test", null);
            xmlObj.TryRead("test4");
            NoThrows(() => xmlObj.ReopenFile());
            NoThrows(() => xmlObj.Write("test", "testnew"));
            xmlObj.TryRead("test");
            xmlObj.TryRead("test2", "test");
            xmlObj.TryRead("test3", "test", null);
            NoThrows(() => xmlObj.Delete("test"));
            NoThrows(() => xmlObj.Delete("test2", "test"));
            NoThrows(() => xmlObj.Save());
            File.Delete(
                $"{Environment.CurrentDirectory}{Path.DirectorySeparatorChar}test.xml");
            xmlObj = new XmlObject($"{Path.DirectorySeparatorChar}test.xml", testXml, true);
            xmlObj = new XmlObject($"{Path.DirectorySeparatorChar}test.xml", testXml, true);
            xmlObj = new XmlObject($"{Environment.CurrentDirectory}{Path.DirectorySeparatorChar}test.xml", testXml, true);
            Assert.Throws<ArgumentNullException>(() => xmlObj = new XmlObject(null, testXml, true));
        }

        [Fact]
        public void Test_contructor_root_missing_Fail()
            => Assert.Throws<ArgumentNullException>(() => new XmlObject(""));

        [Fact]
        public void Test_create_file_current_directory_Pass()
        {
            var testXmlFile = @"testCreate.xml";
            if (File.Exists($"{Environment.CurrentDirectory}{Path.DirectorySeparatorChar}" + testXmlFile))
            {
                File.Delete($"{Environment.CurrentDirectory}{Path.DirectorySeparatorChar}" + testXmlFile);
            }

            Assert.False(File.Exists(testXmlFile));
            var xmlObj = new XmlObject(testXmlFile, testXml, true);
            xmlObj.Save();
            Assert.True(File.Exists(testXmlFile));
        }

        // I do not like using "C:\" in case there is no "C:\"
        // or if "C:\" is not Windows. As such I really like the %SystemDrive% value.

        [Fact]
        public void Test_create_file_remote_directory_Pass()
        {
            // test with a real directory
            var testXmlFile = @"C:\Temp\testCreate.xml";
            if (File.Exists(testXmlFile))
            {
                File.Delete(testXmlFile);
            }

            if (!Directory.Exists(@"C:\Temp\"))
            {
                // create if this directory does not exist so this test pass.
                Directory.CreateDirectory(@"C:\Temp\");
            }

            Assert.False(File.Exists(testXmlFile));
            var xmlObj = new XmlObject(testXmlFile, testXml);
            xmlObj.Save();
            Assert.True(File.Exists(testXmlFile));
            File.Delete(testXmlFile);
            Directory.Delete(@"C:\Temp\");
        }

        // Seems that under AppVeyor, the tests could
        // be running in Administrator mode and so this test fails.
        // We need to somehow force this test to run in normal user mode instead.

        [Fact]
        public void Test_create_file_remote_violation_Fail()
        {
            // test with a real directory that doesn't have write access
            var testXmlFile = @"C:\Windows\WinSxS\testCreate.xml";
            if (File.Exists(testXmlFile))
            {
                File.Delete(testXmlFile);
            }

            Assert.False(File.Exists(testXmlFile));
            var xmlObj = new XmlObject(testXmlFile, testXml);
            Assert.Throws<UnauthorizedAccessException>(() => xmlObj.Save());
            Assert.False(File.Exists(testXmlFile));
        }

        [Fact]
        public void Test_create_file_remote_not_found_Fail()
        {
            // test with a real directory that doesnt have write access
            var testXmlFile = @"C:\nothere\testCreate.xml";
            if (File.Exists(testXmlFile))
            {
                File.Delete(testXmlFile);
                Directory.Delete(@"C:\nothere\");
            }

            Assert.False(File.Exists(testXmlFile));
            Assert.Throws<DirectoryNotFoundException>(() => new XmlObject(testXmlFile, testXml));
            Assert.Throws<DirectoryNotFoundException>(() => new XmlObject($"{Environment.CurrentDirectory}{Path.DirectorySeparatorChar}nothere{Path.DirectorySeparatorChar}testCreate.xml", testXml, true));
        }

        [Fact]
        public void Test_add_attribute_Pass()
        {
            var testXmlFile = @"testAddAttribute.xml";
            if (File.Exists($"{Environment.CurrentDirectory}{Path.DirectorySeparatorChar}" + testXmlFile))
            {
                File.Delete($"{Environment.CurrentDirectory}{Path.DirectorySeparatorChar}" + testXmlFile);
            }

            Assert.False(File.Exists(testXmlFile));
            var element = "TestElement";
            var attribute = "TestAttribute";
            var attributeValue = "my cool value";
            var xmlObj = new XmlObject(testXmlFile, testXml, true);
            xmlObj.AddAttribute(element, attribute, attributeValue);
            xmlObj.Save();
            xmlObj.ReopenFile();
            var result = xmlObj.TryRead(element, attribute);
            Assert.Equal(result, attributeValue);
            File.Delete($"{Environment.CurrentDirectory}{Path.DirectorySeparatorChar}" + testXmlFile);
        }

        // Should be able to add attribute and update that attribute if it hasnt been saved
        [Fact]
        public void Test_add_update_attribute_Pass()
        {
            var testXmlFile = @"testAddAttribute.xml";
            if (File.Exists($"{Environment.CurrentDirectory}{Path.DirectorySeparatorChar}" + testXmlFile))
            {
                File.Delete($"{Environment.CurrentDirectory}{Path.DirectorySeparatorChar}" + testXmlFile);
            }

            Assert.False(File.Exists(testXmlFile));
            var element = "TestElement";
            var attribute = "TestAttribute";
            var attributeValue = "my cool value";
            var newAttributeValue = "my new cool value";
            var xmlObj = new XmlObject(testXmlFile, testXml, true);
            xmlObj.AddAttribute(element, attribute, attributeValue);
            xmlObj.AddAttribute(element, attribute, newAttributeValue);
            xmlObj.Save();
            xmlObj.ReopenFile();
            var result = xmlObj.TryRead(element, attribute);
            Assert.Equal(result, newAttributeValue);
            File.Delete($"{Environment.CurrentDirectory}{Path.DirectorySeparatorChar}" + testXmlFile);
        }

        // Should not be able to update attribute that exists if the file has been loaded/saved
        [Fact]
        public void Test_update_attribute_Fail()
        {
            var testXmlFile = @"testAddAttribute.xml";
            if (File.Exists($"{Environment.CurrentDirectory}{Path.DirectorySeparatorChar}" + testXmlFile))
            {
                File.Delete($"{Environment.CurrentDirectory}{Path.DirectorySeparatorChar}" + testXmlFile);
            }

            Assert.False(File.Exists(testXmlFile));
            var element = "TestElement";
            var attribute = "TestAttribute";
            var attributeValue = "my cool value";
            var newAttributeValue = "my new cool value";
            var xmlObj = new XmlObject(testXmlFile, testXml, true);
            xmlObj.AddAttribute(element, attribute, attributeValue);
            xmlObj.Save();
            Assert.Throws<Exception>(() => xmlObj.AddAttribute(element, attribute, newAttributeValue));
            File.Delete($"{Environment.CurrentDirectory}{Path.DirectorySeparatorChar}" + testXmlFile);
        }

        [Fact]
        public void Test_add_element_Pass()
        {
            var testXmlFile = @"testAddElement.xml";
            if (File.Exists($"{Environment.CurrentDirectory}{Path.DirectorySeparatorChar}" + testXmlFile))
            {
                File.Delete($"{Environment.CurrentDirectory}{Path.DirectorySeparatorChar}" + testXmlFile);
            }

            Assert.False(File.Exists(testXmlFile));
            var element = "TestElement";
            var elementValue = "element value";
            var xmlObj = new XmlObject(testXmlFile, testXml, true);
            xmlObj.Write(element, elementValue);
            xmlObj.Save();
            xmlObj.ReopenFile();
            var result = xmlObj.TryRead(element);
            Assert.Equal(result, elementValue);
            File.Delete($"{Environment.CurrentDirectory}{Path.DirectorySeparatorChar}" + testXmlFile);
        }

        [Fact]
        public void Test_delete_file_element_Pass()
        {
            var testXmlFile = @"testDelElement.xml";
            if (File.Exists($"{Environment.CurrentDirectory}{Path.DirectorySeparatorChar}" + testXmlFile))
            {
                File.Delete($"{Environment.CurrentDirectory}{Path.DirectorySeparatorChar}" + testXmlFile);
            }

            Assert.False(File.Exists(testXmlFile));
            var element = "TestElement";
            var elementValue = "element value";
            var xmlObj = new XmlObject(testXmlFile, testXml, true);
            xmlObj.Write(element, elementValue);
            xmlObj.Save();
            xmlObj.ReopenFile();
            xmlObj.Delete(element);
            xmlObj.Save();
            xmlObj.ReopenFile();
            var result = xmlObj.TryRead(element);
            Assert.NotEqual(result, element);
            File.Delete($"{Environment.CurrentDirectory}{Path.DirectorySeparatorChar}" + testXmlFile);
        }

        [Fact]
        public void Test_Subelements()
        {
            var testXmlFile = @"testCreate.xml";
            if (File.Exists($"{Environment.CurrentDirectory}{Path.DirectorySeparatorChar}" + testXmlFile))
            {
                File.Delete($"{Environment.CurrentDirectory}{Path.DirectorySeparatorChar}" + testXmlFile);
            }

            var xmlObj = new XmlObject(testXmlFile, testXml, true);
            xmlObj.Save();
            var values = new string[] {"test subelement value 1", "test subelement value 2"};
            xmlObj.Write("testsubelements", "subelement", values);
            Assert.NotEqual(Array.Empty<string>(), xmlObj.TryRead("testsubelements", "subelement", null));
            xmlObj.Save();
            Assert.Equal(values, xmlObj.TryRead("testsubelements", "subelement", null));
            File.Delete($"{Environment.CurrentDirectory}{Path.DirectorySeparatorChar}" + testXmlFile);
        }
    }
}
