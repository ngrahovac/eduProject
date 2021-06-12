using eduProjectModel.Domain;
using System;
using System.ComponentModel.DataAnnotations;

namespace eduProjectModel.Input
{
    public class NewsInputModel
    {
        [Required(ErrorMessage = "Polje ne može biti prazno.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Polje ne može biti prazno.")]
        public string Content { get; set; }

        public NewsInputModel(string title, string content)
        {
            Title = title;
            Content = content;
        }

        public NewsInputModel()
        {

        }

        public void MapTo(News news)
        {
            news.Title = Title;
            news.Content = Content;
            news.Date = DateTime.Now;
        }
    }
}
