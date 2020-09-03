using UnityEngine;

namespace Assets.Scripts.Resources.Game_Resources_DB
{
    class GetGameDataFromDBOnStart : MonoBehaviour
    {
        /// <summary>
        /// When game is started - get players data from database
        /// </summary>
        public static void GetResourcesFromDB()
        {
            using (MyDBContext db = new MyDBContext())
            {
                var res = db.ResourcesDBSet;
                foreach (var item in res)
                {
                    if (item.Id.Equals(0)) // need get real player id and check it by Equals
                    {
                        PlayerPrefs.SetInt(PlayerPrefsKeys.Money, item.Money);
                        PlayerPrefs.SetInt(PlayerPrefsKeys.Gems, item.Gems);
                        PlayerPrefs.SetInt(PlayerPrefsKeys.TicketsForEnterToChallenge, item.Tickets);
                        PlayerPrefs.SetInt(PlayerPrefsKeys.BestScoreClassic, item.BestScore_Classic);
                        PlayerPrefs.SetInt(PlayerPrefsKeys.BestScoreChallenge, item.BestScore_Challenge);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// When game is ended - save players data to database
        /// </summary>
        private void OnApplicationQuit()
        {
            using (MyDBContext db = new MyDBContext())
            {
                var res = db.ResourcesDBSet;
                foreach (var item in res)
                {
                    if (item.Id.Equals(0)) // need get real player id and check it by Equals
                    {
                        item.Money = PlayerPrefs.GetInt(PlayerPrefsKeys.Money);
                        item.Gems = PlayerPrefs.GetInt(PlayerPrefsKeys.Gems);
                        item.Tickets = PlayerPrefs.GetInt(PlayerPrefsKeys.TicketsForEnterToChallenge);
                        item.BestScore_Classic = PlayerPrefs.GetInt(PlayerPrefsKeys.BestScoreClassic);
                        item.BestScore_Challenge = PlayerPrefs.GetInt(PlayerPrefsKeys.BestScoreChallenge);
                        db.SaveChanges();
                        break;
                    }
                }
            }
        }
    }
}
