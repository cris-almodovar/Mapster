﻿using NUnit.Framework;
using Shouldly;

namespace Mapster.Tests
{
    [TestFixture]
    public class WhenMappingWithFlexibleName
    {
        [TearDown]
        public void TearDown()
        {
            TypeAdapterConfig.GlobalSettings.Clear();
        }

        [Test]
        public void Not_Set_Match_Only_Exact_Name()
        {
            var mix = new MixName
            {
                PascalCase = "A",
                camelCase = "B",
                __under__SCORE__ = "C",
                lower_case = "D",
                UPPER_CASE = "E",
                MIX_UnderScore = "F",
            };

            var simple = TypeAdapter.Adapt<SimpleName>(mix);

            simple.PascalCase.ShouldBe(mix.PascalCase);
            simple.CamelCase.ShouldBeNull();
            simple.UnderScore.ShouldBeNull();
            simple.LowerCase.ShouldBeNull();
            simple.UpperCase.ShouldBeNull();
            simple.MixUnder_SCORE.ShouldBeNull();
        }

        [Test]
        public void Map_Flexible_Name()
        {
            TypeAdapterConfig<MixName, SimpleName>.NewConfig().NameMatchingStrategy(NameMatchingStrategy.Flexible);

            var mix = new MixName
            {
                PascalCase = "A",
                camelCase = "B",
                __under__SCORE__ = "C",
                lower_case = "D",
                UPPER_CASE = "E",
                MIX_UnderScore = "F",
            };

            var simple = TypeAdapter.Adapt<SimpleName>(mix);

            simple.PascalCase.ShouldBe(mix.PascalCase);
            simple.CamelCase.ShouldBe(mix.camelCase);
            simple.UnderScore.ShouldBe(mix.__under__SCORE__);
            simple.LowerCase.ShouldBe(mix.lower_case);
            simple.UpperCase.ShouldBe(mix.UPPER_CASE);
            simple.MixUnder_SCORE.ShouldBe(mix.MIX_UnderScore);
        }

        [Test]
        public void Test_Name()
        {
            NameMatchingStrategy.ToPascalCase("PascalCase").ShouldBe("PascalCase");
            NameMatchingStrategy.ToPascalCase("camelCase").ShouldBe("CamelCase");
            NameMatchingStrategy.ToPascalCase("lower_case").ShouldBe("LowerCase");
            NameMatchingStrategy.ToPascalCase("UPPER_CASE").ShouldBe("UpperCase");
            NameMatchingStrategy.ToPascalCase("IPAddress").ShouldBe("IpAddress");
            NameMatchingStrategy.ToPascalCase("ItemID").ShouldBe("ItemId");
            NameMatchingStrategy.ToPascalCase("__under__SCORE__").ShouldBe("UnderScore");
            NameMatchingStrategy.ToPascalCase("__MixMIXMix_mix").ShouldBe("MixMixMixMix");
        }

        public class MixName
        {
            public string PascalCase { get; set; }
            public string camelCase { get; set; }
            public string __under__SCORE__ { get; set; }
            public string lower_case { get; set; }
            public string UPPER_CASE { get; set; }
            public string MIX_UnderScore { get; set; }
        }

        public class SimpleName
        {
            public string PascalCase { get; set; }
            public string CamelCase { get; set; }
            public string UnderScore { get; set; }
            public string LowerCase { get; set; }
            public string UpperCase { get; set; }
            public string MixUnder_SCORE { get; set; }
        }
    }
}
