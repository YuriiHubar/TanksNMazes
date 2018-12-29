using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{   
    class Stats
    {

        // Singleton-related code
        private static Stats instance;
        private Stats() { }
        public static Stats getInstance()
        {
            if (instance == null)
                instance = new Stats();
            return instance;
        }

        // Stats
        private int shotCount = 0;
        private int enemiesDefeated = 0;
        private int score = 0;
        
        // Overall stats
        private int overallShotCount = 0;
        private int overallEnemiesDefeated = 0;
        private int overallScore = 0;
        private int overallDeathCount = 0;

        public int ShotCount
        {
            get { return shotCount; }
        }
        public int EnemiesDefeated
        {
            get { return enemiesDefeated; }
        }
        public float Precision
        {
            get { return shotCount == 0 ? 0 : (float)enemiesDefeated * 100 / (float)shotCount; }
        }
        public int Score
        {
            get { return score; }
        }

        public int OverallShotCount
        {
            get { return overallShotCount; }
        }
        public int OverallEnemiesDefeated
        {
            get { return overallEnemiesDefeated; }
        }
        public float OverallPrecision
        {
            get { return overallShotCount == 0 ? 0 : (float)overallEnemiesDefeated * 100 / (float)overallShotCount; }
        }
        public int OverallScore
        {
            get { return overallScore; }
        }
        public float OverallDeathCount
        {
            get { return overallDeathCount; }
        }

        public void ShotFired()
        {
            ++shotCount;
            ++overallShotCount;
        }

        public void EnemyDefeated()
        {
            ++enemiesDefeated;
            ++overallEnemiesDefeated;
        }

        public void PlayerDefeated()
        {
            ++overallDeathCount;
        }

        public void AddScore(int gain)
        {
            score += gain;
            overallScore += gain;
        }

        public void NewRound()
        {
            shotCount = 0;
            enemiesDefeated = 0;
            score = 0;
        }

    }
}
