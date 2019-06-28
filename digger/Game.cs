using System.Windows.Forms;

namespace Digger
{
    public static class Game
    {
        private const string mapWithPlayerTerrain = @"
P TTTT TT
T T  TTTT
TTTTTT TT
T TTTT TT
T TTTT TT
TTTTTT TT";

        private const string mapWithPlayerTerrainSackGold = @"
 TTGTT TS
TST  TSTT
T TTTTSTT
TPTSTS TT
T TTTG ST
TSTSTT TT";

        private const string mapWithPlayerTerrainSackGold2 = @"
PTTGTT TS
TST  TSTT
TTTTTTSTT
T TSTS TT
TPTTTG ST
TSTSTT TT";

        private const string mapWithPlayerTerrainSackGoldMonster = @"
PTTGTT TST
TST  TSTTM
TTT TTSTTT
T TSTS TTT
T TTTGMSTS
T TST M TS
TSTMTTMTTT
S TTST  TG
 TGST MTTT
 T  TMTTTT";

        public static ICreature[,] Map;
        public static int Scores;
        public static bool IsOver;

        public static Keys KeyPressed;
        public static int MapWidth => Map.GetLength(0);
        public static int MapHeight => Map.GetLength(1);

        public static void CreateMap()
        {
            Map = CreatureMapCreator.CreateMap(mapWithPlayerTerrainSackGoldMonster);
        }
    }
}