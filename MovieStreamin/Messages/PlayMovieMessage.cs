namespace MovieStreamin.Messages
{
    public class PlayMovieMessage
    {
        public PlayMovieMessage(string title, int userId)
        {
            Title = title;
            UserId = userId;
        }

        public string Title { get; private set; }
        public int UserId { get; private set; }
    }
}