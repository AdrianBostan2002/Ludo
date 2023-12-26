namespace Ludo.Domain.DTOs
{
    public class PlayerDto
    {
        public string? Name { get; set; }

        public List<PieceDto>? Pieces { get; set; }

        public bool IsReady { get; set; }
    }
}