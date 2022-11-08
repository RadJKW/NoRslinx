using NoRslinx.Application.Common.Interfaces;

namespace NoRslinx.Infrastructure.Services;
public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
}
