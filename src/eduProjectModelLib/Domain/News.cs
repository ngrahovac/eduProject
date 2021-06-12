using System;

namespace eduProjectModel.Domain
{
    public class News
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }

        public News(string title, string content, DateTime date)
        {
            Title = title;
            Content = content;
            Date = date;
        }

        public News()
        {

        }
    }
}
