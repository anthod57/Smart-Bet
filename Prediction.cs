using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MathNet.Numerics.Distributions;

namespace Smart_Bet
{
    public class Prediction
    {

        Team HomeTeam;
        Team AwayTeam;
        League League;

        public Dictionary<string, double> scorePrediction = new Dictionary<string, double>();

        public double Home;
        public double Draw;
        public double Away;

        public DateTime Date;

        public double HomeGoals;
        public double AwayGoals;

        public double BTTS;
        public double NoBTTS;

        public double Over15;
        public double Over25;
        public double Over35;
        public double Over45;
        public double Over55;

        public double Under15;
        public double Under25;
        public double Under35;
        public double Under45;
        public double Under55;

        public Prediction(Team homeTeam, Team awayTeam, League league, DateTime date)
        {
            HomeTeam = homeTeam;
            AwayTeam = awayTeam;
            League = league;
            Date = date;

            PredictScore();
            PredictResult();
            PredictBTTS();
            Predict15();
            Predict25();
            Predict35();
            Predict45();
            Predict55();
        }

        public void PredictScore()
        {
            try
            {
                int homeGoals = 0;
                int awayGoals = 0;

                double homeAttackStrength = HomeTeam.HomeScoredGoalsAverage / League.homeGoalsPerMatch;
                double awayAttackStrength = AwayTeam.AwayScoredGoalsAverage / League.awayGoalsPerMatch;

                double homeDefensePotential = HomeTeam.HomeConcededGoalsAverage / League.awayGoalsPerMatch;
                double awayDefensePotential = AwayTeam.AwayConcededGoalsAverage / League.homeGoalsPerMatch;

                double homeGoalsPrediction = homeAttackStrength * awayDefensePotential * League.homeGoalsPerMatch;
                double awayGoalsPrediction = awayAttackStrength * homeDefensePotential * League.awayGoalsPerMatch;

                for (homeGoals = 0; homeGoals < 10; homeGoals++)
                {
                    for (awayGoals = 0; awayGoals < 10; awayGoals++)
                    {
                        scorePrediction.Add(homeGoals.ToString() + "-" + awayGoals.ToString(), Math.Round(Poisson.PMF(homeGoalsPrediction, homeGoals) * Poisson.PMF(awayGoalsPrediction, awayGoals) * 100, 5));
                    }
                }
            }
            catch (Exception)
            {

                
            }
            
        }

