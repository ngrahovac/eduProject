using eduProjectModel.Domain;
using System;

namespace eduProjectModel.Display
{
    public class NewsDisplayModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }

        public NewsDisplayModel(News news)
        {
            Id = news.Id;
            Title = news.Title;
            Content = news.Content;
            Date = news.Date;
        }

        public NewsDisplayModel()
        {

        }
    }
}
