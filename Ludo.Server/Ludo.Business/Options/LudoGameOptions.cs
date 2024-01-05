using System.ComponentModel.DataAnnotations;

namespace Ludo.Business.Options
{
    public class LudoGameOptions
    {
        public const string Key = "Ludo";

        #region LobbyOptions

        [Required(ErrorMessage = "Max Lobby Participants Required")]
        [Range(1, 4, ErrorMessage = "MaxLobbyParticipants out of range (1 - 4)")]
        public int MaxLobbyParticipants { get; set; }

        [Required(ErrorMessage = "LobbyIdLowerRange Required")]
        public int LobbyIdLowerRange { get; set; }

        [Required(ErrorMessage = "LobbyIdUpperRange Required")]
        public int LobbyIdUpperRange { get; set; }

        #endregion

        #region GameOptions

        [Required(ErrorMessage = "PiceMaxNumber Required")]
        public int PieceMaxNumber { get; set; }

        #region BoardOptions

        [Required(ErrorMessage = "HomeCellSideNumber Required")]
        public int HomeCellSideNumber { get; set; }
        
        [Required(ErrorMessage = "BasicCellsSideNumber Required")]
        public int BasicCellsSideNumber { get; set; }

        [Required(ErrorMessage = "SpecialCellsSideNumber Required")]
        public int SpecialCellsSideNumber { get; set; }

        [Required(ErrorMessage = "BasicCellSideNumber Required")]
        public int BasicCellSideNumber { get; set; }

        [Required(ErrorMessage = "GreenStartPosition Required")]
        public int GreenStartPosition { get; set; }

        [Required(ErrorMessage = "YellowStartPositio Required")]
        public int YellowStartPosition { get; set; }

        [Required(ErrorMessage = "BlueStartPosition  Required")]
        public int BlueStartPosition { get; set; }

        [Required(ErrorMessage = "RedStartPosition { Required")]
        public int RedStartPosition { get; set; }

        [Required(ErrorMessage = "FullRoadCellsNumber Required")]
        public int FullRoadCellsNumber { get; set; }

        [Required(ErrorMessage = "SpawnPositionLowerBound Required")]
        public int SpawnPositionLowerBound { get; set; }

        [Required(ErrorMessage = "SpawnPositionUpperBound Required")]
        public int SpawnPositionUpperBound { get; set; }

        [Required(ErrorMessage = "FinalCellPositionLowerBound Required")]
        public int FinalCellPositionLowerBound { get; set; }

        [Required(ErrorMessage = "FinalCellPositionUpperBound Required")]
        public int FinalCellPositionUpperBound { get; set; }
        
        #endregion

        #endregion
    }
}