        public void PredictBTTS()
        {
            double homeLastBTTS = 0;
            double awayLastBTTS = 0;
            double lastBTTS = 0;

            double lastEncounterCount = 0;
            double lastEncounterBTTS = 0;

            List<League> seasonList = new List<League>();

            seasonList.Add(League);

            foreach (League season in League.lastSeasons)
            {
                seasonList.Add(season);
            }

            foreach(League league in seasonList)
            {
                Team homeTeam = new Team("Zebi");
                try
                {
                    homeTeam = league.Teams.Where(p => p.Name == HomeTeam.Name).First();
                }
                catch (Exception)
                {

                }

                if (homeTeam != null)
                {
                    foreach (Match match in homeTeam.HomeMatches)
                    {
                        if (match.AwayTeam == AwayTeam.Name)
                        {
                            DateTime matchDate = new DateTime();
                            if (DateTime.TryParseExact(match.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out matchDate))
                            {

                            }
                            else
                            {
                                if (DateTime.TryParseExact(match.Date, "dd/MM/yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out matchDate))
                                {

                                }
                            }
                            int compare = DateTime.Compare(matchDate, Date);
                            if (compare < 0)
                            {
                                lastEncounterCount++;
                                if (match.HomeGoals > 0 && match.AwayGoals > 0)
                                {
                                    lastEncounterBTTS++;
                                }
                            }
                        }
                    }
                }
            }
            lastEncounterBTTS = lastEncounterBTTS / lastEncounterCount * 100;
            
            int iHome = 0;
            int iAway = 0;

            foreach (Match match in HomeTeam.HomeMatches)
            {
                if (iHome < 6 || iHome != 5)
                {
                    if (HomeTeam.HomeMatches[iHome].HomeGoals > 0 && HomeTeam.HomeMatches[iHome].AwayGoals > 0)
                    {
                        homeLastBTTS++;
                    }
                }

                iHome++;
            }

            foreach (Match match in AwayTeam.AwayMatches)
            {
                if (iAway < 6 || iAway != 5)
                {
                    if (AwayTeam.AwayMatches[iAway].HomeGoals > 0 && AwayTeam.AwayMatches[iAway].AwayGoals > 0)
                    {
                        awayLastBTTS++;
                    }
                }

                iAway++;
            }

            lastBTTS = ((homeLastBTTS / iHome) + (awayLastBTTS / iAway)) / 2 * 100;
            BTTS = 0;

            foreach (string score in scorePrediction.Keys)
            {
                string[] goals = score.Split('-');

                if (int.Parse(goals[0]) > 0 && int.Parse(goals[1]) > 0)
                {
                    BTTS += scorePrediction[score];
                }
            }

            if (!Double.IsNaN(lastEncounterBTTS))
            {
                BTTS = Math.Round((BTTS + (League.BTTSYes/ League.playedMatch * 100) + lastBTTS + (HomeTeam.HomeBTTSYes / HomeTeam.homePlayedMatch * 100) + (AwayTeam.AwayBTTSYes / AwayTeam.awayPlayedMatch * 100) + lastEncounterBTTS) / 6, 2);
            }
            else
            {
                BTTS = Math.Round((BTTS + (League.BTTSYes / League.playedMatch * 100) + lastBTTS + (HomeTeam.HomeBTTSYes / HomeTeam.homePlayedMatch * 100) + (AwayTeam.AwayBTTSYes / AwayTeam.awayPlayedMatch * 100)) / 5, 2);
            }

            NoBTTS = 100 - BTTS;
        }

        public void PredictResult()
        {
            double homeLastWins = 0;
            double awayLastWins = 0;
            double lastDraws = 0;

            List<League> seasonList = new List<League>();

            seasonList.Add(League);

            foreach (League season in League.lastSeasons)
            {
                seasonList.Add(season);
            }

            double lastEncounterCount = 0;
            double lastEncounterHomeWin = 0;
            double lastEncounterAwayWin = 0;
            double lastEncounterDraw = 0;

            foreach(League league in seasonList)
            {
                Team homeTeam = new Team("Zebi");
                try
                {
                    homeTeam = league.Teams.Where(p => p.Name == HomeTeam.Name).First();
                }
                catch (Exception)
                {

                }

                if (homeTeam != null)
                { 
                    foreach(Match match in homeTeam.HomeMatches)
                    {
                        if(match.AwayTeam == AwayTeam.Name)
                        {
                            DateTime matchDate = new DateTime();
                            if (DateTime.TryParseExact(match.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out matchDate))
                            {

                            }
                            else
                            {
                                if (DateTime.TryParseExact(match.Date, "dd/MM/yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out matchDate))
                                {

                                }
                            }
                            int compare = DateTime.Compare(matchDate, Date);

                            {
                                lastEncounterCount++;
                                if (match.HomeGoals > match.AwayGoals)
                                {

                                    lastEncounterHomeWin++;

                                }
                                else if (match.HomeGoals < match.AwayGoals)
                                {

                                    lastEncounterAwayWin++;

                                }
                                else if (match.HomeGoals == match.AwayGoals)
                                {

                                    lastEncounterDraw++;

                                }
                            }
                        }
                    }
                }
            }

            lastEncounterHomeWin = lastEncounterHomeWin / lastEncounterCount * 100;
            lastEncounterAwayWin = lastEncounterAwayWin / lastEncounterCount * 100;
            lastEncounterDraw = lastEncounterDraw / lastEncounterCount * 100;

            int iHome = 0;

            foreach(Match match in HomeTeam.HomeMatches)
            {

                if(iHome < 6 || iHome != 5)
                {
                    if (HomeTeam.HomeMatches[iHome].HomeGoals > HomeTeam.HomeMatches[iHome].AwayGoals)
                    {
                        homeLastWins++;
                    }

                    if (HomeTeam.HomeMatches[iHome].HomeGoals == HomeTeam.HomeMatches[iHome].AwayGoals)
                    {
                        lastDraws++;
                    }
                }

                iHome++;

            }

            int iAway = 0;

            foreach (Match match in AwayTeam.AwayMatches)
            {

                if (iAway < 6 || iAway != 5)
                {
                    if (AwayTeam.AwayMatches[iAway].AwayGoals > AwayTeam.AwayMatches[iAway].HomeGoals)
                    {
                        awayLastWins++;
                    }

                    if (AwayTeam.AwayMatches[iAway].AwayGoals == AwayTeam.AwayMatches[iAway].HomeGoals)
                    {
                        lastDraws++;
                    }
                }

                iAway++;

            }


            foreach (string score in scorePrediction.Keys)
            {
                string[] goals = score.Split('-');

                if (int.Parse(goals[0]) > int.Parse(goals[1]))
                {
                    Home += scorePrediction[score];
                }

                if (int.Parse(goals[0]) == int.Parse(goals[1]))
                {
                    Draw += scorePrediction[score];
                }

                if (int.Parse(goals[0]) < int.Parse(goals[1]))
                {
                    Away += scorePrediction[score];
                }
            }

            homeLastWins = homeLastWins / iHome * 100;
            awayLastWins = awayLastWins / iAway * 100;
            lastDraws = lastDraws / (iHome + iAway) * 100;
            if (!Double.IsNaN(lastEncounterHomeWin) && !Double.IsNaN(lastEncounterAwayWin) && !Double.IsNaN(lastEncounterDraw))
            {
                Home = Math.Round((Home + (HomeTeam.HomeW / HomeTeam.homePlayedMatch * 100) + (League.homeW / League.playedMatch * 100) + (AwayTeam.AwayL / AwayTeam.awayPlayedMatch * 100) + homeLastWins + (100 - awayLastWins) + lastEncounterHomeWin) / 7, 2);
                Draw = Math.Round((Draw + (HomeTeam.HomeD / HomeTeam.homePlayedMatch * 100) + (League.draws / League.playedMatch * 100) + (AwayTeam.AwayD / AwayTeam.awayPlayedMatch * 100) + lastDraws + lastEncounterDraw) / 6, 2);
                Away = Math.Round((Away + (AwayTeam.AwayW / AwayTeam.awayPlayedMatch * 100) + (League.awayW / League.playedMatch * 100) + (HomeTeam.HomeL / HomeTeam.homePlayedMatch * 100) + awayLastWins + (100 - homeLastWins) + lastEncounterAwayWin) / 7, 2);
            }
            else
            {
                Home = Math.Round((Home + (HomeTeam.HomeW / HomeTeam.homePlayedMatch * 100) + (League.homeW / League.playedMatch * 100) + (AwayTeam.AwayL / AwayTeam.awayPlayedMatch * 100) + homeLastWins + (100 - awayLastWins)) / 6, 2);
                Draw = Math.Round((Draw + (HomeTeam.HomeD / HomeTeam.homePlayedMatch * 100) + (League.draws / League.playedMatch * 100) + (AwayTeam.AwayD / AwayTeam.awayPlayedMatch * 100) + lastDraws) / 5, 2);
                Away = Math.Round((Away + (AwayTeam.AwayW / AwayTeam.awayPlayedMatch * 100) + (League.awayW / League.playedMatch * 100) + (HomeTeam.HomeL / HomeTeam.homePlayedMatch * 100) + awayLastWins + (100 - homeLastWins)) / 6, 2);
            }



            float homeCons = 1f;

            Home += homeCons;
            Away -= homeCons * 2;
        }

        public void Predict15()
        {
            double homeLastOver15 = 0;
            double awayLastOver15 = 0;

            List<League> seasonList = new List<League>();

            seasonList.Add(League);

            foreach(League season in League.lastSeasons)
            {
                seasonList.Add(season);
            }

            double lastEncounterCount = 0;
            double lastEncounterOver15 = 0;
            double lastOver15 = 0;

            foreach (League league in seasonList)
            {
                Team homeTeam = new Team("Zebi");
                try
                {
                    homeTeam = league.Teams.Where(p => p.Name == HomeTeam.Name).First();
                }
                catch (Exception)
                {

                }
                
                if(homeTeam != null)
                {
                    foreach(Match match in homeTeam.HomeMatches)
                    {
                        if(match.AwayTeam == AwayTeam.Name)
                        {
                            DateTime matchDate = new DateTime();
                            if (DateTime.TryParseExact(match.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out matchDate))
                            {

                            }
                            else
                            {
                                if (DateTime.TryParseExact(match.Date, "dd/MM/yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out matchDate))
                                {

                                }
                            }
                            int compare = DateTime.Compare(matchDate, Date);
                            if (compare < 0)
                            {                               
                                lastEncounterCount++;
                                if (match.HomeGoals + match.AwayGoals > 1)
                                {
                                    lastEncounterOver15++;
                                }
                            }
                        }
                    }                   
                }
            }

            lastEncounterOver15 = lastEncounterOver15 / lastEncounterCount * 100;

            int iHome = 0;
            int iAway = 0;

            foreach(Match match in HomeTeam.HomeMatches)
            {
                if(iHome < 6 || iHome != 5)
                {
                    if (HomeTeam.HomeMatches[iHome].HomeGoals + HomeTeam.HomeMatches[iHome].AwayGoals > 1)
                    {
                        homeLastOver15++;
                    }
                }

                iHome++;
            }

            foreach(Match match in AwayTeam.AwayMatches)
            {
                if(iAway < 6 || iAway != 5)
                {
                    if (AwayTeam.AwayMatches[iAway].AwayGoals + AwayTeam.AwayMatches[iAway].HomeGoals > 1)
                    {
                        awayLastOver15++;
                    }
                }

                iAway++;
            }

            lastOver15 = ((homeLastOver15 / iHome) + (awayLastOver15 / iAway)) / 2 * 100;

            Over15 = 0;

            foreach (string score in scorePrediction.Keys)
            {
                string[] goals = score.Split('-');

                if (int.Parse(goals[0]) + int.Parse(goals[1]) > 1)
                {
                    Over15 += scorePrediction[score];
                }
            }

           
            if (!Double.IsNaN(lastEncounterOver15))
            {
                Over15 = Math.Round((Over15 + (League.Over15 / League.playedMatch * 100) + lastOver15 + (HomeTeam.HOver15 / HomeTeam.playedMatch * 100) + (AwayTeam.AOver15 / AwayTeam.awayPlayedMatch * 100) + lastEncounterOver15) / 6, 2);
            }
            else
            {
                Over15 = Math.Round((Over15 + (League.Over15 / League.playedMatch * 100) + lastOver15 + (HomeTeam.HOver15 / HomeTeam.playedMatch * 100) + (AwayTeam.AOver15 / AwayTeam.awayPlayedMatch * 100)) / 5, 2);
            }
        }

        public void Predict25()
        {
            double homeLastOver25 = 0;
            double awayLastOver25 = 0;

            List<League> seasonList = new List<League>();

            seasonList.Add(League);

            foreach (League season in League.lastSeasons)
            {
                seasonList.Add(season);
            }

            double lastEncounterCount = 0;
            double lastEncounterOver25 = 0;
            double lastOver25 = 0;

            foreach (League league in seasonList)
            {
                Team homeTeam = new Team("Zebi");
                try
                {
                    homeTeam = league.Teams.Where(p => p.Name == HomeTeam.Name).First();
                }
                catch (Exception)
                {

                }

                if (homeTeam != null)
                {
                    foreach (Match match in homeTeam.HomeMatches)
                    {
                        if (match.AwayTeam == AwayTeam.Name)
                        {
                            DateTime matchDate = new DateTime();
                            if (DateTime.TryParseExact(match.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out matchDate))
                            {

                            }
                            else
                            {
                                if (DateTime.TryParseExact(match.Date, "dd/MM/yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out matchDate))
                                {

                                }
                            }
                            int compare = DateTime.Compare(matchDate, Date);
                            if (compare < 0)
                            {
                                lastEncounterCount++;
                                if (match.HomeGoals + match.AwayGoals > 2)
                                {
                                    lastEncounterOver25++;
                                }
                            }
                        }
                    }
                }
            }

            lastEncounterOver25 = lastEncounterOver25 / lastEncounterCount * 100;

            int iHome = 0;
            int iAway = 0;

            foreach (Match match in HomeTeam.HomeMatches)
            {
                if (iHome < 6 || iHome != 5)
                {
                    if (HomeTeam.HomeMatches[iHome].HomeGoals + HomeTeam.HomeMatches[iHome].AwayGoals > 2)
                    {
                        homeLastOver25++;
                    }
                }

                iHome++;
            }

            foreach (Match match in AwayTeam.AwayMatches)
            {
                if (iAway < 6 || iAway != 5)
                {
                    if (AwayTeam.AwayMatches[iAway].AwayGoals + AwayTeam.AwayMatches[iAway].HomeGoals > 2)
                    {
                        awayLastOver25++;
                    }
                }

                iAway++;
            }


            lastOver25 = ((homeLastOver25 / iHome) + (awayLastOver25 / iAway)) / 2 * 100;

            Over25 = 0;

            foreach (string score in scorePrediction.Keys)
            {
                string[] goals = score.Split('-');

                if (int.Parse(goals[0]) + int.Parse(goals[1]) > 2)
                {
                    Over25 += scorePrediction[score];
                }
            }


            if (!Double.IsNaN(lastEncounterOver25))
            {
                Over25 = Math.Round((Over15 + (League.Over15 / League.playedMatch * 100) + lastOver25 + (HomeTeam.HOver15 / HomeTeam.playedMatch * 100) + (AwayTeam.AOver15 / AwayTeam.awayPlayedMatch * 100) + lastEncounterOver25) / 6, 2);
            }
            else
            {
                Over25 = Math.Round((Over15 + (League.Over15 / League.playedMatch * 100) + lastOver25 + (HomeTeam.HOver15 / HomeTeam.playedMatch * 100) + (AwayTeam.AOver15 / AwayTeam.awayPlayedMatch * 100)) / 5, 2);
            }
        }

        public void Predict35()
        {
            double homeLastOver35 = 0;
            double awayLastOver35 = 0;

            List<League> seasonList = new List<League>();

            seasonList.Add(League);

            foreach (League season in League.lastSeasons)
            {
                seasonList.Add(season);
            }

            double lastEncounterCount = 0;
            double lastEncounterOver35 = 0;
            double lastOver35 = 0;

            foreach (League league in seasonList)
            {
                Team homeTeam = new Team("Zebi");
                try
                {
                    homeTeam = league.Teams.Where(p => p.Name == HomeTeam.Name).First();
                }
                catch (Exception)
                {

                }

                if (homeTeam != null)
                {
                    foreach (Match match in homeTeam.HomeMatches)
                    {
                        if (match.AwayTeam == AwayTeam.Name)
                        {
                            DateTime matchDate = new DateTime();
                            if (DateTime.TryParseExact(match.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out matchDate))
                            {

                            }
                            else
                            {
                                if (DateTime.TryParseExact(match.Date, "dd/MM/yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out matchDate))
                                {

                                }
                            }
                            int compare = DateTime.Compare(matchDate, Date);
                            if (compare < 0)
                            {
                                lastEncounterCount++;
                                if (match.HomeGoals + match.AwayGoals > 3)
                                {
                                    lastEncounterOver35++;
                                }
                            }
                        }
                    }
                }
            }

            lastEncounterOver35 = lastEncounterOver35 / lastEncounterCount * 100;

            int iHome = 0;
            int iAway = 0;

            foreach (Match match in HomeTeam.HomeMatches)
            {
                if (iHome < 6 || iHome != 5)
                {
                    if (HomeTeam.HomeMatches[iHome].HomeGoals + HomeTeam.HomeMatches[iHome].AwayGoals > 3)
                    {
                        homeLastOver35++;
                    }
                }

                iHome++;
            }

            foreach (Match match in AwayTeam.AwayMatches)
            {
                if (iAway < 6 || iAway != 5)
                {
                    if (AwayTeam.AwayMatches[iAway].AwayGoals + AwayTeam.AwayMatches[iAway].HomeGoals > 3)
                    {
                        awayLastOver35++;
                    }
                }

                iAway++;
            }


            lastOver35 = ((homeLastOver35 / iHome) + (awayLastOver35 / iAway)) / 2 * 100;

            Over35 = 0;

            foreach (string score in scorePrediction.Keys)
            {
                string[] goals = score.Split('-');

                if (int.Parse(goals[0]) + int.Parse(goals[1]) > 3)
                {
                    Over35 += scorePrediction[score];
                }
            }


            if (!Double.IsNaN(lastEncounterOver35))
            {
                Over35 = Math.Round((Over35 + (League.Over35 / League.playedMatch * 100) + lastOver35 + (HomeTeam.HOver35 / HomeTeam.playedMatch * 100) + (AwayTeam.AOver35 / AwayTeam.awayPlayedMatch * 100) + lastEncounterOver35) / 6, 2);
            }
            else
            {
                Over35 = Math.Round((Over35 + (League.Over35 / League.playedMatch * 100) + lastOver35 + (HomeTeam.HOver35 / HomeTeam.playedMatch * 100) + (AwayTeam.AOver35 / AwayTeam.awayPlayedMatch * 100)) / 5, 2);
            }
        }

        public void Predict45()
        {
            double homeLastOver45 = 0;
            double awayLastOver45 = 0;

            List<League> seasonList = new List<League>();

            seasonList.Add(League);

            foreach (League season in League.lastSeasons)
            {
                seasonList.Add(season);
            }

            double lastEncounterCount = 0;
            double lastEncounterOver45 = 0;
            double lastOver45 = 0;

            foreach (League league in seasonList)
            {
                Team homeTeam = new Team("Zebi");
                try
                {
                    homeTeam = league.Teams.Where(p => p.Name == HomeTeam.Name).First();
                }
                catch (Exception)
                {

                }

                if (homeTeam != null)
                {
                    foreach (Match match in homeTeam.HomeMatches)
                    {
                        if (match.AwayTeam == AwayTeam.Name)
                        {
                            DateTime matchDate = new DateTime();
                            if (DateTime.TryParseExact(match.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out matchDate))
                            {

                            }
                            else
                            {
                                if (DateTime.TryParseExact(match.Date, "dd/MM/yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out matchDate))
                                {

                                }
                            }
                            int compare = DateTime.Compare(matchDate, Date);
                            if (compare < 0)
                            {
                                lastEncounterCount++;
                                if (match.HomeGoals + match.AwayGoals > 4)
                                {
                                    lastEncounterOver45++;
                                }
                            }
                        }
                    }
                }
            }

            lastEncounterOver45 = lastEncounterOver45 / lastEncounterCount * 100;

            int iHome = 0;
            int iAway = 0;

            foreach (Match match in HomeTeam.HomeMatches)
            {
                if (iHome < 6 || iHome != 5)
                {
                    if (HomeTeam.HomeMatches[iHome].HomeGoals + HomeTeam.HomeMatches[iHome].AwayGoals > 4)
                    {
                        homeLastOver45++;
                    }
                }

                iHome++;
            }

            foreach (Match match in AwayTeam.AwayMatches)
            {
                if (iAway < 6 || iAway != 5)
                {
                    if (AwayTeam.AwayMatches[iAway].AwayGoals + AwayTeam.AwayMatches[iAway].HomeGoals > 4)
                    {
                        awayLastOver45++;
                    }
                }

                iAway++;
            }


            lastOver45 = ((homeLastOver45 / iHome) + (awayLastOver45 / iAway)) / 2 * 100;

            Over45 = 0;

            foreach (string score in scorePrediction.Keys)
            {
                string[] goals = score.Split('-');

                if (int.Parse(goals[0]) + int.Parse(goals[1]) > 4)
                {
                    Over45 += scorePrediction[score];
                }
            }


            if (!Double.IsNaN(lastEncounterOver45))
            {
                Over45 = Math.Round((Over45 + (League.Over45 / League.playedMatch * 100) + lastOver45 + (HomeTeam.HOver45 / HomeTeam.playedMatch * 100) + (AwayTeam.AOver45 / AwayTeam.awayPlayedMatch * 100) + lastEncounterOver45) / 6, 2);
            }
            else
            {
                Over45 = Math.Round((Over45 + (League.Over45 / League.playedMatch * 100) + lastOver45 + (HomeTeam.HOver45 / HomeTeam.playedMatch * 100) + (AwayTeam.AOver45 / AwayTeam.awayPlayedMatch * 100)) / 5, 2);
            }
        }

        public void Predict55()
        {
            double homeLastOver55 = 0;
            double awayLastOver55 = 0;

            List<League> seasonList = new List<League>();

            seasonList.Add(League);

            foreach (League season in League.lastSeasons)
            {
                seasonList.Add(season);
            }

            double lastEncounterCount = 0;
            double lastEncounterOver55 = 0;
            double lastOver55 = 0;

            foreach (League league in seasonList)
            {
                Team homeTeam = new Team("Zebi");
                try
                {
                    homeTeam = league.Teams.Where(p => p.Name == HomeTeam.Name).First();
                }
                catch (Exception)
                {

                }

                if (homeTeam != null)
                {
                    foreach (Match match in homeTeam.HomeMatches)
                    {
                        if (match.AwayTeam == AwayTeam.Name)
                        {
                            DateTime matchDate = new DateTime();
                            if (DateTime.TryParseExact(match.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out matchDate))
                            {

                            }
                            else
                            {
                                if (DateTime.TryParseExact(match.Date, "dd/MM/yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out matchDate))
                                {

                                }
                            }
                            int compare = DateTime.Compare(matchDate, Date);
                            if (compare < 0)
                            {
                                lastEncounterCount++;
                                if (match.HomeGoals + match.AwayGoals > 5)
                                {
                                    lastEncounterOver55++;
                                }
                            }
                        }
                    }
                }
            }

            lastEncounterOver55 = lastEncounterOver55 / lastEncounterCount * 100;

            int iHome = 0;
            int iAway = 0;

            foreach (Match match in HomeTeam.HomeMatches)
            {
                if (iHome < 6 || iHome != 5)
                {
                    if (HomeTeam.HomeMatches[iHome].HomeGoals + HomeTeam.HomeMatches[iHome].AwayGoals > 5)
                    {
                        homeLastOver55++;
                    }

                }

                iHome++;
            }

            foreach (Match match in AwayTeam.AwayMatches)
            {
                if (iAway < 6 || iAway != 5)
                {
                    if (AwayTeam.AwayMatches[iAway].AwayGoals + AwayTeam.AwayMatches[iAway].HomeGoals > 5)
                    {
                        awayLastOver55++;
                    }
                }

                iAway++;
            }


            lastOver55 = ((homeLastOver55 / iHome) + (awayLastOver55 / iAway)) / 2 * 100;

            Over55 = 0;

            foreach (string score in scorePrediction.Keys)
            {
                string[] goals = score.Split('-');

                if (int.Parse(goals[0]) + int.Parse(goals[1]) > 5)
                {
                    Over55 += scorePrediction[score];
                }
            }


            if (!Double.IsNaN(lastEncounterOver55))
            {
                Over55 = Math.Round((Over55 + (League.Over55 / League.playedMatch * 100) + lastOver55 + (HomeTeam.HOver55 / HomeTeam.playedMatch * 100) + (AwayTeam.AOver55 / AwayTeam.awayPlayedMatch * 100) + lastEncounterOver55) / 6, 2);
            }
            else
            {
                Over55 = Math.Round((Over55 + (League.Over55 / League.playedMatch * 100) + lastOver55 + (HomeTeam.HOver55 / HomeTeam.playedMatch * 100) + (AwayTeam.AOver55 / AwayTeam.awayPlayedMatch * 100)) / 5, 2);
            }
        }
    }
}
