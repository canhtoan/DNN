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
using System;
using System.Linq;

using NUnit.Framework;
using System.Collections.Generic;
using System.Diagnostics;

using DotNetNuke.Entities.Urls;

namespace DotNetNuke.Tests.Core
{
    [TestFixture]
    public class FriendlyUrlControllerTests
    {
        [Test]
        public void DoesNothingToSimpleText()
        {
            bool replacedUnwantedChars;
            string result = FriendlyUrlController.CleanNameForUrl("abcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ", CreateFriendlyUrlOptions(), out replacedUnwantedChars);

            Assert.IsFalse(replacedUnwantedChars);
            Assert.AreEqual("abcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ", result);
        }

        [Test]
        public void RemoveSpace()
        {
            bool replacedUnwantedChars;
            string result = FriendlyUrlController.CleanNameForUrl("123 abc", CreateFriendlyUrlOptions(), out replacedUnwantedChars);

            Assert.IsFalse(replacedUnwantedChars);
            Assert.AreEqual("123abc", result);
        }

        [Test]
        public void ReplaceSpaceWithHyphen()
        {
            bool replacedUnwantedChars;
            string result = FriendlyUrlController.CleanNameForUrl("123 abc", CreateFriendlyUrlOptions(replaceSpaceWith: "-"), out replacedUnwantedChars);

            Assert.IsFalse(replacedUnwantedChars);
            Assert.AreEqual("123-abc", result);
        }

        [Test]
        public void RemoveApostrophe()
        {
            bool replacedUnwantedChars;
            string result = FriendlyUrlController.CleanNameForUrl("Fred's House", CreateFriendlyUrlOptions(), out replacedUnwantedChars);

            Assert.IsTrue(replacedUnwantedChars);
            Assert.AreEqual("FredsHouse", result);
        }

        [Test]
        public void RemoveCharactersInReplaceList()
        {
            bool replacedUnwantedChars;
            string result = FriendlyUrlController.CleanNameForUrl(@"a b&c$d+e,f/g?h~i#j<k>l(m)n¿o¡p«q»r!s""t", CreateFriendlyUrlOptions(), out replacedUnwantedChars);

            Assert.IsTrue(replacedUnwantedChars);
            Assert.AreEqual("abcdefghijklmnopqrst", result);
        }

        [Test]
        public void ReplaceCharactersInReplaceList()
        {
            bool replacedUnwantedChars;
            string result = FriendlyUrlController.CleanNameForUrl(@"a b&c$d+e,f/g?h~i#j<k>l(m)n¿o¡p«q»r!s""t", CreateFriendlyUrlOptions(replaceSpaceWith: "_"), out replacedUnwantedChars);

            Assert.IsTrue(replacedUnwantedChars);
            Assert.AreEqual("a_b_c_d_e_f_g_h_i_j_k_l_m_n_o_p_q_r_s_t", result);
        }

        [Test]
        public void RemoveCharactersInReplaceListWhenReplacementCharacterIsNotAMatchingCharacter()
        {
            bool replacedUnwantedChars;
            string result = FriendlyUrlController.CleanNameForUrl(@"a b&c$d+e,f/g?h~i#j<k>l(m)n¿o¡p«q»r!s""t", CreateFriendlyUrlOptions(replaceSpaceWith: "."), out replacedUnwantedChars);

            Assert.IsTrue(replacedUnwantedChars);
            Assert.AreEqual("abcdefghijklmnopqrst", result);
        }

        [Test]
        public void RemovePunctuatuation()
        {
            bool replacedUnwantedChars;
            string result = FriendlyUrlController.CleanNameForUrl("Dr. Cousteau, where are you?", CreateFriendlyUrlOptions(), out replacedUnwantedChars);

            Assert.IsTrue(replacedUnwantedChars);
            Assert.AreEqual("DrCousteauwhereareyou", result);
        }

