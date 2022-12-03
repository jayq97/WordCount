using System;
using System.Diagnostics;
using Xunit;

namespace WordCount.Tests
{
    public class MapReduceWordsTests
    {
        [Fact]
        public void Test_CountUniqueWords()
        {
            IEnumerable<string> lines = new[] { 
                "The quick brown fox jumps over the lazy dog.", 
                "The five boxing wizards jump quickly.",
                "Sphinx of black quartz, judge my vow."}; // 20 Unique Words (Total count would be 22, but the word "the" appears thrice.

            IEnumerable<string> linesWithSpecialSigns = new[] {
                "How.vexingly,quick;daft:zebras(jump)!",
                "Glib_jocks\nquiz\rnymph\tto\'vex$dwarf.",
                "Pack\\my*box/with,<five[dozen]liquor%jugs."};

            Assert.Equal(20, Program.MapReduceWordsFromFiles(lines).Count);
            Assert.Equal(21, Program.MapReduceWordsFromFiles(linesWithSpecialSigns).Count);
        }

        [Fact]
        public void Test_CountAppearanceOfWords()
        {
            IEnumerable<string> lines = new[] {
                "The quick brown fox jumps over the lazy dog.",
                "The five boxing wizards jump quickly.",
                "Sphinx of black quartz, judge my vow."};

            Assert.Equal(3, Program.MapReduceWordsFromFiles(lines).FirstOrDefault(x => x.Key == "the").Value); // The word "the" appears thrice.
            Assert.Equal(1, Program.MapReduceWordsFromFiles(lines).FirstOrDefault(x => x.Key == "fox").Value);
            Assert.Equal(0, Program.MapReduceWordsFromFiles(lines).FirstOrDefault(x => x.Key == "zebras").Value);
        }
    }
}