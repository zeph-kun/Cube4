using Cube_4.Datas;
using Cube_4.models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cube_4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public StockController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet("stock")]
        public IActionResult GetStock()
        {
            List<Stock> myStock = context.Stocks.ToList();

            if (myStock.Count > 0)
            {
                return Ok(new
                {
                    Message = "Voici votre Stock :",
                    Stock = myStock
                });

            }
            else
            {
                return NotFound(new
                {
                    Message = "Aucun objet dans le Stock !"
                });
            }
        }


        [HttpGet("stock/{articleId}")]
        public IActionResult GetStockById(int articleId)
        {
            Stock? findStock = context.Stocks.FirstOrDefault(x => x.ArticleId == articleId);

            if (findStock == null)
            {
                return NotFound(new
                {
                    Message = "Aucun article trouvé avec cet ID dans le stock !"
                });
            }
            else
            {
                return Ok(new
                {
                    Message = "Article dans le stock trouvé !",
                    Article = new StockDTO() { Id = findStock.Id, ArticleId = findStock.ArticleId, Quantite = findStock.Quantite }
                });
            }
        }
        
        [HttpPost("stock")]
        public IActionResult AddStock(StockDTO newStock, int articleId, int Quantite)
        {
            Article? findArticle = context.Articles.FirstOrDefault(x => x.Id == articleId);
            
            if (findArticle == null)
            {
                return NotFound(new
                {
                    Message = "Aucun article trouvé avec cet ID !"
                });
            }
            Stock addStock = new Stock()
            {
                Quantite = Quantite,
                ArticleId = articleId
            };
            context.Stocks.Add(addStock);
            if (context.SaveChanges() > 0)
            {
                return Ok(new
                {
                    Message = "L'article a été ajouté au stock avec succès!",
                    StockId = addStock.Id
                });
            }
            else
            {
                return BadRequest(new
                {
                    Message = "Une erreur est survenue..."
                });
            }
        }
        
        [HttpPatch("stock")]
        public IActionResult EditStock(int articleId, int newQuantite)
        {
            Stock? findStock = context.Stocks.FirstOrDefault(x => x.ArticleId == articleId);

            if (findStock != null)
            {
                findStock.Quantite = newQuantite;
                context.Stocks.Update(findStock);
                if (context.SaveChanges() > 0)
                {
                    return Ok(new
                    {
                        Message = "Le stock a bien été modifié !"
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        Message = "Une erreur a eu lieu durant la modification..."
                    });
                }
            }
            else
            {
                return NotFound(new
                {
                    Message = "Aucun article n'a été trouvé avec cet ID !"
                });
            }
        }
        
        [HttpDelete("stock/{articleId}")]
        public IActionResult DeleteStock(int articleId)
        {
            Stock? findStock = context.Stocks.FirstOrDefault(x => x.ArticleId == articleId);

            if (findStock == null)
            {
                return NotFound(new
                {
                    Message = "Aucun Article trouvé avec cet ID !"
                });
            }
            else
            {
                context.Stocks.Remove(findStock);
                if (context.SaveChanges() > 0)
                {
                    return Ok(new
                    {
                        Message = "L'article a bien été enlevé du stock",
                    });
                }
                else
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