        [Test]
        public void RemoveDoubleReplacements()
        {
            bool replacedUnwantedChars;
            string result = FriendlyUrlController.CleanNameForUrl("This, .Has Lots Of---Replacements   Don't you think?", CreateFriendlyUrlOptions(replaceSpaceWith: "-"), out replacedUnwantedChars);

            Assert.IsTrue(replacedUnwantedChars);
            Assert.AreEqual("This-Has-Lots-Of-Replacements-Dont-you-think", result);
        }

        [Test]
        public void DoNotRemoveDoubleReplacements()
        {
            bool replacedUnwantedChars;
            string result = FriendlyUrlController.CleanNameForUrl("This, ,Has Lots Of---Replacements   Don't you think?", CreateFriendlyUrlOptions(replaceDoubleChars: false, replaceSpaceWith: "-"), out replacedUnwantedChars);

            Assert.IsTrue(replacedUnwantedChars);
            Assert.AreEqual("This---Has-Lots-Of---Replacements---Dont-you-think", result);
        }

        [Test]
        public void DoNotConvertVietnameseDiacritics()
        {
            bool replacedUnwantedChars;
            string result = FriendlyUrlController.CleanNameForUrl("D\u1EA5uNg\u00E3S\u1EAFcHuy\u1EC1nN\u1EB7ngH\u1ECFi", CreateFriendlyUrlOptions(), out replacedUnwantedChars);

            Assert.IsFalse(replacedUnwantedChars);
            Assert.AreEqual("D\u1EA5uNg\u00E3S\u1EAFcHuy\u1EC1nN\u1EB7ngH\u1ECFi", result);
        }

        [Test]
        public void DoNotConvertFrenchDiacritics()
        {
            bool replacedUnwantedChars;
            string result = FriendlyUrlController.CleanNameForUrl("Cr\u00E8meFra\u00EEcheC\u00E9dille", CreateFriendlyUrlOptions(), out replacedUnwantedChars);

            Assert.IsFalse(replacedUnwantedChars);
            Assert.AreEqual("Cr\u00E8meFra\u00EEcheC\u00E9dille", result);
        }

        [Test]
        public void DoNotConvertRussianDiacritics()
        {
            bool replacedUnwantedChars;
            string result = FriendlyUrlController.CleanNameForUrl("\u043F\u0438\u0441\u0430\u0301\u0442\u044C\u0431\u043E\u0301\u043B\u044C\u0448\u0430\u044F", CreateFriendlyUrlOptions(), out replacedUnwantedChars);

            Assert.IsFalse(replacedUnwantedChars);
            Assert.AreEqual("\u043F\u0438\u0441\u0430\u0301\u0442\u044C\u0431\u043E\u0301\u043B\u044C\u0448\u0430\u044F", result);
        }

        [Test]
        public void DoNotConvertLeoneseDiacritics()
        {
            bool replacedUnwantedChars;
            string result = FriendlyUrlController.CleanNameForUrl("\u00F1avid\u00E1", CreateFriendlyUrlOptions(), out replacedUnwantedChars);

            Assert.IsFalse(replacedUnwantedChars);
            Assert.AreEqual("\u00F1avid\u00E1", result);
        }

        [Test]
        public void ConvertVietnameseDiacritics()
        {
            bool replacedUnwantedChars;
            string result = FriendlyUrlController.CleanNameForUrl("D\u1EA5uNg\u00E3S\u1EAFcHuy\u1EC1nN\u1EB7ngH\u1ECFi", CreateFriendlyUrlOptions(autoAsciiConvert: true), out replacedUnwantedChars);

            Assert.IsTrue(replacedUnwantedChars);
            Assert.AreEqual("DauNgaSacHuyenNangHoi", result);
        }

        [Test]
        public void ConvertFrenchDiacritics()
        {
            bool replacedUnwantedChars;
            string result = FriendlyUrlController.CleanNameForUrl("Cr\u00E8meFra\u00EEcheC\u00E9dille", CreateFriendlyUrlOptions(autoAsciiConvert: true), out replacedUnwantedChars);

            Assert.IsTrue(replacedUnwantedChars);
            Assert.AreEqual("CremeFraicheCedille", result);
        }

