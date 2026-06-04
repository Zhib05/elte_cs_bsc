using System;
using System.Collections.Generic;

namespace ELTE.DocuStat.Model
{
    public interface IDocumentStatistics
    {
        event EventHandler? FileContentReady;
        event EventHandler? TextStatisticsReady;
        string FileContent { get; }
        IDictionary<string, int> DistinctWordCount { get; }
        int CharacterCount { get; }
        int NonWhiteSpaceCharacterCount { get;}
        int SentenceCount { get; }
        int ProperNounCount { get; }
        double ColemanLieuIndex { get; }
        double FleschReadingEase { get; }

        void Load();
    }
}
