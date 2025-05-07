using System;

namespace SistemaContable.Models
{
    public class FinancialSummary
    {
        public decimal Sales { get; set; }
        public decimal Expenses { get; set; }
        public decimal Profits { get; set; }
        public decimal PendingPayments { get; set; }
        public decimal SalesTrend { get; set; }
        public decimal ExpensesTrend { get; set; }
        public decimal ProfitsTrend { get; set; }
        public decimal PendingPaymentsTrend { get; set; }
    }

    public class PendingInvoice
    {
        public string InvoiceNumber { get; set; }
        public string ClientName { get; set; }
        public decimal Amount { get; set; }
        public DateTime DueDate { get; set; }
        public string Status { get; set; }
    }

    public class DashboardViewModel
    {
        public FinancialSummary FinancialSummary { get; set; }
        public List<PendingInvoice> PendingInvoices { get; set; }
        public DateTime CurrentDate { get; set; }
    }
} 