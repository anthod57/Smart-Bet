using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Bet
{
    public class Team
    {
        public string Name;
        public double Rank;
        public double Points;

        public double HomeW;
        public double HomeD;
        public double HomeL;
        public double AwayW;
        public double AwayD;
        public double AwayL;

        public double playedMatch;
        public double homePlayedMatch;
        public double awayPlayedMatch;

        public List<Match> HomeMatches = new List<Match>();
        public List<Match> AwayMatches = new List<Match>();
        public List<Match> lastMatches = new List<Match>();

        public double HomeScoredGoals;
        public double HomeConcededGoals;
        public double AwayScoredGoals;
        public double AwayConcededGoals;

        public double HomeScoredGoalsAverage;
        public double HomeConcededGoalsAverage;
        public double AwayScoredGoalsAverage;
        public double AwayConcededGoalsAverage;

        public double HomeBTTSYes;
        public double AwayBTTSYes;

        public double HomeBTTSNo;
        public double AwayBTTSNo;

        public double HOver15;
        public double HOver25;
        public double HOver35;
        public double HOver45;
        public double HOver55;

        public double AOver15;
        public double AOver25;
        public double AOver35;
        public double AOver45;
        public double AOver55;

        public double HUnder15;
        public double HUnder25;
        public double HUnder35;
        public double HUnder45;
        public double HUnder55;

        public double AUnder15;
        public double AUnder25;
        public double AUnder35;
        public double AUnder45;
        public double AUnder55;



        public Team(string name)
        {
            Name = name;
        }

        public void RefreshStats()
        {
            HomeScoredGoalsAverage = HomeScoredGoals / homePlayedMatch;
            AwayScoredGoalsAverage = AwayScoredGoals / awayPlayedMatch;

            HomeConcededGoalsAverage = HomeConcededGoals / homePlayedMatch;
            AwayConcededGoalsAverage = AwayConcededGoals / awayPlayedMatch;
        }
    }
}
