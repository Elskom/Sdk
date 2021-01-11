namespace XmlAbstraction.Benchmark
{
    using System;
    using System.IO;
    using System.Text;
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Jobs;
    using XmlAbstraction;

    // hope this change actually works.
    [SimpleJob(runtimeMoniker: RuntimeMoniker.Net461, baseline: true)]
    [SimpleJob(runtimeMoniker: RuntimeMoniker.Net462, baseline: true)]
    [SimpleJob(runtimeMoniker: RuntimeMoniker.Net47, baseline: true)]
    [SimpleJob(runtimeMoniker: RuntimeMoniker.Net471, baseline: true)]
    [SimpleJob(runtimeMoniker: RuntimeMoniker.Net472, baseline: true)]
    [SimpleJob(runtimeMoniker: RuntimeMoniker.Mono)]
    [SimpleJob(runtimeMoniker: RuntimeMoniker.NetCoreApp20)]
    [SimpleJob(runtimeMoniker: RuntimeMoniker.NetCoreApp21)]
    [SimpleJob(runtimeMoniker: RuntimeMoniker.NetCoreApp22)]
    [SimpleJob(runtimeMoniker: RuntimeMoniker.NetCoreApp30)]
    [SimpleJob(runtimeMoniker: RuntimeMoniker.CoreRt20)]
    [SimpleJob(runtimeMoniker: RuntimeMoniker.CoreRt21)]
    [SimpleJob(runtimeMoniker: RuntimeMoniker.CoreRt22)]
    [SimpleJob(runtimeMoniker: RuntimeMoniker.CoreRt30)]
    // [ClrJob(baseline: true), CoreJob, MonoJob, CoreRtJob]
    [RPlotExporter, RankColumn]
    [InProcess]
    public class XmlObjectBenchmarks
    {
        private XmlObject xmlObj;
        private readonly string XmlFile = $"{Environment.CurrentDirectory}{Path.DirectorySeparatorChar}BenchmarkTest.xml";
        
        [Params(@"<?xml version=""1.0"" encoding=""utf-8"" ?><test></test>", @"<?xml version=""1.0"" encoding=""utf-8"" ?><test><test1 TestAttribute1=""0"">test</test1><test2 TestAttribute1=""0"">test2</test2></test>")]
        public string InputXml;
        
        [GlobalSetup]
        public void Setup()
        {
            using (var fstrm = File.Create(this.XmlFile))
            {
                fstrm.Write(Encoding.UTF8.GetBytes(this.InputXml), 0, this.InputXml.Length);
            }

            this.xmlObj = new XmlObject(this.XmlFile, this.InputXml);
        }

        [Benchmark]
        public void ReopenFile()
            => this.xmlObj.ReopenFile();

        [Benchmark]
        public void AddElement()
            => this.xmlObj.AddElement("test3", "test3");

        [Benchmark]
        public void AddAttribute()
            => this.xmlObj.AddAttribute("test3", "TestAttribute1", "0");

        [Benchmark]
        public void Write()
            => this.xmlObj.Write("test4", "test4");

        [Benchmark]
        public void Write2()
            => this.xmlObj.Write("test4", "TestAttribute1", "0");

        [Benchmark]
        public void Write3()
            => this.xmlObj.Write("test5", "testholder", new string[] { "test1", "test2", "test3", "test4" });

        [Benchmark]
        public void Read()
            => this.xmlObj.Read("test3");

        [Benchmark]
        public void Read2()
            => this.xmlObj.Read("test3", "TestAttribute1");

        [Benchmark]
        public void Read3()
            => this.xmlObj.Read("test5", "testholder", null);

        [Benchmark]
        public void TryRead()
            => this.xmlObj.TryRead("test6");

        [Benchmark]
        public void TryRead2()
            => this.xmlObj.TryRead("test7", "TestAttribute1");

        [Benchmark]
        public void TryRead3()
            => this.xmlObj.TryRead("test8", "testholder", null);

        [Benchmark]
        public void Delete()
            => this.xmlObj.Delete("test6");

        [Benchmark]
        public void Delete2()
            => this.xmlObj.Delete("test7", "TestAttribute1");

        [Benchmark]
        public void Save()
            => this.xmlObj.Save();

        [GlobalCleanup]
        public void Cleanup()
            => File.Delete(this.XmlFile);
    }
}
