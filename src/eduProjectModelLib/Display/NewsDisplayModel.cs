using eduProjectModel.Domain;
using System;

namespace eduProjectModel.Display
{
    public class NewsDisplayModel
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }

        public NewsDisplayModel(News news)
        {
            Title = news.Title;
            Content = news.Content;
            Date = news.Date;
        }

        public NewsDisplayModel()
        {

        }
    }
}
