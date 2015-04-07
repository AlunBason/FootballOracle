using System;

namespace FootballOracle.Foundation
{
    public enum AreaType
    {
        Adm,
        Acc,
        Cmp,
        Cnt,
        Mtc,
        Org,
        Ppl,
        Tms,
        Ven
    }

    public enum CompetitionType
    {
        League,
        Cup
    }

    public enum DisplayType
    {
        Success,
        Info,
        Warning,
        Danger
    }

    public enum ImportSite
    {
        Espn = 1,
        Soccerbase = 2
    }

    public enum LanguageType
    {
        Native = 0,
        English
    }

    public enum MatchEventInRunningType
    {
        BroughtOn,
        TakenOff,
        Scored,
        OwnGoal,
        Booked,
        SentOff
    }

    public enum MatchEventStartType
    {
        Started,
        Substitute
    }

    public enum MatchEventType
    {
        Started,
        Substitute,
        BroughtOn,
        TakenOff,
        Scored,
        OwnGoal,
        Booked,
        SentOff
    }

    public enum MatchImportType
    {
        AutomaticFixture = 1,
        ManualFixture,
        AutomaticResultOnly,
        AutomaticResultWithEvents,
        ManualResult
    }

    public enum MatchResult
    {
        UnResolved = -1,
        Lose = 0,
        Draw = 1,
        Win = 2
    }


    //DELETE
    public enum MatchType
    {
        League = 1,
        LeaguePlayoff,
        Cup1Leg,
        Cup2Leg,
        CupGroupStage
    }

    public enum PeriodType
    {
        Day,
        Week,
        Month,
        Year
    }

    public enum PositionType
    {
        Goalkeeper = 1,
        Defender = 101,
        Midfielder = 201,
        Forward = 301
    }

    public enum SaveChangesMessageType
    {
        ChangesSaved,
        RecordDeleted,
        RecordActivated
    }

    public enum SearchDirection
    {
        Down = -1,
        Current = 0,
        Up = 1
    }

    public enum StageType
    {
        QualyfingRound1,
        QualyfingRound2,
        QualyfingRound3,
        PlayoffRound,
        GroupA,
        GroupB,
        GroupC,
        GroupD,
        GroupE,
        GroupF,
        GroupG,
        GroupH,
        GroupI,
        GroupJ,
        GroupK,
        GroupL,
        RoundOf16,
        QuarterFinal,
        SemiFinal,
        Final,
        SecondRound,
        ThirdRound
    }

    public enum TeamNameType
    {
        Primary = 0,
        ShortName,
        Nickname,
        FullName
    }

    public static class _EnumExtensions
    {
        public static MatchEventType? ToMatchEventType(this MatchEventStartType? value)
        {
            switch (value)
            {
                case MatchEventStartType.Started:
                    return MatchEventType.Started;

                case MatchEventStartType.Substitute:
                    return MatchEventType.Substitute;

                default:
                    return null;
            }
        }

        public static MatchEventType? ToMatchEventType(this MatchEventInRunningType? value)
        {
            switch (value)
            {
                case MatchEventInRunningType.Booked:
                    return MatchEventType.Booked;

                case MatchEventInRunningType.BroughtOn:
                    return MatchEventType.BroughtOn;

                case MatchEventInRunningType.OwnGoal:
                    return MatchEventType.OwnGoal;

                case MatchEventInRunningType.Scored:
                    return MatchEventType.Scored;

                case MatchEventInRunningType.SentOff:
                    return MatchEventType.SentOff;

                case MatchEventInRunningType.TakenOff:
                    return MatchEventType.TakenOff;

                default:
                    return null;
            }
        }

        public static string ToPhrase(this StageType? value)
        {
            switch(value)
            {
                case StageType.RoundOf16:
                    return "Round of 16";

                default:
                    return value.ToSpacedString(true);
            }
        }
    }
}
