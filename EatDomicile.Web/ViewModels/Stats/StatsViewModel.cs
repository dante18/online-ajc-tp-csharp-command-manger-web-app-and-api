using EatDomicile.Web.Services.Domains.Statistics.DTO;

namespace EatDomicile.Web.ViewModels.Stats;

public class StatsViewModel
{
    public int totalNumberOrder { get; set; }

    public IEnumerable<OrderDistributions> totalNumberOrderByStatus { get; set; }

    public IEnumerable<OrderDistributions> totalNumberOrderByUser { get; set; }

    public IEnumerable<OrderDistributions> totalNumberOrderByDay { get; set; }

    public IEnumerable<OrderDistributions> userMoreOrder { get; set; }

    public double AverageDeliveryTimeHours { get; set; }
}
