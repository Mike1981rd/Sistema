using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SistemaContable.Models;

namespace SistemaContable.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            // TODO: Replace with actual data from database
            var viewModel = new DashboardViewModel
            {
                FinancialSummary = new FinancialSummary
                {
                    Sales = 8459.00m,
                    Expenses = 3285.00m,
                    Profits = 5174.00m,
                    PendingPayments = 2560.00m,
                    SalesTrend = 12.5m,
                    ExpensesTrend = 8.3m,
                    ProfitsTrend = 15.2m,
                    PendingPaymentsTrend = -3.7m
                },
                PendingInvoices = new List<PendingInvoice>
                {
                    new PendingInvoice
                    {
                        InvoiceNumber = "0001",
                        ClientName = "Cliente A",
                        Amount = 1250.00m,
                        DueDate = DateTime.Now.AddDays(5),
                        Status = "Vence en 5 días"
                    },
                    new PendingInvoice
                    {
                        InvoiceNumber = "0002",
                        ClientName = "Cliente B",
                        Amount = 850.00m,
                        DueDate = DateTime.Now.AddDays(3),
                        Status = "Vence en 3 días"
                    },
                    new PendingInvoice
                    {
                        InvoiceNumber = "0003",
                        ClientName = "Cliente C",
                        Amount = 2100.00m,
                        DueDate = DateTime.Now.AddDays(7),
                        Status = "Vence en 7 días"
                    }
                }
            };

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
