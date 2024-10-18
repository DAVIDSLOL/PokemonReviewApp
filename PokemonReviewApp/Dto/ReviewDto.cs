namespace PokemonReviewApp.Dto
{
    public class ReviewDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = "Нет заголовка";
        public string Text { get; set; } = "";
        public int Rating { get; set; }
    }
}
