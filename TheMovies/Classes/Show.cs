namespace TheMovies
{
    public class Show
    {
        private static int id = 0;

        public int Id { get { return id; } }
        public String StartTime { get; set; }
        public DateTime Date { get; set; } 

        public Movie Movie { get; set; }

        public Show(string startTime, DateTime date, Movie movie)
        {
            StartTime = startTime;
            Date = date;
            Movie = movie;
            
            id++;
        }

        public string GetMovieInfo()
        {
            return $"{Movie.Title};{Movie.Genre};{Movie.Duration};{Movie.Director}";
        }

        public override string ToString()
        {
            return $"{StartTime};{Date.ToShortDateString()}";
        }
    }
}