        [Test]
        public void ConvertRussianDiacritics()
        {
            bool replacedUnwantedChars;
            string result = FriendlyUrlController.CleanNameForUrl("\u043F\u0438\u0441\u0430\u0301\u0442\u044C\u0431\u043E\u0301\u043B\u044C\u0448\u0430\u044F", CreateFriendlyUrlOptions(autoAsciiConvert: true), out replacedUnwantedChars);

            Assert.IsTrue(replacedUnwantedChars);
            Assert.AreEqual("\u043F\u0438\u0441\u0430\u0442\u044C\u0431\u043E\u043B\u044C\u0448\u0430\u044F", result);
        }

        [Test]
        public void ConvertLeoneseDiacritics()
        {
            bool replacedUnwantedChars;
            string result = FriendlyUrlController.CleanNameForUrl("\u00F1avid\u00E1", CreateFriendlyUrlOptions(autoAsciiConvert: true), out replacedUnwantedChars);

            Assert.IsTrue(replacedUnwantedChars);
            Assert.AreEqual("navida", result);
        }

        [Test]
        public void ReplaceBeforeConvertingDiacritics()
        {
            bool replacedUnwantedChars;
            var replacements = new Dictionary<string, string>(1) { { "\u00F1", "nn" }, };
            string result = FriendlyUrlController.CleanNameForUrl("Carre\u00F1o", CreateFriendlyUrlOptions(replaceCharacterDictionary: replacements), out replacedUnwantedChars);

            Assert.IsTrue(replacedUnwantedChars);
            Assert.AreEqual("Carrenno", result);
        }

        [Test]
        [Ignore]
        public void PerfTest()
        {
            var watch = new Stopwatch();
            watch.Start();

            const int iterations = 100000;
            for (var i = 0; i < iterations; i++)
            {
                bool replacedUnwantedChars;
                string result = FriendlyUrlController.CleanNameForUrl("Jimmy Eat World", CreateFriendlyUrlOptions(), out replacedUnwantedChars);

                Assert.IsFalse(replacedUnwantedChars);
                Assert.AreEqual("JimmyEatWorld", result);
            }

            watch.Stop();

            Assert.Inconclusive("{0} iterations took {1}ms", iterations, watch.Elapsed);
        }

        private static FriendlyUrlOptions CreateFriendlyUrlOptions(
            string replaceSpaceWith = FriendlyUrlSettings.ReplaceSpaceWithNothing,
            string spaceEncodingValue = FriendlyUrlSettings.SpaceEncodingHex,
            bool autoAsciiConvert = false,
            string regexMatch = @"[^\w\d _-]",
            string illegalChars = @"<>/\?:&=+|%#",
            string replaceChars = @" &$+,/?~#<>()¿¡«»!""",
            bool replaceDoubleChars = true,
            Dictionary<string, string> replaceCharacterDictionary = null,
            PageExtensionUsageType pageExtensionUsageType = PageExtensionUsageType.Never,
            string pageExtension = ".aspx")
        {
            replaceCharacterDictionary = replaceCharacterDictionary ?? new Dictionary<string, string>(0);
            return new FriendlyUrlOptions
            {
                PunctuationReplacement = (replaceSpaceWith != FriendlyUrlSettings.ReplaceSpaceWithNothing)
                                                ? replaceSpaceWith
                                                : string.Empty,
                SpaceEncoding = spaceEncodingValue,
                MaxUrlPathLength = 200,
                ConvertDiacriticChars = autoAsciiConvert,
                RegexMatch = regexMatch,
                IllegalChars = illegalChars,
                ReplaceChars = replaceChars,
                ReplaceDoubleChars = replaceDoubleChars,
                ReplaceCharWithChar = replaceCharacterDictionary,
                PageExtension = (pageExtensionUsageType == PageExtensionUsageType.Never)
                                        ? ""
                                        : pageExtension
            };
        }
    }
}