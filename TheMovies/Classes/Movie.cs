namespace TheMovies
{
    public class Movie
    {
        public string Title { get; set; }
        public string Genre { get; set; }

        public string Duration { get; set; }

        public string Director {  get; set; }
        public Movie(string title, string genre, string duration, string director) 
        {
            Title = title;
            Genre = genre;
            Duration = duration;
            Director = director;
        }
    }
}
