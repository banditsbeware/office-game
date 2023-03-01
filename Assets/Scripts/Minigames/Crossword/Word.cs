using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Word
{
    public string word;
    public string postItDes;
    public bool used = false;
    public (int, int) direction;
    public (int, int) origin;
    public GameObject startButton;
    public GameObject endButton;
}


///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


// public void CreateWords()
// {
// //define every word and its responses
// hello = new Word(allWords){
//   word = "hello",
//   playerTalk_normal = "",
//   haroldTalk_normal = "",};

// goodbye = new Word (allWords) {
//   word = "GOODBYE",
//   postItDes = "end the exchange",
//   playerTalk_normal = "alright, bud, I should get back to work",
//   haroldTalk_normal = "",};

// Word weather = new Word (allWords, avalibleWords) {
//   word = "WEATHER",
//   postItDes = "the climate",
//   playerTalk_normal = "so, how about that weather, huh?",
//   haroldTalk_normal = "yeah, quite the weather",};

//   Word snow = new Word (allWords) {
//     word = "SNOW",
//     postItDes = "precipitation",
//     playerTalk_normal = "I really wish it snowed more, I quite like snow",
//     haroldTalk_normal = "I'm more of a sunshine guy myself"};

//   Word clothes = new Word (allWords) {
//     word = "CLOTHES",
//     postItDes = "garments",
//     playerTalk_normal = "have enough clothes for the upcoming season?",
//     haroldTalk_normal = "plan on going to the store with my kids to get jackets next week"};

//   Word coffee = new Word (allWords) {
//     word = "COFFEE",
//     postItDes = "warm drinks",
//     playerTalk_normal = "cup o' joe really helps with the cold",
//     haroldTalk_normal = "I take mine with 2 sugars"};
  
// Word child = new Word (allWords, avalibleWords) {
//   word = "CHILD",
//   postItDes = "harold's offspring",
//   playerTalk_normal = "how are the kids doing these days?",
//   haroldTalk_normal = "little Jimmy's playing soccer, he's quite the young man"};

//   Word school = new Word (allWords) {
//     word = "SCHOOL",
//     postItDes = "education",
//     playerTalk_normal = "nice, how is the schooling coming along?",
//     haroldTalk_normal = "very well. B's across the board"};

//   Word growth = new Word (allWords) {
//     word = "GROWTH",
//     postItDes = "how tall Jimmy is",
//     playerTalk_normal = "he's growing up so fast, pretty tall for his age",
//     haroldTalk_normal = "yep, soon enough he'll pass me up"};

//   Word soccer = new Word (allWords) {
//     word = "SOCCER",
//     postItDes = "Jimmy's athletics",
//     playerTalk_normal = "Jimmy still playing ball with the team?",
//     haroldTalk_normal = "oh, yeah, he's got a scrimmage next week"};

//   Word internet = new Word (allWords) {
//     word = "INTERNET",
//     postItDes = "kids these days",
//     playerTalk_normal = "he on that tink tonk like all the other kids",
//     haroldTalk_normal = "tink tonk, instant grams, all the same to me"};

// Word work = new Word (allWords, avalibleWords) {
//       word = "WORK",
//       postItDes = "the job",
//       playerTalk_normal = "work's really been grinding my gears lately",
//       haroldTalk_normal = "tell me about it"};

//   Word boss = new Word (allWords) {
//     word = "BOSS",
//     postItDes = "the big man",
//     playerTalk_normal = "boss man's been breathing down my neck",
//     haroldTalk_normal = "guess that's why they pay him the big bucks"};
  
//   Word hours = new Word (allWords) {
//     word = "HOURS",
//     postItDes = "9 to 5",
//     playerTalk_normal = "can't wait till we're off at 5",
//     haroldTalk_normal = "yep, I'll be going home to watch some ball"};
  
//   Word merger = new Word (allWords) {
//     word = "MERGER",
//     postItDes = "getting bought out",
//     playerTalk_normal = "any updates on the merger?",
//     haroldTalk_normal = "no updates yet from the man upstairs"};
  
//   Word client = new Word (allWords) {
//     word = "CLIENT",
//     postItDes = "the people you sell to",
//     playerTalk_normal = "I dealth with a pretty annoying client today",
//     haroldTalk_normal = "they whine like children, I tell ya"};

// Word movies = new Word (allWords, avalibleWords) {
//       word = "MOVIES",
//       postItDes = "entertainment",
//       playerTalk_normal = "you a movie guy?",
//       haroldTalk_normal = "love the big screen"};

//   Word release = new Word (allWords) {
//       word = "RELEASE",
//       postItDes = "new to theaters",
//       playerTalk_normal = "see any new ones letely?",
//       haroldTalk_normal = "the family did a movie marathon when it snowed a few days ago"};
  
