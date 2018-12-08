using Microsoft.VisualStudio.TestTools.UnitTesting;
using RemindMe.Core.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemindMe.Test
{
    [TestClass]
    public class ReminderConverterTests
    {
        [TestMethod]
        public void ShortenCommentLessThanOneHundredChars()
        {
            ShortenedCommentValueConverter converter = new ShortenedCommentValueConverter();
            const string COMMENT =
@"Lorem Ipsum
dolor sit amet

consectetur adipiscing elit";
            const string EXPECTED_COMMENT = "Lorem Ipsum dolor sit amet consectetur adipiscing elit";

            string convertedComment = converter.Convert(COMMENT, typeof(string), null, new CultureInfo("fr-FR")) as string;

            Assert.AreEqual(EXPECTED_COMMENT, convertedComment);
        }
    }
}
