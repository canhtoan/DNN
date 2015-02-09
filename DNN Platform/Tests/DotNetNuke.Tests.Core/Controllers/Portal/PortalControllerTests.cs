#region Copyright
// 
// DotNetNuke® - http://www.dotnetnuke.com
// Copyright (c) 2002-2014
// by DotNetNuke Corporation
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
// the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
// to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions 
// of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

#endregion
using System.Collections.Generic;
using System.IO;

using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Portals.Internal;

using Moq;

using NUnit.Framework;

namespace DotNetNuke.Tests.Core.Controllers.Portal
{
    [TestFixture]
    public class PortalControllerTests
    {
        private Mock<IPortalTemplateIO> _mockPortalTemplateIO;
        private const string HostMapPath = @"C:\path";

        private const string DefaultName = "Default";
        private static readonly string s_defaultPath = MakePath(DefaultName);
        private static readonly string s_defaultDePath = MakePath(DefaultName, "de-DE");
        private const string DefaultDeName = "R\u00FCckstellungs-Web site";
        private const string DefaultDeDescription = "A new german description";
        private static readonly Dictionary<string, string> s_defaultExpectationsDe = new Dictionary<string, string>
                                                                                    {
                                                                                        {"Name", DefaultDeName },
                                                                                        {"TemplateFilePath", s_defaultPath},
                                                                                        {"LanguageFilePath", s_defaultDePath},
                                                                                        {"CultureCode", "de-DE"},
                                                                                        {"Description", DefaultDeDescription}
                                                                                    };

        private static readonly string s_defaultUsPath = MakePath(DefaultName, "en-US");
        private static readonly Dictionary<string, string> s_defaultExpectationsUs = new Dictionary<string, string>
                                                                                    {
                                                                                        {"Name", DefaultName },
                                                                                        {"TemplateFilePath", s_defaultPath},
                                                                                        {"LanguageFilePath", s_defaultUsPath},
                                                                                        {"CultureCode", "en-US"},
                                                                                    };

        private const string StaticName = "Static";
        private static readonly string s_staticPath = MakePath(StaticName);
        private const string StaticDescription = "An description from a template file";
        private static readonly Dictionary<string, string> s_staticExpectations = new Dictionary<string, string>
                                                                                    {
                                                                                        {"Name", StaticName},
                                                                                        {"TemplateFilePath", s_staticPath},
                                                                                        {"Description", StaticDescription}
                                                                                    };

        private const string AlternateName = "Alternate";
        private static readonly string s_alternatePath = MakePath(AlternateName);
        private static readonly string s_alternateDePath = MakePath(AlternateName, "de-DE");
        private const string AlternateDeName = "Alternate in German";
        private static readonly Dictionary<string, string> s_alternateExpectationsDe = new Dictionary<string, string>
                                                                                    {
                                                                                        {"Name", AlternateDeName },
                                                                                        {"TemplateFilePath", s_alternatePath},
                                                                                        {"LanguageFilePath", s_alternateDePath},
                                                                                        {"CultureCode", "de-DE"},
                                                                                    };

        private const string ResourceName = "Resource";
        private static readonly string s_resourcePath = MakePath(ResourceName);
        private static readonly string s_resourceFilePath = s_resourcePath + ".resources";
        private static readonly Dictionary<string, string> s_resourceExpectations = new Dictionary<string, string>
                                                                                    {
                                                                                        {"Name", ResourceName},
                                                                                        {"TemplateFilePath", s_resourcePath},
                                                                                        {"ResourceFilePath", s_resourceFilePath}
                                                                                    };

        [SetUp]
        public void SetUp()
        {
            _mockPortalTemplateIO = new Mock<IPortalTemplateIO>();
            PortalTemplateIO.SetTestableInstance(_mockPortalTemplateIO.Object);
        }

        [TearDown]
        public void TearDown()
        {
            PortalTemplateIO.ClearInstance();
        }

        [Test]
        public void NoTemplatesReturnsEmptyList()
        {
            //Arrange


            //Act
            var templates = PortalController.Instance.GetAvailablePortalTemplates();

            //Assert
            Assert.AreEqual(0, templates.Count);
        }

        [Test]
        public void LanguageFileWithoutATemplateIsIgnored()
        {
            //Arrange
            _mockPortalTemplateIO.Setup(x => x.EnumerateLanguageFiles()).Returns(ToEnumerable(s_defaultDePath));

            //Act
            var templates = PortalController.Instance.GetAvailablePortalTemplates();

            //Assert
            Assert.AreEqual(0, templates.Count);
        }

