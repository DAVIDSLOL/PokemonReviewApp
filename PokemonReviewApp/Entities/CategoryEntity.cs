namespace PokemonReviewApp.Models
{
    public class CategoryEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List <PokemonCategory> PokemonCategories { get; set; }
    }
}



