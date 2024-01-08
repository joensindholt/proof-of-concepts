using Dapper;
using DapperDotnetCore.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DapperDotnetCore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public WeatherForecastController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get(CancellationToken cancellationToken)
        {
            var db = _unitOfWork.Begin();
            var forecasts = await db.QueryAsync<WeatherForecast>("select * from weatherforecast");
            return forecasts;
        }
    }
}