//   Word actor = new Word (allWords) {
//       word = "ACTOR",
//       postItDes = "the guy on screen",
//       playerTalk_normal = "that one guy's getting really famous, huh",
//       haroldTalk_normal = "he's in everything these days"};

//   Word scifi = new Word (allWords) {
//       word = "SCIFI",
//       postItDes = "movies in space",
//       playerTalk_normal = "I miss all the good scifi from our childhood",
//       haroldTalk_normal = "visual effects have come such a long way"};

//   Word prices = new Word (allWords) {
//       word = "PRICES",
//       postItDes = "ticket costs",
//       playerTalk_normal = "theaters are really ripping us off these days",
//       haroldTalk_normal = "back when we were kids, tickets were just a dollar, remember?"};

// Word sports = new Word (allWords) {
//       word = "SPORTS",
//       postItDes = "athletics",
//       playerTalk_normal = "how bout them sports",
//       haroldTalk_normal = "yup"};

//   Word game = new Word (allWords) {
//       word = "GAME",
//       postItDes = "a recent competition",
//       playerTalk_normal = "did you watch the game last night?",
//       haroldTalk_normal = "oh yeah, what a comeback"};

//   Word team = new Word (allWords) {
//       word = "TEAM",
//       postItDes = "blue jerseys",
//       playerTalk_normal = "I'm a pretty big fan of the blue team",
//       haroldTalk_normal = "I'm a red team fan myself"};

//   Word varsity = new Word (allWords) {
//       word = "VARSITY",
//       postItDes = "back in high school",
//       playerTalk_normal = "reminds me of the good ol' days as a lineman for the high school",
//       haroldTalk_normal = "that sure takes me back"};




// //add branches to each word
// //check milanote for map
// weather.normalOptions = new List<Word>(){snow, clothes, coffee};
//   snow.normalOptions = new List<Word>(){clothes, coffee};
//   clothes.normalOptions = new List<Word>(){child, coffee, snow};
//   coffee.normalOptions = new List<Word>(){clothes, snow};
// child.normalOptions = new List<Word>(){school, growth, soccer, internet};
//   school.normalOptions = new List<Word>(){growth, soccer, internet};
//   growth.normalOptions = new List<Word>(){school, soccer, internet};
//   soccer.normalOptions = new List<Word>(){growth, internet, school, sports};
//   internet.normalOptions = new List<Word>(){growth, soccer, school};
// work.normalOptions = new List<Word>(){boss, hours, merger, client};
//   boss.normalOptions = new List<Word>(){hours, merger, client};
//   hours.normalOptions = new List<Word>(){boss, merger, sports};
//   merger.normalOptions = new List<Word>(){boss, client};
//   client.normalOptions = new List<Word>(){boss, hours, merger, child};
// movies.normalOptions = new List<Word>(){release, actor, scifi, prices};
//   release.normalOptions = new List<Word>(){actor, scifi, prices, snow, child};
//   actor.normalOptions = new List<Word>(){release, scifi, prices};
//   scifi.normalOptions = new List<Word>(){release, actor, prices};
//   prices.normalOptions = new List<Word>(){release, actor, scifi, prices};
// sports.normalOptions = new List<Word>(){game, team, varsity};
//   game.normalOptions = new List<Word>(){team, varsity};
//   team.normalOptions = new List<Word>(){game, varsity};
//   varsity.normalOptions = new List<Word>(){game, team, soccer};


// foreach (Word word in allWords)
// {
//   word.post = "Play_" + word.word.ToLower();
//   word.playerTime = ((float)word.playerTalk_normal.Length * .05f) + Random.Range(0f, 1f);
//   word.haroldTime = ((float)word.haroldTalk_normal.Length * .05f);
// }

// int goodbyeIndex = Random.Range(0, 4);
// goodbyeRTPC.SetValue(gameObject, goodbyeIndex);
// goodbye.haroldTime = 2f;
// switch (goodbyeIndex)
// {
//   case 0:
//   goodbye.haroldTalk_normal = "adios amigo";
//   break;

//   case 1:
//   goodbye.haroldTalk_normal = "alrighty";
//   break;

//   case 2:
//   goodbye.haroldTalk_normal = "see ya";
//   break;

//   case 3:
//   goodbye.haroldTalk_normal = "till next time";
//   break;
// }

// int helloIndex = Random.Range(0, 4);
// helloRTPC.SetValue(gameObject, helloIndex);
// hello.haroldTime = 2f;
// switch (helloIndex)
// {
//   case 0:
//   hello.haroldTalk_normal = "hello";
//   break;

//   case 1:
//   hello.haroldTalk_normal = "howdy";
//   break;

//   case 2:
//   hello.haroldTalk_normal = "good to see ya again";
//   break;

//   case 3:
//   hello.haroldTalk_normal = "oh, hey";
//   break;
// }
// }
// }
//     }
// }

