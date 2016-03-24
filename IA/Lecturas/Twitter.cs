using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TweetSharp;

namespace IA.Lecturas
{
    public class Twitter
    {
        public string getTweet(string txtTwitterName, int cant)
        {
            //TwitterService("consumer key", "consumer secret");
            var service = new TwitterService("VSZWPgVuf1dzi0PeMynxbY3Cj", "4wjWYszBkyrdhOIGC2jqzC22MWEXO0wfNgh6ZbXyp1aLDdsAT0");

            //AuthenticateWith("Access Token", "AccessTokenSecret");
            service.AuthenticateWith("711068382272081920-ne7HQBhZOLtaUZsIqSDtZCfhDDMxFCM", "N6Evw7D1Cvrjh8nTHgTEiF447ynuvrv3p3YdQtoZpysjm");
            
            IEnumerable<TwitterStatus> tweets = service.ListTweetsOnUserTimeline(new ListTweetsOnUserTimelineOptions { ScreenName = txtTwitterName, Count = cant, });

            string result = "";
            foreach (var tweet in tweets)
            {
                result += @tweet.Text + "\n";
            }

            return result;
        }
    }
}