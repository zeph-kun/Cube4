using Cube_4.Datas;
using Cube_4.models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cube_4.Controllers
{
    [Route("api")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public ArticleController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet("articles")]
        public IActionResult GetArticles()
        {
            List<Article> myArticles = context.Articles.ToList();
            
            if (myArticles.Count > 0)
            {
                return Ok(new
                {
                    Message = "Voici vos Articles:",
                    Article = myArticles
                });

            } else
            {
                return NotFound(new
                {
                    Message = "Aucun Articles dans la base de données !"
                });
            }
        }
        
        [HttpGet("articles/{articleId}")] 
        public IActionResult GetArticlesById(int articleId)
        {
            Article? findArticle = context.Articles.FirstOrDefault(x => x.Id == articleId);

            if (findArticle == null)
            {
                return NotFound(new
                {
                    Message = "Aucun article trouvé avec cet ID !"
                });
            } else
            {
                return Ok(new
                {
                    Message = "Article trouvé !",
                    Article = new ArticleDTO() { Id = findArticle.Id, Libelle = findArticle.Libelle, Prix = findArticle.Prix, Famille = findArticle.Famille, Fournisseur = findArticle.Fournisseur}
                });
            }
        }

        [HttpPost("articles")]
        public IActionResult AddArticles(ArticleDTO newArticle)
        {
            Article addArticle = new Article()
            {
                Libelle = newArticle.Libelle, Prix = newArticle.Prix, Famille = newArticle.Famille,
                Fournisseur = newArticle.Fournisseur
            };
            context.Articles.Add(addArticle);
            if (context.SaveChanges() > 0)
            {
                return Ok(new
                {
                    Message = "L'article a été ajouté avec succès!",
                    ArticleId = addArticle.Id
                });
            } else
            {
                return BadRequest(new
                {
                    Message = "Une erreur est survenue..."
                });
            }
        }
        
        [HttpPatch("articles")]
        public IActionResult EditArticle(ArticleDTO newInfos)
        {
            Article? findArticle = context.Articles.FirstOrDefault(x => x.Id == newInfos.Id);

            if (findArticle != null )
            {
                findArticle.Libelle = newInfos.Libelle;
                findArticle.Prix = newInfos.Prix;
                findArticle.Famille = newInfos.Famille;
                findArticle.Fournisseur = newInfos.Fournisseur;

                context.Articles.Update(findArticle);
                if (context.SaveChanges() > 0)
                {
                    return Ok(new
                    {
                        Message = "L'article a bien été modifié !"
                    });
                } else
                {
                    return BadRequest(new
                    {
                        Message = "Une erreur a eu lieu durant la modification..."
                    });
                }
            } else
            {
                return NotFound(new
                {
                    Message = "Aucun article n'a été trouvé avec cet ID !"
                });
            }
        }
        [HttpDelete("articles/{articleId}")]
        public IActionResult DeleteArticle(int articleId)
        {
            Article? findArticle = context.Articles.FirstOrDefault(x => x.Id == articleId);

            if (findArticle == null)
            {
                return NotFound(new
                {
                    Message = "Aucun Article trouvé avec cet ID !"
                });
            }
            else
            {
                context.Articles.Remove(findArticle);
                if (context.SaveChanges() > 0)
                {
                    return Ok(new
                    {
                        Message = "L'article a bien été supprimé",
                    });
                } else
                {
                    return BadRequest(new
                    {
                        Message = "Une erreur est survenue..."
                    });
                }
            }
        }
    }
}