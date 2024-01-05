namespace Ludo.Domain.DTOs
{
    public class GameDto
    {
        public int Id { get; set; }
        public List<PlayerDto>? Players { get; set; }
        public string FirstDiceRoller { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is GameDto dto &&
                   Id == dto.Id &&
                   FirstDiceRoller == dto.FirstDiceRoller;
        }
    }
}