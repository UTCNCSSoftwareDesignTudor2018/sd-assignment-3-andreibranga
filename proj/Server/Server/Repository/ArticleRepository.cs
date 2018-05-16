using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Server.Models;

namespace Server.Repository
{
    public class ArticleRepository
    {
        private NewsAgencyDBEntities ctx;

        public ArticleRepository()
        {
            this.ctx=new NewsAgencyDBEntities();
        }

        public IQueryable<ArticleModel> GetAllArticles()
        {
            return ctx.Articles.Select(z => new ArticleModel()
            {
                ArticleId = z.ArticleId,
                Date = z.Date,
                Author = z.Author,
                Title = z.Title,
                Abstract = z.Abstract,
                Body = z.Body
            });
        }

        public DateTime GetLatestArticleTime()
        {
            if (ctx.Articles != null) return ctx.Articles.OrderByDescending(p => p.Date).FirstOrDefault().Date;
            return DateTime.MinValue;
        }

        public void PublishArticle(string author, DateTime date, string title, string abstractt, string body)
        {
            Article article=new Article();
            article.Author = author;
            article.Date = date;
            article.Title = title;
            article.Abstract = abstractt;
            article.Body = body;


            ctx.Articles.Add(article);
            ctx.SaveChanges();
        }
    }
}
