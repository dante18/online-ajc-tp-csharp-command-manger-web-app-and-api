using EatDomicile.Web.Services.Dtos.Application.Statistics;

namespace EatDomicile.Web.ViewsModels.Stats;

public class StatsViewModel
{
    public int totalNumberOrder { get; set; }

    public IEnumerable<OrderDistributions> totalNumberOrderByStatus { get; set; }

    public IEnumerable<OrderDistributions> totalNumberOrderByUser { get; set; }

    public IEnumerable<OrderDistributions> totalNumberOrderByDay { get; set; }

    public IEnumerable<OrderDistributions> userMoreOrder { get; set; }

    public double AverageDeliveryTimeHours { get; set; }
}
