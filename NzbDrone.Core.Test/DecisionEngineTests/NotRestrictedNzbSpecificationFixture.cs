﻿using FluentAssertions;
using NUnit.Framework;
using NzbDrone.Core.Configuration;
using NzbDrone.Core.DecisionEngine.Specifications;
using NzbDrone.Core.Parser.Model;
using NzbDrone.Core.Test.Framework;

namespace NzbDrone.Core.Test.DecisionEngineTests
{
    [TestFixture]
    public class NotRestrictedNzbSpecificationFixture : CoreTest<NotRestrictedNzbSpecification>
    {
        private RemoteEpisode _parseResult;

        [SetUp]
        public void Setup()
        {
            _parseResult = new RemoteEpisode
                {
                    Report = new ReportInfo
                        {
                            Title = "Dexter.S08E01.EDITED.WEBRip.x264-KYR"
                        }
                };
        }

        [Test]
        public void should_be_true_when_restrictions_are_empty()
        {
            Subject.IsSatisfiedBy(_parseResult).Should().BeTrue();
        }

        [TestCase("KYR")]
        [TestCase("EDITED")]
        [TestCase("2HD\nKYR")]
        public void should_be_false_when_nzb_contains_a_restricted_term(string restrictions)
        {
            Mocker.GetMock<IConfigService>().SetupGet(c => c.NzbRestrictions).Returns(restrictions);
            Subject.IsSatisfiedBy(_parseResult).Should().BeFalse();
        }

        [TestCase("NotReal")]
        [TestCase("LoL")]
        [TestCase("Hello\nWorld")]
        public void should_be_true_when_nzb_does_not_contain_a_restricted_term(string restrictions)
        {
            Mocker.GetMock<IConfigService>().SetupGet(c => c.NzbRestrictions).Returns(restrictions);
            Subject.IsSatisfiedBy(_parseResult).Should().BeTrue();
        }
    }
}