        [Test]
        public void TemplatesWithNoLanguageFilesAreLoaded()
        {
            //Arrange
            _mockPortalTemplateIO.Setup(x => x.EnumerateTemplates()).Returns(ToEnumerable(s_staticPath));
            _mockPortalTemplateIO.Setup(x => x.OpenTextReader(s_staticPath)).Returns(CreateTemplateFileReader(StaticDescription));

            //Act
            var templates = PortalController.Instance.GetAvailablePortalTemplates();

            //Assert
            Assert.AreEqual(1, templates.Count);
            AssertTemplateInfo(s_staticExpectations, templates[0]);
        }


        [Test]
        public void TemplateWith2Languages()
        {
            //Arrange
            _mockPortalTemplateIO.Setup(x => x.EnumerateTemplates()).Returns(ToEnumerable(s_defaultPath));
            _mockPortalTemplateIO.Setup(x => x.EnumerateLanguageFiles()).Returns(ToEnumerable(s_defaultDePath, s_defaultUsPath));
            _mockPortalTemplateIO.Setup(x => x.GetLanguageFilePath(s_defaultPath, "de-DE")).Returns(s_defaultDePath);
            _mockPortalTemplateIO.Setup(x => x.OpenTextReader(s_defaultDePath)).Returns(CreateLanguageFileReader(DefaultDeName, DefaultDeDescription));
            _mockPortalTemplateIO.Setup(x => x.GetLanguageFilePath(s_defaultPath, "en-US")).Returns(s_defaultUsPath);
            _mockPortalTemplateIO.Setup(x => x.OpenTextReader(s_defaultUsPath)).Returns(CreateLanguageFileReader(DefaultName));

            //Act
            var templates = PortalController.Instance.GetAvailablePortalTemplates();

            //Assert
            Assert.AreEqual(2, templates.Count);
            AssertTemplateInfo(s_defaultExpectationsDe, templates[0]);
            AssertTemplateInfo(s_defaultExpectationsUs, templates[1]);
        }

        [Test]
        public void TwoTemplatesAssortedLanguages()
        {
            //Arrange
            _mockPortalTemplateIO.Setup(x => x.EnumerateTemplates()).Returns(ToEnumerable(s_defaultPath, s_alternatePath));
            _mockPortalTemplateIO.Setup(x => x.EnumerateLanguageFiles()).Returns(ToEnumerable(s_defaultDePath, s_defaultUsPath, s_alternateDePath));
            _mockPortalTemplateIO.Setup(x => x.GetLanguageFilePath(s_defaultPath, "de-DE")).Returns(s_defaultDePath);
            _mockPortalTemplateIO.Setup(x => x.OpenTextReader(s_defaultDePath)).Returns(CreateLanguageFileReader(DefaultDeName, DefaultDeDescription));
            _mockPortalTemplateIO.Setup(x => x.GetLanguageFilePath(s_defaultPath, "en-US")).Returns(s_defaultUsPath);
            _mockPortalTemplateIO.Setup(x => x.OpenTextReader(s_defaultUsPath)).Returns(CreateLanguageFileReader(DefaultName));
            _mockPortalTemplateIO.Setup(x => x.GetLanguageFilePath(s_alternatePath, "de-DE")).Returns(s_alternateDePath);
            _mockPortalTemplateIO.Setup(x => x.OpenTextReader(s_alternateDePath)).Returns(CreateLanguageFileReader(AlternateDeName));

            //Act
            var templates = PortalController.Instance.GetAvailablePortalTemplates();

            //Assert
            Assert.AreEqual(3, templates.Count);
            AssertTemplateInfo(s_defaultExpectationsDe, templates[0]);
            AssertTemplateInfo(s_defaultExpectationsUs, templates[1]);
            AssertTemplateInfo(s_alternateExpectationsDe, templates[2]);
        }

        [Test]
        public void ResourceFileIsLocatedWhenPresent()
        {
            //Arrange
            _mockPortalTemplateIO.Setup(x => x.EnumerateTemplates()).Returns(ToEnumerable(s_resourcePath));
            _mockPortalTemplateIO.Setup(x => x.GetResourceFilePath(s_resourcePath)).Returns(s_resourceFilePath);

            //Act
            var templates = PortalController.Instance.GetAvailablePortalTemplates();

            //Assert
            Assert.AreEqual(1, templates.Count);
            AssertTemplateInfo(s_resourceExpectations, templates[0]);
        }

        [Test]
        public void SingleTemplateAndLanguage()
        {
            //Arrange
            _mockPortalTemplateIO.Setup(x => x.EnumerateTemplates()).Returns(ToEnumerable(s_defaultPath));
            _mockPortalTemplateIO.Setup(x => x.EnumerateLanguageFiles()).Returns(ToEnumerable(s_defaultDePath));
            _mockPortalTemplateIO.Setup(x => x.GetLanguageFilePath(s_defaultPath, "de-DE")).Returns(s_defaultDePath);
            _mockPortalTemplateIO.Setup(x => x.OpenTextReader(s_defaultDePath)).Returns(CreateLanguageFileReader(DefaultDeName, DefaultDeDescription));

            //Act
            var templates = PortalController.Instance.GetAvailablePortalTemplates();

            //Assert
            Assert.AreEqual(1, templates.Count);
            AssertTemplateInfo(s_defaultExpectationsDe, templates[0]);
        }

