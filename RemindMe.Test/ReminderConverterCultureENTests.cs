using Microsoft.VisualStudio.TestTools.UnitTesting;
using RemindMe.Core.Converters;
using System;
using System.Globalization;

namespace RemindMe.Test
{
    [TestClass]
    public class ReminderConverterCultureENTests
    {
        private CultureInfo _cultureInfo;
        private ShortenedCommentValueConverter _shortenedCommentConverter;
        private TimestampToReadableDateValueConverter _timestampToReadableDateConverter;
        private TimestampToTimeLeftValueConverter _timestampToTimeLeftConverter;

        [TestInitialize]
        public void Init()
        {
            _cultureInfo = new CultureInfo("en-US");
            CultureInfo.CurrentCulture = _cultureInfo;

            _shortenedCommentConverter = new ShortenedCommentValueConverter();
            _timestampToReadableDateConverter = new TimestampToReadableDateValueConverter();
            _timestampToTimeLeftConverter = new TimestampToTimeLeftValueConverter();
        }

        [TestMethod]
        public void ShortenACommentWithLessThanOneHundredChars()
        {
            const string COMMENT =
@"Lorem Ipsum
dolor sit amet

consectetur adipiscing elit";
            const string EXPECTED_COMMENT = "Lorem Ipsum dolor sit amet consectetur adipiscing elit";

            string convertedComment = _shortenedCommentConverter.Convert(COMMENT, typeof(string), null, _cultureInfo) as string;

            Assert.AreEqual(EXPECTED_COMMENT, convertedComment);
        }

        [TestMethod]
        public void ShortenACommentWithMoreThanOneHundredChars()
        {
            const string COMMENT =
@"Lorem Ipsum
dolor sit amet

consectetur adipiscing elit
Lorem Ipsum
dolor sit amet

consectetur adipiscing elit


Lorem Ipsum
dolor sit amet

consectetur adipiscing elit";
            const string EXPECTED_COMMENT = "Lorem Ipsum dolor sit amet consectetur adipiscing elit Lorem Ipsum dolor sit amet consectetur adipis...";

            string convertedComment = _shortenedCommentConverter.Convert(COMMENT, typeof(string), null, _cultureInfo) as string;

            Assert.AreEqual(EXPECTED_COMMENT, convertedComment);
        }

        [TestMethod]
        public void ConvertDateTimeToReadableDate()
        {
            DateTime date = new DateTime(2018, 12, 8);

            string readableDate = ReadableDateConverter.Convert(date);

            Assert.AreEqual("Saturday, December 8, 2018", readableDate);
        }

        [TestMethod]
        public void ConvertTimestampToReadableDate()
        {
            long timestamp = 1524126032;

            string readableDate = _timestampToReadableDateConverter.Convert(timestamp, typeof(long), null, _cultureInfo) as string;

            Assert.AreEqual("Thursday, Apr 19, 2018", readableDate);
        }

        [TestMethod]
        public void ConvertCurrentTimestampWithOneMonthAddedToTimeleft()
        {
            long timestamp = DateTimeOffset.UtcNow.AddMonths(1).ToUnixTimeSeconds();

            string readableTimeLeft = _timestampToTimeLeftConverter.Convert(timestamp, typeof(long), null, _cultureInfo) as string;

            Assert.AreEqual("1 month left", readableTimeLeft);
        }

        [TestMethod]
        public void ConvertCurrentTimestampWithOneMonthAndOneDayAddedToTimeleft()
        {
            long timestamp = DateTimeOffset.UtcNow.AddMonths(1)
                .AddDays(1)
                .ToUnixTimeSeconds();

            string readableTimeLeft = _timestampToTimeLeftConverter.Convert(timestamp, typeof(long), null, _cultureInfo) as string;

            Assert.AreEqual("1 month left", readableTimeLeft);
        }

        [TestMethod]
        public void ConvertCurrentTimestampWithOneYearAddedToTimeLeft()
        {
            long timestamp = DateTimeOffset.UtcNow.AddYears(1).ToUnixTimeSeconds();

            string readableTimeLeft = _timestampToTimeLeftConverter.Convert(timestamp, typeof(long), null, _cultureInfo) as string;

            Assert.AreEqual("1 year left", readableTimeLeft);
        }

