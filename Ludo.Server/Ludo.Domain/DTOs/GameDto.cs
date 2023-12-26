namespace Ludo.Domain.DTOs
{
    public class GameDto
    {
        public int Id { get; set; }
        public List<PlayerDto>? Players { get; set; }
        public string FirstDiceRoller { get; set; }
    }
}