        [Test]
        public void ATemplateCanBeLoadedDirectly()
        {
            //Arrange
            _mockPortalTemplateIO.Setup(x => x.EnumerateTemplates()).Returns(ToEnumerable(s_defaultPath));
            _mockPortalTemplateIO.Setup(x => x.EnumerateLanguageFiles()).Returns(ToEnumerable(s_defaultDePath));
            _mockPortalTemplateIO.Setup(x => x.GetLanguageFilePath(s_defaultPath, "de-DE")).Returns(s_defaultDePath);
            _mockPortalTemplateIO.Setup(x => x.OpenTextReader(s_defaultDePath)).Returns(CreateLanguageFileReader(DefaultDeName, DefaultDeDescription));

            //Act
            var template = PortalController.Instance.GetPortalTemplate(s_defaultPath, "de-DE");

            //Assert
            AssertTemplateInfo(s_defaultExpectationsDe, template);
        }

        [Test]
        public void GetPortalTemplateReturnsNullIfCultureDoesNotMatch()
        {
            //Arrange
            _mockPortalTemplateIO.Setup(x => x.EnumerateTemplates()).Returns(ToEnumerable(s_defaultPath));
            _mockPortalTemplateIO.Setup(x => x.EnumerateLanguageFiles()).Returns(ToEnumerable(s_defaultDePath));
            _mockPortalTemplateIO.Setup(x => x.GetLanguageFilePath(s_defaultPath, "de-DE")).Returns(s_defaultDePath);
            _mockPortalTemplateIO.Setup(x => x.OpenTextReader(s_defaultDePath)).Returns(CreateLanguageFileReader(DefaultDeName, DefaultDeDescription));

            //Act
            var template = PortalController.Instance.GetPortalTemplate(s_defaultPath, "de");

            //Assert
            Assert.IsNull(template);
        }

        [Test]
        public void GetPortalTemplateCanReturnAStaticTemplate()
        {
            //Arrange
            _mockPortalTemplateIO.Setup(x => x.EnumerateTemplates()).Returns(ToEnumerable(s_staticPath));
            _mockPortalTemplateIO.Setup(x => x.OpenTextReader(s_staticPath)).Returns(CreateTemplateFileReader(StaticDescription));

            //Act
            var template = PortalController.Instance.GetPortalTemplate(s_staticPath, null);

            //Assert
            AssertTemplateInfo(s_staticExpectations, template);
        }

        private TextReader CreateLanguageFileReader(string name)
        {
            return CreateLanguageFileReader(name, null);
        }

        private TextReader CreateLanguageFileReader(string name, string description)
        {
            if (description != null)
            {
                description = string.Format("<data name=\"PortalDescription.Text\" xml:space=\"preserve\"><value>{0}</value></data>", description);
            }
            var xml = string.Format("<root><data name=\"LocalizedTemplateName.Text\" xml:space=\"preserve\"><value>{0}</value></data>{1}</root>", name, description);
            return new StringReader(xml);
        }

        private TextReader CreateTemplateFileReader(string description)
        {
            var xml = string.Format("<portal><description>{0}</description></portal>", description);
            return new StringReader(xml);
        }

        private static void AssertTemplateInfo(Dictionary<string, string> expectations, PortalController.PortalTemplateInfo templateInfo)
        {
            AssertTemplateField(expectations, "Name", templateInfo.Name);
            AssertTemplateField(expectations, "TemplateFilePath", templateInfo.TemplateFilePath);
            AssertTemplateField(expectations, "CultureCode", templateInfo.CultureCode);
            AssertTemplateField(expectations, "LanguageFilePath", templateInfo.LanguageFilePath);
            AssertTemplateField(expectations, "ResourceFilePath", templateInfo.ResourceFilePath);
            AssertTemplateField(expectations, "Description", templateInfo.Description);
        }

        private static void AssertTemplateField(Dictionary<string, string> expectations, string key, string value)
        {
            string expected;
            expectations.TryGetValue(key, out expected);
            if (string.IsNullOrEmpty(expected))
            {
                Assert.IsNullOrEmpty(value, string.Format("Checking value of " + key));
            }
            else
            {
                Assert.AreEqual(expected, value, string.Format("Checking value of " + key));
            }
        }

        private IEnumerable<T> ToEnumerable<T>(params T[] input)
        {
            return input;
        }

        private static string MakePath(string name)
        {
            var fileName = name + ".template";
            return Path.Combine(HostMapPath, fileName);
        }

        private static string MakePath(string name, string culture)
        {
            return string.Format(@"{0}.{1}.resx", MakePath(name), culture);
        }
    }
}