        [TestMethod]
        public void ConvertCurrentTimestampWithOneYearAndFiveDaysAddedToTimeLeft()
        {
            long timestamp = DateTimeOffset.UtcNow.AddYears(1)
                .AddDays(5)
                .ToUnixTimeSeconds();

            string readableTimeLeft = _timestampToTimeLeftConverter.Convert(timestamp, typeof(long), null, _cultureInfo) as string;

            Assert.AreEqual("1 year left", readableTimeLeft);
        }

        [TestMethod]
        public void ConvertCurrentTimestampWithSixMonthsAddedToTimeleft()
        {
            long timestamp = DateTimeOffset.UtcNow.AddMonths(6).ToUnixTimeSeconds();

            string readableTimeLeft = _timestampToTimeLeftConverter.Convert(timestamp, typeof(long), null, _cultureInfo) as string;

            Assert.AreEqual("6 months left", readableTimeLeft);
        }

        [TestMethod]
        public void ConvertCurrentTimestampWithSixMonthsAndOneDayAddedToTimeleft()
        {
            long timestamp = DateTimeOffset.UtcNow.AddMonths(6)
                .AddDays(1)
                .ToUnixTimeSeconds();

            string readableTimeLeft = _timestampToTimeLeftConverter.Convert(timestamp, typeof(long), null, _cultureInfo) as string;

            Assert.AreEqual("6 months left", readableTimeLeft);
        }

        [TestMethod]
        public void ConvertCurrentTimestampWithFourDaysAddedToTimeleft()
        {
            long timestamp = DateTimeOffset.UtcNow.AddDays(4).ToUnixTimeSeconds();

            string readableTimeLeft = _timestampToTimeLeftConverter.Convert(timestamp, typeof(long), null, _cultureInfo) as string;

            Assert.AreEqual("4 days left", readableTimeLeft);
        }

        [TestMethod]
        public void ConvertCurrentTimestampWithFourDaysAndOneHourAddedToTimeleft()
        {
            long timestamp = DateTimeOffset.UtcNow.AddDays(4)
                .AddHours(1)
                .ToUnixTimeSeconds();

            string readableTimeLeft = _timestampToTimeLeftConverter.Convert(timestamp, typeof(long), null, _cultureInfo) as string;

            Assert.AreEqual("4 days left", readableTimeLeft);
        }

        [TestMethod]
        public void ConvertCurrentTimestampWithOneHourAddedToTimeleft()
        {
            long timestamp = DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds();

            string readableTimeLeft = _timestampToTimeLeftConverter.Convert(timestamp, typeof(long), null, _cultureInfo) as string;

            Assert.AreEqual("1 hour left", readableTimeLeft);
        }

        [TestMethod]
        public void ConvertTimestampPastSinceTwentyFiveMinutesToTimeleft()
        {
            long timestamp = DateTimeOffset.UtcNow.AddMinutes(-25).ToUnixTimeSeconds();

            string readableTimeLeft = _timestampToTimeLeftConverter.Convert(timestamp, typeof(long), null, _cultureInfo) as string;

            Assert.AreEqual("25 minutes ago", readableTimeLeft);
        }

        [TestMethod]
        public void ConvertTimestampPastSinceOneDayToTimeleft()
        {
            long timestamp = DateTimeOffset.UtcNow.AddDays(-1).ToUnixTimeSeconds();

            string readableTimeLeft = _timestampToTimeLeftConverter.Convert(timestamp, typeof(long), null, _cultureInfo) as string;

            Assert.AreEqual("1 day ago", readableTimeLeft);
        }

        [TestMethod]
        public void ConvertTimestampPastSinceSeventeenDaysToTimeleft()
        {
            long timestamp = DateTimeOffset.UtcNow.AddDays(-17).ToUnixTimeSeconds();

            string readableTimeLeft = _timestampToTimeLeftConverter.Convert(timestamp, typeof(long), null, _cultureInfo) as string;

            Assert.AreEqual("17 days ago", readableTimeLeft);
        }
    }
}
