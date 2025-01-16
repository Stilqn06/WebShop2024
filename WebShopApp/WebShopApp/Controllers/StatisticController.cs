using Microsoft.AspNetCore.Mvc;

using WebShopApp.Core.Contracts;
using WebShopApp.Models.Statistic;

namespace WebShopApp.Controllers
{
    public class StatisticController : Controller
    {
        private readonly IStatisticsService statisticsService;
        public StatisticController(IStatisticsService statisticsService)
        {
            this.statisticsService = statisticsService;
        }
        public IActionResult Index()
        {
            StatisticVM statistics =new StatisticVM();
            statistics.CountClients=statisticsService.CountClients();
            statistics.CountProducts = statisticsService.CountProducts();
            statistics.CountOrders = statisticsService.CountOrders();
            statistics.CountOrders = statisticsService.CountOrders();
            return View(statistics);
        }
    }
}
